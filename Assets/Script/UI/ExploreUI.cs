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

    private bool _isLock = false;
    private float deltaTime;

    private void Lock() 
    {
        _isLock = true;
        ExploreManager.Instance.Player.CanMove = false;
    }

    private void Unlock() 
    {
        _isLock = false;
        ExploreManager.Instance.Player.CanMove = true;
    }

    // Start is called before the first frame update
    void Awake()
    {
        TreasureUI.gameObject.SetActive(false);
        TreasureUI.CloseHandler += Unlock;
    }

    // Update is called once per frame
    void Update()
    {
        if (!_isLock)
        {
            if (Input.GetKeyDown(KeyCode.I))
            {
                BagUI bagUI = BagUI.Open();
                bagUI.SetNormalState();
                bagUI.CloseHandler = Unlock;
                Lock();
            }
            if (Input.GetKeyDown(KeyCode.Space))
            {
                Treasure treasure = ExploreManager.Instance.GetTreasure();
                if (treasure != null)
                {
                    TreasureUI.Open(treasure.ID);
                    Lock();
                }
            }
            if (Input.GetKeyDown(KeyCode.C)) 
            {
                SelectCharacterUI selectCharacterUI = SelectCharacterUI.Open();
            }
        }

        if (Input.GetKeyDown(KeyCode.M))
        {
            BigMap.SetActive(!BigMap.activeSelf);
            if (BigMap.activeSelf)
            {
                BigMapCamera.Render();
                Lock();
            }
            else
            {
                Unlock();
            }
        }
        if (Input.GetKeyDown(KeyCode.S))
        {
            FileSystem.Instance.Save();
        }

        deltaTime += (Time.deltaTime - deltaTime) * 0.1f;
        float fps = 1.0f / deltaTime;
        fpsText.text = Mathf.Ceil(fps).ToString();
    }
}
