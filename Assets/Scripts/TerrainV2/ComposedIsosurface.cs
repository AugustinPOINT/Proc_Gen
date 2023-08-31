using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Terrain.Surface
{
    /* Isosurface class.
        Will require later an editor to be able to instanciate such functions, with a given input string as the isosurface function.
        It can also be useful to add parameters such as scale, rotation.
    */
    public class ComposedIsosurface : IIsosurface
    {
        public float surfaceLevel_ = 0;
        public float surfaceLevel { get => surfaceLevel_; set => surfaceLevel_ = value; }
        public SingularIsosurface[] isosurfaces = new SingularIsosurface[] { };

        public float Function(Vector3 position)
        {
            float retValue = 0;
            foreach (SingularIsosurface isosurface in isosurfaces)
            {
                retValue += isosurface.Function(position);
            }
            return retValue;
            //return position.x + position.y + position.z;
            //return Mathf.Pow(position.x, 2) + Mathf.Pow(position.z, 2) + 10 - 2 * Mathf.Sin(position.y);
        }

    }
}