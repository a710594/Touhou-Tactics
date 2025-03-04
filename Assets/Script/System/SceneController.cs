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
    private ChangeSceneUI.TypeEnum _changeType;
    private ChangeSceneUI _changeSceneUI;
    //private SceneMemo _sceneMemo;

    public void Init()
    {
        GameObject obj = GameObject.Find("ChangeSceneUI");
        if (!_isInit && obj != null)
        {
            SceneManager.sceneLoaded += SceneLoaded;
            _changeSceneUI = obj.GetComponent<ChangeSceneUI>();
            _isInit = true;
        }
    }

    public void ChangeScene(string scene, ChangeSceneUI.TypeEnum type, Action<string> callback)
    {
        if (!_isLock)
        {
            _isLock = true;
            _tempScene = scene;
            _lastScene = CurrentScene;
            _changeType = type;

            if (BeforeSceneLoadedHandler != null)
            {
                BeforeSceneLoadedHandler();
                BeforeSceneLoadedHandler = null;
            }

            _changeSceneUI.Open(type, () =>
            {
                SceneManager.LoadSceneAsync(scene);
            });

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

        _changeSceneUI.Close(_changeType);

        if (AfterSceneLoadedHandler != null)
        {
            AfterSceneLoadedHandler(scene.name);
            AfterSceneLoadedHandler -= _tempHandler;
        }

        EventManager.Instance.CheckEvent(scene.name, SystemManager.Instance.Info.MaxFloor);
    }
}
