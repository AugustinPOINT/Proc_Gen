using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditor.AnimatedValues;
using terrain;

[CustomEditor(typeof(TerrainManagerMB))]
public class TerrainManagerEditor : Editor
{
    public string[] terrainManagerOptions = new string[] { "None", "Global 1 LOD", "Player-Based 1 LOD" };
    static bool terrainPropertiesToggleValue = false;
    static bool toggleWireframe = false;
    static int terrainManagerSelection = 0;
    static int currentTerrainManagerSelection;
    static UnityEngine.Object terrainProperties;

    public override void OnInspectorGUI()
    {
        serializedObject.Update();
        TerrainManagerMB terrainManagerMB = (TerrainManagerMB)target;
        EditorUtility.SetDirty(terrainManagerMB);

        //------------------------|| CREATION OF THE TERRAIN MANAGER ||---------------------------//
        EditorGUILayout.BeginHorizontal();
        terrainManagerSelection = EditorGUILayout.Popup("Terrain Manager :", terrainManagerSelection, terrainManagerOptions);
        EditorGUILayout.EndHorizontal();
        EditorGUILayout.BeginHorizontal();
        terrainProperties = EditorGUILayout.ObjectField("Terrain Properties :", terrainProperties, typeof(TerrainProperties));
        EditorGUILayout.EndHorizontal();
        EditorGUILayout.BeginHorizontal();
        if (GUILayout.Button("Set") && terrainProperties != null)
        {
            switch (terrainManagerSelection)
            {
                case 0: // No manager selected
                    currentTerrainManagerSelection = 0;
                    break;
                case 1: // Simple 1 LOD
                    terrainManagerMB.terrainManager = new TerrainManager("Type 1");
                    terrainManagerMB.terrainManager.terrainProperties = (TerrainProperties)terrainProperties;
                    currentTerrainManagerSelection = 1;
                    break;
                case 2: // Player-Based 1 LOD
                    terrainManagerMB.terrainManager = new TerrainManager("Type 2");
                    terrainManagerMB.terrainManager.terrainProperties = (TerrainProperties)terrainProperties;
                    currentTerrainManagerSelection = 2;
                    break;
                default:
                    currentTerrainManagerSelection = 0;
                    break;
            }
        }
        EditorGUILayout.EndHorizontal();


        //------------------------|| OPTIONS OF THE TERRAIN MANAGER ONCE SET ||---------------------------//
        if (terrainManagerMB.terrainManager != null)
        {
            UnityEngine.Debug.Log("TERRAIN MANAGER IS HERE");
            EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);
            // Setup the toggle of the properties
            EditorGUILayout.BeginHorizontal();
            terrainPropertiesToggleValue = EditorGUILayout.Toggle(terrainPropertiesToggleValue, GUILayout.Width(20f));
            EditorGUILayout.LabelField("Terrain properties");
            EditorGUILayout.EndHorizontal();
            if (terrainPropertiesToggleValue)
            {
                // Display the correct properties depending on the ITerrainManager implementation
                switch (currentTerrainManagerSelection)
                {
                    case 0: // No manager set
                        break;
                    case 1: // Simple 1 LOD
                        EditorGUILayout.Separator();
                        EditorGUI.indentLevel++;
                        Vector3 terrainDimensions = EditorGUILayout.Vector3Field("Terrain Dimensions", terrainManagerMB.terrainManager.terrainProperties.terrainDimensions);
                        Vector3 chunkSize = EditorGUILayout.Vector3Field("Chunk Size", terrainManagerMB.terrainManager.terrainProperties.chunkSize);
                        terrainManagerMB.terrainManager.terrainProperties.terrainDimensions = terrainDimensions;
                        terrainManagerMB.terrainManager.terrainProperties.chunkSize = chunkSize;
                        EditorGUI.indentLevel--;
                        break;
                    case 2: // Player-Based 1 LOD
                        EditorGUILayout.Separator();
                        EditorGUI.indentLevel++;
                        EditorGUI.indentLevel--;
                        break;
                    default:
                        break;
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
                    terrainManagerMB.terrainManager.GenerateChunks(terrainManagerMB.gameObject, "editor terrain");
                }
            }
        }

        //------------------------|| CONTROLS OF THE TERRAIN MANAGER ||---------------------------//
        if (terrainManagerMB.terrainManager != null)
        {
            EditorGUILayout.Separator();
            EditorGUILayout.BeginHorizontal();
            toggleWireframe = EditorGUILayout.Toggle(toggleWireframe, GUILayout.Width(20f));
            EditorGUILayout.LabelField("Toggle Wireframe");
            EditorGUILayout.EndHorizontal();
            if (terrainManagerMB.terrainManager != null)
            {
                terrainManagerMB.displayWireframe = toggleWireframe;
            }
            else
            {
                terrainManagerMB.displayWireframe = false;
                toggleWireframe = false;
            }
        }


        serializedObject.ApplyModifiedProperties();
        Repaint();
        // base.OnInspectorGUI();
    }

}
