using System.Collections.Generic;
using UnityEngine;

namespace Pieces
{
    public class Bishop : Piece
    {
        public override List<Vector2Int> Movements()
        {
            return (List<Vector2Int>) DMovement(PiecesManager.GetOccupied(), PiecesManager.GetDimensions());
        }
    }
}