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
            return SystemManager.Instance.SystemInfo.CurrentScene;
        }
        set 
        {
            SystemManager.Instance.SystemInfo.CurrentScene = value;
        }
    }

    public Action BeforeSceneLoadedHandler;
    public Action<string> AfterSceneLoadedHandler;

    private Action<string> _tempHandler;

    private bool _isLock = false;
    private bool _isInit = false;
    private string _tempType;
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
            _tempType = scene;

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
        if (_tempType != null)
        {
            CurrentScene = _tempType;
        }
        LoadingUI.Close();

        if (AfterSceneLoadedHandler != null)
        {
            AfterSceneLoadedHandler(scene.name);
            AfterSceneLoadedHandler -= _tempHandler;
        }

        if (scene.name == "Camp")
        {
            //if (!FlagManager.Instance.Info.FlagDic[FlagInfo.FlagEnum.Camp])
            if(FlowController.Instance.Info.CurrentStep == FlowInfo.StepEnum.Camp)
            {
                Event_6 event_6 = new Event_6();
                event_6.Start();
            }
            else if (FlowController.Instance.Info.CurrentStep == FlowInfo.StepEnum.BackCamp)
            {
                Event_9 event_9 = new Event_9();
                event_9.Start();
            }
        }
        //else if(scene.name == "Explore") 
        //{
        //    if (!FlagManager.Instance.Info.FlagDic[FlagInfo.FlagEnum.BackCamp])
        //    {
        //        Event_8 event_8 = new Event_8();
        //        event_8.Start();
        //    }
        //}
    }
}
