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
        public Vector3Int index;
        public Vector3 size;
        public Vector3Int subdivisions;
        public Vector3 globalPosition;
        public IIsosurfaceDrawer isosurfaceDrawer;
        private MeshFilter meshFilter;

        //-------------------------|| METHODS ||-----------------------------//

        public void Initialize(Vector3Int index_, Vector3 size_, Vector3Int subdivisions_)
        {
            // Initialize attributes
            index = index_;
            size = size_;
            subdivisions = subdivisions_; //Maybe move this attribute inside the GenerateTerrain function
            globalPosition = this.transform.position;
            meshFilter = this.transform.GetComponent<MeshFilter>();
        }

        public void SetSurfaceDrawerAlgorithm(Algorithm algorithm)
        {
            switch (algorithm)
            {
                case Algorithm.SurfaceNets:
                    isosurfaceDrawer = new SurfaceNets();
                    break;
                case Algorithm.MarchingCubes:
                    isosurfaceDrawer = new MarchingCubes();
                    break;
                case Algorithm.DualContouring:
                    isosurfaceDrawer = new DualContouring();
                    break;
                case Algorithm.Transvoxel:
                    isosurfaceDrawer = new Transvoxel();
                    break;
            }
        }

        public void GenerateSurface(IIsosurface isosurface)
        {
            isosurfaceDrawer.ComputeMesh(size, subdivisions, globalPosition, isosurface);
        }

        public void OnDrawGizmosSelected()
        {
            Transform terrainManagerTransform = this.transform.parent.parent;
            if (terrainManagerTransform.GetComponent<TerrainManager>().displayWireframe)
            {
                DrawWireframe(globalPosition, size);
                if (Selection.activeGameObject == this.gameObject)
                {
                    DrawGrid(globalPosition, size, subdivisions);
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
