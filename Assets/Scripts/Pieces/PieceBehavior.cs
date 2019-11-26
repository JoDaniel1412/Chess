using System;
using UnityEngine;

namespace Pieces
{
    public class PieceBehavior : MonoBehaviour
    {
        private Piece _piece;
        private PiecesManager _piecesManager;


        public void Selected()
        {
            _piecesManager.SelectTarget(gameObject, _piece.Movements());
        }

        public void Dropped()
        {
            _piecesManager.DropTarget();
        }
        
        private void Start()
        {
            _piece = GetComponent<Piece>();
            _piecesManager = GetComponentInParent<PiecesManager>();
        }

        private void OnMouseDown() => Selected();

        private void OnMouseUp() => Dropped();
    }
}