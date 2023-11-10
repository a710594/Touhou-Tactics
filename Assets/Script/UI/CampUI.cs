using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CampUI : MonoBehaviour
{
    public Button ShopButton;
    public Button CookButton;
    public Button ExploreButton;
    public ShopUI ShopUI;
    public CookUI CookUI;
    public BagUI BagUI;
    public GameObject FloorGroup;
    public ScrollView FloorScrollView;

    private void ShopOnClick() 
    {
        ShopUI.Open();
    }

    private void CookOnClick() 
    {
        CookUI.Open();
    }

    private void ExploreOnClick() 
    {
        FloorGroup.SetActive(true);
        List<object> floorList = new List<object>();
        for (int i=1; i<=SystemManager.Instance.SystemInfo.MaxFloor; i++) 
        {
            floorList.Add(i);
        }
        FloorScrollView.SetData(floorList);
    }

    private void FloorOnClick(object data, ScrollItem item) 
    {
        SceneController.Instance.ChangeScene("Explore", () =>
        {
            Generator2D generator2D = (GameObject.Find("Generator2D")).GetComponent<Generator2D>();
            generator2D.Generate((int)data);
        });
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.S))
        {
            FileSystem.Instance.Save();
        }
        if (Input.GetKeyDown(KeyCode.I))
        {
            BagUI.Open();
        }
    }

    private void Awake()
    {
        FloorGroup.SetActive(false);

        ShopButton.onClick.AddListener(ShopOnClick);
        CookButton.onClick.AddListener(CookOnClick);
        ExploreButton.onClick.AddListener(ExploreOnClick);
        FloorScrollView.ItemOnClickHandler += FloorOnClick;
    }
}
