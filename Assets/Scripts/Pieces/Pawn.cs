using System.Collections.Generic;
using UnityEngine;

namespace Pieces
{
    public class Pawn : Piece
    {
        private bool _firstMove = true;
        
        public override List<Vector2Int> Movements()
        {
            
            // Direction of Movement
            var dir = 1;
            if (team.Equals(Team.Black)) dir = -1;
            
            
            // Checks collisions
            var occupied = PiecesManager.GetOccupied();
            var i = poss.x + dir;
            var vect2 = new Vector2Int(i, poss.y);

            if (occupied.Contains(vect2)) return new List<Vector2Int>();
            
            // First extra step
            if (_firstMove && !occupied.Contains(new Vector2Int(i + dir, poss.y))) i += dir;
            
            return new List<Vector2Int> {vect2};
        }

        protected override void Move(Vector2Int newPoss)
        {
            base.Move(newPoss);
            _firstMove = false;
        }
    }
}