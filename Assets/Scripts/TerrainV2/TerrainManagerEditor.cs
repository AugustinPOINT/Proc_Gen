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

        // Display the properties
        EditorGUILayout.PropertyField(serializedObject.FindProperty("surfaceBuilderType"));
        EditorGUILayout.Separator();
        if (terrainSettings.surfaceBuilderSettings != null)
        {
            SerializedProperty surfaceBuilderSettingsProperty = serializedObject.FindProperty("surfaceBuilderSettings");
            EditorGUILayout.PropertyField(surfaceBuilderSettingsProperty, true);
            EditorGUILayout.Separator();
        }
        EditorGUILayout.PropertyField(serializedObject.FindProperty("algorithm"));

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
        // Update the object with the changes
        serializedObject.ApplyModifiedProperties();
        // Display the changes in the editor
        Repaint();
    }

}
