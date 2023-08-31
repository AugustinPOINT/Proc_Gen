using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Terrain.Surface;

namespace Terrain
{
    public class TerrainSettings : MonoBehaviour
    {
        public SurfaceBuilderType surfaceBuilderType;
        [SerializeReference] public SurfaceBuilderSettings surfaceBuilderSettings;
        public Algorithm algorithm;
        public IsosurfaceType isosurfaceType;
        [SerializeReference] public IIsosurface isosurface;
    }

    public enum SurfaceBuilderType
    {
        Static,
        PlayerBased,
    }

    public enum Algorithm
    {
        MarchingCubes,
        SurfaceNets,
        DualContouring,
        Transvoxel
    }

    public enum IsosurfaceType
    {
        Primitive,
        Functional,
        Composed,
    }
}
