using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameSystem : MonoBehaviour
{
    public Generator2D Generator2D;

    // Start is called before the first frame update
    void Start()
    {
        SystemManager.Instance.Init();
        DataContext.Instance.Init();
        SceneController.Instance.Init();
        ItemManager.Instance.Init();
        CharacterManager.Instance.Init();
        if (SystemManager.Instance.SystemInfo.CurrentScene == "Explore")
        {
            Explore.ExploreManager.Instance.Init();
        }
        else
        {
            SceneController.Instance.ChangeScene("Camp", null);
        }

        //Init Data

        //ItemManager.Instance.AddItem(ItemModel.CategoryEnum.Medicine, 1, 1);
        //ItemManager.Instance.AddItem(ItemModel.CategoryEnum.Card, 1, 1);
        //ItemManager.Instance.AddItem(ItemModel.CategoryEnum.Card, 2, 1);
        //ItemManager.Instance.AddItem(ItemModel.CategoryEnum.Card, 3, 1);
        //ItemManager.Instance.AddItem(ItemModel.CategoryEnum.Card, 4, 1);
        //ItemManager.Instance.AddItem(ItemModel.CategoryEnum.Card, 5, 1);
        //ItemManager.Instance.Save();
        //CharacterManager.Instance.Save();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}