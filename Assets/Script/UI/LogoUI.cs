using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LogoUI : MonoBehaviour
{
    public Button StartButton;
    public Button ContinueButton;
    public Button NewGameButton;
    public GameObject LogoGroup;
    public GameObject ButtonGroup;

    private void StartOnClick() 
    {
        LogoGroup.SetActive(false);
        ButtonGroup.SetActive(true);
    }

    private void ContinueOnClick() 
    {
        if (SystemManager.Instance.SystemInfo.CurrentScene == "Explore")
        {
            SceneController.Instance.ChangeScene("Explore", () =>
            {
                Explore.ExploreManager.Instance.Init();
            });
        }
        else
        {
            SceneController.Instance.ChangeScene("Camp", null);
        }
    }

    private void NewGameOnClick() 
    {
        if (DataContext.Instance.IsSaveEmpty())
        {
            SceneController.Instance.ChangeScene("Explore", () =>
            {
                Explore.ExploreManager.Instance.Init();
            });
        }
        else
        {
            ConfirmUI.Open("確定要刪除存檔，重新開始遊戲嗎？", "確定", "取消", () =>
            {
                FileSystem.Instance.Delete();
                FileSystem.Instance.Init();
                SceneController.Instance.ChangeScene("Explore", () =>
                {
                    Explore.ExploreManager.Instance.Init();
                });

            }, null);
        }
    }

    private void Awake()
    {
        FileSystem.Instance.Init();
        if (DataContext.Instance.IsSaveEmpty())
        {
            ContinueButton.gameObject.SetActive(false);
        }

        StartButton.onClick.AddListener(StartOnClick);
        ContinueButton.onClick.AddListener(ContinueOnClick);
        NewGameButton.onClick.AddListener(NewGameOnClick);
    }
}
