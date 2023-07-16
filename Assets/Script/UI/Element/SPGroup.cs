using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SPGroup : MonoBehaviour
{
    public GameObject[] Obj;

    public void SetData(int SP) 
    {
        for (int i=0; i<Obj.Length; i++) 
        {
            Obj[i].SetActive(i<SP);
        }
    }
}
