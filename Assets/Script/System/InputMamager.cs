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

    public Action SpaceHandler;
    public Action CHandler;
    public Action VHandler;
    public Action LHandler;

    public BaseUI CurrentUI;

    public void Init() 
    {
        TimerUpdater.UpdateHandler -= Update;
        TimerUpdater.UpdateHandler += Update;
    }

    private void Update() 
    {
        if (Input.GetKeyDown(KeyCode.I) && CurrentUI != null)
        {
            CurrentUI.IOnClick();
        }
        if (Input.GetKeyDown(KeyCode.C) && CurrentUI != null)
        {
            CurrentUI.COnClick();

            if (CHandler != null) 
            {
                CHandler();
            }
        }
        if (Input.GetKeyDown(KeyCode.V)) 
        {
            if (VHandler != null) 
            {
                VHandler();
            }
        }
        if (Input.GetKeyDown(KeyCode.L)) 
        {
            if (LHandler != null) 
            {
                LHandler();
            }
        }
        if (Input.GetKeyDown(KeyCode.Escape) && CurrentUI != null)
        {
            CurrentUI.EscapeOnClick();
        }

        if (Input.GetKeyDown(KeyCode.Space)) 
        {
            if (SpaceHandler != null) 
            {
                SpaceHandler();
            }
        }
    }
}
