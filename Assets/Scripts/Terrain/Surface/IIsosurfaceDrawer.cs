using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Terrain.Surfaces
{
    public interface IIsosurfaceDrawer
    {
        void ComputeMesh(float size, int subdivisions, Vector3 position, IIsosurface isosurface);
        List<Vector3> GetVertices();
        List<int> GetTriangles();
    }
}