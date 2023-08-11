using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Terrain.Utils;

namespace Terrain.Surfaces
{
    [CreateAssetMenu(fileName = "Isosurface", menuName = "Isosurface")]
    /* Isosurface class.
        Will require later an editor to be able to instanciate such functions, with a given input string as the isosurface function.
        It can also be useful to add parameters such as scale, rotation.
    */
    public class Isosurface : ScriptableObject, IIsosurface
    {
        public float surfaceLevel_ = 0;
        public float surfaceLevel { get => surfaceLevel_; set => surfaceLevel_ = value; }
        public float scale = 4;
        public Vector3 shift = new Vector3(16, 16, 16);
        public float Function(Vector3 position)
        {
            float x = (position.x - shift[0]) / scale;
            float y = (position.y - shift[1]) / scale;
            float z = (position.z - shift[2]) / scale;
            return Mathf.Min(Mathf.Pow(x, 2) + Mathf.Pow(y, 2) + Mathf.Pow(z, 2) - 9 // Sphere
                    , Mathf.Pow(x / 2, 2) + Mathf.Pow(z / 2, 2) - 1); //Cylinder
            //return position.x + position.y + position.z;
            //return Mathf.Pow(position.x, 2) + Mathf.Pow(position.z, 2) + 10 - 2 * Mathf.Sin(position.y);
        }

    }
}