using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace terrain
{
    public interface ITerrainManager
    {
        TerrainProperties terrainProperties { get; set; }
        void GenerateChunks(UnityEngine.GameObject a, string b);
        void ManageChunks();
    }
}