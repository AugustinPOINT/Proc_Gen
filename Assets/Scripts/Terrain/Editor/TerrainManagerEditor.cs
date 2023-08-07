using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditor.AnimatedValues;
using terrain;

[CustomEditor(typeof(TerrainManagerMB))]
public class TerrainManagerEditor : Editor
{
    static bool terrainPropertiesToggleValue = true;
    static bool toggleWireframe = false;

    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        TerrainManagerMB terrain = (TerrainManagerMB)target;

        /*Terrain Manager object*/
        {
            EditorGUILayout.BeginHorizontal();
            SerializedObject terrainManagerMBSer = new UnityEditor.SerializedObject(terrain);
            EditorGUILayout.PropertyField(terrainManagerMBSer.FindProperty("terrainManager_"));
            terrainManagerMBSer.ApplyModifiedProperties();
            EditorGUILayout.EndHorizontal();
            EditorGUILayout.Separator();
        }

        /*Terrain Properties*/
        {
            EditorGUILayout.BeginHorizontal();
            bool terrainPropertiesToggleValue_ = EditorGUILayout.Toggle(terrainPropertiesToggleValue, GUILayout.Width(20f));
            terrainPropertiesToggleValue = (terrain.terrainManager_ != null) && terrainPropertiesToggleValue_;
            EditorGUILayout.LabelField("Terrain manager properties");
            EditorGUILayout.EndHorizontal();

            if (terrainPropertiesToggleValue)
            {
                EditorGUILayout.Separator();
                // Serialize the SciptableObject on the inspector to be able to change values seamlessly
                EditorGUI.indentLevel++;
                EditorGUILayout.LabelField("Empty");
                // EditorGUILayout.PropertyField(terrainManager.FindProperty("chunkSize"));
                // EditorGUILayout.PropertyField(terrainManager.FindProperty("terrainDimensions"));
                EditorGUILayout.Separator();
                EditorGUI.indentLevel--;
                // terrainManager.ApplyModifiedProperties();
            }
        }

        /*Display*/
        {
            EditorGUILayout.BeginHorizontal();
            toggleWireframe = EditorGUILayout.Toggle(toggleWireframe, GUILayout.Width(20f));
            EditorGUILayout.LabelField("Toggle Wireframe");
            EditorGUILayout.EndHorizontal();
            if (terrain.terrainManager != null)
            {
                terrain.displayWireframe = toggleWireframe;
            }
            else
            {
                terrain.displayWireframe = false;
                toggleWireframe = false;
            }
        }

        /*Generate Terrain*/
        {
            if (GUILayout.Button("Generate Chunks"))
            {
                if (terrain.terrainManager != null)
                {
                    terrain.terrainManager.GenerateChunks(terrain.gameObject, "editor terrain");
                }
            }
        }


        serializedObject.ApplyModifiedProperties();
        Repaint();
        // base.OnInspectorGUI();
    }

}
