using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using UnityEngine.Networking;

public class LogoUI : MonoBehaviour
{
    public Button StartButton;
    public Button ContinueButton;
    public Button NewGameButton;
    public GameObject LogoGroup;
    public GameObject ButtonGroup;
    public GameObject StartLabel;
    public FileManager FileManager;

    private void StartOnClick() 
    {
        LogoGroup.SetActive(false);
        ButtonGroup.SetActive(true);
    }

    private void ContinueOnClick() 
    {
        if (SceneController.Instance.Info.CurrentScene == "Explore")
        {
            SceneController.Instance.ChangeScene("Explore", ChangeSceneUI.TypeEnum.Loading,(sceneName) =>
            {
                Explore.ExploreManager.Instance.LoadFile();
            });
        }
        else
        {
            SceneController.Instance.ChangeScene("Camp", ChangeSceneUI.TypeEnum.Loading, null);
        }
    }

    private void NewGameOnClick() 
    {
        if (SceneController.Instance.Info.CurrentFloor == 0)
        {
            SceneController.Instance.ChangeScene("Explore", ChangeSceneUI.TypeEnum.Loading, (sceneName) =>
            {
                Explore.ExploreManager.Instance.CreateFile(1);
            });
        }
        else
        {
            ConfirmUI.Open("確定要刪除存檔，重新開始遊戲嗎？", "確定", "取消", () =>
            {
                SaveManager.Instance.Delete();
                SaveManager.Instance.Load(()=> 
                {
                    SceneController.Instance.ChangeScene("Explore", ChangeSceneUI.TypeEnum.Loading, (sceneName) =>
                    {
                        Explore.ExploreManager.Instance.CreateFile(1);
                    });
                });

            }, null);
        }
    }

    private void LoadData() 
    {
        DataTable.Instance.Load(LoadSave);
    }

    private void LoadSave() 
    {
        SaveManager.Instance.Load(()=> 
        {
            if((SceneController.Instance.Info.CurrentFloor == 0)) 
            {
                ContinueButton.gameObject.SetActive(false);
            }

            StartButton.onClick.AddListener(StartOnClick);
            ContinueButton.onClick.AddListener(ContinueOnClick);
            NewGameButton.onClick.AddListener(NewGameOnClick);
            StartLabel.transform.DOScale(Vector3.one * 1.2f, 1).SetLoops(-1, LoopType.Yoyo);
        });
    }

    private void OnDestroy()
    {
        StartLabel.transform.DOKill();
    }

    private void Awake()
    {
        FileManager.Init();
        InputMamager.Instance.Init();
        DataTable.Instance.SetFileManager(FileManager);
        SaveManager.Instance.SetFileManager(FileManager);
        LoadData();
        Cursor.lockState = CursorLockMode.None;
    }
}
