using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Terrain.Surface;

namespace Terrain.Surface.Drawer
{
    // Terrain Manager Class
    public class SurfaceNets : IIsosurfaceDrawer
    {
        //------------------------|| ATTRIBUTES ||---------------------------//

        private List<int> triangles;
        private List<Vector3> vertices;
        private List<Vector3> normals;


        //--------------------------|| METHODS ||----------------------------//

        public SurfaceNets() { }

        public MeshData ComputeMesh(Vector3 size, Vector3Int subdivisions, Vector3 position, IIsosurface isosurface)
        {
            UnityEngine.Debug.Log("No Surface Nets mesh drawer currently implemented.");
            MeshData meshData = new MeshData { };
            return meshData;
        }

    }
}