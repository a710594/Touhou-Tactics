using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventManager
{
    private readonly string _fileName = "EventInfo";

    private static EventManager _instance;
    public static EventManager Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = new EventManager();
            }
            return _instance;
        }
    }

    public EventInfo Info;

    public void Init(EventInfo info) 
    {
        Info = info;
    }

    public void CheckEvent(string scene, int maxFloor) 
    {
        if (scene == "Camp")
        {
            if (!Info.SanaeJoin && maxFloor == 2)
            {
                SanaeJoinEvent sanaeJoinEvent = new SanaeJoinEvent();
                sanaeJoinEvent.Start();
                EventManager.Instance.Info.SanaeJoin = true;
            }
            else if (!Info.MarisaJoin && maxFloor == 3)
            {
                MarisaJoinEvent marisaJoinEvent = new MarisaJoinEvent();
                marisaJoinEvent.Start();
                EventManager.Instance.Info.MarisaJoin = true;
            }
        }
        else if (scene == "Explore")
        {
            if (!EventManager.Instance.Info.F2 && maxFloor == 2)
            {
                F2Event f2Event = new F2Event();
                f2Event.Start();
                EventManager.Instance.Info.F2 = true;
            }
        }
    }
}
