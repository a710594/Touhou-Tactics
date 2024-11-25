using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using Explore;

[CustomEditor(typeof(ExploreMapBuilder))]
public class ExploreMapBuilderEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        ExploreMapBuilder builder = (ExploreMapBuilder)target;
        if (GUILayout.Button("¿é¥XÀÉ®×"))
        {
            builder.GetRandom(builder.Floor, builder.Seed);
        }
    }
}
