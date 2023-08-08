using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using terrain.Chunk;

namespace terrain
{
    // Terrain Manager Class
    [System.Serializable]
    public class SimpleTerrainManager : ITerrainManager
    {
        //------------------------|| ATTRIBUTES ||---------------------------//

        [SerializeField] private TerrainProperties terrainProperties_;
        public TerrainProperties terrainProperties { get => terrainProperties_; set => terrainProperties_ = value; }

        [SerializeField] private ChunkMB[] chunks;
        GameObject terrainManagerGO;

        //--------------------------|| METHODS ||----------------------------//

        public SimpleTerrainManager(GameObject terrainManagerGO_)
        {
            terrainManagerGO = terrainManagerGO_;
            RegisterExistingTerrain();
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
            terrainGO.transform.SetPositionAndRotation(new Vector3(0, 0, 0), Quaternion.identity);
            terrainGO.transform.SetParent(terrainManagerGO.transform);

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
                        Vector3 chunkPosition = new Vector3(chunkX * (int)terrainProperties.chunkSize.x, chunkY * (int)terrainProperties.chunkSize.y, chunkZ * (int)terrainProperties.chunkSize.z);
                        chunkGO.transform.SetPositionAndRotation(chunkPosition, Quaternion.identity);
                        chunkGO.transform.SetParent(terrainGO.transform);
                        chunkGO.AddComponent<ChunkMB>();
                        chunks[index] = chunkGO.GetComponent<ChunkMB>();
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

        public void ManageChunks() { }

    }
}

