using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Terrain.Chunk;
using Terrain.Surfaces;
using Terrain.Utils;

namespace Terrain
{
    // Terrain Manager Class
    [System.Serializable]
    public class SimpleTerrainManager : ITerrainManager
    {
        //------------------------|| ATTRIBUTES ||---------------------------//

        [SerializeField] private TerrainProperties terrainProperties_;
        public TerrainProperties terrainProperties { get => terrainProperties_; set => terrainProperties_ = value; }

        [SerializeField] private ChunkMB[] chunks;              // SerializeField allows the saving of the attribute between unity engine sessions
        [SerializeField] private GameObject terrainManagerGO;
        [SerializeField] private Isosurface ground;
        [SerializeField] private Tools.surfaceDrawerAlgorithm algorithm = Tools.surfaceDrawerAlgorithm.SurfaceNets;
        [SerializeField] private bool terrainGenerated;

        //--------------------------|| METHODS ||----------------------------//

        public SimpleTerrainManager(GameObject terrainManagerGO_)
        {
            // Store reference to the attached gameObject
            terrainManagerGO = terrainManagerGO_;
            // Register the already existing chunks
            RegisterExistingTerrain();
            // Register the isosurface function (create it here for now, attach one in the future)
            terrainGenerated = false;
            ground = (Isosurface)ScriptableObject.CreateInstance(typeof(Isosurface));
        }

        public void GenerateChunks(string terrainName)
        {
            //Create Terrain root GameObject
            Transform previousTerrain = terrainManagerGO.transform.Find(terrainName);
            if (previousTerrain != null)
            {
                UnityEngine.Debug.Log("Removing existing chunks");
                GameObject.DestroyImmediate(previousTerrain.gameObject);
            }
            GameObject terrainGO = new GameObject(terrainName);
            terrainGO.transform.SetParent(terrainManagerGO.transform);
            terrainGO.transform.SetLocalPositionAndRotation(new Vector3(0, 0, 0), Quaternion.identity);

            // Populate the terrain with chunks
            int index = 0;
            chunks = new ChunkMB[(int)(terrainProperties.terrainDimensions[0] * terrainProperties.terrainDimensions[1] * terrainProperties.terrainDimensions[2])];
            for (int chunkX = 0; chunkX < terrainProperties.terrainDimensions.x; chunkX++)
            {
                for (int chunkY = 0; chunkY < terrainProperties.terrainDimensions.y; chunkY++)
                {
                    for (int chunkZ = 0; chunkZ < terrainProperties.terrainDimensions.z; chunkZ++)
                    {
                        string chunkName = string.Format("chunk({0},{1},{2})", chunkX, chunkY, chunkZ);
                        GameObject chunkGO = new GameObject(chunkName);
                        Vector3 chunkPosition = new Vector3(chunkX * terrainProperties.chunkSize.x, chunkY * terrainProperties.chunkSize.y, chunkZ * terrainProperties.chunkSize.z);
                        chunkGO.transform.SetParent(terrainGO.transform);
                        chunkGO.transform.SetLocalPositionAndRotation(chunkPosition, Quaternion.identity);
                        // Add the MeshFilter and MeshRenderer components
                        chunkGO.AddComponent<MeshFilter>();
                        chunkGO.AddComponent<MeshRenderer>();
                        // Add the ChunkMB component and initializes it. Note that the subdiv is decided by the manager, to choose a subdivision level in real-time if we want.
                        ChunkMB chunkMB = chunkGO.AddComponent<ChunkMB>();
                        chunks[index] = chunkMB;
                        chunkMB.Initialize(new Vector3Int(chunkX, chunkY, chunkZ), terrainProperties.chunkSize[0], 2 * (int)terrainProperties.chunkSize[0]);
                        index++;
                    }
                }
            }
        }

        public void RegisterExistingTerrain()
        {
            if (terrainManagerGO.transform.childCount > 0)
            {
                Transform previousTerrain = terrainManagerGO.transform.GetChild(0);
                chunks = new ChunkMB[previousTerrain.childCount];
                for (int chunkIndex = 0; chunkIndex < previousTerrain.childCount; chunkIndex++)
                {
                    Transform chunkTransform = previousTerrain.GetChild(chunkIndex);
                    chunks[chunkIndex] = chunkTransform.gameObject.GetComponent<ChunkMB>();
                }
            }
        }

        public void ManageChunks()
        {
            if (!terrainGenerated)
            {
                for (int chunkIndex = 0; chunkIndex < chunks.Length; chunkIndex++)
                {
                    // Set the surface drawer algorithm
                    chunks[chunkIndex].SetSurfaceDrawerAlgorithm(algorithm);
                    // Generate the terrain
                    chunks[chunkIndex].GenerateTerrain(ground);
                }
                terrainGenerated = true;
            }
        }

    }
}

