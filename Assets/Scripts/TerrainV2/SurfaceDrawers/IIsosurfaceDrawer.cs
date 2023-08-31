using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Terrain.Surface.Drawer
{
    public interface IIsosurfaceDrawer
    {
        MeshData ComputeMesh(Vector3 size, Vector3Int subdivisions, Vector3 position, IIsosurface isosurface);
    }

    [System.Serializable]
    public struct MeshData
    {
        public List<int> triangles;
        public List<Vector3> vertices;
        public List<Vector3> normals;
    }
}