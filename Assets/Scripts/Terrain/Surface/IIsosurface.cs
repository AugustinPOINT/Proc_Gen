using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Terrain.Surfaces
{
    public interface IIsosurface
    {
        float surfaceLevel { get; set; }
        float Function(Vector3 v);
    }
}