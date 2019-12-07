using System;
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
            if (occupied.Contains(enemy1) && PiecesManager.IsEnemy(enemy1, team)) enemies.Add(enemy1);
            if (occupied.Contains(enemy2) && PiecesManager.IsEnemy(enemy2, team)) enemies.Add(enemy2);


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
        
        // Return the possible movements that the Pawn can Check 
        public override (List<Vector2Int> movements, List<Vector2Int> enemies) MovementsForCheck()
        {
            var enemies = new List<Vector2Int>();
            
            // Direction of Movement
            var dir = 1;
            if (team.Equals(Team.Black)) dir = -1;
            
            // Check Enemies
            enemies.Add(new Vector2Int(poss.x + dir, poss.y + 1));
            enemies.Add(new Vector2Int(poss.x + dir, poss.y - 1));

            return (new List<Vector2Int>(), enemies);
        }

        public override void Move(Vector2Int newPoss)
        {
            // Checks if this is the first move
            if (newPoss.Equals(new Vector2Int(-1, -1))) return;
            base.Move(newPoss);
            _firstMove = false;
            
        }

        public void LateUpdate()
        {
            // Promotion
            var limit = team.Equals(Team.White) ? 7 : 0;
            if (transform.position.Equals(Target)
                && poss.x.Equals(limit)
                && !_firstMove)
                Promote();
        }

        // Converts the pawn to a queen
        private void Promote()
        {
            Debug.LogFormat("Pawn promoted, team: {0}, poss: {1}", team, poss);
            PiecesManager.PromotePawn(transform.position, poss, team);
            PiecesManager.KillPiece(gameObject, poss);
        }
    }
}