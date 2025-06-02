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

    private Action _callback;

    public static SystemUI Open(Action callback)
    {
        GameObject obj = (GameObject)GameObject.Instantiate(Resources.Load("Prefab/UI/SystemUI"), Vector3.zero, Quaternion.identity);
        obj.transform.SetParent(GameObject.Find("Canvas").transform);
        obj.transform.localPosition = Vector3.zero;
        obj.GetComponent<SystemUI>().Init();
        SystemUI systemUI = obj.GetComponent<SystemUI>();
        systemUI._callback = callback;

        return systemUI;
    }

    public void Close()
    {
        Destroy(gameObject);

        if (_callback != null) 
        {
            _callback();
        }
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

    private void CampOnClick() 
    {
        ConfirmUI.Open("�n��^��a�ܡH", "�T�w", "����", () =>
        {
            Close();
            SceneController.Instance.ChangeScene("Camp", ChangeSceneUI.TypeEnum.Loading, (sceneName)=> 
            {
                Cursor.lockState = CursorLockMode.None;
                CharacterManager.Instance.RecoverAllHP();
                ItemManager.Instance.Info.Key = 0;
            });
        }, null);
    }

    private void SaveOnClick()
    {
        ConfirmUI.Open("�n�s�ɶܡH", "�T�w", "����", () =>
        {
            SaveManager.Instance.Save();
            Close();
        }, null);
    }

    private void ExitOnClick()
    {
        ConfirmUI.Open("�n���}�C���ܡH", "�T�w", "����", () =>
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
