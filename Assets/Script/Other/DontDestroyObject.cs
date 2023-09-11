using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DontDestroyObject : MonoBehaviour
{
    private static bool _exists;

    void Start()
    {
        if (!_exists)
        {
            _exists = true;
            DontDestroyOnLoad(transform.gameObject);//�Ϫ�����������ɤ�����
        }
        else
        {
            Destroy(gameObject); //�}�a���������᭫�Ʋ��ͪ�����
            return;
        }
    }
}
