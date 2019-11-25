using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Pieces
{
    public class King : Piece
    {
        public override List<Vector2Int> Movements()
        {
            return AMovement(PiecesManager.GetOccupied(), PiecesManager.GetDimensions(), 8);
        }
    }
}