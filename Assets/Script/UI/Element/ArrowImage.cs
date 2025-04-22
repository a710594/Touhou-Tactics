using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowImage : MonoBehaviour
{
    public float Height;

    private Transform _anchor = null;

    public void Show(Transform anchor) 
    {
        _anchor = anchor;
        gameObject.SetActive(true);
    }

    public void Hide() 
    {
        _anchor = null;
        gameObject.SetActive(false);
    }

    private void Update()
    {
        if (_anchor != null)
        {
            transform.position = Camera.main.WorldToScreenPoint(_anchor.position) + Vector3.up * (Height * (1 + 0.1f * (Mathf.Sin(Time.time * Mathf.PI))));
        }
    }
}
