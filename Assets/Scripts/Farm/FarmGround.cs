using UnityEngine;
using System.Collections.Generic;

[System.Serializable]
public class FarmGround
{
    private Vector3 meshSize;
    private Vector3[] vertices;
    private Vector3[] normals;
    private Vector2[] uv;
    private int[] triangles;
    private FarmCell[] cells;

    [System.NonSerialized] private Mesh mesh;

    public Mesh Initialize(Vector3 size)
    {
        meshSize = size;
        if (vertices == null || normals == null || uv == null) mesh = newMesh(size.x, size.z); // New game or error loading data

        mesh.vertices = vertices;
        mesh.normals = normals;
        mesh.uv = uv;
        mesh.triangles = triangles;

        return mesh;
    }

    public Mesh Plow(Vector3 position)
    {
        Vector3 roundedPosition = new Vector3(Mathf.Round(position.x - 0.5f) / meshSize.x, 0, Mathf.Round(position.z - 0.5f) / meshSize.z);
        int cellId = Mathf.RoundToInt((roundedPosition.x + 0.5f) * 100) + Mathf.RoundToInt((roundedPosition.z + 0.5f) * 10);

        if (cells[cellId] != null)
        {
            Debug.Log("Already plowed");
            return mesh;
        }

        cells[cellId] = new FarmCell();

        int startingVertex = 20 + cellId * 4;
        for (int i = 0; i < 4; i++)
        {
            vertices[startingVertex + i] = new Vector3(vertices[startingVertex + i].x, vertices[startingVertex + i].y - 0.1f, vertices[startingVertex + i].z);
        }
        mesh.vertices = vertices;

        return mesh;
    }

