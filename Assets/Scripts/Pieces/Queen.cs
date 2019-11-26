using System;
using System.Collections.Generic;
using UnityEngine;

namespace Pieces
{
    public class Queen : Piece
    {
        public override List<Vector2Int> Movements()
        {
            return AMovement(PiecesManager.GetOccupied(), PiecesManager.GetDimensions());
        }
    }
}