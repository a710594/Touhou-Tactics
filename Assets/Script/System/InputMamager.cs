using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputMamager
{
    private static InputMamager _instance;
    public static InputMamager Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = new InputMamager();
            }
            return _instance;
        }
    }

    public Action UpHandler;
    public Action DownHandler;
    public Action LeftHandler;
    public Action RightHandler;

    public bool IsLock = false;

    private BagUI _bagUI;
    private SelectCharacterUI _selectCharacterUI;

    public void Init() 
    {
        TimerUpdater.UpdateHandler += Update;
    }

    public void Lock()
    {
        IsLock = true;
    }

    public void Unlock()
    {
        IsLock = false;
    }

    private void Update() 
    {
        if(SceneController.Instance.CurrentScene != "Battle") 
        {
            if (!IsLock)
            {
                if (Input.GetKeyDown(KeyCode.S))
                {
                    FileSystem.Instance.Save();
                }
                if (Input.GetKeyDown(KeyCode.I))
                {
                    if (_bagUI == null)
                    {
                        _bagUI = BagUI.Open();
                        _bagUI.SetNormalState();
                        _bagUI.CloseHandler += Unlock;
                        Lock();
                    }
                }
                if (Input.GetKeyDown(KeyCode.C))
                {
                    if (_selectCharacterUI == null)
                    {
                        _selectCharacterUI = SelectCharacterUI.Open();
                        _selectCharacterUI.CloseHandler += Unlock;
                        Lock();
                    }
                }

                //debug
                if (Input.GetKeyDown(KeyCode.T)) 
                {
                    SceneController.Instance.ChangeScene("Explore", () =>
                    {
                        Explore.ExploreManager.Instance.Test();
                    });
                }

                if (Input.GetKey(KeyCode.UpArrow))
                {
                    if(UpHandler != null)
                    {
                        UpHandler();
                    }
                }
                else if (Input.GetKey(KeyCode.DownArrow))
                {
                    if (DownHandler != null)
                    {
                        DownHandler();
                    }
                }
                else if (Input.GetKey(KeyCode.LeftArrow))
                {
                    if (LeftHandler != null)
                    {
                        LeftHandler();
                    }
                }
                else if (Input.GetKey(KeyCode.RightArrow))
                {
                    if (RightHandler != null)
                    {
                        RightHandler();
                    }
                }
            }
            else
            {
                if (Input.GetKeyDown(KeyCode.I))
                {
                    if (_bagUI != null)
                    {
                        _bagUI.Close();
                        _bagUI = null;
                    }
                }
                if (Input.GetKeyDown(KeyCode.C))
                {
                    if (_selectCharacterUI != null)
                    {
                        _selectCharacterUI.Close();
                        _selectCharacterUI = null;
                    }
                }
            }
        }
    }
}