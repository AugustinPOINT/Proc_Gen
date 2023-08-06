using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace terrain.Chunk
{
    public class ChunkMB : MonoBehaviour
    {
        //------------------------|| ATTRIBUTES ||---------------------------//

        public Transform terrainManagerTransform;
        public Chunk chunk; // Contains all the attributes and methods to generate, manage, store the chunk's terrain


        //-------------------------|| METHODS ||-----------------------------//

        /* Sets the reference to the transform that generated this chunk */
        public void SetTerrainManagerTransform(Transform terrainManagerTransform)
        {
            this.terrainManagerTransform = terrainManagerTransform;
        }

        /* Called when the object or its parent get selected */
        private void OnDrawGizmosSelected()
        {
            if (terrainManagerTransform.GetComponent<TerrainManagerMB>().displayWireframe)
            {
                DrawWireRectangle(transform.position, terrainManagerTransform.GetComponent<TerrainManagerMB>().terrainManager.terrainProperties.chunkSize);
            }
        }

        /* Draws the wireframe of the Chunk */
        private void DrawWireRectangle(Vector3 origin, Vector3 size)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawLine(origin, origin + new Vector3(size.x, 0, 0));
            Gizmos.DrawLine(origin + new Vector3(size.x, 0, 0), origin + new Vector3(size.x, size.y, 0));
            Gizmos.DrawLine(origin + new Vector3(size.x, size.y, 0), origin + new Vector3(0, size.y, 0));
            Gizmos.DrawLine(origin + new Vector3(0, size.y, 0), origin);

            Gizmos.DrawLine(origin + new Vector3(0, 0, size.z), origin + new Vector3(size.x, 0, size.z));
            Gizmos.DrawLine(origin + new Vector3(0, 0, size.z) + new Vector3(size.x, 0, 0), origin + new Vector3(size.x, size.y, size.z));
            Gizmos.DrawLine(origin + new Vector3(0, 0, size.z) + new Vector3(size.x, size.y, 0), origin + new Vector3(0, size.y, size.z));
            Gizmos.DrawLine(origin + new Vector3(0, size.y, size.z), origin + new Vector3(0, 0, size.z));

            Gizmos.DrawLine(origin, origin + new Vector3(0, 0, size.z));
            Gizmos.DrawLine(origin + new Vector3(size.x, 0, 0), origin + new Vector3(size.x, 0, size.z));
            Gizmos.DrawLine(origin + new Vector3(0, size.y, 0), origin + new Vector3(0, size.y, size.z));
            Gizmos.DrawLine(origin + new Vector3(size.x, size.y, 0), origin + new Vector3(size.x, size.y, size.z));
        }
    }
}

