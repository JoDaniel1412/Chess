using System;
using UnityEngine;

namespace Pieces
{
    public class PieceBehavior : MonoBehaviour
    {
        private Piece _piece;
        private PiecesManager _piecesManager;

        private void Start()
        {
            
            _piece = GetComponent<Piece>();
            _piecesManager = GetComponentInParent<PiecesManager>();
        }

        private void OnMouseDown()
        {
            _piecesManager.SelectTarget(gameObject, _piece.Movements());
        }

        private void OnMouseUp()
        {
            _piecesManager.DropTarget();
        }
    }
}