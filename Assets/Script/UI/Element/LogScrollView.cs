using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LogScrollView : MonoBehaviour
{
    public ScrollView ScrollView;

    private void Start()
    {
        List<string> list = new List<string>();
        for (int i=0; i<10; i++) 
        {
            list.Add(i.ToString());
        }

        ScrollView.SetData(new List<object>(list));
    }
}
