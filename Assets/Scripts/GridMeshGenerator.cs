using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridMeshGenerator : MonoBehaviour
{
    public GameObject tilePrefab;
    public int gridSize = 10;
    public float cellSize = 1f;
    Vector3 gridOrigin;
    bool isInitated = false;
    public Vector3 GridOrigin
    {
        get
        {
            if (!isInitated)
            {
                GenerateGrid();
            }
            return gridOrigin;
        }
        set => gridOrigin = value; 
    }
    public void GenerateGrid()
    {
        gridOrigin  = new Vector3( cellSize / 2f - gridSize * cellSize / 2f, 0f, 0f); 
        for (int x = 0; x < gridSize; x++)
        {
            for (int z = 0; z < gridSize; z++)
            {
                Vector3 position = new Vector3(x * cellSize , 0, z * cellSize) + gridOrigin;
                Instantiate(tilePrefab, position, Quaternion.identity, transform);
            }
        }
        isInitated = true;
    }
}