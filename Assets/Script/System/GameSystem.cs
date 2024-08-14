using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameSystem : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        SystemManager.Instance.Init();
        DataContext.Instance.Init();
        SceneController.Instance.Init();
        ItemManager.Instance.Init();
        CharacterManager.Instance.Init();
        InputMamager.Instance.Init();
        FlagManager.Instance.Init();
        FlowController.Instance.Load();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