    private Mesh newMesh(float xSize, float ySize)
    {
        Mesh mesh = new Mesh();

        int numberOfVertices = 20 + Mathf.RoundToInt(xSize * ySize * 16); // Sides (4 * 4) + floor (4) vertices, face to modify vertices (4x * 4y units)
        int numberOfTriangles = (10 + Mathf.RoundToInt(xSize * ySize * 32)) * 3; // Sides (4 * 2) + floor (2) triangles, face to modify triangles (4x * 4y units * 2), two triangles per square

        vertices = new Vector3[numberOfVertices];
        normals = new Vector3[numberOfVertices];
        uv = new Vector2[numberOfVertices];
        triangles = new int[numberOfTriangles];

        // Floor
        vertices[0] = new Vector3(-0.5f, -0.5f, -0.5f);
        vertices[1] = new Vector3(0.5f, -0.5f, -0.5f);
        vertices[2] = new Vector3(0.5f, -0.5f, 0.5f);
        vertices[3] = new Vector3(-0.5f, -0.5f, 0.5f);
        normals[0] = new Vector3(0, -1, 0);
        normals[1] = new Vector3(0, -1, 0);
        normals[2] = new Vector3(0, -1, 0);
        normals[3] = new Vector3(0, -1, 0);
        uv[0] = new Vector2(0.0f, 0.0f);
        uv[1] = new Vector2(1.0f, 0.0f);
        uv[2] = new Vector2(1.0f, 1.0f);
        uv[3] = new Vector2(0.0f, 1.0f);
        triangles[0] = 0;
        triangles[1] = 1;
        triangles[2] = 2;
        triangles[3] = 2;
        triangles[4] = 3;
        triangles[5] = 0;

        // Sides
        // Down left
        vertices[4] = new Vector3(0.5f, -0.5f, -0.5f);
        vertices[5] = new Vector3(-0.5f, -0.5f, -0.5f);
        vertices[6] = new Vector3(-0.5f, 0.5f, -0.5f);
        vertices[7] = new Vector3(0.5f, 0.5f, -0.5f);
        normals[4] = new Vector3(0, 0, -1);
        normals[5] = new Vector3(0, 0, -1);
        normals[6] = new Vector3(0, 0, -1);
        normals[7] = new Vector3(0, 0, -1);
        uv[4] = new Vector2(0.0f, 0.0f);
        uv[5] = new Vector2(1.0f, 0.0f);
        uv[6] = new Vector2(1.0f, 1.0f);
        uv[7] = new Vector2(0.0f, 1.0f);
        triangles[6] = 4;
        triangles[7] = 5;
        triangles[8] = 6;
        triangles[9] = 6;
        triangles[10] = 7;
        triangles[11] = 4;
        // Down right
        vertices[8] = new Vector3(0.5f, -0.5f, 0.5f);
        vertices[9] = new Vector3(0.5f, -0.5f, -0.5f);
        vertices[10] = new Vector3(0.5f, 0.5f, -0.5f);
        vertices[11] = new Vector3(0.5f, 0.5f, 0.5f);
        normals[8] = new Vector3(1, 0, 0);
        normals[9] = new Vector3(1, 0, 0);
        normals[10] = new Vector3(1, 0, 0);
        normals[11] = new Vector3(1, 0, 0);
        uv[8] = new Vector2(0.0f, 0.0f);
        uv[9] = new Vector2(1.0f, 0.0f);
        uv[10] = new Vector2(1.0f, 1.0f);
        uv[11] = new Vector2(0.0f, 1.0f);
        triangles[12] = 8;
        triangles[13] = 9;
        triangles[14] = 10;
        triangles[15] = 10;
        triangles[16] = 11;
        triangles[17] = 8;
        // Up left
        vertices[12] = new Vector3(-0.5f, -0.5f, -0.5f);
        vertices[13] = new Vector3(-0.5f, -0.5f, 0.5f);
        vertices[14] = new Vector3(-0.5f, 0.5f, 0.5f);
        vertices[15] = new Vector3(-0.5f, 0.5f, -0.5f);
        normals[12] = new Vector3(-1, 0, 0);
        normals[13] = new Vector3(-1, 0, 0);
        normals[14] = new Vector3(-1, 0, 0);
        normals[15] = new Vector3(-1, 0, 0);
        uv[12] = new Vector2(0.0f, 0.0f);
        uv[13] = new Vector2(1.0f, 0.0f);
        uv[14] = new Vector2(1.0f, 1.0f);
        uv[15] = new Vector2(0.0f, 1.0f);
        triangles[18] = 12;
        triangles[19] = 13;
        triangles[20] = 14;
        triangles[21] = 14;
        triangles[22] = 15;
        triangles[23] = 12;
        // Up right
        vertices[16] = new Vector3(-0.5f, -0.5f, 0.5f);
        vertices[17] = new Vector3(0.5f, -0.5f, 0.5f);
        vertices[18] = new Vector3(0.5f, 0.5f, 0.5f);
        vertices[19] = new Vector3(-0.5f, 0.5f, 0.5f);
        normals[16] = new Vector3(0, 0, 1);
        normals[17] = new Vector3(0, 0, 1);
        normals[18] = new Vector3(0, 0, 1);
        normals[19] = new Vector3(0, 0, 1);
        uv[16] = new Vector2(0.0f, 0.0f);
        uv[17] = new Vector2(1.0f, 0.0f);
        uv[18] = new Vector2(1.0f, 1.0f);
        uv[19] = new Vector2(0.0f, 1.0f);
        triangles[24] = 16;
        triangles[25] = 17;
        triangles[26] = 18;
        triangles[27] = 18;
        triangles[28] = 19;
        triangles[29] = 16;

        // Modifiable vertices
        int currentVertice = 20;
        int currentTriangle = 30;
        float squareSizeX = 1.0f / xSize;
        float squareSizeY = 1.0f / ySize;
        for (int i = 0; i < xSize; i++)
        {
            for (int j = 0; j < ySize; j++)
            {
                vertices[currentVertice] = new Vector3(-0.5f + squareSizeX * i, 0.5f, -0.5f + squareSizeY * (j + 1));
                normals[currentVertice] = new Vector3(0, 1, 0);
                uv[currentVertice] = new Vector2(0.0f, 1.0f);
                triangles[currentTriangle] = currentVertice;
                currentTriangle++;
                currentVertice++;

                vertices[currentVertice] = new Vector3(-0.5f + squareSizeX * (i + 1), 0.5f, -0.5f + squareSizeY * (j + 1));
                normals[currentVertice] = new Vector3(0, 1, 0);
                uv[currentVertice] = new Vector2(1.0f, 1.0f);
                triangles[currentTriangle] = currentVertice;
                currentTriangle++;
                currentVertice++;

                vertices[currentVertice] = new Vector3(-0.5f + squareSizeX * (i + 1), 0.5f, -0.5f + squareSizeY * j);
                normals[currentVertice] = new Vector3(0, 1, 0);
                uv[currentVertice] = new Vector2(1.0f, 0.0f);
                triangles[currentTriangle] = currentVertice;
                currentTriangle++;
                triangles[currentTriangle] = currentVertice - 2;
                currentTriangle++;
                currentVertice++;

                vertices[currentVertice] = new Vector3(-0.5f + squareSizeX * i, 0.5f, -0.5f + squareSizeY * j);
                normals[currentVertice] = new Vector3(0, 1, 0);
                uv[currentVertice] = new Vector2(0.0f, 0.0f);
                triangles[currentTriangle] = currentVertice - 1;
                currentTriangle++;
                triangles[currentTriangle] = currentVertice;
                currentTriangle++;
                currentVertice++;
            }
        }

        // Initialize cells data
        cells = new FarmCell[Mathf.RoundToInt(xSize * ySize * 4)];

        return mesh;
    }
}

public class FarmCell
{
    //public FarmCell(){}
}