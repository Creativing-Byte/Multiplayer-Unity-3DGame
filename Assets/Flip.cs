using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flip : MonoBehaviour
{
    public bool flipX = true;
    public bool flipY;
    public bool flipZ;
    public Mesh mesh;

    void Flipz()
    {
        if (mesh == null) return;
        Vector3[] verts = mesh.vertices;
        for (int i = 0; i < verts.Length; i++)
        {
            Vector3 c = verts[i];
            if (flipX) c.x *= -1;
            if (flipY) c.y *= -1;
            if (flipZ) c.z *= -1;
            verts[i] = c;
        }

        mesh.vertices = verts;
        if (flipX ^ flipY ^ flipZ) FlipNormals();
    }
    void FlipNormals()
    {
        int[] tris = mesh.triangles;
        for (int i = 0; i < tris.Length / 3; i++)
        {
            int a = tris[i * 3 + 0];
            int b = tris[i * 3 + 1];
            int c = tris[i * 3 + 2];
            tris[i * 3 + 0] = c;
            tris[i * 3 + 1] = b;
            tris[i * 3 + 2] = a;
        }
        mesh.triangles = tris;
    }
}
