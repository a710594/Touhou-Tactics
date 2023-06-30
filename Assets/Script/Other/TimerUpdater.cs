using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimerUpdater : MonoBehaviour
{
    public static Action UpdateHandler;

    private static bool _exists;

    void Start()
    {
        if (!_exists)
        {
            _exists = true;
            DontDestroyOnLoad(transform.gameObject);//使物件切換場景時不消失
        }
        else
        {
            Destroy(gameObject); //破壞場景切換後重複產生的物件
            return;
        }
    }

    void Update()
    {
        if (UpdateHandler != null)
        {
            UpdateHandler();
        }
    }
}
