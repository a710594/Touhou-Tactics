using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static DataContext;

public class test : MonoBehaviour
{
    public List<RandomFloorModel> RandomFloorList = new List<RandomFloorModel>();
    public Dictionary<int, RandomFloorModel> RandomFloorDic = new Dictionary<int, RandomFloorModel>();

    // Start is called before the first frame update
    void Start()
    {
        RandomFloorModel floorData = new RandomFloorModel();
        floorData.Floor = 2;
        floorData.Width = 30;
        floorData.Height = 30;
        floorData.RoomCount = 30;
        floorData.Room_1 = 1;
        floorData.RoomProbability_1 = 1;
        floorData.EnemyCount = 10;
        floorData.EnemyGroup_1 = 1;
        floorData.EnemyGroupProbability_1 = 1;
        floorData.EnemyGroup_2 = 2;
        floorData.EnemyGroupProbability_2 = 1;
        floorData.EnemyGroup_3 = 3;
        floorData.EnemyGroupProbability_3 = 1;
        floorData.BossEnemyGroup = 4;
        floorData.GetRoomPool();
        floorData.GetEnemyGroupPool();

        RoomModel roomData = new RoomModel();
        roomData.ID = 1;
        roomData.Name = "´¶³q©Ð¶¡";
        roomData.MinWidth = 3;
        roomData.MaxWidth = 7;
        roomData.MinHeight = 3;
        roomData.MaxHeight = 7;
        roomData.TreasureID_1 = 1;
        roomData.Probability_1 = 3;
        roomData.TreasureID_2 = 2;
        roomData.Probability_2 = 3;
        roomData.TreasureID_3 = 4;
        roomData.Probability_3 = 4;
        roomData.MinTreasureCount = 0;
        roomData.MaxTreasureCount = 0;
        DataContext.Instance.RoomList.Add(roomData);
        DataContext.Instance.RoomDic.Clear();
        for (int i = 0; i < DataContext.Instance.RoomList.Count; i++)
        {
            DataContext.Instance.RoomList[i].GetTreasurePool();
            DataContext.Instance.RoomDic.Add(DataContext.Instance.RoomList[i].ID, DataContext.Instance.RoomList[i]);
        }

        Explore.ExploreFile File = ExploreFileRandomGenerator.Instance.Create(floorData);
        Debug.Log(File);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
