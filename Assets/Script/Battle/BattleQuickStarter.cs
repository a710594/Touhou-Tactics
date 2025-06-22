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
    public FileManager FileManager;

    private void LoadSave()
    {
        SaveManager.Instance.Load(() =>
        {
            SceneController.Instance.Info.CurrentScene = "Battle";

            if (CurrentMode == ModeEnum.Fixed)
            {
                BattleController.Instance.Init();
                BattleController.Instance.SetFixed(Tutorial, Map);
            }
            else
            {
                EnemyGroupModel enemyGroup = DataTable.Instance.EnemyGroupDic[EnemyGroupId];
                BattleController.Instance.Init();
                BattleController.Instance.SetRandom("", enemyGroup);
            }
        });
    }

    private void Awake()
    {
        FileManager.Init();
        InputMamager.Instance.Init();
        DataTable.Instance.SetFileManager(FileManager);
        SaveManager.Instance.SetFileManager(FileManager);
        DataTable.Instance.Load(LoadSave);
    }
}
