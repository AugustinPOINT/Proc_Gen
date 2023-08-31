using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Terrain.Surface;

namespace Terrain.Surface.Drawer
{
    [System.Serializable]
    // DualContouring Class
    public class DualContouring : IIsosurfaceDrawer
    {
        //------------------------|| STATIC ATTRIBUTES ||---------------------------//
        private struct Voxel
        {
            public int index;
            public Vector3 verticePos; // Stores the position of the voxel vertice, wr to the terrain Manager, so no necesarily the world position
            public Vector3 pos; // Stores the voxel position wr ...
            public int verticesFlag; // Stores voxel active vertices
            public int edgesFlag; // Stores edges crossed by the surface
        }
        private struct Vertice
        {
            public float density;
            public Vector3 pos; // Stores the vertice position with respect to the terrainManager, so not necesarily the world position
        }
        [SerializeField] private int[] edgeTable = new int[256]; // From voxel active vertices to voxel crossed edges
        [SerializeField] private int[] cubeEdges = new int[24]; // Stores pairs of vertices representing edges
        private Vector3Int[] verticeCoordinates = new Vector3Int[]{
            new Vector3Int(0,0,0),
            new Vector3Int(1,0,0),
            new Vector3Int(0,0,1),
            new Vector3Int(1,0,1),
            new Vector3Int(0,1,0),
            new Vector3Int(1,1,0),
            new Vector3Int(0,1,1),
            new Vector3Int(1,1,1)
        };

        //------------------------|| NON-STATIC ATTRIBUTES ||---------------------------//
        private List<int> triangles;
        private List<Vector3> vertices;
        private List<Vector3> normals;
        private Voxel[,,] voxels;
        private Vector3 voxelSize;

        //--------------------------|| METHODS ||----------------------------//

        public DualContouring()
        {
            GenerateCubeEdgesTable();
            GenerateIntersectionTable();
        }

        public MeshData ComputeMesh(Vector3 size, Vector3Int subdivisions, Vector3 position, IIsosurface isosurface)
        {
            // Initialize non-static attributes (that changes between mesh generations)
            triangles = new List<int>();
            vertices = new List<Vector3>();
            normals = new List<Vector3>();
            voxels = new Voxel[subdivisions.x + 1, subdivisions.y + 1, subdivisions.z + 1];
            voxelSize = new Vector3(size.x / subdivisions.x, size.y / subdivisions.y, size.z / subdivisions.z);

            GenerateVertices(size, subdivisions, position, isosurface);
            GenerateTriangles(size, subdivisions, position);

            //UnityEngine.Debug.Log("No Dual Contouring mesh drawer currently implemented.");
            MeshData meshData = new MeshData { };
            meshData.vertices = vertices;
            meshData.triangles = triangles;
            return meshData;
        }

        /// <summary>
        /// Computes the vertices of the surface
        /// </summary>
        /// <param name="size"></param>
        /// <param name="subdivisions"></param>
        /// <param name="position"></param>
        /// <param name="isosurface"></param>
        void GenerateVertices(Vector3 size, Vector3Int subdivisions, Vector3 position, IIsosurface isosurface)
        {
            // Initialize a vertices buffer and the ranges of the nested for loops
            int xmax = subdivisions.x + 1; // width
            int ymax = subdivisions.y + 1; // height
            int zmax = subdivisions.z + 1; // depth

            for (int x = 0; x < xmax; x++)
                for (int y = 0; y < ymax; y++)
                    for (int z = 0; z < zmax; z++)
                    {
                        // Initialize the vertices flag of the current voxel
                        voxels[x, y, z].pos = Vector3.Scale(new Vector3(x, y, z), voxelSize);
                        voxels[x, y, z].verticesFlag = 0;

                        // Loop through the neighbours in search of the zero surface level
                        Vertice[] vertex = new Vertice[8];
                        for (int i = 0; i < 8; i++)
                        {
                            vertex[i].pos = voxels[x, y, z].pos + Vector3.Scale(verticeCoordinates[i], voxelSize);
                            vertex[i].density = isosurface.Function(position + vertex[i].pos);
                            voxels[x, y, z].verticesFlag |= ((vertex[i].density >= isosurface.surfaceLevel) ? (1 << i) : 0);
                        }

                        // Skip if the surface level isn't nearby
                        if (voxels[x, y, z].verticesFlag == 0 || voxels[x, y, z].verticesFlag == 0xff)
                            continue;

                        // Initialize the edges flag of the current voxel
                        voxels[x, y, z].edgesFlag = edgeTable[voxels[x, y, z].verticesFlag];
                        int nCrossedEdges = 0;
                        voxels[x, y, z].verticePos = Vector3.zero;

                        // Loop through all of the crossed edges to create the final voxel's vertice
                        for (int i = 0; i < 12; i++)
                        {
                            if (((voxels[x, y, z].edgesFlag & (1 << i)) == 0x00))
                                continue;
                            nCrossedEdges++;

                            // Move the vertex in the edge direction depending on the edge's density values
                            int vertice1 = cubeEdges[2 * i];
                            int vertice2 = cubeEdges[2 * i + 1];
                            float density1 = vertex[vertice1].density;                                // Density of the first edge's vertice
                            float density2 = vertex[vertice2].density;                                // Density of the second edge's vertice
                            float t = (isosurface.surfaceLevel - density1) / (density2 - density1);     // Lerp value
                            voxels[x, y, z].verticePos += Vector3.Lerp(vertex[vertice1].pos, vertex[vertice2].pos, t);
                        }
                        voxels[x, y, z].verticePos /= nCrossedEdges;
                    }
        }

