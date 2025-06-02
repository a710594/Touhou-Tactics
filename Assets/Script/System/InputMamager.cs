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
    public Action SpaceHandler;

    public bool IsLock = false;

    private BagUI _bagUI;
    private CharacterUI _selectCharacterUI;
    private SystemUI _systemUI;

    public void Init() 
    {
        TimerUpdater.UpdateHandler -= Update;
        TimerUpdater.UpdateHandler += Update;
    }

    private void Update() 
    {
        if(SceneController.Instance.Info != null && SceneController.Instance.Info.CurrentScene != "Battle") 
        {
            if (!IsLock)
            {
                if (Input.GetKeyDown(KeyCode.I))
                {
                    if (_bagUI == null)
                    {
                        Cursor.lockState = CursorLockMode.None;
                        IsLock = true;
                        _bagUI = BagUI.Open(()=> 
                        {
                            Cursor.lockState = CursorLockMode.Locked;
                            IsLock = false;
                        });
                        _bagUI.SetNormalState();
                    }
                }
                if (Input.GetKeyDown(KeyCode.C))
                {
                    if (_selectCharacterUI == null)
                    {
                        Cursor.lockState = CursorLockMode.None;
                        IsLock = true;
                        _selectCharacterUI = CharacterUI.Open(()=> 
                        {
                            IsLock = false;
                            if (SceneController.Instance.Info.CurrentScene == "Explore")
                            {
                                Cursor.lockState = CursorLockMode.Locked;
                            }
                        });
                    }
                }
                if (Input.GetKeyDown(KeyCode.Escape))
                {
                    if (_systemUI == null)
                    {
                        IsLock = true;
                        Cursor.lockState = CursorLockMode.None;
                        _systemUI = SystemUI.Open(()=> 
                        {
                            IsLock = false;
                            if (SceneController.Instance.Info.CurrentScene == "Explore")
                            {
                                Cursor.lockState = CursorLockMode.Locked;
                            }
                        });
                    }
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
                else if (Input.GetKeyDown(KeyCode.Space)) 
                {
                    if (SpaceHandler != null) 
                    {
                        SpaceHandler();
                    }
                }

#if UNITY_EDITOR
                if (Input.GetKeyDown(KeyCode.O))
                {
                    Explore.ExploreManager.Instance.OpenAllMap();
                }
#endif
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
                if (Input.GetKeyDown(KeyCode.Escape))
                {
                    if (_systemUI != null)
                    {
                        _systemUI.Close();
                        _systemUI = null;
                        IsLock = false;
                    }
                }
            }
        }
        else
        {
//#if UNITY_EDITOR
            if (Input.GetKeyDown(KeyCode.V))
            {
                Battle.BattleController.Instance.EndTutorial();
                Battle.BattleController.Instance.SetWin();
            }
            if (Input.GetKeyDown(KeyCode.L))
            {
                Battle.BattleController.Instance.EndTutorial();
                Battle.BattleController.Instance.SetLose();
            }
//#endif
        }
    }
}
