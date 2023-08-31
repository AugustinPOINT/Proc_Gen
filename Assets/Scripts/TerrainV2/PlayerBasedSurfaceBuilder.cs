using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Terrain;

namespace Terrain.Surface
{
    [System.Serializable]
    public class PlayerBasedSurfaceBuilder : SurfaceBuilder
    {
        public Transform parent;
        public Algorithm algorithm;
        [SerializeReference] private IIsosurface ground;

        public PlayerBasedSurfaceBuilderSettings settings;

        public PlayerBasedSurfaceBuilder(Transform _parent, PlayerBasedSurfaceBuilderSettings _settings, Algorithm _algorithm, IIsosurface isosurface)
        {
            parent = _parent;
            settings = _settings;
            algorithm = _algorithm;
            ground = isosurface;
        }

        public override void Initialize()
        {
        }

        public override void ManageSurface() { }
    }

    [System.Serializable]
    public class PlayerBasedSurfaceBuilderSettings : SurfaceBuilderSettings
    {
        [SerializeField] private Vector3 terrainSize;
        [SerializeField] private Vector3Int terrainResolution;
        [SerializeField] private Vector3Int chunkResolution;
        [SerializeField] private GameObject player;
        [SerializeField] private float maxLoadDistance;
    }
}
