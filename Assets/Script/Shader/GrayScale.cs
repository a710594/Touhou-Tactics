using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrayScale : MonoBehaviour
{
    public bool IsGray = false;
    public SpriteRenderer SpriteRenderer;

    // Start is called before the first frame update
    public void SetScale()
    {
        MaterialPropertyBlock mpb = new MaterialPropertyBlock();
        SpriteRenderer.GetPropertyBlock(mpb);
        mpb.SetInteger("IsGray", IsGray ? 1 : 0);
        SpriteRenderer.SetPropertyBlock(mpb);
    }

    private void Start()
    {
        //(GrayscaleAmount);
    }

    // Update is called once per frame
    void Update()
    {
        SetScale();
    }
}
