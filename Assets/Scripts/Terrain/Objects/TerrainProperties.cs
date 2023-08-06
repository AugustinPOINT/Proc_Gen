using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using terrain.Chunk;
using System.Diagnostics;

namespace terrain
{
    [CreateAssetMenu(fileName = "TerrainProperties", menuName = "Terrain Properties")]
    public class TerrainProperties : ScriptableObject
    {
        //------------------------|| ATTRIBUTES ||---------------------------//

        public Vector3 chunkSize;
        public Vector3 terrainDimensions;


        //--------------------------|| METHODS ||----------------------------//

        public TerrainProperties()
        {
            chunkSize = new Vector3(32, 32, 32);
            terrainDimensions = new Vector3(5, 1, 5);
        }
    }
}