using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Explore;

public class ExploreUI : MonoBehaviour
{
    public GameObject BigMapBG;
    public RectTransform BigMap;
    public GameObject SpaceLabel;
    public TreasureUI TreasureUI;
    public Text fpsText;
    public Text FloorLabel;
    public Camera BigMapCamera;
    public CanvasGroup CanvasGroup;

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

    private void Awake()
    {
        SpaceLabel.SetActive(false);
    }

    void Update()
    {
        ExploreFile file = ExploreManager.Instance.File;
        if (!InputMamager.Instance.IsLock)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                Treasure treasure = ExploreManager.Instance.GetTreasure();
                if (treasure != null)
                {
                    TreasureUI.Open(treasure.ItemID);
                    InputMamager.Instance.Lock();
                }
            }
        }

        if (Input.GetKeyDown(KeyCode.M))
        {
            BigMapBG.SetActive(!BigMapBG.activeSelf);
            if (BigMapBG.activeSelf)
            {
                float x = (file.Size.x / 2 - Camera.main.transform.position.x) / file.Size.x * 1080 * _scale;
                float y = (file.Size.y / 2 - Camera.main.transform.position.z) / file.Size.y * 1080 * _scale;
                BigMap.anchoredPosition = new Vector2(x, y);
                BigMapCamera.Render();
                FloorLabel.text = file.Floor + "F";
                InputMamager.Instance.Lock();
            }
            else
            {
                InputMamager.Instance.Unlock();
            }
        }

        if (file != null && !InputMamager.Instance.IsLock)
        {
            Vector2Int v2 = Utility.ConvertToVector2Int(Camera.main.transform.position + Camera.main.transform.forward);
            SpaceLabel.SetActive(ExploreManager.Instance.CheckTreasure(v2));
        }

        deltaTime += (Time.deltaTime - deltaTime) * 0.1f;
        float fps = 1.0f / deltaTime;
        fpsText.text = Mathf.Ceil(fps).ToString();
    }
}
