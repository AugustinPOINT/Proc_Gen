using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Terrain;
using Terrain.Utils;
using Terrain.Surfaces;

namespace Terrain.Chunk
{
    /* Chunk monobehavior class.
        This class is meant to be fully managed by the terrain manager, so it contains no reference to it.
    */
    public class ChunkMB : MonoBehaviour
    {
        //------------------------|| ATTRIBUTES ||---------------------------//
        public Vector3Int index;
        public float size;
        public int subdivisions;
        public Vector3 position;
        public IIsosurfaceDrawer isosurfaceDrawer;
        private MeshFilter meshFilter;

        //-------------------------|| METHODS ||-----------------------------//

        public void Initialize(Vector3Int index_, float size_, int subdivisions_)
        {
            // Initialize attributes
            index = index_;
            size = size_;
            subdivisions = subdivisions_; //Maybe move this attribute inside the GenerateTerrain function
            position = this.transform.position;
            meshFilter = this.transform.GetComponent<MeshFilter>();
        }

        /* Called when the object or its parent get selected */
        private void OnDrawGizmos()
        {
            Transform terrainManagerTransform = this.transform.parent.parent;
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

        public void SetSurfaceDrawerAlgorithm(Tools.surfaceDrawerAlgorithm algorithm)
        {
            switch (algorithm)
            {
                case Tools.surfaceDrawerAlgorithm.SurfaceNets:
                    isosurfaceDrawer = new SurfaceNets();
                    break;
                case Tools.surfaceDrawerAlgorithm.MarchingCubes:
                    isosurfaceDrawer = new MarchingCubes();
                    break;
                case Tools.surfaceDrawerAlgorithm.DualContouring:
                    isosurfaceDrawer = new DualContouring();
                    break;
            }
        }

        public void GenerateTerrain(IIsosurface isosurface)
        {
            // Verify that there is a mesh filter attached
            if (meshFilter == null)
            {
                UnityEngine.Debug.Log("No mesh filter assigned to the GameObject");
                return;
            }
            // Create a new mesh
            Mesh mesh = new Mesh();
            mesh.name = "Terrain";
            // Generate the vertices and triangles
            isosurfaceDrawer.ComputeMesh(size, subdivisions, position, isosurface);
            // Assign them to the mesh
            mesh.SetVertices(isosurfaceDrawer.GetVertices());
            mesh.triangles = isosurfaceDrawer.GetTriangles().ToArray();
            // Compute normals
            mesh.RecalculateNormals();
            // Put the mesh in the mesh filter for display
            meshFilter.sharedMesh = mesh;
        }
    }
}

