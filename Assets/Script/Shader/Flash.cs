using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Flash : MonoBehaviour
{
    public float Speed;

    private bool _enable = false;
    private float _amount = 0;
    private UnityEngine.Material _material;
    private Renderer[] _renderers;

    public void Begin()
    {
        _enable = true;
    }

    public void End()
    {
        _enable = false;
        foreach (var renderer in _renderers)
        {
            SetColor(renderer, 0);
        }
    }

    private void SetColor(Renderer renderer, float amount) 
    {
        MaterialPropertyBlock mpb = new MaterialPropertyBlock();
        renderer.GetPropertyBlock(mpb);
        mpb.SetFloat("_FlashAmount", amount);
        renderer.SetPropertyBlock(mpb);
    }

    private void Awake()
    {
        _material = Instantiate(Resources.Load<UnityEngine.Material>("Material/Flash"));
        _renderers = GetComponentsInChildren<Renderer>();

        foreach (var renderer in _renderers)
        {
            var materials = renderer.sharedMaterials.ToList();
            materials.Add(_material);
            renderer.materials = materials.ToArray();
            SetColor(renderer, 0);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(_enable)
        { 
            _amount = Mathf.PingPong(Time.time * Speed, 1.0f);
            foreach (var renderer in _renderers)
            {
                SetColor(renderer, _amount);
            }
        }
    }
}
