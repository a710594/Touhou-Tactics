using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using Explore;

[CustomEditor(typeof(ExploreFileGenerator))]
public class ExploreFileGeneratorEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        ExploreFileGenerator exploreFileGenerator = (ExploreFileGenerator)target;
        if (GUILayout.Button("�إ��ɮ�"))
        {
            exploreFileGenerator.BuildFile();
        }
    }
}
