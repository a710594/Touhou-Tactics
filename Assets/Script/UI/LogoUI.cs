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

    private void StartOnClick() 
    {
        LogoGroup.SetActive(false);
        ButtonGroup.SetActive(true);
    }

    private void ContinueOnClick() 
    {
        if (SystemManager.Instance.Info.CurrentScene == "Explore")
        {
            SceneController.Instance.ChangeScene("Explore", ChangeSceneUI.TypeEnum.Loading,(sceneName) =>
            {
                Explore.ExploreManager.Instance.Init();
            });
        }
        else
        {
            SceneController.Instance.ChangeScene("Camp", ChangeSceneUI.TypeEnum.Loading, null);
        }
    }

    private void NewGameOnClick() 
    {
        if (DataContext.Instance.IsSaveEmpty())
        {
            SceneController.Instance.ChangeScene("Explore", ChangeSceneUI.TypeEnum.Loading, (sceneName) =>
            {
                Explore.ExploreManager.Instance.Init();
            });
        }
        else
        {
            ConfirmUI.Open("�T�w�n�R���s�ɡA���s�}�l�C���ܡH", "�T�w", "����", () =>
            {
                FileSystem.Instance.Delete();
                FileSystem.Instance.Init();
                SceneController.Instance.ChangeScene("Explore", ChangeSceneUI.TypeEnum.Loading, (sceneName) =>
                {
                    Explore.ExploreManager.Instance.Init();
                });

            }, null);
        }
    }

    private void OnDestroy()
    {
        StartLabel.transform.DOKill();
    }

    private void Awake()
    {
        FileSystem.Instance.Init();
        //if (DataContext.Instance.IsSaveEmpty())
        //{
        //    ContinueButton.gameObject.SetActive(false);
        //}
        //StartCoroutine(LoadJSON());

        StartButton.onClick.AddListener(StartOnClick);
        ContinueButton.onClick.AddListener(ContinueOnClick);
        NewGameButton.onClick.AddListener(NewGameOnClick);
        StartLabel.transform.DOScale(Vector3.one * 1.2f, 1).SetLoops(-1, LoopType.Yoyo);
    }

    IEnumerator LoadJSON()
    {
        string path = Application.streamingAssetsPath + "/Data/Cook.json";

        // WebGL �ݭn�� UnityWebRequest
        UnityWebRequest request = UnityWebRequest.Get(path);
        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.Success)
        {
            string json = request.downloadHandler.text;
            Debug.Log(json);
        }
        else
        {
            Debug.LogError("�L�kŪ�� JSON�G" + request.error);
        }
    }
}
