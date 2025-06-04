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
    public Action IHandler;
    public Action CHandler;
    public Action VHandler;
    public Action LHandler;
    public Action EscapeHandler;

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
        if (Input.GetKeyDown(KeyCode.I))
        {
            if (IHandler != null) 
            {
                IHandler();
            }
        }
        if (Input.GetKeyDown(KeyCode.C))
        {
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
        if (Input.GetKeyDown(KeyCode.Escape))
        {

            if (EscapeHandler != null) 
            {
                EscapeHandler();
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
    }
}
