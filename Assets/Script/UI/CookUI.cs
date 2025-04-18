using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CookUI : MonoBehaviour
{
    public ScrollView BagScrollView;
    public Button CookButton;
    public Button CloseButton;
    public TipLabel TipLabel;
    public Text ResultNameLabel;
    public Text ResultCommentLabel;
    public ButtonPlus[] MaterialButton;

    private Food _food = null;
    private List<ItemModel> _materialList = new List<ItemModel>();

    public void Open()
    {
        gameObject.SetActive(true);
        BagScrollView.SetData(new List<object>(ItemManager.Instance.Info.GetFoodMaterial()));
        _materialList.Clear();
        SetMaterialLabel();
        ResultNameLabel.text = "";
        ResultCommentLabel.text = "";
    }

    private void SetMaterialLabel() 
    {
        for (int i = 0; i < 5; i++)
        {
            if (i < _materialList.Count)
            {
                MaterialButton[i].Label.text = _materialList[i].Name;
                MaterialButton[i].Data = _materialList[i];
            }
            else
            {
                MaterialButton[i].Label.text = "";
                MaterialButton[i].Data = null;
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
        for (int i=0; i< DataTable.Instance.CookList.Count; i++) 
        {
            check = true;
            for (int j=0; j< DataTable.Instance.CookList[i].MaterialList.Count; j++) 
            {
                if (!materialIdList.Contains(DataTable.Instance.CookList[i].MaterialList[j])) 
                {
                    check = false;
                    break;
                }
            }
            if (check) 
            {
                cookList.Add(DataTable.Instance.CookList[i]);
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

        if (cook != null)
        {
            _food = new Food(DataTable.Instance.ItemDic[cook.Result], DataTable.Instance.FoodResultDic[cook.Result], materialIdList);
            ResultNameLabel.text = _food.Name;
            ResultCommentLabel.text = _food.Comment;
            CookButton.gameObject.SetActive(true);
        }
        else
        {
            ResultNameLabel.text = "";
            ResultCommentLabel.text = "";
            CookButton.gameObject.SetActive(false);
        }
    }

    private void ScrollItemOnClick(PointerEventData eventData, ButtonPlus button)
    {
        Item item = (Item)button.Data;
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

    private void MaterialOnClick(PointerEventData eventData, ButtonPlus button) 
    {
        if (button != null) 
        {
            _materialList.Remove((ItemModel)button.Data);
            SetMaterialLabel();
            SetResult();
        }
    }

    private void CookOnClick() 
    {
        if (_food != null) 
        {
            ItemManager.Instance.AddFood(_food);
            for (int i=0; i<_materialList.Count; i++) 
            {
                ItemManager.Instance.MinusItem(_materialList[i].ID, 1);
            }
            TipLabel.SetLabel("獲得 " + _food.Name, true);
            _food = null;
            _materialList.Clear();
            SetMaterialLabel();
            BagScrollView.SetData(new List<object>(ItemManager.Instance.Info.GetFoodMaterial()));
            ResultNameLabel.text = "";
            ResultCommentLabel.text = "";
            CookButton.gameObject.SetActive(false);
        }
    }

    private void CloseOnClick()
    {
        gameObject.SetActive(false);
    }

    private void Awake()
    {
        CookButton.gameObject.SetActive(false);
        CookButton.onClick.AddListener(CookOnClick);
        CloseButton.onClick.AddListener(CloseOnClick);
        BagScrollView.ClickHandler += ScrollItemOnClick;
        for (int i=0; i<MaterialButton.Length; i++) 
        {
            MaterialButton[i].ClickHandler += MaterialOnClick;
        }
    }
}
