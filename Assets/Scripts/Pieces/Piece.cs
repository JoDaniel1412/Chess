using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Pieces
{
    public abstract class Piece : MonoBehaviour
    {
        public Vector2Int poss;
        
        protected PiecesManager PiecesManager;
        
        
        // Returns all possible movements a piece can do
        public abstract List<Vector2Int> Movements();
        
        protected void Start()
        {
            PiecesManager = FindObjectOfType<PiecesManager>();
        }

        // Updates the position
        protected void Move(Vector2Int newPoss)
        {
            poss = newPoss;
        }
        
        /**
        * Return all movements in all 8 directions
        * @occupied are all tiles that are occupied by another piece
        * @iRange are the columns count of the Board
        * @jRange are the rows count of the Board
        * @range is the piece max movement tiles
        */
        protected List<Vector2Int> AMovement(List<Vector2Int> occupied, (int, int) dimensions, int range)
        {
            var cMovement = CMovement(occupied, dimensions, 1);
            var dMovement = DMovement(occupied, dimensions, 1);
            return cMovement.Concat(dMovement).ToList();
        }
        
        /**
         * Return all movements in cross form
         * @occupied are all tiles that are occupied by another piece
         * @iRange are the columns count of the Board
         * @jRange are the rows count of the Board
         * @range is the piece max movement tiles
         */
        protected IEnumerable<Vector2Int> CMovement(List<Vector2Int> occupied, (int, int) dimensions, int range)
        {
            var (rows, columns) = dimensions;
            var hMovement = HMovement(occupied, rows, range);
            var vMovement = VMovement(occupied, columns, range);
            return hMovement.Concat(vMovement).ToList();
        }
        
        /**
         * Return all movements in diagonal form
         * @occupied are all tiles that are occupied by another piece
         * @iRange are the columns count of the Board
         * @jRange are the rows count of the Board
         * @range is the piece max movement tiles
         */
        protected IEnumerable<Vector2Int> DMovement(List<Vector2Int> occupied, (int, int) dimensions, int range)
        {
            var function = new Vector2Int(1, 1);
            var left = IterateVectors(occupied, function, dimensions, range);
            
            function = new Vector2Int(-1, 1);
            var right = IterateVectors(occupied, function, dimensions, range);

            return left.Concat(right).ToList();
        }

        // Return all possible movements in horizontal
        private IEnumerable<Vector2Int> HMovement(ICollection<Vector2Int> occupied, int columns, int range)
        {
            var function = new Vector2Int(1, 0);
            return IterateVectors(occupied, function, (0, columns), range);
        }
        
        // Return all possible movements in vertical
        private IEnumerable<Vector2Int> VMovement(ICollection<Vector2Int> occupied, int rows, int range)
        {
            
            var function = new Vector2Int(0, 1);
            return IterateVectors(occupied, function, (rows, 0), range);
        }

        /**
         * Recursively applies the operator(+-) function to the Poss vector checking if doesnt collide with
         * other piece and keeping it inside the board range and max movements range
         */
        private IEnumerable<Vector2Int> IterateVectors(ICollection<Vector2Int> occupied, Vector2Int function, (int, int) dimensions, int range)
        {
            var result = new List<Vector2Int>();
            var i = poss.x++;
            var j = poss.y++;
            var movements = 0;
            
            #region Evaluates positives

            var (rows, columns) = dimensions;
            while (i < rows || j < columns)
            {
                var vect2 = new Vector2Int(i, j);
                if (occupied.Contains(vect2) || movements >= range )  // Breaks if collides or max movements reached
                    break;
                
                result.Add(vect2);
                
                i += function.x;
                j += function.y;
                movements++;
            }
            
            #endregion

            i = poss.x--;
            j = poss.y--;
            movements = 0;
            
            #region Evaluates negatives
            
            while (i > 0 || j > 0)
            {
                var vect2 = new Vector2Int(i, poss.y);
                if (occupied.Contains(vect2) || movements >= range)  // Breaks if collides or max movements reached
                    break;
                
                result.Add(vect2);
                
                i -= function.x;
                j -= function.y;
                movements++;
            }
            
            #endregion

            return result;
        }
        
    }
}