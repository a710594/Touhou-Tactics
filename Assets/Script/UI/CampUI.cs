using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CampUI : MonoBehaviour
{
    public Action ShopHandler;
    public Action CookHandler;
    public Action ExploreHandler;

    public Button ShopButton;
    public Button CookButton;
    public Button ExploreButton;
    public ShopUI ShopUI;
    public CookUI CookUI;
    public BagUI BagUI;
    public GameObject MainGroup;
    public GameObject FloorGroup;
    public ScrollView FloorScrollView;

    private SystemUI _systemUI;
    private BagUI _bagUI;
    private CharacterUI _selectCharacterUI;

    public void SetCookButton(bool enable) 
    {
        CookButton.enabled = enable;
        CookButton.GetComponent<ButtonColorSetting>().SetColor(enable);
    }

    private void ShopOnClick() 
    {
        ShopUI.Open();

        if (ShopHandler != null) 
        {
            ShopHandler();
        }
    }

    private void CookOnClick() 
    {
        CookUI.Open();

        if(CookHandler != null) 
        {
            CookHandler();
        }
    }

    private void ExploreOnClick()
    {
        if (!FloorGroup.activeSelf)
        {
            FloorGroup.SetActive(true);
            List<object> floorList = new List<object>();
            for (int i = 1; i <= SceneController.Instance.Info.MaxFloor; i++)
            {
                floorList.Add(i);
            }
            floorList.Remove(1);
            FloorScrollView.SetData(floorList);

            if (ExploreHandler != null) 
            {
                ExploreHandler();
            }
        }
        else
        {
            FloorGroup.SetActive(false);
        }
    }

    private void FloorOnClick(PointerEventData eventData, ButtonPlus buttonPlus) 
    {
        int floor = (int)buttonPlus.Data;
        if (floor < 4)
        {
            SceneController.Instance.ChangeScene("Explore", ChangeSceneUI.TypeEnum.Loading, (sceneName) =>
            {
                Explore.ExploreManager.Instance.CreateFile(floor);
            });
        }
        else
        {
            ConfirmUI.Open("·q½Ð´Á«Ý¡I", "½T©w", null);
        }
    }

    private void EscapeOnCLikc()
    {
        if (_systemUI == null)
        {
            InputMamager.Instance.IsLock = true;
            Cursor.lockState = CursorLockMode.None;
            _systemUI = SystemUI.Open(() =>
            {
                InputMamager.Instance.IsLock = false;
                if (SceneController.Instance.Info.CurrentScene == "Explore")
                {
                    Cursor.lockState = CursorLockMode.Locked;
                }
            });
            _systemUI.CampHandler += DeregisterEscapeHandler;
            _systemUI.SaveHandler += DeregisterEscapeHandler;
            _systemUI.ExitHandler += DeregisterEscapeHandler;
            _systemUI.ConfirmCloseHandler += RegisterEscapeHandler;
        }
        else
        {
            _systemUI.Close();
        }
    }

    private void RegisterEscapeHandler()
    {
        InputMamager.Instance.EscapeHandler += EscapeOnCLikc;
    }

    private void DeregisterEscapeHandler()
    {
        InputMamager.Instance.EscapeHandler -= EscapeOnCLikc;
    }

    private void IOnClclick()
    {
        if (_bagUI == null)
        {
            Cursor.lockState = CursorLockMode.None;
            InputMamager.Instance.IsLock = true;
            _bagUI = BagUI.Open(() =>
            {
                InputMamager.Instance.IsLock = false;
            });
            _bagUI.SetNormalState();
        }
        else
        {
            _bagUI.Close();
        }
    }

    private void COnClick()
    {
        if (_selectCharacterUI == null)
        {
            Cursor.lockState = CursorLockMode.None;
            InputMamager.Instance.IsLock = true;
            _selectCharacterUI = CharacterUI.Open(() =>
            {
                InputMamager.Instance.IsLock = false;
            });
        }
        else
        {
            _selectCharacterUI.Close();
        }
    }

    private void Awake()
    {
        FloorGroup.SetActive(false);
        CookButton.gameObject.SetActive(EventManager.Instance.Info.UnlockCook);
        ShopButton.gameObject.SetActive(EventManager.Instance.Info.UnlockShop);

        ShopButton.onClick.AddListener(ShopOnClick);
        CookButton.onClick.AddListener(CookOnClick);
        ExploreButton.onClick.AddListener(ExploreOnClick);
        FloorScrollView.ClickHandler += FloorOnClick;
        InputMamager.Instance.IHandler += IOnClclick;
        InputMamager.Instance.CHandler += COnClick;
        RegisterEscapeHandler();
    }

    private void OnDestroy()
    {
        InputMamager.Instance.IHandler -= IOnClclick;
        InputMamager.Instance.CHandler -= COnClick;
        DeregisterEscapeHandler();
    }
}
