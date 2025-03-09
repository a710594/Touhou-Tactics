using Battle;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleQuickStarter : MonoBehaviour
{
    public enum ModeEnum 
    {
        Fixed,
        Random,
    }

    public ModeEnum CurrentMode;
    public int EnemyGroupId;
    public string Map;
    public string Tutorial;

    private void LoadData()
    {
        DataTable.Instance.Load(LoadSave);
    }

    private void LoadSave()
    {
        SaveManager.Instance.Load(() =>
        {
            if (CurrentMode == ModeEnum.Fixed)
            {
                BattleController.Instance.Init(Tutorial, Map);
            }
            else
            {
                EnemyGroupModel enemyGroup = DataTable.Instance.EnemyGroupDic[EnemyGroupId];
                BattleController.Instance.Init(Tutorial, enemyGroup);
            }
        });
    }

    void Start()
    {
        LoadData();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
