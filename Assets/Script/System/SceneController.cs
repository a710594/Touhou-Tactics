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

            if(CurrentScene == "Explore" && scene == "Camp") 
            {
                Explore.ExploreManager.Instance.Save();
            }

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
            else if (FlowController.Instance.Info.CurrentStep == FlowInfo.StepEnum.Cook)
            {
                Event_10 event_10 = new Event_10();
                event_10.Start();
            }
            else if (FlowController.Instance.Info.CurrentStep == FlowInfo.StepEnum.AddRemainCharacter)
            {
                Event_12 event_12 = new Event_12();
                event_12.Start();
            }
        }
        else if(scene.name == "Explore") 
        {
            if (FlowController.Instance.Info.CurrentStep == FlowInfo.StepEnum.UseItem_1)
            {
                Event_11 event_11 = new Event_11();
                event_11.Start();
            }
        }
    }
}
