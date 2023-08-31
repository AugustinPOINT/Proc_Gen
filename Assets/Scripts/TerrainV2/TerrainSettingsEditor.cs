using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditor.AnimatedValues;
using Terrain;
using Terrain.Surface;

[CustomEditor(typeof(TerrainSettings))]
public class TerrainSettingsEditor : Editor
{
    // public void OnMachin()
    public override void OnInspectorGUI()
    {
        // Initialize the serialized object and set it dirty
        serializedObject.Update();
        TerrainSettings terrainSettings = (TerrainSettings)target;
        EditorUtility.SetDirty(terrainSettings);

        // Display the terrain settings properties
        EditorGUILayout.PropertyField(serializedObject.FindProperty("surfaceBuilderType"));
        EditorGUILayout.Separator();
        if (terrainSettings.surfaceBuilderSettings != null)
        {
            EditorGUI.indentLevel++;
            EditorGUILayout.PropertyField(serializedObject.FindProperty("surfaceBuilderSettings"), true);
            EditorGUILayout.Separator();
            EditorGUI.indentLevel--;
        }
        // Initialize the terrain settings (do it only on change)
        switch (terrainSettings.surfaceBuilderType)
        {
            case SurfaceBuilderType.Static:
                if (terrainSettings.surfaceBuilderSettings is not StaticSurfaceBuilderSettings)
                    terrainSettings.surfaceBuilderSettings = new StaticSurfaceBuilderSettings();
                break;
            case SurfaceBuilderType.PlayerBased:
                if (terrainSettings.surfaceBuilderSettings is not PlayerBasedSurfaceBuilderSettings)
                    terrainSettings.surfaceBuilderSettings = new PlayerBasedSurfaceBuilderSettings();
                break;
            default:
                break;
        }

        // Display the algorithm choice property
        EditorGUILayout.PropertyField(serializedObject.FindProperty("algorithm"));
        EditorGUILayout.Separator();

        // Display the isosurface choice property
        EditorGUILayout.PropertyField(serializedObject.FindProperty("isosurfaceType"));
        EditorGUILayout.Separator();
        if (terrainSettings.isosurface != null)
        {
            EditorGUI.indentLevel++;
            EditorGUILayout.PropertyField(serializedObject.FindProperty("isosurface"), true);
            EditorGUILayout.Separator();
            EditorGUI.indentLevel--;
        }
        // Initialize the isosurface (do it only on change)
        switch (terrainSettings.isosurfaceType)
        {
            case IsosurfaceType.Functional:
                if (terrainSettings.isosurface is not SingularIsosurface)
                    terrainSettings.isosurface = new SingularIsosurface();
                break;
            case IsosurfaceType.Composed:
                if (terrainSettings.isosurface is not ComposedIsosurface)
                    terrainSettings.isosurface = new ComposedIsosurface();
                break;
            default:
                break;
        }

        // Update the object with the changes
        serializedObject.ApplyModifiedProperties();
        // Display the changes in the editor
        Repaint();
    }

}
