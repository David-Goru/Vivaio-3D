using UnityEngine;

[System.Serializable]
public class FarmMeshes
{
    private Vector3 meshSize;
    private FarmCell[] cells;
    private FarmMesh groundData;

    [System.NonSerialized] private Mesh ground;

    public Mesh Ground { get => ground; }

    public void Initialize(Vector3 size)
    {
        meshSize = size;

        if (groundData == null) // New game or error loading data
        {
            cells = new FarmCell[Mathf.RoundToInt(size.x * size.z)];
            newMesh(size.x, size.z);
        }

        ground = new Mesh();
        ground.indexFormat = UnityEngine.Rendering.IndexFormat.UInt32; // Up to 4 billion vertices
        ground.vertices = groundData.Vertices;
        ground.normals = groundData.Normals;
        ground.uv = groundData.UV;
        ground.triangles = groundData.Triangles;
    }

    public bool Plow(Vector3 position)
    {
        Vector3 roundedPosition = new Vector3(Mathf.Round(position.x - 0.5f) / meshSize.x, 0, Mathf.Round(position.z - 0.5f) / meshSize.z);
        int cellId = Mathf.RoundToInt((roundedPosition.x + 0.5f) * 100) + Mathf.RoundToInt((roundedPosition.z + 0.5f) * 10);

        if (cells[cellId].Plowed) return false;

        foreach (int i in cells[cellId].FloorVertices)
        {
            groundData.Vertices[i] = new Vector3(groundData.Vertices[i].x, 0.45f, groundData.Vertices[i].z);
        }
        ground.vertices = groundData.Vertices;

        cells[cellId].Position = new Vector3(Mathf.Round(position.x - 0.5f) + 0.5f, 0, Mathf.Round(position.z - 0.5f) + 0.5f);
        cells[cellId].CreateRidge();
        cells[cellId].Plowed = true;

        return true;
    }

    public bool Unplow(Vector3 position)
    {
        Vector3 roundedPosition = new Vector3(Mathf.Round(position.x - 0.5f) / meshSize.x, 0, Mathf.Round(position.z - 0.5f) / meshSize.z);
        int cellId = Mathf.RoundToInt((roundedPosition.x + 0.5f) * 100) + Mathf.RoundToInt((roundedPosition.z + 0.5f) * 10);

        if (!cells[cellId].Plowed) return false;

        foreach (int i in cells[cellId].FloorVertices)
        {
            groundData.Vertices[i] = new Vector3(groundData.Vertices[i].x, 0.5f, groundData.Vertices[i].z);
        }
        ground.vertices = groundData.Vertices;

        cells[cellId].Plowed = false;

        return true;
    }

