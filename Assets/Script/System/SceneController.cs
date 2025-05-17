using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneController
{
    private readonly string _fileName = "SceneInfo";

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

    public Action BeforeSceneLoadedHandler;
    public Action<string> AfterSceneLoadedHandler;

    private Action<string> _tempHandler;

    public SceneInfo Info;

    private bool _isLock = false;
    private bool _isInit = false;
    private string _tempScene;
    private string _lastScene;
    private ChangeSceneUI.TypeEnum _changeType;
    private ChangeSceneUI _changeSceneUI;
    //private SceneMemo _sceneMemo;

    public void Init(SceneInfo info)
    {
        Info = info;
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
            _lastScene = Info.CurrentScene;
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
            Info.CurrentScene = _tempScene;
        }

        _changeSceneUI.Close(_changeType);

        if (AfterSceneLoadedHandler != null)
        {
            AfterSceneLoadedHandler(scene.name);
            AfterSceneLoadedHandler -= _tempHandler;
        }

        EventManager.Instance.CheckSceneEvent(scene.name, Info.MaxFloor);
    }
}
