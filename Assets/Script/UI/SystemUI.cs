using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SystemUI : MonoBehaviour
{
    public Button CampButton;
    public Button SaveButton;
    public Button ExitButton;

    public static SystemUI Open()
    {
        GameObject obj = (GameObject)GameObject.Instantiate(Resources.Load("Prefab/UI/SystemUI"), Vector3.zero, Quaternion.identity);
        obj.transform.SetParent(GameObject.Find("Canvas").transform);
        obj.transform.localPosition = Vector3.zero;
        obj.GetComponent<SystemUI>().Init();

        return obj.GetComponent<SystemUI>();
    }

    public void Close()
    {
        Destroy(gameObject);
    }


    public void Init() 
    {
        CampButton.gameObject.SetActive(SceneController.Instance.CurrentScene == "Explore");
    }

    private void CampOnClick() 
    {
        ConfirmUI.Open("要返回營地嗎？", "確定", "取消", () =>
        {
            if (FlowController.Instance.Info.CurrentStep == FlowInfo.StepEnum.BackCamp)
            {
                FlowController.Instance.Info.CurrentStep++;
                FlowController.Instance.Info.LockDic[FlowInfo.LockEnum.BackCamp] = false;
            }

            SceneController.Instance.ChangeScene("Camp", (sceneName)=> 
            {
                CharacterManager.Instance.RecoverAllHP();
                InputMamager.Instance.Unlock();
            });
        }, null);
    }

    private void SaveOnClick()
    {
        ConfirmUI.Open("要存檔嗎？", "確定", "取消", () =>
        {
            FileSystem.Instance.Save();
            InputMamager.Instance.Unlock();
            Destroy(gameObject);
        }, null);
    }

    private void ExitOnClick()
    {
        ConfirmUI.Open("要離開遊戲嗎？", "確定", "取消", () =>
        {
            Application.Quit();
        }, null);
    }

    private void Awake()
    {
        CampButton.onClick.AddListener(CampOnClick);
        SaveButton.onClick.AddListener(SaveOnClick);
        ExitButton.onClick.AddListener(ExitOnClick);
    }
}
