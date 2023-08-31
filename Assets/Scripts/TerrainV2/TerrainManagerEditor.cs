using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditor.AnimatedValues;
using Terrain;
using Terrain.Surface;

[CustomEditor(typeof(TerrainManager))]
public class TerrainManagerEditor : Editor
{
    public override void OnInspectorGUI()
    {
        // Initialize the serialized object and set it dirty
        serializedObject.Update();
        TerrainManager terrainManager = (TerrainManager)target;

        // Display the properties
        DrawDefaultInspector();

        // Update the object with the changes
        serializedObject.ApplyModifiedProperties();
        // Display the changes in the editor
        Repaint();
    }

}
