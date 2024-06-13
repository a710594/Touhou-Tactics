using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LogScrollView : MonoBehaviour
{
    public ScrollView ScrollView;

    private List<string> _list = new List<string>();

    private void Start()
    {

        for (int i=0; i<20; i++) 
        {
            _list.Add(i.ToString());
        }

        ScrollView.SetData(new List<object>(_list));

    }  

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space))
        {
            ScrollView.SetData(new List<object>(_list));
            ScrollView.SetIndex(13);

        }
    }
}
