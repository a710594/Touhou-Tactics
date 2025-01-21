using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Battle;

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
            //Info.CharacterList.Add(new CharacterInfo(DataContext.Instance.JobDic[1]));
            //Info.CharacterList.Add(new CharacterInfo(DataContext.Instance.JobDic[2]));
            //Info.CharacterList.Add(new CharacterInfo(DataContext.Instance.JobDic[3]));
            Info.CharacterList.Add(new CharacterInfo(DataContext.Instance.JobDic[4]));
            //Info.CharacterList.Add(new CharacterInfo(DataContext.Instance.JobDic[5]));
            //Info.CharacterList.Add(new CharacterInfo(DataContext.Instance.JobDic[6]));
            //Info.CharacterList.Add(new CharacterInfo(DataContext.Instance.JobDic[7]));
        }
    }

    public void Save()
    {
        DataContext.Instance.Save(Info, _fileName, DataContext.PrePathEnum.Save);
    }

    public void Delete()
    {
        DataContext.Instance.DeleteData(_fileName, DataContext.PrePathEnum.Save);
    }

    public void AddExp(int addExp) 
    {
        int needExp;
        while (addExp>0) 
        {
            needExp = NeedExp(Info.Lv) - Info.Exp;
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

    public int NeedExp(int lv) 
    {
        if (lv == 1)
        {
            return 2;
        }
        else
        {
            return (int)Mathf.Pow(Info.Lv, 3);
        }
    }

    public void SetLv(int lv) //for debug
    {
        Info.Lv = lv;
        Info.Exp = 0;

        for (int i = 0; i < Info.CharacterList.Count; i++)
        {
            Info.CharacterList[i].SetLv(Info.Lv);
        }
    }

    public void Refresh(List<BattleCharacterController> list) 
    {
        for (int i = 0; i < Info.CharacterList.Count; i++)
        {
            for (int j = 0; j < list.Count; j++)
            {
                if(list[j].Info is BattlePlayerInfo && Info.CharacterList[i].JobId == ((BattlePlayerInfo)list[j].Info).Job.ID) 
                {
                    Info.CharacterList[i].Refresh((BattlePlayerInfo)list[j].Info);
                }
            }
        }
    }

    public void RecoverAllHP() 
    {
        for (int i = 0; i < Info.CharacterList.Count; i++) 
        {
            Info.CharacterList[i].CurrentHP = Info.CharacterList[i].MaxHP;
        }
    }

    public int SurvivalCount() 
    {
        int count = 0;
        for (int i=0; i<Info.CharacterList.Count; i++) 
        {
            if(Info.CharacterList[i].CurrentHP > 0) 
            {
                count++;
            }
        }
        return count;
    }

    public CharacterInfo GetCharacterInfoById(int jobId) 
    {
        for (int i = 0; i < Info.CharacterList.Count; i++)
        {
            if (Info.CharacterList[i].JobId == jobId)
            {
                return Info.CharacterList[i];
            }
        }

        return null;
    }
}
