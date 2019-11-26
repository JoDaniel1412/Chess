using System.Collections.Generic;
using UnityEngine;

namespace Pieces
{
    public class Bishop : Piece
    {
        public override (List<Vector2Int> movements, List<Vector2Int> enemies) Movements()
        {
            return DMovement(PiecesManager.GetOccupied(), PiecesManager.GetDimensions());
        }
    }
}