using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Terrain;
using Terrain.Utils;

namespace Terrain.Surfaces
{
    // Terrain Manager Class
    public class MarchingCubes : IIsosurfaceDrawer
    {
        //------------------------|| ATTRIBUTES ||---------------------------//

        private List<Vector3> vertices;
        private List<int> triangles;


        //--------------------------|| METHODS ||----------------------------//

        public MarchingCubes() { }

        public void ComputeMesh(float size, int subdivisions, Vector3 position, IIsosurface isosurface)
        {

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

