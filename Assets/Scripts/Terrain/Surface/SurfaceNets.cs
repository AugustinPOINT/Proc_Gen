using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Terrain;
using Terrain.Utils;

namespace Terrain.Surfaces
{
    // Terrain Manager Class
    public class SurfaceNets : IIsosurfaceDrawer
    {
        //------------------------|| ATTRIBUTES ||---------------------------//

        private List<Vector3> vertices;
        private List<int> triangles;


        //--------------------------|| METHODS ||----------------------------//

        public SurfaceNets() { }

        public void ComputeMesh(float size, int subdivisions, Vector3 position, IIsosurface isosurface)
        {
            // Initialize the arrays
            vertices = new List<Vector3>();
            triangles = new List<int>();

            // Iterate over the voxels in the positive direction
            for (int x = 0; x < subdivisions; x++)
            {
                for (int y = 0; y < subdivisions; y++)
                {
                    for (int z = 0; z < subdivisions; z++)
                    {
                        // check 8 neighboring voxels
                        int sum = 0;
                        for (int dx = x; dx <= x + 1; dx++)
                        {
                            for (int dy = y; dy <= y + 1; dy++)
                            {
                                for (int dz = z; dz <= z + 1; dz++)
                                {
                                    // If a voxel exists, add one to our (check-)sum
                                    Vector3 originPoint = new Vector3(dx, dy, dz) * size / subdivisions + position;
                                    Vector3 middlePoint = originPoint + Vector3.one * size / (2 * subdivisions);
                                    if (isosurface.Function(middlePoint) >= isosurface.surfaceLevel)
                                    {
                                        sum++;
                                    }
                                }
                            }
                        }

                        // The sum is only 0 or 8 if all checked blocks are the same
                        if (sum % 8 != 0)
                        {
                            // Surface Cube found, add Vertex
                            GameObject cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
                            Vector3 originPoint = new Vector3(x, y, z) * size / subdivisions + position;
                            cube.transform.position = originPoint;
                            cube.transform.localScale = Vector3.one * size / subdivisions;
                        }
                    }
                }
            }
        }

        public List<Vector3> GetVertices()
        {
            return vertices;
        }

        public List<int> GetTriangles()
        {
            return triangles;
        }
    }

}

