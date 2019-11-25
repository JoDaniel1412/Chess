using System;
using System.Collections.Generic;
using Board;
using UnityEngine;

namespace Pieces
{
    public class PiecesManager : MonoBehaviour
    {
        private GameController _gameController;
        private Board.Board _board;
        
        public void HighlightMovements(List<Vector2Int> movements, bool state)
        {
            _board.GetComponentInChildren<TilesController>().SendMessage("HighlightMovements", (movements, state));
        }
        
        public List<Vector2Int> GetOccupied()
        {
            return _board.GetOccupied();
        }

        public (int, int) GetDimensions()
        {
            return (_board.rows, _board.columns);
        }
            
        private void Start()
        {
            _gameController = FindObjectOfType<GameController>();
            _board = FindObjectOfType<Board.Board>();
        }

        public List<Piece> Pieces { get; set; }

    }
}