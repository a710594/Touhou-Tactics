﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class StateContext
{
    public Action<State> ChangeStateHandler;

    public State CurrentState;
    public object Parent; //產生 StateContext 的物件

    private Dictionary<Type, State> _stateDic = new Dictionary<Type, State>();

    public void AddState(State state)
    {
        _stateDic.Add(state.GetType(), state);
    }

    public void ClearState() 
    {
        _stateDic.Clear();
    }

    public void SetState<T>()
    {
        if (CurrentState != null)
        {
            CurrentState.End();
        }

        if (_stateDic.ContainsKey(typeof(T)))
        {
            CurrentState = _stateDic[typeof(T)];

            if (ChangeStateHandler != null) 
            {
                ChangeStateHandler(CurrentState);
            }
            
            CurrentState.Begin();
        }
        else
        {
            //Debug.Log("Context.ChangeState() cannot find the state! Did you add the state you are trying to change to?\n");
        }
    }
}
