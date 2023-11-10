using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CookUI : MonoBehaviour
{
    public ScrollView BagScrollView;
    public Button CookButton;
    public Button CloseButton;
    public TipLabel TipLabel;
    public Text ResultNameLabel;
    public Text ResultCommentLabel;
    public Text[] MaterialLabel;

    private Food _food = null;
    private List<ItemModel> _materialList = new List<ItemModel>();

    public void Open()
    {
        gameObject.SetActive(true);
        BagScrollView.SetData(new List<object>(ItemManager.Instance.BagInfo.FoodList));
        _materialList.Clear();
        SetMaterialLabel();
        ResultNameLabel.text = "";
    }

    private void SetMaterialLabel() 
    {
        for (int i = 0; i < 5; i++)
        {
            if (i < _materialList.Count)
            {
                MaterialLabel[i].text = _materialList[i].Name;
            }
            else
            {
                MaterialLabel[i].text = "";
            }
        }
    }

    private void SetResult() 
    {
        List<int> materialIdList = new List<int>();
        for (int i=0; i<_materialList.Count; i++) 
        {
            materialIdList.Add(_materialList[i].ID);
        }

        bool check;
        List<CookModel> cookList = new List<CookModel>();
        for (int i=0; i<DataContext.Instance.CookList.Count; i++) 
        {
            check = true;
            for (int j=0; j< DataContext.Instance.CookList[i].MaterialList.Count; j++) 
            {
                if (!materialIdList.Contains(DataContext.Instance.CookList[i].MaterialList[j])) 
                {
                    check = false;
                    break;
                }
            }
            if (check) 
            {
                cookList.Add(DataContext.Instance.CookList[i]);
            }
        }

        int score;
        int maxScore = -1;
        CookModel cook = null;
        for (int i=0; i<cookList.Count; i++) 
        {
            score = cookList[i].MaterialList.Count;
            if (score > maxScore) 
            {
                cook = cookList[i];
                maxScore = score;
            }
        }

        //檢查有沒有超出料理需求的素材,有的話就會有 addValue
        for (int i=0; i<cook.MaterialList.Count; i++) 
        {
            materialIdList.Remove(cook.MaterialList[i]);
        }

        List<CookAddModel> addList = new List<CookAddModel>();
        for (int i=0; i<materialIdList.Count; i++) 
        {
            addList.Add(DataContext.Instance.CookAddDic[materialIdList[i]]);
        }

        _food = new Food(DataContext.Instance.ItemDic[cook.Result], DataContext.Instance.FoodDic[cook.Result], addList);
        ResultNameLabel.text = _food.Name;
        ResultCommentLabel.text = _food.Comment;
    }

    private void ScrollItemOnClick(object obj, ScrollItem scrollItem)
    {
        Item item = (Item)obj;
        if (_materialList.Count < 5) 
        {
            _materialList.Add(item.Data);
            SetMaterialLabel();
            SetResult();
        }
        else
        {
            TipLabel.SetLabel("最多只能放五個材料");
        }
    }

    private void CookOnClick() 
    {
        if (_food != null) 
        {
        
        }
    }

    private void CloseOnClick()
    {
        gameObject.SetActive(false);
    }

    private void Awake()
    {
        CloseButton.onClick.AddListener(CloseOnClick);
        BagScrollView.ItemOnClickHandler += ScrollItemOnClick;
    }
}
