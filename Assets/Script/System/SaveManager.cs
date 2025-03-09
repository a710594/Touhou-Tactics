using Explore;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveManager
{
    private static readonly int _fileCount = 4;

    private static SaveManager _instance;
    public static SaveManager Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = new SaveManager();
            }
            return _instance;
        }
    }

    private int _count = 0;
    private Action _callback;
    private FileLoader _fileLoader;
    private BagInfo _baginfo;
    private CharacterGroupInfo _characterGroupInfo;
    private EventInfo _eventInfo;
    private SceneInfo _sceneInfo;
    private ExploreFile _exploreFile;

    public void Load(Action callback) 
    {
        _count = 0;
        _callback = callback;
        _fileLoader = GameObject.Find("FileLoader").GetComponent<FileLoader>();

        _fileLoader.Load<BagInfo>("BagInfo", FileLoader.PathEnum.Save, (obj) =>
        {
            _baginfo = (BagInfo)obj;
            if (_baginfo != null)
            {
                foreach (KeyValuePair<int, Consumables> pair in _baginfo.ConsumablesDic)
                {
                    pair.Value.Init();
                }

                foreach (KeyValuePair<int, Item> pair in _baginfo.ItemDic)
                {
                    pair.Value.Init();
                }

                for (int i = 0; i < _baginfo.FoodList.Count; i++)
                {
                    _baginfo.FoodList[i].Init();
                }
            }
            else
            {
                _baginfo = new BagInfo();
            }
            ItemManager.Instance.Init(_baginfo);
            CheckComplete();
        });

        _fileLoader.Load<CharacterGroupInfo>("CharacterGroupInfo", FileLoader.PathEnum.Save, (obj) =>
        {
            _characterGroupInfo = (CharacterGroupInfo)obj;
            if (_characterGroupInfo != null)
            {
                for (int i = 0; i < _characterGroupInfo.CharacterList.Count; i++)
                {
                    _characterGroupInfo.CharacterList[i].Init();
                }
            }
            else
            {
                _characterGroupInfo = new CharacterGroupInfo();
                _characterGroupInfo.Lv = 1;
                _characterGroupInfo.Exp = 0;
                //Info.CharacterList.Add(new CharacterInfo(DataContext.Instance.JobDic[1]));
                //Info.CharacterList.Add(new CharacterInfo(DataContext.Instance.JobDic[2]));
                //Info.CharacterList.Add(new CharacterInfo(DataContext.Instance.JobDic[3]));
                _characterGroupInfo.CharacterList.Add(new CharacterInfo(DataTable.Instance.JobDic[4]));
                //Info.CharacterList.Add(new CharacterInfo(DataContext.Instance.JobDic[5]));
                //Info.CharacterList.Add(new CharacterInfo(DataContext.Instance.JobDic[6]));
                //Info.CharacterList.Add(new CharacterInfo(DataContext.Instance.JobDic[7]));
            }
            CharacterManager.Instance.Init(_characterGroupInfo);
            CheckComplete();
        });

        _fileLoader.Load<EventInfo>("EventInfo", FileLoader.PathEnum.Save, (obj) =>
        {
            _eventInfo = (EventInfo)obj;
            if (_eventInfo == null)
            {
                _eventInfo = new EventInfo();
            }
            EventManager.Instance.Init(_eventInfo);
            CheckComplete();
        });

        _fileLoader.Load<SceneInfo>("SceneInfo", FileLoader.PathEnum.Save, (obj) =>
        {
            _sceneInfo = (SceneInfo)obj;
            if (_sceneInfo == null)
            {
                _sceneInfo = new SceneInfo();
            }
            SceneController.Instance.Init(_sceneInfo);
            CheckComplete();
        });
    }

    public void CreateExploreFile(int floor, Action<ExploreFile> callback) 
    {
        SceneController.Instance.Info.CurrentFloor = floor;
        if (DataTable.Instance.FixedFloorDic.ContainsKey(floor))
        {
            FixedFloorModel data = DataTable.Instance.FixedFloorDic[floor];
            _fileLoader.Load<ExploreFile>(data.Name, FileLoader.PathEnum.MapExplore, (obj) =>
            {
                _exploreFile = (ExploreFile)obj;
                callback(_exploreFile);
            });
        }
        else
        {
            RandomFloorModel data = DataTable.Instance.RandomFloorDic[floor];
            _exploreFile = ExploreFileRandomGenerator.Instance.Create(data);
            callback(_exploreFile);
        }
    }

    public void LoadExploreFile(Action<ExploreFile> callback) 
    {
        _fileLoader.Load<ExploreFile>("ExploreFile", FileLoader.PathEnum.Save, (obj) =>
        {
            _exploreFile = (ExploreFile)obj;
            callback(_exploreFile);
        });
    }

    public void Save() 
    {
        _fileLoader.Save(_baginfo, "BagInfo", FileLoader.PathEnum.Save);
        _fileLoader.Save(_characterGroupInfo, "CharacterGroupInfo", FileLoader.PathEnum.Save);
        _fileLoader.Save(_eventInfo, "EventInfo", FileLoader.PathEnum.Save);
        _fileLoader.Save(_sceneInfo, "SceneInfo", FileLoader.PathEnum.Save);
        _fileLoader.Save(_exploreFile, "ExploreFile", FileLoader.PathEnum.Save);
    }

    public void Delete() 
    {
        _fileLoader.Delete("BagInfo", FileLoader.PathEnum.Save);
        _fileLoader.Delete("CharacterGroupInfo", FileLoader.PathEnum.Save);
        _fileLoader.Delete("EventInfo", FileLoader.PathEnum.Save);
        _fileLoader.Delete("SceneInfo", FileLoader.PathEnum.Save);
        _fileLoader.Delete("ExploreFile", FileLoader.PathEnum.Save);
    }

    private void CheckComplete()
    {
        _count++;
        if (_count == _fileCount)
        {
            _callback();
        }
    }
}
