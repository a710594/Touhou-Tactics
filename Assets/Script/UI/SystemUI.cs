using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SystemUI : BaseUI
{
    public Button CampButton;
    public Button SaveButton;
    public Button ExitButton;

    public Action CampHandler;
    public Action SaveHandler;
    public Action ExitHandler;
    public Action CloseHandler;
    public Action ConfirmCloseHandler;

    public static SystemUI Open()
    {
        GameObject obj = (GameObject)GameObject.Instantiate(Resources.Load("Prefab/UI/SystemUI"), Vector3.zero, Quaternion.identity);
        obj.transform.SetParent(GameObject.Find("Canvas").transform);
        obj.transform.localPosition = Vector3.zero;
        obj.GetComponent<SystemUI>().Init();
        SystemUI systemUI = obj.GetComponent<SystemUI>();
        InputMamager.Instance.CurrentUI = systemUI;

        return systemUI;
    }

    public void Close()
    {
        if (CloseHandler != null)
        {
            CloseHandler();
        }

        Destroy(gameObject);
    }


    public void Init() 
    {
        if (SceneController.Instance.Info.CurrentScene == "Explore")
        {
            Explore.ExploreManager.Instance.UpdateFile();
            CampButton.gameObject.SetActive(true);
        }
        else
        {
            CampButton.gameObject.SetActive(false);
        }
    }

    public override void EscapeOnClick()
    {
        Close();
    }

    private void CampOnClick() 
    {
        if (CampHandler != null) 
        {
            CampHandler();
        }

        ConfirmUI.Open("要返回營地嗎？", "確定", "取消", () =>
        {
            Close();
            OnConfirmClose();
            SceneController.Instance.ChangeScene("Camp", ChangeSceneUI.TypeEnum.Loading, (sceneName)=> 
            {
                Cursor.lockState = CursorLockMode.None;
                CharacterManager.Instance.RecoverAllHP();
                ItemManager.Instance.Info.Key = 0;
            });
        }, OnConfirmClose);
    }

    private void SaveOnClick()
    {
        if (SaveHandler != null)
        {
            SaveHandler();
        }

        ConfirmUI.Open("要存檔嗎？", "確定", "取消", () =>
        {
            SaveManager.Instance.Save();
            OnConfirmClose();
            Close();
        }, OnConfirmClose);
    }

    private void ExitOnClick()
    {
        if (ExitHandler != null) 
        {
            ExitHandler();
        }

        ConfirmUI.Open("要離開遊戲嗎？", "確定", "取消", () =>
        {
            Application.Quit();
        }, OnConfirmClose);
    }

    private void OnConfirmClose() 
    {
        if (ConfirmCloseHandler != null) 
        {
            ConfirmCloseHandler();
        }

        InputMamager.Instance.CurrentUI = this;
    }

    private void Awake()
    {
        CampButton.onClick.AddListener(CampOnClick);
        SaveButton.onClick.AddListener(SaveOnClick);
        ExitButton.onClick.AddListener(ExitOnClick);
    }
}
