using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
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
    public GameObject FloorGroup;
    public ScrollView FloorScrollView;

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
            for (int i = 1; i <= SystemManager.Instance.Info.MaxFloor; i++)
            {
                floorList.Add(i);
            }
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

    private void FloorOnClick(ScrollItem scrollItem) 
    {
        SceneController.Instance.ChangeScene("Explore", (sceneName) =>
        {
            Explore.ExploreManager.Instance.Init((int)scrollItem.Data);
        });
    }

    private void Update()
    {
        //if (Input.GetKeyDown(KeyCode.S))
        //{
        //    FileSystem.Instance.Save();
        //}
        //if (Input.GetKeyDown(KeyCode.I))
        //{
        //    BagUI bagUI = BagUI.Open();
        //    bagUI.SetNormalState();
        //}
        //if (Input.GetKeyDown(KeyCode.C))
        //{
        //    SelectCharacterUI selectCharacterUI = SelectCharacterUI.Open();
        //}
    }

    private void Awake()
    {
        FloorGroup.SetActive(false);

        ShopButton.onClick.AddListener(ShopOnClick);
        CookButton.onClick.AddListener(CookOnClick);
        ExploreButton.onClick.AddListener(ExploreOnClick);
        FloorScrollView.ClickHandler += FloorOnClick;

        ShopButton.enabled = !FlowController.Instance.Info.LockDic[FlowInfo.LockEnum.Shop];
        ShopButton.GetComponent<ButtonColorSetting>().SetColor(ShopButton.enabled);
        SetCookButton(!FlowController.Instance.Info.LockDic[FlowInfo.LockEnum.Cook]);
    }
}
