using System;
using UnityEngine;

namespace Pieces
{
    /**
     * Class fot the Piece events
     */
    public class PieceBehavior : MonoBehaviour
    {
        private Piece _piece;
        private PiecesManager _piecesManager;


        public void Selected()
        {
            if (_piecesManager.Turn != _piece.team) return;
            _piecesManager.SelectTarget(gameObject, _piece.Movements());
        }

        public void Dropped()
        {
            if (_piecesManager.Turn != _piece.team) return;
            _piecesManager.DropTarget();
        }
        
        private void Start()
        {
            _piece = GetComponent<Piece>();
            _piecesManager = GetComponentInParent<PiecesManager>();
        }

        private void OnMouseDown() => Selected();

        private void OnMouseUp() => Dropped();

        private void OnMouseEnter() => _piecesManager.HighlightTile(true, _piece.poss);

        private void OnMouseExit() => _piecesManager.HighlightTile(false, _piece.poss);
        
    }
}