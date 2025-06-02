using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Explore;

public class ExploreUI : MonoBehaviour
{
    public GameObject BigMapBG;
    public RectTransform BigMap;
    public GameObject TreasureLabel;
    public TreasureUI TreasureUI;
    public Text fpsText;
    public Text FloorLabel;
    public Text KeyLabel;
    public Camera BigMapCamera;
    public CanvasGroup CanvasGroup;
    public TipLabel TipLabel;

    private float deltaTime;
    private float _scale;

    public void SetCameraPosition(int x, int y, float scale) 
    {
        _scale = scale;
        BigMapCamera.transform.position = new Vector3(x, 5, y);
        BigMap.localScale = new Vector3(scale, scale, 1);
    }

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

    public void ShowTreasureLabel(bool show) 
    {
        TreasureLabel.gameObject.SetActive(show);
    }

    public void ShowDoorLabel(bool show) 
    {
        KeyLabel.gameObject.SetActive(show);
        if (ItemManager.Instance.Info.Key > 0)
        {
            KeyLabel.text = "按空白鍵使用鑰匙開門";
        }
        else
        {
            KeyLabel.text = "需要鑰匙開門";
        }
    }

    public void OpenTreasure(int id) 
    {
        InputMamager.Instance.IsLock = true;
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
                InputMamager.Instance.IsLock = false;
            }
        });
    }

    private void Awake()
    {
        TreasureLabel.SetActive(false);
        KeyLabel.gameObject.SetActive(false);
    }

    void Update()
    {
        ExploreFile file = ExploreManager.Instance.File;

        if (Input.GetKeyDown(KeyCode.M))
        {
            Cursor.lockState = CursorLockMode.None;
            BigMapBG.SetActive(!BigMapBG.activeSelf);
            if (BigMapBG.activeSelf)
            {
                float size = file.Size.x + 1;
                float x = (size / 2 - Camera.main.transform.position.x) / size * 1080 * _scale;
                float y = (size / 2 - Camera.main.transform.position.z) / size * 1080 * _scale;
                BigMap.anchoredPosition = new Vector2(x, y);
                BigMapCamera.Render();
                FloorLabel.text = file.Floor + "F";
                InputMamager.Instance.IsLock = true;
            }
            else
            {
                Cursor.lockState = CursorLockMode.Locked;
                InputMamager.Instance.IsLock = false;
            }
        }

        deltaTime += (Time.deltaTime - deltaTime) * 0.1f;
        float fps = 1.0f / deltaTime;
        fpsText.text = Mathf.Ceil(fps).ToString();
    }
}
