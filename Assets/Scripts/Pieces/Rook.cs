using System;
using System.Collections.Generic;
using UnityEngine;

namespace Pieces
{
    public class Rook : Piece
    {
        public override List<Vector2Int> Movements()
        {
            return CMovement(PiecesManager.GetOccupied(), PiecesManager.GetDimensions());
        }
    }
}