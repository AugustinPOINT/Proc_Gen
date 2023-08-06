using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace terrain
{
    // Terrain Manager Class
    [CreateAssetMenu(fileName = "TerrainManager", menuName = "Terrain Manager")]
    public class TerrainManager : ScriptableObject, ITerrainManager
    {
        //------------------------|| ATTRIBUTES ||---------------------------//

        [field: SerializeField] public TerrainProperties terrainProperties { get; set; }


        //--------------------------|| METHODS ||----------------------------//

        TerrainManager() { }

        //TODO : Let user chose the chunk generation type : fix, cameraField, radius
        public void GenerateChunks(Transform terrainManagerTransform, string terrainName)
        {
            //Create Terrain root GameObject
            Transform previousTerrain = terrainManagerTransform.Find(terrainName);
            if (previousTerrain != null)
            {
                UnityEngine.Debug.Log("Removing existing chunks");
                GameObject.DestroyImmediate(previousTerrain.gameObject);
            }
            GameObject terrainGO = new GameObject(terrainName);
            terrainGO.transform.SetPositionAndRotation(new Vector3(0, 0, 0), Quaternion.identity);
            terrainGO.transform.SetParent(terrainManagerTransform);

            // Populate the terrain with chunks
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
                        chunkGO.GetComponent<Chunk.ChunkMB>().SetTerrainManagerTransform(terrainManagerTransform);
                    }
                }
            }
        }

        public void ManageChunks() { }

    }
}

