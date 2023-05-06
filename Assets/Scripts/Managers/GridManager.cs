using System.Collections.Generic;
using UnityEngine;

public class GridManager : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private Tile _tilePrefab;
    [SerializeField] private Camera _cam;
    
    [Space(10)]
    [Header("Settings")]
    [SerializeField] private int _size;

    private Dictionary<Vector2, Tile> _tiles;

    #region UNITY EVENTS

    void Start()
    {
        GenerateBoard(_size);
    }

    #endregion

    #region PUBLIC METHODS
    
    public Tile GetTile(Vector2 pos)
    {
        if (_tiles.TryGetValue(pos, out var tile)) { return tile; }
        
        return null;
    }

    #endregion

    #region PRIVATE METHODS

    private void GenerateBoard(int size)
    {
        _tiles = new Dictionary<Vector2, Tile>();

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

        // Center the camera on the board
        var bounds = new Bounds(new Vector3((size - 1) / 2f, (size - 1) / 2f, 0f), new Vector3(size, size, 0f));
        var cameraDistance = Mathf.Max(bounds.size.x, bounds.size.y) / (2f * Mathf.Tan(Mathf.Deg2Rad * _cam.fieldOfView / 2f));
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
    #endregion
}