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
    public Action AfterSceneLoadedHandler;

    private bool _isLock = false;
    private bool _isInit = false;
    private string _tempType;
    private LoadingUI LoadingUI;
    //private SceneMemo _sceneMemo;

    public void Init()
    {
        if (!_isInit)
        {
            SceneManager.sceneLoaded += SceneLoaded;
            LoadingUI = GameObject.Find("LoadingUI").GetComponent<LoadingUI>();
            _isInit = true;
        }
    }

    public void ChangeScene(string scene, Action callback = null)
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
            AfterSceneLoadedHandler += callback;
        }
    }

    private void SceneLoaded(Scene scene, LoadSceneMode mode)
    {
        _isLock = false;
        CurrentScene = _tempType;
        LoadingUI.Close();

        if (AfterSceneLoadedHandler != null)
        {
            AfterSceneLoadedHandler();
            AfterSceneLoadedHandler = null;
        }
    }
}
