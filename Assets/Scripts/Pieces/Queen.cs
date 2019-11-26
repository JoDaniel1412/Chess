using System;
using System.Collections.Generic;
using UnityEngine;

namespace Pieces
{
    public class Queen : Piece
    {
        public override (List<Vector2Int> movements, List<Vector2Int> enemies) Movements()
        {
            return AMovement(PiecesManager.GetOccupied(), PiecesManager.GetDimensions());
        }
    }
}