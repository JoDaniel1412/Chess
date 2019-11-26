using System.Collections.Generic;
using UnityEngine;

namespace Pieces
{
    public class Pawn : Piece
    {
        private bool _firstMove = true;
        
        public override (List<Vector2Int> movements, List<Vector2Int> enemies) Movements()
        {
            var result = new List<Vector2Int>();
            var enemies = new List<Vector2Int>();
            var occupied = PiecesManager.GetOccupied();
            
            // Direction of Movement
            var dir = 1;
            if (team.Equals(Team.Black)) dir = -1;
            var i = poss.x + dir;
            
            // Check Enemies
            var enemy1 = new Vector2Int(poss.x + dir, poss.y + 1);
            var enemy2 = new Vector2Int(poss.x + dir, poss.y - 1);
            if (occupied.Contains(enemy1)) enemies.Add(enemy1);
            if (occupied.Contains(enemy2)) enemies.Add(enemy2);


            // Checks collisions
            var vect = new Vector2Int(i, poss.y);
            if (occupied.Contains(vect)) return (result, enemies);
            result.Add(vect);
            
            // First extra step
            var vect2 = new Vector2Int(i + dir, poss.y);
            if (_firstMove && !occupied.Contains(vect2)) 
                result.Add(vect2);
            
            return (result, enemies);
        }

        protected override void Move(Vector2Int newPoss)
        {
            base.Move(newPoss);
            _firstMove = false;
        }
    }
}