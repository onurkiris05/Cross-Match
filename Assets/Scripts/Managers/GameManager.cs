using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    [Header("Components")]
    [SerializeField] private MatchManager _matchManager;

    #region PUBLIC METHODS

    public void OnTileCrossed(Tile tile)
    {
        if (_matchManager.AreThereMatches(tile))
        {
            // Do some win condition stuff here
        }
    }

    #endregion
}