using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace Pieces
{
    public abstract class Piece : MonoBehaviour
    {
        public Vector2Int poss;
        public Team team;
        
        protected PiecesManager PiecesManager;


        public enum Team {White, Black};
        
        // Returns all possible movements a piece can do
        public abstract List<Vector2Int> Movements();

        // Changes the Piece material base on Team
        public void SetTeam(Team newTeam)
        {
            team = newTeam;
            var mesh = GetComponent<MeshRenderer>();
            var path = "Assets/Materials/Pieces/";

            switch (newTeam)
            {
                case Team.Black:
                    path += "BlackPiece.mat";
                    break;
                case Team.White:
                    path += "WhitePiece.mat";
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(newTeam), newTeam, null);
            }
            
            var material = AssetDatabase.LoadAssetAtPath<Material>(path);
            mesh.material = material;
        }
        
        protected void Start()
        {
            PiecesManager = FindObjectOfType<PiecesManager>();
        }

        // Updates the position
        protected virtual void Move(Vector2Int newPoss)
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
        protected List<Vector2Int> AMovement(List<Vector2Int> occupied, (int, int) dimensions, int range=8)
        {
            var cMovement = CMovement(occupied, dimensions, range);
            var dMovement = DMovement(occupied, dimensions, range);
            return cMovement.Concat(dMovement).ToList();
        }
        
        /**
         * Return all movements in cross form
         * @occupied are all tiles that are occupied by another piece
         * @iRange are the columns count of the Board
         * @jRange are the rows count of the Board
         * @range is the piece max movement tiles
         */
        protected List<Vector2Int> CMovement(List<Vector2Int> occupied, (int, int) dimensions, int range=8)
        {
            var hMovement = HMovement(occupied, dimensions, range);
            var vMovement = VMovement(occupied, dimensions, range);
            return hMovement.Concat(vMovement).ToList();
        }
        
        /**
         * Return all movements in diagonal form
         * @occupied are all tiles that are occupied by another piece
         * @iRange are the columns count of the Board
         * @jRange are the rows count of the Board
         * @range is the piece max movement tiles
         */
        protected IEnumerable<Vector2Int> DMovement(List<Vector2Int> occupied, (int, int) dimensions, int range=8)
        {
            var function = new Vector2Int(1, 1);
            var left = IterateVectors(occupied, function, dimensions, range);
            
            function = new Vector2Int(-1, 1);
            var right = IterateVectors(occupied, function, dimensions, range);

            return left.Concat(right).ToList();
        }

        // Return all possible movements in horizontal
        private IEnumerable<Vector2Int> HMovement(ICollection<Vector2Int> occupied, (int, int) dimensions, int range)
        {
            var function = new Vector2Int(1, 0);
            return IterateVectors(occupied, function, dimensions, range);
        }
        
        // Return all possible movements in vertical
        private IEnumerable<Vector2Int> VMovement(ICollection<Vector2Int> occupied, (int, int) dimensions, int range)
        {
            
            var function = new Vector2Int(0, 1);
            return IterateVectors(occupied, function, dimensions, range);
        }

        /**
         * Recursively applies the operator(+-) function to the Poss vector checking if doesnt collide with
         * other piece and keeping it inside the board range and max movements range
         */
        private IEnumerable<Vector2Int> IterateVectors(ICollection<Vector2Int> occupied, Vector2Int function, (int, int) dimensions, int range)
        {
            var result = new List<Vector2Int>();
            var i = poss.x + function.x;
            var j = poss.y + function.y;
            var movements = 0;
            
            #region Evaluates positives

            var (rows, columns) = dimensions;
            while (i < columns || j < rows)
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

            i = poss.x - function.x;
            j = poss.y - function.y;
            movements = 0;
            
            #region Evaluates negatives
            
            while (i > 0 || j > 0)
            {
                var vect2 = new Vector2Int(i, j);
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