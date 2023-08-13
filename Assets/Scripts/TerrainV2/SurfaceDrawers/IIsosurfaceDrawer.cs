using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Terrain.Surface.Drawer
{
    public interface IIsosurfaceDrawer
    {
        MeshData ComputeMesh(Vector3 size, Vector3Int subdivisions, Vector3 position, IIsosurface isosurface);
    }

    public struct MeshData
    {
        List<int> triangles;
        List<Vector3> vertices;
        List<Vector3> normals;
    }
}