    private void newMesh(float xSize, float ySize)
    {
        int numberOfTiles = Mathf.RoundToInt(xSize * ySize);
        int numberOfVertices = 20 + numberOfTiles * 4; // Sides (4 * 4) + floor (4) vertices, face to modify vertices (20 per tile)
        int numberOfTriangles = (10 + numberOfTiles * 2) * 3; // Sides (4 * 2) + floor (2) triangles, face to modify triangles (10 per unit)

        groundData = new FarmMesh();
        groundData.Vertices = new Vector3[numberOfVertices];
        groundData.Normals = new Vector3[numberOfVertices];
        groundData.UV = new Vector2[numberOfVertices];
        groundData.Triangles = new int[numberOfTriangles];

        // Floor
        groundData.Vertices[0] = new Vector3(-0.5f, -0.5f, -0.5f);
        groundData.Vertices[1] = new Vector3(0.5f, -0.5f, -0.5f);
        groundData.Vertices[2] = new Vector3(0.5f, -0.5f, 0.5f);
        groundData.Vertices[3] = new Vector3(-0.5f, -0.5f, 0.5f);
        groundData.Normals[0] = new Vector3(0, -1, 0);
        groundData.Normals[1] = new Vector3(0, -1, 0);
        groundData.Normals[2] = new Vector3(0, -1, 0);
        groundData.Normals[3] = new Vector3(0, -1, 0);
        groundData.UV[0] = new Vector2(0.0f, 0.0f);
        groundData.UV[1] = new Vector2(1.0f, 0.0f);
        groundData.UV[2] = new Vector2(1.0f, 1.0f);
        groundData.UV[3] = new Vector2(0.0f, 1.0f);
        groundData.Triangles[0] = 0;
        groundData.Triangles[1] = 1;
        groundData.Triangles[2] = 2;
        groundData.Triangles[3] = 2;
        groundData.Triangles[4] = 3;
        groundData.Triangles[5] = 0;

        // Sides
        // Down left
        groundData.Vertices[4] = new Vector3(0.5f, -0.5f, -0.5f);
        groundData.Vertices[5] = new Vector3(-0.5f, -0.5f, -0.5f);
        groundData.Vertices[6] = new Vector3(-0.5f, 0.5f, -0.5f);
        groundData.Vertices[7] = new Vector3(0.5f, 0.5f, -0.5f);
        groundData.Normals[4] = new Vector3(0, 0, -1);
        groundData.Normals[5] = new Vector3(0, 0, -1);
        groundData.Normals[6] = new Vector3(0, 0, -1);
        groundData.Normals[7] = new Vector3(0, 0, -1);
        groundData.UV[4] = new Vector2(0.0f, 0.0f);
        groundData.UV[5] = new Vector2(1.0f, 0.0f);
        groundData.UV[6] = new Vector2(1.0f, 1.0f);
        groundData.UV[7] = new Vector2(0.0f, 1.0f);
        groundData.Triangles[6] = 4;
        groundData.Triangles[7] = 5;
        groundData.Triangles[8] = 6;
        groundData.Triangles[9] = 6;
        groundData.Triangles[10] = 7;
        groundData.Triangles[11] = 4;
        // Down right
        groundData.Vertices[8] = new Vector3(0.5f, -0.5f, 0.5f);
        groundData.Vertices[9] = new Vector3(0.5f, -0.5f, -0.5f);
        groundData.Vertices[10] = new Vector3(0.5f, 0.5f, -0.5f);
        groundData.Vertices[11] = new Vector3(0.5f, 0.5f, 0.5f);
        groundData.Normals[8] = new Vector3(1, 0, 0);
        groundData.Normals[9] = new Vector3(1, 0, 0);
        groundData.Normals[10] = new Vector3(1, 0, 0);
        groundData.Normals[11] = new Vector3(1, 0, 0);
        groundData.UV[8] = new Vector2(0.0f, 0.0f);
        groundData.UV[9] = new Vector2(1.0f, 0.0f);
        groundData.UV[10] = new Vector2(1.0f, 1.0f);
        groundData.UV[11] = new Vector2(0.0f, 1.0f);
        groundData.Triangles[12] = 8;
        groundData.Triangles[13] = 9;
        groundData.Triangles[14] = 10;
        groundData.Triangles[15] = 10;
        groundData.Triangles[16] = 11;
        groundData.Triangles[17] = 8;
        // Up left
        groundData.Vertices[12] = new Vector3(-0.5f, -0.5f, -0.5f);
        groundData.Vertices[13] = new Vector3(-0.5f, -0.5f, 0.5f);
        groundData.Vertices[14] = new Vector3(-0.5f, 0.5f, 0.5f);
        groundData.Vertices[15] = new Vector3(-0.5f, 0.5f, -0.5f);
        groundData.Normals[12] = new Vector3(-1, 0, 0);
        groundData.Normals[13] = new Vector3(-1, 0, 0);
        groundData.Normals[14] = new Vector3(-1, 0, 0);
        groundData.Normals[15] = new Vector3(-1, 0, 0);
        groundData.UV[12] = new Vector2(0.0f, 0.0f);
        groundData.UV[13] = new Vector2(1.0f, 0.0f);
        groundData.UV[14] = new Vector2(1.0f, 1.0f);
        groundData.UV[15] = new Vector2(0.0f, 1.0f);
        groundData.Triangles[18] = 12;
        groundData.Triangles[19] = 13;
        groundData.Triangles[20] = 14;
        groundData.Triangles[21] = 14;
        groundData.Triangles[22] = 15;
        groundData.Triangles[23] = 12;
        // Up right
        groundData.Vertices[16] = new Vector3(-0.5f, -0.5f, 0.5f);
        groundData.Vertices[17] = new Vector3(0.5f, -0.5f, 0.5f);
        groundData.Vertices[18] = new Vector3(0.5f, 0.5f, 0.5f);
        groundData.Vertices[19] = new Vector3(-0.5f, 0.5f, 0.5f);
        groundData.Normals[16] = new Vector3(0, 0, 1);
        groundData.Normals[17] = new Vector3(0, 0, 1);
        groundData.Normals[18] = new Vector3(0, 0, 1);
        groundData.Normals[19] = new Vector3(0, 0, 1);
        groundData.UV[16] = new Vector2(0.0f, 0.0f);
        groundData.UV[17] = new Vector2(1.0f, 0.0f);
        groundData.UV[18] = new Vector2(1.0f, 1.0f);
        groundData.UV[19] = new Vector2(0.0f, 1.0f);
        groundData.Triangles[24] = 16;
        groundData.Triangles[25] = 17;
        groundData.Triangles[26] = 18;
        groundData.Triangles[27] = 18;
        groundData.Triangles[28] = 19;
        groundData.Triangles[29] = 16;

        // Modifiable vertices
        int currentVertex = 20;
        int currentTriangle = 30;
        float squareSizeX = 1.0f / xSize;
        float squareSizeY = 1.0f / ySize;
        for (int i = 0; i < xSize; i++)
        {
            for (int j = 0; j < ySize; j++)
            {
                int cellId = i * 10 + j;
                cells[cellId] = new FarmCell();

                cells[cellId].FloorVertices[0] = currentVertex;
                groundData.Vertices[currentVertex] = new Vector3(-0.5f + squareSizeX * i, 0.5f, -0.5f + squareSizeY * (j + 1));
                groundData.Normals[currentVertex] = new Vector3(0, 1, 0);
                groundData.UV[currentVertex] = new Vector2(0.0f, 1.0f);
                groundData.Triangles[currentTriangle] = currentVertex;
                currentTriangle++;
                currentVertex++;

                cells[cellId].FloorVertices[1] = currentVertex;
                groundData.Vertices[currentVertex] = new Vector3(-0.5f + squareSizeX * (i + 1), 0.5f, -0.5f + squareSizeY * (j + 1));
                groundData.Normals[currentVertex] = new Vector3(0, 1, 0);
                groundData.UV[currentVertex] = new Vector2(1.0f, 1.0f);
                groundData.Triangles[currentTriangle] = currentVertex;
                currentTriangle++;
                currentVertex++;

                cells[cellId].FloorVertices[2] = currentVertex;
                groundData.Vertices[currentVertex] = new Vector3(-0.5f + squareSizeX * (i + 1), 0.5f, -0.5f + squareSizeY * j);
                groundData.Normals[currentVertex] = new Vector3(0, 1, 0);
                groundData.UV[currentVertex] = new Vector2(1.0f, 0.0f);
                groundData.Triangles[currentTriangle] = currentVertex;
                currentTriangle++;
                groundData.Triangles[currentTriangle] = currentVertex - 2;
                currentTriangle++;
                currentVertex++;

                cells[cellId].FloorVertices[3] = currentVertex;
                groundData.Vertices[currentVertex] = new Vector3(-0.5f + squareSizeX * i, 0.5f, -0.5f + squareSizeY * j);
                groundData.Normals[currentVertex] = new Vector3(0, 1, 0);
                groundData.UV[currentVertex] = new Vector2(0.0f, 0.0f);
                groundData.Triangles[currentTriangle] = currentVertex - 1;
                currentTriangle++;
                groundData.Triangles[currentTriangle] = currentVertex;
                currentTriangle++;
                currentVertex++;
            }
        }
    }
}

[System.Serializable]
public class FarmCell
{
    private Vector3 position;
    private int[] floorVertices;
    private bool plowed;
    [System.NonSerialized] private GameObject ridge;

    public Vector3 Position { get => position; set => position = value; }
    public int[] FloorVertices { get => floorVertices; set => floorVertices = value; }
    public bool Plowed { get => plowed; set => plowed = value; }

    public FarmCell()
    {
        floorVertices = new int[4];
    }

    public void CreateRidge()
    {
        Object.Instantiate(Farm.GetRidgePrefab(), position, Quaternion.identity);
    }
}

[System.Serializable]
public class FarmMesh
{
    private Vector3[] vertices;
    private Vector3[] normals;
    private Vector2[] uv;
    private int[] triangles;

    public Vector3[] Vertices { get => vertices; set => vertices = value; }
    public Vector3[] Normals { get => normals; set => normals = value; }
    public Vector2[] UV { get => uv; set => uv = value; }
    public int[] Triangles { get => triangles; set => triangles = value; }

    public FarmMesh()
    {
        vertices = new Vector3[0];
        normals = new Vector3[0];
        uv = new Vector2[0];
        triangles = new int[0];
    }
}