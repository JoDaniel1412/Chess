using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Board
{
    /**
     * Class that turns effects and actions over the tiles
     */
    public class TilesController : MonoBehaviour
    {
        private Tilemap _tilemap;
        private Board _board;

        private void Start()
        {
            _tilemap = GetComponent<Tilemap>();
            _board = GetComponentInParent<Board>();
        }

        // Calls the Highlight for each Tile in movement or enemies and set its state
        public void HighlightMovements((List<Vector2Int> movements, bool state, bool enemies) message)
        {
            var (movements, state, enemies) = message;
            var tiles = _board.GetTiles(movements);
            foreach (var tile in tiles)
            {
                if (enemies) tile.HighlightEnemy(state);
                else tile.HighlightMovement(state);
            }
        }

        // Search the Poss of the current elevated Tile
        public Vector2Int GetTileOnMouse()
        {
            var result = new Vector2Int(-1, -1);
            
            foreach (var row in _board.Matrix)
            {
                foreach (var tile in row.Where(tile => tile.Elevated))
                {
                    result = tile.Poss;
                    break;
                }
            }

            return result;
        }

        
        // Properties
        
        public GameObject TileTarget { get; set; }
    }
}