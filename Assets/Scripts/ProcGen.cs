using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class procGen : MonoBehaviour
{
    // Awake is called when the object is enabled ?
    private void Awake()
    {
        // Create the mesh object
        Mesh mesh = new Mesh();
        mesh.name = "Procedural Mesh";

        // Create the vertices (just 3D coordinates, no other attributes for now, like normals etc)
        List<Vector3> points = new List<Vector3>()
        {
            new Vector3(-0.5f,  0.5f, 0),
            new Vector3( 0.5f,  0.5f, 0),
            new Vector3(-0.5f, -0.5f, 0),
            new Vector3( 0.5f, -0.5f, 0)
        };

        // Create the triangles
        int[] triangles = new int[]
        {
            2, 1, 0,
            2, 3, 1
        };

        // Create normals
        // Note that they are defined in local (mesh) space
        // It's the shader's job to make the transformation to/from world space
        List<Vector3> normals = new List<Vector3>()
        {
            new Vector3(0, 0, 1),
            new Vector3(0, 0, 1),
            new Vector3(0, 0, 1),
            new Vector3(0, 0, 1)
        };

        // Create UVs
        List<Vector2> uvs = new List<Vector2>()
        {
            new Vector2(1, 1),
            new Vector2(0, 1),
            new Vector2(1, 0),
            new Vector2(0, 0)
        };

        // Assign the vertices, triangles, and uvs to the mesh
        mesh.SetVertices(points);
        mesh.triangles = triangles;
        mesh.SetUVs(0, uvs);

        // Automatically compute normals
        //mesh.RecalculateNormals();

        // Assign the normals
        mesh.SetNormals(normals);

        // Assign the mesh to the mesh filter
        GetComponentInChildren<MeshFilter>().sharedMesh = mesh;
    }


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
