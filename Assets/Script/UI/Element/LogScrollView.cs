using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LogScrollView : MonoBehaviour
{
    public ScrollView VerticalScrollView;
    public ScrollView HorizontalScrollView;

    private List<string> _list = new List<string>();

    private void Start()
    {

        for (int i=0; i<20; i++) 
        {
            _list.Add(i.ToString());
        }

        VerticalScrollView.SetData(new List<object>(_list));
        HorizontalScrollView.SetData(new List<object>(_list));

    }  

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space))
        {
            //VerticalScrollView.SetData(new List<object>(_list));
            VerticalScrollView.SetIndex(8);
            //HorizontalScrollView.SetData(new List<object>(_list));
            HorizontalScrollView.SetIndex(8);

        }
    }
}
