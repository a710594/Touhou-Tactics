using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConversationModel
{
    public enum MotionEnum
    {
        None,
        Jump = 1,
        Shake,
    }

    public int ID;
    public int Page;
    public bool Pause;
    public string Name;
    public string Dialog;
    public string Image_1;
    public MotionEnum Motion_1;
    public string Image_2;
    public MotionEnum Motion_2;
    public string Image_3;
    public List<string> ImageList = new List<string>();
    public List<MotionEnum> MotionList = new List<MotionEnum>();

    public void GetList()
    {
        ImageList.Add(Image_1);
        ImageList.Add(Image_2);
        ImageList.Add(Image_3);
        MotionList.Add(Motion_1);
        MotionList.Add(Motion_2);
    }
}
