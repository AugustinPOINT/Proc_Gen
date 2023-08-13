using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Terrain;

namespace Terrain.Surface
{
    public class StaticSurfaceBuilder : SurfaceBuilder
    {
        public Transform parent;
        public StaticSurfaceBuilderSettings settings;
        public Algorithm algorithm;
        public Chunk[,,] chunks;
        private string surfaceName = "surface";
        private bool surfaceGenerated;
        private IIsosurface ground;

        public StaticSurfaceBuilder(Transform _parent, StaticSurfaceBuilderSettings _settings, Algorithm _algorithm)
        {
            parent = _parent;
            settings = _settings;
            algorithm = _algorithm;
        }

        public override void Initialize()
        {
            GenerateChunks();
            surfaceGenerated = false;
        }

        public override void ManageSurface()
        {
            if (!surfaceGenerated)
            {
                foreach (Chunk chunk in chunks)
                {
                    chunk.SetSurfaceDrawerAlgorithm(algorithm);
                    chunk.GenerateSurface(ground);
                }
                surfaceGenerated = true;
            }
        }

        private void GenerateChunks()
        {
            //Create surface root GameObject
            GameObject rootSurfaceGO = new GameObject(surfaceName);
            rootSurfaceGO.transform.SetParent(parent);
            rootSurfaceGO.transform.SetLocalPositionAndRotation(new Vector3(0, 0, 0), Quaternion.identity);

            // Populate the terrain with chunks
            int index = 0;
            chunks = new Chunk[settings.terrainResolution.x, settings.terrainResolution.y, settings.terrainResolution.z];
            Vector3 chunkSize = new Vector3(settings.terrainSize.x / settings.terrainResolution.x, settings.terrainSize.y / settings.terrainResolution.y, settings.terrainSize.z / settings.terrainResolution.z);
            for (int chunkX = 0; chunkX < settings.terrainResolution.x; chunkX++)
            {
                for (int chunkY = 0; chunkY < settings.terrainResolution.y; chunkY++)
                {
                    for (int chunkZ = 0; chunkZ < settings.terrainResolution.z; chunkZ++)
                    {
                        string chunkName = string.Format("chunk({0},{1},{2})", chunkX, chunkY, chunkZ);
                        GameObject chunkGO = new GameObject(chunkName);
                        Vector3 chunkLocalPosition = new Vector3(chunkX * chunkSize.x, chunkY * chunkSize.y, chunkZ * chunkSize.z);
                        chunkGO.transform.SetParent(rootSurfaceGO.transform);
                        chunkGO.transform.SetLocalPositionAndRotation(chunkLocalPosition, Quaternion.identity);
                        // Add the MeshFilter and MeshRenderer components
                        chunkGO.AddComponent<MeshFilter>();
                        chunkGO.AddComponent<MeshRenderer>();
                        // Add the ChunkMB component and initializes it. Note that the subdiv is decided by the manager, to choose a subdivision level in real-time if we want.
                        Chunk chunk = chunkGO.AddComponent<Chunk>();
                        chunks[chunkX, chunkY, chunkZ] = chunk;
                        chunk.Initialize(new Vector3Int(chunkX, chunkY, chunkZ), chunkSize, settings.chunkResolution);
                        index++;
                    }
                }
            }
        }

    }

    [System.Serializable]
    public class StaticSurfaceBuilderSettings : SurfaceBuilderSettings
    {
        [SerializeField] public Vector3 terrainSize;
        [SerializeField] public Vector3Int terrainResolution;
        [SerializeField] public Vector3Int chunkResolution;
    }
}