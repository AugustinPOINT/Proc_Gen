using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Terrain.Surface
{
    [System.Serializable]
    public abstract class SurfaceBuilder
    {
        public abstract void Initialize();

        public abstract void ManageSurface();
    }

    [System.Serializable]
    public class SurfaceBuilderSettings { }
}
