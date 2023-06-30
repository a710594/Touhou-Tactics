using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraDraw : MonoBehaviour
{
    private bool _isBlock = false;
    private Material _mat;
    private Vector3 _p1;
    private Vector3 _p2;
    private List<Vector3> _list = new List<Vector3>();

    public void DrawLine(Vector3 p1, Vector3 p2, bool isBlock) 
    {
        _list = new List<Vector3>() { p1, p2 };
        _isBlock = isBlock;
    }

    public void DrawParabola(Vector3 p1, Vector3 p2, int height) 
    {
        _list = Utility.DrawParabola(p1, p2, height);
    }

    public void Clear() 
    {
        _list.Clear();
        _isBlock = false;
    }

    public void OnPostRender()
    {
        CreateLineMaterial();
        GL.PushMatrix();
        _mat.SetPass(0);
        GL.LoadOrtho();

        GL.Begin(GL.LINES);
        if (_isBlock)
        {
            GL.Color(Color.red);

            //µe¤e¤e
            if (_list.Count > 0)
            {
                _p1 = Camera.main.WorldToScreenPoint(_list[_list.Count - 1] + new Vector3(0.3f, 0, 0));
                _p2 = Camera.main.WorldToScreenPoint(_list[_list.Count - 1] + new Vector3(-0.3f, 0, 0));
                GL.Vertex(new Vector3(_p1.x / Screen.width, _p1.y / Screen.height, 0));
                GL.Vertex(new Vector3(_p2.x / Screen.width, _p2.y / Screen.height, 0));

                _p1 = Camera.main.WorldToScreenPoint(_list[_list.Count - 1] + new Vector3(0, 0, 0.3f));
                _p2 = Camera.main.WorldToScreenPoint(_list[_list.Count - 1] + new Vector3(0, 0, -0.3f));
                GL.Vertex(new Vector3(_p1.x / Screen.width, _p1.y / Screen.height, 0));
                GL.Vertex(new Vector3(_p2.x / Screen.width, _p2.y / Screen.height, 0));
            }
        }
        else
        {
            GL.Color(Color.blue);
        }
        //_p1 = Camera.main.WorldToScreenPoint(new Vector3(0, 1, 0));
        //_p2 = Camera.main.WorldToScreenPoint(new Vector3(5, 1, 5));
        //GL.Vertex(new Vector3(_p1.x / Screen.width, _p1.y / Screen.height, 0));
        //GL.Vertex(new Vector3(_p2.x / Screen.width, _p2.y / Screen.height, 0));
        for (int i=1; i<_list.Count; i++) 
        {
            _p1 = Camera.main.WorldToScreenPoint(_list[i - 1]);
            _p2 = Camera.main.WorldToScreenPoint(_list[i]);
            GL.Vertex(new Vector3(_p1.x / Screen.width, _p1.y / Screen.height, 0));
            GL.Vertex(new Vector3(_p2.x / Screen.width, _p2.y / Screen.height, 0));
        }
        GL.End();

        GL.PopMatrix();
    }

    private void CreateLineMaterial()
    {
        if (!_mat)
        {
            // Unity has a built-in shader that is useful for drawing
            // simple colored things.
            Shader shader = Shader.Find("Hidden/Internal-Colored");
            _mat = new Material(shader);
            _mat.hideFlags = HideFlags.HideAndDontSave;
            // Turn on alpha blending
            _mat.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.SrcAlpha);
            _mat.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);
            // Turn backface culling off
            _mat.SetInt("_Cull", (int)UnityEngine.Rendering.CullMode.Off);
            // Turn off depth writes
            _mat.SetInt("_ZWrite", 0);
        }
    }
}
