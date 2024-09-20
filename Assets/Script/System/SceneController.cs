using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneController
{
    private static SceneController _instance;
    public static SceneController Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = new SceneController();
            }
            return _instance;
        }
    }

    public string CurrentScene
    {
        get 
        { 
            return SystemManager.Instance.Info.CurrentScene;
        }
        set 
        {
            SystemManager.Instance.Info.CurrentScene = value;
        }
    }

    public Action BeforeSceneLoadedHandler;
    public Action<string> AfterSceneLoadedHandler;

    private Action<string> _tempHandler;

    private bool _isLock = false;
    private bool _isInit = false;
    private string _tempScene;
    private string _lastScene;
    private LoadingUI LoadingUI;
    //private SceneMemo _sceneMemo;

    public void Init()
    {
        GameObject obj = GameObject.Find("LoadingUI");
        if (!_isInit && obj != null)
        {
            SceneManager.sceneLoaded += SceneLoaded;
            LoadingUI = obj.GetComponent<LoadingUI>();
            _isInit = true;
        }
    }

    public void ChangeScene(string scene, Action<string> callback)
    {
        if (!_isLock)
        {
            _isLock = true;
            _tempScene = scene;
            _lastScene = CurrentScene;

            if (BeforeSceneLoadedHandler != null)
            {
                BeforeSceneLoadedHandler();
                BeforeSceneLoadedHandler = null;
            }

            LoadingUI.Open();
            SceneManager.LoadSceneAsync(scene);

            if (callback != null)
            {
                _tempHandler = callback;
                AfterSceneLoadedHandler += _tempHandler;
            }
        }
    }

    private void SceneLoaded(Scene scene, LoadSceneMode mode)
    {
        _isLock = false;
        if (_tempScene != null)
        {
            CurrentScene = _tempScene;
        }
        LoadingUI.Close();

        if (AfterSceneLoadedHandler != null)
        {
            AfterSceneLoadedHandler(scene.name);
            AfterSceneLoadedHandler -= _tempHandler;
        }

        if (scene.name == "Camp") 
        {
            if(!FlowController.Instance.Info.EventConditionList.Contains("SanaeJoin")) 
            {
                SanaeJoinEvent sanaeJoinEvent = new SanaeJoinEvent();
                sanaeJoinEvent.Start();
                FlowController.Instance.Info.EventConditionList.Add("SanaeJoin");
            }
            else if(SystemManager.Instance.Info.MaxFloor == 3 && !FlowController.Instance.Info.EventConditionList.Contains("MarisaJoin"))
            {
                MarisaJoinEvent marisaJoinEvent = new MarisaJoinEvent();
                marisaJoinEvent.Start();
                FlowController.Instance.Info.EventConditionList.Add("MarisaJoin");
            }
            else if(!FlowController.Instance.Info.EventConditionList.Contains("Cook") && _lastScene == "Explore")
            {
                CookEvent cookEvent = new CookEvent();
                cookEvent.Start();
                FlowController.Instance.Info.EventConditionList.Add("Cook");
            }
        }
        else if(scene.name == "Explore" && _lastScene == "Camp")
        {
            if(!FlowController.Instance.Info.EventConditionList.Contains("F2")) 
            {
                F2Event f2Event = new F2Event();
                f2Event.Start();
                FlowController.Instance.Info.EventConditionList.Add("F2");
            }
        }
    }
}
