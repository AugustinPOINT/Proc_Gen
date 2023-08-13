using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Terrain;

namespace Terrain.Surface
{
    public class PlayerBasedSurfaceBuilder : SurfaceBuilder
    {
        public Transform parent;
        public PlayerBasedSurfaceBuilderSettings settings;
        public Algorithm algorithm;

        public PlayerBasedSurfaceBuilder(Transform _parent, PlayerBasedSurfaceBuilderSettings _settings, Algorithm _algorithm)
        {
            parent = _parent;
            settings = _settings;
            algorithm = _algorithm;
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
