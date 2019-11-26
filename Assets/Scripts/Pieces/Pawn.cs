using System.Collections.Generic;
using UnityEngine;

namespace Pieces
{
    public class Pawn : Piece
    {
        private bool _firstMove = true;
        
        public override List<Vector2Int> Movements()
        {
            var result = new List<Vector2Int>();
            var occupied = PiecesManager.GetOccupied();
            
            // Direction of Movement
            var dir = 1;
            if (team.Equals(Team.Black)) dir = -1;
            var i = poss.x + dir;

            // Checks collisions
            var vect = new Vector2Int(i, poss.y);
            if (occupied.Contains(vect)) return result;
            result.Add(vect);
            
            // First extra step
            var vect2 = new Vector2Int(i + dir, poss.y);
            if (_firstMove && !occupied.Contains(vect2)) 
                result.Add(vect2);
            
            return result;
        }

        protected override void Move(Vector2Int newPoss)
        {
            base.Move(newPoss);
            _firstMove = false;
        }
    }
}