using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Pieces
{
    public class Knight : Piece
    {
        public override List<Vector2Int> Movements()
        {
            var occupied = PiecesManager.GetOccupied();
            var dimensions = PiecesManager.GetDimensions();
            
            var function = new Vector2Int(1, 2);
            var positives = KnightMovement(occupied, function, dimensions);
            
            function = new Vector2Int(2, 1);
            var negatives = KnightMovement(occupied, function, dimensions);
            
            return positives.Concat(negatives).ToList();
        }

        private IEnumerable<Vector2Int> KnightMovement(ICollection<Vector2Int> occupied, Vector2Int function, (int, int) dimensions)
        {
            var result = new List<Vector2Int>();

            var vect = new Vector2Int(poss.x + function.x, poss.y + function.y);
            if (IsMovementLegal(vect, occupied, dimensions)) 
                result.Add(vect);
            
            vect = new Vector2Int(poss.x - function.x, poss.y - function.y); 
            if (IsMovementLegal(vect, occupied, dimensions)) 
                result.Add(vect);
            
            vect = new Vector2Int(poss.x + function.x, poss.y - function.y); 
            if (IsMovementLegal(vect, occupied, dimensions)) 
                result.Add(vect);  
            
            vect = new Vector2Int(poss.x - function.x, poss.y + function.y); 
            if (IsMovementLegal(vect, occupied, dimensions)) 
                result.Add(vect);
            
            return result;
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