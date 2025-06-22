using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Explore;

public class ExploreUI : BaseUI
{
    public TreasureUI TreasureUI;
    public Text fpsText;
    public Text FloorLabel;
    public Text ObjectInfoLabel;
    public CanvasGroup CanvasGroup;
    public TipLabel TipLabel;
    public Image MapMask;
    public MapUI MapUI;

    private bool _showBigMap = false;
    private float deltaTime;
    private SystemUI _systemUI;
    private BagUI _bagUI;
    private CharacterUI _selectCharacterUI;

    public void SetVisible(bool isVisible) 
    {
        if (isVisible) 
        {
            CanvasGroup.alpha = 1;
        }
        else
        {
            CanvasGroup.alpha = 0;
        }
    }

    public void ShowObjectInfoLabel(string text) 
    {
        ObjectInfoLabel.gameObject.SetActive(true);
        ObjectInfoLabel.text = text;
        //if (ItemManager.Instance.Info.Key > 0)
        //{
        //    KeyLabel.text = "按空白鍵使用鑰匙開門";
        //}
        //else
        //{
        //    KeyLabel.text = "需要鑰匙開門";
        //}
    }

    public void HideObjectInfoLabel() 
    {
        ObjectInfoLabel.gameObject.SetActive(false);
    }

    public void OpenTreasure(int id) 
    {
        Explore.ExploreManager.Instance.Player.Enable = false;
        ItemModel data = DataTable.Instance.ItemDic[id];
        TreasureUI.Open(data, ()=> 
        {
            if(data.Category == ItemModel.CategoryEnum.Equip && !EventManager.Instance.Info.EquipIntroduction) 
            {
                EquipIntroduction equipIntroduction = new EquipIntroduction();
                equipIntroduction.Start(data);
                EventManager.Instance.Info.EquipIntroduction = true;
            }
            else
            {
                Explore.ExploreManager.Instance.Player.Enable = true;
            }
        });
    }

    public void InitMap(int width, int height) 
    {
        MapUI.InitMap(width, height);
    }

    public void SetMap(Vector2Int position, Color color) 
    {
        MapUI.SetMap(position, color);
    }

    public void SetPlayerPosition(Vector2 position) 
    {
        MapUI.SetPlayerPosition(position);
    }

    public void SetIcon(Vector2Int position, string name)
    {
        MapUI.SetIcon(position, name);
    }

    public void ClearIcon(Vector2Int position)
    {
        MapUI.ClearIcon(position);
    }

    public void ShowEnemy(Vector3 position, ExploreEnemyController enemy) 
    {
        MapUI.ShowEnemy(position, enemy);
    }

    public void HideEnemy(ExploreEnemyController enemy) 
    {
        MapUI.HideEnemy(enemy);
    }

    public override void EscapeOnClick()
    {
        Cursor.lockState = CursorLockMode.None;
        ExploreManager.Instance.Player.Enable = false;
        _systemUI = SystemUI.Open();
        _systemUI.CloseHandler = CursorLockModeLock;
        _systemUI.CloseHandler = () =>
        {
            InputMamager.Instance.CurrentUI = this;
            ExploreManager.Instance.Player.Enable = true;
        };
    }

    public override void IOnClick()
    {
        Cursor.lockState = CursorLockMode.None;
        ExploreManager.Instance.Player.Enable = false;
        _bagUI = BagUI.Open();
        _bagUI.SetNormalState();
        _bagUI.CloseHandler = CursorLockModeLock;
        _bagUI.CloseHandler = () =>
        {
            InputMamager.Instance.CurrentUI = this;
            ExploreManager.Instance.Player.Enable = true;
        };
    }

    public override void COnClick() 
    {
        Cursor.lockState = CursorLockMode.None;
        ExploreManager.Instance.Player.Enable = false;
        _selectCharacterUI = CharacterUI.Open();
        _selectCharacterUI.CloseHandler = CursorLockModeLock;
        _selectCharacterUI.CloseHandler = () =>
        {
            InputMamager.Instance.CurrentUI = this;
            ExploreManager.Instance.Player.Enable = true;
        };
    }

    private void CursorLockModeLock() 
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

    private void Awake()
    {
        ObjectInfoLabel.gameObject.SetActive(false);
        InputMamager.Instance.CurrentUI = this;
    }

    void Update()
    {
        ExploreFile file = ExploreManager.Instance.File;

        if (Input.GetKeyDown(KeyCode.M))
        {
            if (!_showBigMap)
            {
                MapUI.ShowBigMap();
                _showBigMap = true;
            }
            else
            {
                MapUI.HideBigMap();
                _showBigMap = false;
            }
        }

        deltaTime += (Time.deltaTime - deltaTime) * 0.1f;
        float fps = 1.0f / deltaTime;
        fpsText.text = Mathf.Ceil(fps).ToString();
    }
}
