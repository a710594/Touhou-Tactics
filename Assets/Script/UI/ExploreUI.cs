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

    private float deltaTime;
    private float _scale;

    public void SetCameraPosition(int x, int y, float scale) 
    {
        _scale = scale;
        BigMapCamera.transform.position = new Vector3(x, 5, y);
        BigMap.localScale = new Vector3(scale, scale, 1);
    }

    private void Awake()
    {
        SpaceLabel.SetActive(false);
    }

    void Update()
    {
        if (!InputMamager.Instance.IsLock)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                Treasure treasure = ExploreManager.Instance.GetTreasure();
                if (treasure != null)
                {
                    TreasureUI.Open(treasure.ID);
                    InputMamager.Instance.Lock();
                }
            }
        }

        if (Input.GetKeyDown(KeyCode.M))
        {
            BigMapBG.SetActive(!BigMapBG.activeSelf);
            if (BigMapBG.activeSelf)
            {
                ExploreInfo info = ExploreManager.Instance.Info;
                float x = (info.Size.x / 2 - Camera.main.transform.position.x) / info.Size.x * 1080 * _scale;
                float y = (info.Size.y / 2 - Camera.main.transform.position.z) / info.Size.y * 1080 * _scale;
                Debug.Log(x + " " + y);
                BigMap.anchoredPosition = new Vector2(x, y);
                BigMapCamera.Render();
                FloorLabel.text = ExploreManager.Instance.Info.Floor + "F";
                InputMamager.Instance.Lock();
            }
            else
            {
                InputMamager.Instance.Unlock();
            }
        }

        if (ExploreManager.Instance.Info != null && !InputMamager.Instance.IsLock)
        {
            Vector2Int v2 = Utility.ConvertToVector2Int(Camera.main.transform.position + Camera.main.transform.forward);
            SpaceLabel.SetActive(ExploreManager.Instance.Info.TreasureDic.ContainsKey(v2));
        }

        deltaTime += (Time.deltaTime - deltaTime) * 0.1f;
        float fps = 1.0f / deltaTime;
        fpsText.text = Mathf.Ceil(fps).ToString();
    }
}
