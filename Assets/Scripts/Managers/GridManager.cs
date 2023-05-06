using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GridManager : MonoBehaviour
{
    [Header("Components")] 
    [SerializeField] private Tile _tilePrefab;
    [SerializeField] private Camera _cam;
    [SerializeField] private TMP_InputField _input;

    private Dictionary<Vector2, Tile> _tiles = new();
    

    #region PUBLIC METHODS

    public void GenerateBoard()
    {
        ClearBoard();

        // Get the size of the board
        if (!int.TryParse(_input.text, out var size) || size < 2)
        {
            Debug.Log("Invalid input. Enter a value greater than or equal to 2!");
            return;
        }

        // Create the grid
        for (int x = 0; x < size; x++)
        {
            for (int y = 0; y < size; y++)
            {
                var spawnedTile = Instantiate(_tilePrefab, new Vector3(x, y), Quaternion.identity, transform);
                spawnedTile.name = $"Tile ({x},{y})";

                // Offset the tiles to create a checkered pattern
                var isOffset = (x % 2 == 0 && y % 2 != 0) || (x % 2 != 0 && y % 2 == 0);
                spawnedTile.Init(isOffset);

                // Add the tile to the dictionary
                _tiles[new Vector2(x, y)] = spawnedTile;
            }
        }

        AdjustCamera(size);
    }

    public Tile GetTile(Vector2 pos)
    {
        if (_tiles.TryGetValue(pos, out var tile))
        {
            return tile;
        }

        return null;
    }

    #endregion

    #region PRIVATE METHODS
    
    private void AdjustCamera(int size)
    {
        // Center the camera on the board
        var bounds = new Bounds(new Vector3((size - 1) / 2f, (size - 1) / 2f, 0f),
            new Vector3(size, size, 0f));
        var cameraDistance = Mathf.Max(bounds.size.x, bounds.size.y) /
                             (2f * Mathf.Tan(Mathf.Deg2Rad * _cam.fieldOfView / 2f));
        _cam.transform.position = bounds.center - cameraDistance * _cam.transform.forward;
    
        // Adjust the camera to fit the board on the screen
        float aspectRatio = (float)Screen.width / (float)Screen.height;
        float orthoSize = bounds.size.y / 2f;
        if (bounds.size.x > bounds.size.y * aspectRatio)
        {
            orthoSize = bounds.size.x / (2f * aspectRatio);
        }
        
        _cam.orthographicSize = orthoSize;
    }

    private void ClearBoard()
    {
        if (_tiles.Count > 0)
        {
            foreach (var tile in _tiles.Values)
            {
                Destroy(tile.gameObject);
            }

            _tiles.Clear();
        }
    }

    #endregion
}