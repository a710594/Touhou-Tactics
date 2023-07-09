using UnityEngine;

[ExecuteInEditMode]
public class SpriteOutline : MonoBehaviour
{
    public Color color = Color.white;

    public bool showOutline = false;
    public int OutlineSize = 10;
    public SpriteRenderer SpriteRenderer;
    public Material Material;

    public void SetOutline(bool show)
    {
        SpriteRenderer.material = Material;
        showOutline = show;
    }

    void OnEnable()
    {
        UpdateOutline(showOutline);
    }

    void OnDisable()
    {
        UpdateOutline(false);
    }

    void Update()
    {
        UpdateOutline(showOutline);
    }

    void UpdateOutline(bool outline)
    {
        MaterialPropertyBlock mpb = new MaterialPropertyBlock();
        SpriteRenderer.GetPropertyBlock(mpb);
        mpb.SetFloat("_Outline", outline ? 1f : 0);
        mpb.SetColor("_OutlineColor", color);
        mpb.SetFloat("_OutlineSize", OutlineSize);
        SpriteRenderer.SetPropertyBlock(mpb);
    }
}
