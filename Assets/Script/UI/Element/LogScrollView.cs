using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LogScrollView : MonoBehaviour
{
    public ScrollView ScrollView;

    private void Start()
    {
        List<string> list = new List<string>();
        for (int i=0; i<20; i++) 
        {
            list.Add(i.ToString());
        }

        ScrollView.SetData(new List<object>(list));

    }  

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space))
        {
            ScrollView.SetIndex(5);

        }
    }
}
