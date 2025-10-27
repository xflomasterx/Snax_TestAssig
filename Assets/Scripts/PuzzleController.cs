using UnityEngine;



public class PuzzleController : MonoBehaviour
{
    public Vector3 gridOrigin = Vector3.zero;
    public int gridWidth = 10;
    public int gridHeight = 10;
    public Vector2Int startingPos;
    public float cellSize = 1f;
    public GameObject markerPref; 
    GameObject marker;

    Vector2Int currentCell;

    public GameObject Marker
    {
        get
        {
            if (marker == null)
                CreateMarker();
            return marker;
        }
        set => marker = value;
    }


    void CreateMarker()
    {
        currentCell = startingPos;
        gridOrigin = gameObject.GetComponent<GridMeshGenerator>().GridOrigin;
        marker = Instantiate(markerPref, gridOrigin, Quaternion.identity);
        UpdateMarker();
    }
    void Update()
    {
        bool moved = false;
        if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow)) { currentCell.y += 1; moved = true; }
        if (Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow)) { currentCell.y -= 1; moved = true; }
        if (Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.LeftArrow)) { currentCell.x -= 1; moved = true; }
        if (Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow)) { currentCell.x += 1; moved = true; }


        // clamp to grid bounds
        currentCell.x = Mathf.Clamp(currentCell.x, 0, gridWidth - 1);
        currentCell.y = Mathf.Clamp(currentCell.y, 0, gridHeight - 1);


        if (moved)
        {
            UpdateMarker();
            Debug.Log($"[PuzzleController] Moved to cell {currentCell.x},{currentCell.y}");
        }
    }


    void UpdateMarker()
    {
        if (marker != null)
        {
            Vector3 worldPos = gridOrigin + new Vector3(currentCell.x * cellSize, 0f, currentCell.y * cellSize);
            marker.transform.position = worldPos - Vector3.up * 0.01f;
        }
    }
}