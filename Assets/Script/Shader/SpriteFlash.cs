using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteFlash : MonoBehaviour
{
    public Color FlashColor;
    public SpriteRenderer SpriteRenderer;

    private bool _enable = false;
    private Color _originalColor;

    public void SetFlash(bool enable)
    {
        _enable = enable;
        if (!enable) 
        {
            SpriteRenderer.color = _originalColor;
        }
    }

    // Start is called before the first frame update
    void Awake()
    {
        _originalColor = SpriteRenderer.color;
    }

    float amount = 0;
    // Update is called once per frame
    void Update()
    {
        if (_enable)
        {
            amount = Mathf.PingPong(Time.time * 1, 1.0f);
            MaterialPropertyBlock mpb = new MaterialPropertyBlock();
            SpriteRenderer.GetPropertyBlock(mpb);
            mpb.SetFloat("_FlashAmount", amount);
            mpb.SetColor("_FlashColor", FlashColor);
            SpriteRenderer.SetPropertyBlock(mpb);
        }
    }
}
