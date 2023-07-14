﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class StateContext
{
    public State CurrentState;

    private Dictionary<Type, State> _stateDic = new Dictionary<Type, State>();

    public void AddState(State state)
    {
        _stateDic.Add(state.GetType(), state);
    }

    public void SetState<T>(object obj = null)
    {
        if (CurrentState != null)
        {
            CurrentState.End();
        }

        if (_stateDic.ContainsKey(typeof(T)))
        {
            CurrentState = _stateDic[typeof(T)];
            CurrentState.Begin(obj);
        }
        else
        {
            //Debug.Log("Context.ChangeState() cannot find the state! Did you add the state you are trying to change to?\n");
        }
    }
}