        /// <summary>
        /// Creates the triangles of the surface from the computed vertices
        /// </summary>
        /// <param name="size"></param>
        /// <param name="subdivisions"></param>
        /// <param name="position"></param>
        /// <param name="isosurface"></param>
        void GenerateTriangles(Vector3 size, Vector3Int subdivisions, Vector3 position)
        {
            // Init loop variables
            int xmax = subdivisions.x + 1; // width
            int ymax = subdivisions.y + 1; // height
            int zmax = subdivisions.z + 1; // depth
            int[] pos = new int[3] { 0, 0, 0 };
            // Init a vertice index buffer
            int[] buffer = new int[2 * xmax * ymax];
            int bufferIndex = 0; // Position in the buffer to place the vertice ID
            int bufferSlice = 1; // 1 or 0, to access the first/second half of buffer
            int nVertices = 0; // Number of added vertices
            int[] bufferOffsets = new int[] { 1, ymax, xmax * ymax }; // Offsets to find voxel neighbors in the x,y,z directions

            // Loop along voxels
            for (pos[2] = 0; pos[2] < zmax; pos[2]++)
            {
                // Set the buffer's origin (first element of the first or second slice)
                bufferIndex = bufferSlice * xmax * ymax;
                for (pos[1] = 0; pos[1] < ymax; pos[1]++)
                    for (pos[0] = 0; pos[0] < xmax; pos[0]++, bufferIndex++)
                    {
                        // Get the edges and vertices flags we calculated earlier
                        int verticesFlag = voxels[pos[0], pos[1], pos[2]].verticesFlag;
                        int edgesFlag = voxels[pos[0], pos[1], pos[2]].edgesFlag;

                        // Early Termination Check
                        if (verticesFlag == 0 || verticesFlag == 0xff)
                            continue;

                        //Add Vertex to Buffer, Store Pointer to Vertex Index
                        nVertices++; // Increment vertice's number (1 more than vertice index)
                        vertices.Add(voxels[pos[0], pos[1], pos[2]].verticePos);
                        buffer[bufferIndex] = nVertices - 1;

                        //Add Faces (Loop Over 3 negative neighbors)
                        for (int i = 0; i < 3; i++)
                        {
                            // Look for the active edges
                            if (!Convert.ToBoolean(edgesFlag & (1 << i)))
                                continue;

                            // iu, iv : directions colinear to the edge (ex : edge 0 in x direction gets du = 1 (y) and dv = 2 (z))
                            int iu = (4 - i) % 3; // i=0,1,2 => iu = 1,0,2
                            int iv = (5 - i) % 3; // i=0,1,2 => iv = 2,1,0

                            // Skip if the current edge belongs to the boundary
                            if (pos[iu] == 0 || pos[iv] == 0)
                                continue;

                            // Get the vertice offset, depending on the direction
                            int du = bufferOffsets[iu];
                            int dv = bufferOffsets[iv];

                            // Create the triangles with the correct normals
                            if (Convert.ToBoolean(verticesFlag & 1)) // Negative corner vertice is activated
                            {
                                triangles.Add(buffer[bufferIndex]);
                                triangles.Add(buffer[bufferIndex - du - dv]);
                                triangles.Add(buffer[bufferIndex - du]);
                                triangles.Add(buffer[bufferIndex]);
                                triangles.Add(buffer[bufferIndex - dv]);
                                triangles.Add(buffer[bufferIndex - du - dv]);
                            }
                            else
                            {
                                triangles.Add(buffer[bufferIndex]);
                                triangles.Add(buffer[bufferIndex - du]);
                                triangles.Add(buffer[bufferIndex - du - dv]);
                                triangles.Add(buffer[bufferIndex]);
                                triangles.Add(buffer[bufferIndex - du - dv]);
                                triangles.Add(buffer[bufferIndex - dv]);
                            }
                        }
                    }
                // Swap between the first/second slice of the buffer
                bufferSlice ^= 1;
                bufferOffsets[2] *= -1;
            }
        }

        /// <summary>
        /// Generate a table that stores pairs of vertice representing each edge of a voxel 
        /// </summary>
        void GenerateCubeEdgesTable()
        {
            int k = 0;
            for (int i = 0; i < 8; ++i)
            {
                for (int j = 1; j <= 4; j <<= 1)
                {
                    int p = i ^ j;
                    if (i <= p)
                    {
                        cubeEdges[k++] = i;
                        cubeEdges[k++] = p;
                    }
                }
            }
        }

        /// <summary>
        /// Generate a table that stores for each vertice configuration of a voxel, the edges that should be crossed by the surface.
        /// </summary>
        void GenerateIntersectionTable()
        {
            for (int i = 0; i < 256; ++i)
            {
                int edgeId = 0;
                for (int j = 0; j < 24; j += 2)
                {
                    var a = Convert.ToBoolean(i & (1 << cubeEdges[j]));
                    var b = Convert.ToBoolean(i & (1 << cubeEdges[j + 1]));
                    edgeId |= a != b ? (1 << (j >> 1)) : 0; // "j>>1 is a division by 2. So for each edge whose vertices aren't of the same sign, add a 1 bit to em at the edge index position
                }
                edgeTable[i] = edgeId;
            }
        }

    }
}
