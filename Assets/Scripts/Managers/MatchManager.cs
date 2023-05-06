using System.Collections.Generic;
using UnityEngine;

public class MatchManager : MonoBehaviour
{
    [Header("Components")] 
    [SerializeField] private GridManager _gridManager;

    private int _crossedTileCount;

    #region PUBLIC METHODS

    public bool AreThereMatches(Tile tile)
    {
        // If not enough crossed tiles, return early
        _crossedTileCount++;
        if (_crossedTileCount < 3) { return false; }

        var matches = new List<Tile>();
        matches.Add(tile);

        // Check for matches in all four directions
        var directions = new[] { Vector2.up, Vector2.down, Vector2.left, Vector2.right };
        CheckMatchesInAllDirections(tile, matches, directions);

        if (matches.Count >= 3)
        {
            ClearMatches(matches);
            return true;
        }

        return false;
    }

    #endregion

    #region PRIVATE METHODS

    private void CheckMatchesInAllDirections(Tile tile, List<Tile> matches, Vector2[] directions)
    {
        Vector2 startPos = tile.transform.position;
        foreach (var direction in directions)
        {
            var pos = startPos + direction;
            var newTile = _gridManager.GetTile(pos);

            // If the new tile exists and is crossed, add it to the matches list
            if (newTile != null && newTile.IsCrossed && !matches.Contains(newTile))
            {
                matches.Add(newTile);

                // Recursively check for matches in a row
                CheckMatchesInAllDirections(newTile, matches, directions);
            }
        }
    }

    private void ClearMatches(List<Tile> matches)
    {
        Debug.Log($"Found {matches.Count} matches!");
        _crossedTileCount -= matches.Count;
        foreach (var tile in matches)
        {
            tile.OnReset();
        }
    }

    #endregion
}