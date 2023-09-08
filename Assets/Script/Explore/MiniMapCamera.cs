using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MiniMapCamera : MonoBehaviour
{
    public Transform Quad;

    private Vector3 position = new Vector3();
    private Vector3 angle = new Vector3();

    // Update is called once per frame
    void Update()
    {
        position.x = Camera.main.transform.position.x;
        position.y = 5;
        position.z = Camera.main.transform.position.z;
        transform.position = position;
        angle.x = 90;
        angle.y = Camera.main.transform.eulerAngles.y;
        Quad.transform.eulerAngles = angle;
    }
}
