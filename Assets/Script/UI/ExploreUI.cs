using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Explore;

public class ExploreUI : MonoBehaviour
{
    public GameObject BigMap;
    public GameObject SpaceLabel;
    public TreasureUI TreasureUI;
    public Text fpsText;
    public Text FloorLabel;
    public Camera BigMapCamera;

    private float deltaTime;

    public void SetCameraPosition(int x, int y) 
    {
        BigMapCamera.transform.position = new Vector3(x, 5, y);
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
            BigMap.SetActive(!BigMap.activeSelf);
            if (BigMap.activeSelf)
            {
                BigMapCamera.Render();
                FloorLabel.text = ExploreManager.Instance.Info.Floor + "F";
                InputMamager.Instance.Lock();
            }
            else
            {
                InputMamager.Instance.Unlock();
            }
        }

        if (ExploreManager.Instance.Info != null)
        {
            Vector2Int v2 = Utility.ConvertToVector2Int(Camera.main.transform.position + Camera.main.transform.forward);
            SpaceLabel.SetActive(ExploreManager.Instance.Info.TreasureDic.ContainsKey(v2));
        }

        deltaTime += (Time.deltaTime - deltaTime) * 0.1f;
        float fps = 1.0f / deltaTime;
        fpsText.text = Mathf.Ceil(fps).ToString();
    }
}
