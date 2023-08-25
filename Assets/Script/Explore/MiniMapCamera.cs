using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MiniMapCamera : MonoBehaviour
{
    private Vector3 position = new Vector3();

    // Update is called once per frame
    void Update()
    {
        position.x = Camera.main.transform.position.x;
        position.y = 5;
        position.z = Camera.main.transform.position.z;
        transform.position = position;
    }
}
