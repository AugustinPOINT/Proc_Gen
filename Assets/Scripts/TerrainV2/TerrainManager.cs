using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Terrain.Surface;

namespace Terrain
{
    public class TerrainManager : MonoBehaviour
    {
        public SurfaceBuilder surfaceBuilder;
        [HideInInspector] public Algorithm algorithm;
        public bool displayWireframe = false;

        // Start is called before the first frame update
        void Start()
        {
            // Retrieve terrain settings
            TerrainSettings terrainSettings = this.gameObject.GetComponent<TerrainSettings>();
            if (terrainSettings == null)
            {
                UnityEngine.Debug.LogError("No TerrainSettings component found");
                return;
            }

            // Set the surface builder
            algorithm = terrainSettings.algorithm;
            UnityEngine.Debug.Log("Setting the Surface Builder Algorithm to " + algorithm.ToString() + ".");
            switch (terrainSettings.surfaceBuilderType)
            {
                case SurfaceBuilderType.Static:
                    surfaceBuilder = new StaticSurfaceBuilder(this.transform, terrainSettings.surfaceBuilderSettings as StaticSurfaceBuilderSettings, algorithm);
                    UnityEngine.Debug.Log("Creating a Static Surface Builder.");
                    break;
                case SurfaceBuilderType.PlayerBased:
                    surfaceBuilder = new PlayerBasedSurfaceBuilder(this.transform, terrainSettings.surfaceBuilderSettings as PlayerBasedSurfaceBuilderSettings, algorithm);
                    UnityEngine.Debug.Log("Creating a Player-Based Surface Builder.");
                    break;
            }

            // Initialize the surface builder
            surfaceBuilder.Initialize();
        }

        // Update is called once per frame
        void Update()
        {
            // Manage the surface
            surfaceBuilder.ManageSurface();
        }
    }
}
