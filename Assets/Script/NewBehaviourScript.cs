using Battle;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewBehaviourScript : MonoBehaviour
{

    // Start is called before the first frame update
    void Start()
    {
        AttachSetting attachSetting = new AttachSetting();
        attachSetting.ID = "Grass";
        attachSetting.MoveCost = 1;
        attachSetting.Height = 0;

        DataContext.Instance.Save(attachSetting, "Grass", DataContext.PrePathEnum.Save);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
