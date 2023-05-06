using System.Collections.Generic;
using UnityEngine;

public class MatchManager : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] GridManager _gridManager;
    
    private int _crossedTileCount;

    #region PUBLIC METHODS

    public bool AreThereMatches(Tile tile)
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
        matches.Add(tile);
        if (matches.Count >= 3)
        {
            ClearMatches(matches);
            return true;
        }

        // VERTICAL MATCHES
        matches.Clear();
        // Check for up direction matches
        var upMatches = FindMatches(pos, Vector2.up);
        matches.AddRange(upMatches);
        // Check for down direction matches
        var downMatches = FindMatches(pos, Vector2.down);
        matches.AddRange(downMatches);
        
        matches.Add(tile);
        if (matches.Count >= 3)
        {
            ClearMatches(matches);
            return true;
        }
        
        return false;
    }

    #endregion

    #region PRIVATE METHODS

    private List<Tile> FindMatches(Vector2 startPos, Vector2 increment)
    {
        var matches = new List<Tile>();
        var pos = startPos;
        for (int i = 1; i <= 2; i++)
        {
            pos += increment;
            var newTile = _gridManager.GetTile(pos);
            if (newTile == null || !newTile.IsCrossed) { break; }
            
            matches.Add(newTile);
        }
        return matches;
    }

    private void ClearMatches(List<Tile> matches)
    {
        Debug.Log($"Found {matches.Count} matches!");
        foreach (var tile in matches)
        {
            tile.OnReset();
        }
    }

    #endregion
}
