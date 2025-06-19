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

    public void CheckSceneEvent(string scene, int maxFloor) 
    {
        if (scene == "Camp")
        {
            if (!Info.ReimuJoin && maxFloor == 2)
            {
                ReimuJoinEvent reimuJoinEvent = new ReimuJoinEvent();
                reimuJoinEvent.Start();
                Info.ReimuJoin = true;
            }
            else if (!Info.MarisaJoin && maxFloor == 3)
            {
                MarisaJoinEvent marisaJoinEvent = new MarisaJoinEvent();
                marisaJoinEvent.Start();
                Info.MarisaJoin = true;
            }
        }
        else if (scene == "Explore")
        {
            if (!Info.F2 && maxFloor == 2)
            {
                F2Event f2Event = new F2Event();
                f2Event.Start();
                Info.F2 = true;
            }
            else if (!Info.F3 && maxFloor == 3)
            {
                F3Event f3Event = new F3Event();
                f3Event.Start();
                Info.F3 = true;
            }
        }
    }

    public void CheckCharacterStateEvent(BattleCharacterController character) 
    {
        bool hasEvent = false;

        if (character.Info is BattlePlayerInfo) 
        {
            int jobId = ((BattlePlayerInfo)character.Info).Job.ID;
            
            if (jobId == 1 && !Info.ReimuIntroduction)
            {
                ReimuIntroduction reimuIntroduction = new ReimuIntroduction();
                reimuIntroduction.Start();
                Info.ReimuIntroduction = true;
                hasEvent = true;
            }
            else if (jobId == 2 && !Info.MarisaIntroduction)
            {
                MarisaIntroduction marisaIntroduction = new MarisaIntroduction();
                marisaIntroduction.Start();
                Info.MarisaIntroduction = true;
                hasEvent = true;
            }
            else if (jobId == 7 && !Info.SanaeIntroduction)
            {
                SanaeIntroduction sanaeIntroduction = new SanaeIntroduction();
                sanaeIntroduction.Start();
                Info.SanaeIntroduction = true;
                hasEvent = true;
            }
        }

        if (!hasEvent) 
        {
            List<object> itemList = ItemManager.Instance.GetBattleItemList();
            if(itemList.Count > 0 && !Info.ItemIntroduction) 
            {
                ItemIntroduction itemIntroduction = new ItemIntroduction();
                itemIntroduction.Start();
                Info.ItemIntroduction = true;
                hasEvent = true;
            }
        }
    }
}
