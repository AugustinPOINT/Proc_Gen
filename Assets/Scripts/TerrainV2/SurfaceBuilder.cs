using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Terrain.Surface
{
    public class SurfaceBuilder
    {
        public virtual void Initialize() { }

        public virtual void ManageSurface() { }
    }

    public class SurfaceBuilderSettings { }
}
