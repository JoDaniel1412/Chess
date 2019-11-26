using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Pieces
{
    public class Knight : Piece
    {
        public override (List<Vector2Int> movements, List<Vector2Int> enemies) Movements()
        {
            var occupied = PiecesManager.GetOccupied();
            var dimensions = PiecesManager.GetDimensions();
            
            var function = new Vector2Int(1, 2);
            var (positives, enemies1) = KnightMovement(occupied, function, dimensions);
            
            function = new Vector2Int(2, 1);
            var (negatives, enemies2) = KnightMovement(occupied, function, dimensions);
            
            var movements = positives.Concat(negatives).ToList();
            var enemies = enemies1.Concat(enemies2).ToList();
            
            return (movements, enemies);
        }

        private (List<Vector2Int> result, List<Vector2Int> enemies) KnightMovement(ICollection<Vector2Int> occupied, Vector2Int function, (int, int) dimensions)
        {
            var result = new List<Vector2Int>();
            var enemies = new List<Vector2Int>();
            var movements = new List<Vector2Int>
            {
                new Vector2Int(poss.x + function.x, poss.y + function.y),
                new Vector2Int(poss.x - function.x, poss.y - function.y),
                new Vector2Int(poss.x + function.x, poss.y - function.y),
                new Vector2Int(poss.x - function.x, poss.y + function.y)
            };

            foreach (var movement in movements)
            {
                if (!IsMovementLegal(movement, occupied, dimensions)) continue;
                
                if (PiecesManager.IsEnemy(movement, team)) enemies.Add(movement);
                else result.Add(movement);
                break;
            }
            
            return (result, enemies);
        }

        private static bool IsMovementLegal(Vector2Int vect, ICollection<Vector2Int> occupied, (int, int) dimensions)
        {
            var (rows, columns) = dimensions;
            return !occupied.Contains(vect) &&
                   vect.x >= 0 && vect.x < columns && 
                   vect.y >= 0 && vect.y < rows;
        }
    }
}