using System;
using System.Collections.Generic;
using UnityEngine;

public class GridManager : MonoBehaviour
{
    [SerializeField] private int _width, _height;
    [SerializeField] private Tile _tilePrefab;
    [SerializeField] private Transform _cam;

    private Dictionary<Vector2, Tile> _tiles;
    private int _crossedTileCount;

    #region UNITY EVENTS

    void Start()
    {
        GenerateGrid();
    }

    #endregion

    #region PUBLIC METHODS

    public bool CheckMatches(Tile tile)
    {
        _crossedTileCount++;
        if (_crossedTileCount < 3) { return false; }

        var pos = tile.transform.position;
        var matches = new List<Tile>();

        // HORIZONTAL MATCHES
        // Check for right direction matches
        var rightMatches = FindMatches(pos, Vector2.right);
        matches.AddRange(rightMatches);
        // Check for left direction matches
        var leftMatches = FindMatches(pos, Vector2.left);
        matches.AddRange(leftMatches);

        // If there is a horizontal match, return early
        if (matches.Count >= 2) { return true; }

        // VERTICAL MATCHES
        matches.Clear();
        // Check for up direction matches
        var upMatches = FindMatches(pos, Vector2.up);
        matches.AddRange(upMatches);
        // Check for down direction matches
        var downMatches = FindMatches(pos, Vector2.down);
        matches.AddRange(downMatches);
        
        return matches.Count >= 2;
    }

    #endregion

    #region PRIVATE METHODS

    private void GenerateGrid()
    {
        _tiles = new Dictionary<Vector2, Tile>();

        // Create the grid
        for (int x = 0; x < _width; x++)
        {
            for (int y = 0; y < _height; y++)
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

        // Center the camera on the grid
        _cam.transform.position = new Vector3((float)_width / 2 - 0.5f, (float)_height / 2 - 0.5f, -10);
    }

    private Tile GetTile(Vector2 pos)
    {
        if (_tiles.TryGetValue(pos, out var tile)) { return tile; }
        
        return null;
    }
    
    private List<Tile> FindMatches(Vector2 startPos, Vector2 increment)
    {
        var matches = new List<Tile>();
        var pos = startPos;
        for (int i = 1; i <= 2; i++)
        {
            pos += increment;
            var newTile = GetTile(pos);
            if (newTile == null || !newTile.IsCrossed) { break; }
            
            matches.Add(newTile);
        }
        return matches;
    }

    #endregion
}