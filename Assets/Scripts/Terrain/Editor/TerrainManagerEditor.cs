using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditor.AnimatedValues;
using Terrain;

[CustomEditor(typeof(TerrainManagerMB))]
public class TerrainManagerEditor : Editor
{
    public string[] terrainManagerOptions = new string[] { "None", "Global 1 LOD", "Player-Based 1 LOD" };
    public int terrainManagerSelection = 0;
    public int currentTerrainManagerSelection;
    public bool terrainPropertiesToggleValue;
    public UnityEngine.Object terrainProperties;

    // public void OnMachin()
    public override void OnInspectorGUI()
    {
        // Initialize the serializedObject and set it dirty so that the inspector saves its state
        serializedObject.Update();
        TerrainManagerMB terrainManagerMB = (TerrainManagerMB)target;
        EditorUtility.SetDirty(terrainManagerMB);

        //------------------------|| TERRAIN MANAGER CREATION ||---------------------------//
        EditorGUILayout.BeginHorizontal();
        terrainManagerSelection = EditorGUILayout.Popup("Terrain Manager :", terrainManagerSelection, terrainManagerOptions);
        EditorGUILayout.EndHorizontal();
        EditorGUILayout.BeginHorizontal();
        terrainProperties = EditorGUILayout.ObjectField("Terrain Properties :", terrainProperties, typeof(TerrainProperties));
        EditorGUILayout.EndHorizontal();
        EditorGUILayout.BeginHorizontal();
        if (GUILayout.Button("Set") && terrainProperties != null)
        {
            SerializedProperty serializedTerrainManagerProperty = serializedObject.FindProperty("terrainManager");
            switch (terrainManagerSelection)
            {
                case 0: // No manager selected
                    currentTerrainManagerSelection = 0;
                    break;
                case 1: // Simple 1 LOD
                    SimpleTerrainManager simpleTerrainManager = new SimpleTerrainManager(terrainManagerMB.transform.gameObject);
                    simpleTerrainManager.terrainProperties = (TerrainProperties)terrainProperties;
                    serializedTerrainManagerProperty.managedReferenceValue = simpleTerrainManager;
                    currentTerrainManagerSelection = 1;
                    break;
                case 2: // Player-Based 1 LOD
                    PlayerBasedTerrainManager playerBasedTerrainManager = new PlayerBasedTerrainManager(terrainManagerMB.transform.gameObject);
                    playerBasedTerrainManager.terrainProperties = (TerrainProperties)terrainProperties;
                    serializedTerrainManagerProperty.managedReferenceValue = playerBasedTerrainManager;
                    currentTerrainManagerSelection = 2;
                    break;
                default:
                    currentTerrainManagerSelection = 0;
                    break;
            }
        }
        EditorGUILayout.EndHorizontal();

        //------------------------|| DISPLAY TERRAIN MANAGER PROPERTIES ||---------------------------//
        if (terrainManagerMB.terrainManager != null)
        {
            EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);
            EditorGUILayout.BeginHorizontal();
            terrainPropertiesToggleValue = EditorGUILayout.Toggle(terrainPropertiesToggleValue, GUILayout.Width(20f));
            EditorGUILayout.LabelField("Terrain Manager");
            EditorGUILayout.EndHorizontal();
            if (terrainPropertiesToggleValue && terrainManagerMB.terrainManager != null)
            {
                EditorGUILayout.TextField("Terrain Manager type", terrainManagerMB.terrainManager.GetType().ToString());
                // Display the correct properties depending on the ITerrainManager implementation
                if (terrainManagerMB.terrainManager is SimpleTerrainManager)
                {
                    //Global 1 LOD
                    EditorGUILayout.Separator();
                    EditorGUI.indentLevel++;
                    Vector3 terrainDimensions = EditorGUILayout.Vector3Field("Terrain Dimensions", terrainManagerMB.terrainManager.terrainProperties.terrainDimensions);
                    Vector3 chunkSize = EditorGUILayout.Vector3Field("Chunk Size", terrainManagerMB.terrainManager.terrainProperties.chunkSize);
                    terrainManagerMB.terrainManager.terrainProperties.terrainDimensions = terrainDimensions;
                    terrainManagerMB.terrainManager.terrainProperties.chunkSize = chunkSize;
                    EditorGUI.indentLevel--;
                }
                else if (terrainManagerMB.terrainManager is PlayerBasedTerrainManager)
                {
                    // Player-Based 1 LOD
                    EditorGUILayout.Separator();
                    EditorGUI.indentLevel++;
                    EditorGUI.indentLevel--;
                }
            }
        }

        //------------------------|| CONTROLS OF THE TERRAIN MANAGER ||---------------------------//
        if (terrainManagerMB.terrainManager != null)
        {
            EditorGUILayout.Separator();
            if (GUILayout.Button("Generate Chunks"))
            {
                if (terrainManagerMB.terrainManager != null)
                {
                    terrainManagerMB.terrainManager.GenerateChunks("editor terrain");
                }
            }
            EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);
        }

        //------------------------|| CONTROLS OF THE TERRAIN MANAGER ||---------------------------//
        if (terrainManagerMB.terrainManager != null)
        {
            EditorGUILayout.Separator();
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.PropertyField(serializedObject.FindProperty("displayWireframe"));
            EditorGUILayout.EndHorizontal();
        }


        serializedObject.ApplyModifiedProperties();
        Repaint();
    }

}
