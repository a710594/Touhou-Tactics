using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterManager
{
    private readonly string _fileName = "CharacterInfoGroup";

    private static CharacterManager _instance;
    public static CharacterManager Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = new CharacterManager();
            }
            return _instance;
        }
    }

    public CharacterInfoGroup Info;

    public void Init() 
    {
        CharacterInfoGroup characterInfoGroup = DataContext.Instance.Load<CharacterInfoGroup>(_fileName, DataContext.PrePathEnum.Save);
        if (characterInfoGroup != null)
        {
            Info = characterInfoGroup;
            for (int i=0; i<characterInfoGroup.CharacterList.Count; i++) 
            {
                characterInfoGroup.CharacterList[i].Init();
            }
        }
        else
        {
            Info = new CharacterInfoGroup();
            Info.Lv = 1;
            Info.Exp = 0;
            Info.CharacterList.Add(new CharacterInfo(DataContext.Instance.JobDic[1]));
            Info.CharacterList.Add(new CharacterInfo(DataContext.Instance.JobDic[2]));
            Info.CharacterList.Add(new CharacterInfo(DataContext.Instance.JobDic[3]));
            Info.CharacterList.Add(new CharacterInfo(DataContext.Instance.JobDic[4]));
            Info.CharacterList.Add(new CharacterInfo(DataContext.Instance.JobDic[5]));
        }
    }

    public void Save()
    {
        DataContext.Instance.Save(Info, _fileName, DataContext.PrePathEnum.Save);
    }

    public void Delete()
    {
        DataContext.Instance.DeleteData(_fileName);
    }

    public void AddExp(int addExp) 
    {
        int needExp;
        while (addExp>0) 
        {
            needExp = (int)Mathf.Pow(Info.Lv, 3) - Info.Exp;
            if (addExp >= needExp) 
            {
                addExp -= needExp;
                Info.Lv++;
                Info.Exp = 0;
            }
            else
            {
                Info.Exp += addExp;
                addExp = 0;
            }
        }

        for (int i=0; i<Info.CharacterList.Count; i++) 
        {
            Info.CharacterList[i].SetLv(Info.Lv);
        }
    }

    public void Refresh(List<BattleCharacterInfo> list) 
    {
        for (int i = 0; i < Info.CharacterList.Count; i++)
        {
            for (int j = 0; j < list.Count; j++)
            {
                if(Info.CharacterList[i].JobId == list[j].JobId) 
                {
                    Info.CharacterList[i].Refresh(list[j]);
                }
            }
        }
    }
}
