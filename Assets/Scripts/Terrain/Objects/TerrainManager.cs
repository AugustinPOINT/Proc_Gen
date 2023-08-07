using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace terrain
{
    // Terrain Manager Class
    [CreateAssetMenu(fileName = "TerrainManager", menuName = "Terrain Manager")]
    public class TerrainManager : ITerrainManager
    {
        //------------------------|| ATTRIBUTES ||---------------------------//

        [field: SerializeField] public TerrainProperties terrainProperties { get; set; }
        [SerializeField] public string managerType { get; set; }
        private Chunk.ChunkMB[] chunks;

        //--------------------------|| METHODS ||----------------------------//

        public TerrainManager() { }
        public TerrainManager(string managerType_) { managerType = managerType_; }

        //TODO : Let user chose the chunk generation type : fix, cameraField, radius
        public void GenerateChunks(UnityEngine.GameObject terrainManager, string terrainName)
        {
            //Create Terrain root GameObject
            Transform previousTerrain = terrainManager.transform.Find(terrainName);
            if (previousTerrain != null)
            {
                UnityEngine.Debug.Log("Removing existing chunks");
                GameObject.DestroyImmediate(previousTerrain.gameObject);
            }
            GameObject terrainGO = new GameObject(terrainName);
            terrainGO.transform.SetPositionAndRotation(new Vector3(0, 0, 0), Quaternion.identity);
            terrainGO.transform.SetParent(terrainManager.transform);

            // Populate the terrain with chunks
            int index = 0;
            chunks = new Chunk.ChunkMB[(int)(terrainProperties.terrainDimensions[0] * terrainProperties.terrainDimensions[1] * terrainProperties.terrainDimensions[2])];
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
                        chunkGO.AddComponent<Chunk.ChunkMB>();
                        chunks[index] = chunkGO.GetComponent<Chunk.ChunkMB>();
                        index++;
                    }
                }
            }
        }

        public void ManageChunks() { }

    }
}

