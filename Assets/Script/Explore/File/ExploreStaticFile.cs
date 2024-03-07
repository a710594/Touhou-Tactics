using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExploreStaticFile
{
    public int Floor;
    public Vector2Int Size;
    public Vector2Int Start;
    public Vector2Int Goal;
    public List<Vector2Int> GroundList = new List<Vector2Int>(); //�ж��Ψ��Y���i�樫���a��
    //json ����H Vector2Int �@�� dictionary �� key
    //�ҥH�n�⥦�� keys �M values ���}���x�s
    public List<Vector2Int> TileKeys = new List<Vector2Int>();
    public List<string> TileValues = new List<string>();
}
