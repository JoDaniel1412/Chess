using System;
using System.Collections.Generic;
using UnityEngine;

namespace Board
{
    public class TilesController : MonoBehaviour
    {
        private Tilemap _tilemap;
        private Board _board;

        private void Start()
        {
            _tilemap = GetComponent<Tilemap>();
            _board = GetComponentInParent<Board>();
        }

        public void HighlightMovements((List<Vector2Int>, bool, bool) message)
        {
            var (movements, state, enemies) = message;
            var tiles = _board.GetTiles(movements);
            foreach (var tile in tiles)
            {
                if (enemies) tile.HighlightEnemy(state);
                else tile.HighlightMovement(state);
            }
        }
        
    }
}