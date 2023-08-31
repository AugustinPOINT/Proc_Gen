using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using Terrain.Surface.Drawer;

namespace Terrain.Surface
{
    public class Chunk : MonoBehaviour
    {
        //------------------------|| ATTRIBUTES ||---------------------------//
        private Vector3Int index;
        private Vector3 size;
        private Vector3Int subdivisions;
        [SerializeField] private Vector3 globalPosition;
        [SerializeField] private Vector3 localPosition;
        private MeshFilter meshFilter;
        private MeshRenderer meshRenderer;
        [SerializeField] private MeshData meshData;
        [SerializeField] private bool displayPoints = true;
        [SerializeField] private bool displayChunkWireframe = true;

        //-------------------------|| METHODS ||-----------------------------//

        public void Initialize(Vector3Int index_, Vector3 size_, Vector3Int subdivisions_)
        {
            // Initialize attributes
            index = index_;
            size = size_;
            subdivisions = subdivisions_; //Maybe move this attribute inside the GenerateTerrain function
            globalPosition = this.transform.position;
            localPosition = this.transform.localPosition;
            meshFilter = this.transform.GetComponent<MeshFilter>();
            meshRenderer = this.transform.GetComponent<MeshRenderer>();
        }

        public void GenerateSurface(IIsosurfaceDrawer isosurfaceDrawer, IIsosurface isosurface)
        {
            // Compute the mesh
            meshData = isosurfaceDrawer.ComputeMesh(size, subdivisions, localPosition, isosurface);
            Mesh mesh = new Mesh();
            mesh.name = "Procedural Mesh";
            // Assign the vertices, triangles, (and uvs) to the mesh
            mesh.SetVertices(meshData.vertices);
            mesh.triangles = meshData.triangles.ToArray();

            // Automatically compute normals
            mesh.RecalculateNormals();

            // Assign the mesh to the mesh filter
            meshFilter.sharedMesh = mesh;

            // Set material
            meshRenderer.material = new Material(Shader.Find("Diffuse"));
        }

        /// <summary>
        /// Draws the chunk information in the editor when it or its parent is selected
        /// </summary>
        public void OnDrawGizmosSelected()
        {
            Transform terrainManagerTransform = this.transform.parent.parent;
            if (terrainManagerTransform.GetComponent<TerrainManager>().displayWireframe)
            { // Draw the chunk borders on selection
                DrawWireframe(globalPosition, size);
                if (Selection.activeGameObject == this.gameObject)
                {
                    if (displayChunkWireframe)
                    { // Draw the chunk divisions only when the chunk is selected, and if the trigger is on
                        DrawGrid(globalPosition, size, subdivisions);
                    }
                    if (displayPoints)
                    { // Draws the triangulated points only when the chunk is selected, and if the trigger is on
                        Gizmos.color = Color.red;
                        for (int i = 0; i < meshData.vertices.Count; i++)
                        {
                            Gizmos.DrawSphere(meshData.vertices[i] + globalPosition, 0.1f);
                        }
                    }
                }
            }
        }

        /* Draws the wireframe of the Chunk */
        private void DrawWireframe(Vector3 origin, Vector3 dims)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawLine(origin, origin + new Vector3(dims.x, 0, 0));
            Gizmos.DrawLine(origin + new Vector3(dims.x, 0, 0), origin + new Vector3(dims.x, dims.y, 0));
            Gizmos.DrawLine(origin + new Vector3(dims.x, dims.y, 0), origin + new Vector3(0, dims.y, 0));
            Gizmos.DrawLine(origin + new Vector3(0, dims.y, 0), origin);

            Gizmos.DrawLine(origin + new Vector3(0, 0, dims.z), origin + new Vector3(dims.x, 0, dims.z));
            Gizmos.DrawLine(origin + new Vector3(0, 0, dims.z) + new Vector3(dims.x, 0, 0), origin + new Vector3(dims.x, dims.y, dims.z));
            Gizmos.DrawLine(origin + new Vector3(0, 0, dims.z) + new Vector3(dims.x, dims.y, 0), origin + new Vector3(0, dims.y, dims.z));
            Gizmos.DrawLine(origin + new Vector3(0, dims.y, dims.z), origin + new Vector3(0, 0, dims.z));

            Gizmos.DrawLine(origin, origin + new Vector3(0, 0, dims.z));
            Gizmos.DrawLine(origin + new Vector3(dims.x, 0, 0), origin + new Vector3(dims.x, 0, dims.z));
            Gizmos.DrawLine(origin + new Vector3(0, dims.y, 0), origin + new Vector3(0, dims.y, dims.z));
            Gizmos.DrawLine(origin + new Vector3(dims.x, dims.y, 0), origin + new Vector3(dims.x, dims.y, dims.z));
        }

        private void DrawGrid(Vector3 origin, Vector3 _size, Vector3Int _subdivisions)
        {
            Gizmos.color = Color.blue;
            Vector3 cellSize = new Vector3(_size.x / _subdivisions.x, size.y / _subdivisions.y, size.z / _subdivisions.z);
            // x axis
            for (int y = 0; y <= _subdivisions.y; y++)
                for (int z = 0; z <= _subdivisions.z; z++)
                {
                    if ((y % _subdivisions.y) != 0 || (z % _subdivisions.z) != 0)
                        Gizmos.DrawLine(origin + new Vector3(0f, y * cellSize.y, z * cellSize.z), origin + new Vector3(size.x, y * cellSize.y, z * cellSize.z));
                }
            // y axis
            for (int x = 0; x <= _subdivisions.x; x++)
                for (int z = 0; z <= _subdivisions.z; z++)
                {
                    if ((x % _subdivisions.x) != 0 || (z % _subdivisions.z) != 0)
                        Gizmos.DrawLine(origin + new Vector3(x * cellSize.x, 0f, z * cellSize.z), origin + new Vector3(x * cellSize.x, size.y, z * cellSize.z));
                }
            // z axis
            for (int x = 0; x <= _subdivisions.x; x++)
                for (int y = 0; y <= _subdivisions.y; y++)
                {
                    if ((x % _subdivisions.x) != 0 || (y % _subdivisions.y) != 0)
                        Gizmos.DrawLine(origin + new Vector3(x * cellSize.x, y * cellSize.y, 0f), origin + new Vector3(x * cellSize.x, y * cellSize.y, size.z));
                }
        }

    }
}
