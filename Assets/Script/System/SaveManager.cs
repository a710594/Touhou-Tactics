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
    private FileManager _fileManager;
    private BagInfo _baginfo;
    private CharacterGroupInfo _characterGroupInfo;
    private EventInfo _eventInfo;
    private SceneInfo _sceneInfo;
    private ExploreFile _exploreFile;

    public void SetFileManager(FileManager fileManager)
    {
        _fileManager = fileManager;
    }

    public void Load(Action callback) 
    {
        _count = 0;
        _callback = callback;

        _fileManager.Load<BagInfo>("BagInfo", FileManager.PathEnum.Save, (Action<object>)((obj) =>
        {
            _baginfo = (BagInfo)obj;
            if (_baginfo != null)
            {
                foreach (KeyValuePair<int, Item> pair in _baginfo.ConsumablesDic)
                {
                    pair.Value.Init();
                }

                foreach (KeyValuePair<int, Item> pair in _baginfo.ItemDic)
                {
                    pair.Value.Init();
                }
            }
            else
            {
                _baginfo = new BagInfo();
            }
            ItemManager.Instance.Init(_baginfo);
            CheckComplete();
        }));

        _fileManager.Load<CharacterGroupInfo>("CharacterGroupInfo", FileManager.PathEnum.Save, (obj) =>
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

        _fileManager.Load<EventInfo>("EventInfo", FileManager.PathEnum.Save, (obj) =>
        {
            _eventInfo = (EventInfo)obj;
            if (_eventInfo == null)
            {
                _eventInfo = new EventInfo();
            }
            EventManager.Instance.Init(_eventInfo);
            CheckComplete();
        });

        _fileManager.Load<SceneInfo>("SceneInfo", FileManager.PathEnum.Save, (obj) =>
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
            _fileManager.Load<ExploreFile>(data.Name, FileManager.PathEnum.MapExplore, (obj) =>
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
        _fileManager.Load<ExploreFile>("ExploreFile", FileManager.PathEnum.Save, (obj) =>
        {
            _exploreFile = (ExploreFile)obj;
            callback(_exploreFile);
        });
    }

    public void Save() 
    {
        _fileManager.Save(_baginfo, "BagInfo", FileManager.PathEnum.Save);
        _fileManager.Save(_characterGroupInfo, "CharacterGroupInfo", FileManager.PathEnum.Save);
        _fileManager.Save(_eventInfo, "EventInfo", FileManager.PathEnum.Save);
        _fileManager.Save(_sceneInfo, "SceneInfo", FileManager.PathEnum.Save);
        _fileManager.Save(_exploreFile, "ExploreFile", FileManager.PathEnum.Save);
    }

    public void Delete() 
    {
        _fileManager.Delete("BagInfo", FileManager.PathEnum.Save);
        _fileManager.Delete("CharacterGroupInfo", FileManager.PathEnum.Save);
        _fileManager.Delete("EventInfo", FileManager.PathEnum.Save);
        _fileManager.Delete("SceneInfo", FileManager.PathEnum.Save);
        _fileManager.Delete("ExploreFile", FileManager.PathEnum.Save);
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
