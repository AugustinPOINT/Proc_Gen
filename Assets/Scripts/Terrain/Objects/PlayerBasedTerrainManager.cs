using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using terrain.Chunk;

namespace terrain
{
    // Terrain Manager Class
    [System.Serializable]
    public class PlayerBasedTerrainManager : ITerrainManager
    {
        //------------------------|| ATTRIBUTES ||---------------------------//

        [SerializeField] private TerrainProperties terrainProperties_;
        public TerrainProperties terrainProperties { get => terrainProperties_; set => terrainProperties_ = value; }

        [SerializeField] private ChunkMB[] chunks;
        GameObject terrainManagerGO;

        //--------------------------|| METHODS ||----------------------------//

        public PlayerBasedTerrainManager(GameObject terrainManagerGO_)
        {
            terrainManagerGO = terrainManagerGO_;
        }

        public void GenerateChunks(string terrainName) { }

        public void RegisterExistingTerrain() { }

        public void ManageChunks() { }

    }
}

