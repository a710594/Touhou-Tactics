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

    public int Lv;
    public int Exp;
    public CharacterInfoGroup CharacterInfoGroup;

    public void Init() 
    {
        CharacterInfoGroup characterInfoGroup = DataContext.Instance.Load<CharacterInfoGroup>(_fileName, DataContext.PrePathEnum.Save);
        if (characterInfoGroup != null)
        {
            CharacterInfoGroup = characterInfoGroup;
            for (int i=0; i<characterInfoGroup.CharacterList.Count; i++) 
            {
                characterInfoGroup.CharacterList[i].Init();
            }
        }
        else
        {
            Lv = 1;
            Exp = 0;
            CharacterInfoGroup = new CharacterInfoGroup();
            CharacterInfoGroup.Lv = Lv;
            CharacterInfoGroup.CharacterList.Add(new CharacterInfo(DataContext.Instance.JobDic[1]));
            CharacterInfoGroup.CharacterList.Add(new CharacterInfo(DataContext.Instance.JobDic[2]));
            CharacterInfoGroup.CharacterList.Add(new CharacterInfo(DataContext.Instance.JobDic[3]));
            CharacterInfoGroup.CharacterList.Add(new CharacterInfo(DataContext.Instance.JobDic[4]));
            CharacterInfoGroup.CharacterList.Add(new CharacterInfo(DataContext.Instance.JobDic[5]));
        }
    }

    public void Save()
    {
        DataContext.Instance.Save(CharacterInfoGroup, _fileName, DataContext.PrePathEnum.Save);
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
            needExp = (int)Mathf.Pow(Lv, 3) - Exp;
            if (addExp >= needExp) 
            {
                addExp -= needExp;
                Lv++;
                Exp = 0;
            }
            else
            {
                Exp += addExp;
                addExp = 0;
            }
        }
        Debug.Log(Lv + " " + Exp);
    }
}
