using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Explore;

public class ExploreUI : MonoBehaviour
{
    public GameObject BigMap; 
    public TreasureUI TreasureUI;
    public Text fpsText;
    public Camera BigMapCamera;

    private float deltaTime;

    // Start is called before the first frame update
    void Awake()
    {
        TreasureUI.gameObject.SetActive(false);
    }

    // Update is called once per frame
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
                InputMamager.Instance.Lock();
            }
            else
            {
                InputMamager.Instance.Unlock();
            }
        }

        deltaTime += (Time.deltaTime - deltaTime) * 0.1f;
        float fps = 1.0f / deltaTime;
        fpsText.text = Mathf.Ceil(fps).ToString();
    }
}
