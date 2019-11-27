using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace Pieces
{
    /**
     * Abstract class, defines the behaviour and movements of all pieces
     */
    public abstract class Piece : MonoBehaviour
    {
        public Vector2Int poss;
        public Team team;
        
        protected PiecesManager PiecesManager;

        private Vector3 _target = Vector3.zero;
        private const float Speed = 10f;

        public enum Team {White, Black};
        
        
        // Returns all possible movements a piece can do
        public abstract (List<Vector2Int> movements, List<Vector2Int> enemies) Movements();

        // Changes the Piece material base on Team
        public void LoadTeamMaterial(Team newTeam)
        {
            team = newTeam;
            var mesh = GetComponent<MeshRenderer>();
            var path = "Assets/Materials/Pieces/";

            #region Selects the material
            
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
            
            #endregion
            
            var material = AssetDatabase.LoadAssetAtPath<Material>(path);
            mesh.material = material;
        }
        
        // Updates the position of the Piece
        public virtual void Move(Vector2Int newPoss)
        {
            if (newPoss.Equals(new Vector2Int(-1, -1))) return;
            poss = newPoss;
            _target = PiecesManager.GetTarget(newPoss);
        }
        
        // Moves the Piece to the starting position
        public void Spawn(Vector2Int newPoss)
        {
            poss = newPoss;
            _target = PiecesManager.GetTarget(newPoss);
        }
        
        protected void Start()
        {
            PiecesManager = FindObjectOfType<PiecesManager>();
        }

        protected void Update()
        {
            // Moves the piece to the target
            var position = transform.position;
            var dir = _target - position;
            var distance = Vector3.Distance(_target, position);
            if (_target.Equals(Vector3.zero) || distance < 0.2f) return;
            transform.Translate(Time.deltaTime * Speed* dir.normalized, Space.World);
        }

        /**
        * Return all movements in all 8 directions
        * @occupied are all tiles that are occupied by another piece
        * @iRange are the columns count of the Board
        * @jRange are the rows count of the Board
        * @range is the piece max movement tiles
        */
        protected (List<Vector2Int> movements, List<Vector2Int> enemies) AMovement(List<Vector2Int> occupied, (int, int) dimensions, int range=8)
        {
            var (cMovement, enemies1) = CMovement(occupied, dimensions, range);
            var (dMovement,enemies2) = DMovement(occupied, dimensions, range);
            
            var movements = cMovement.Concat(dMovement).ToList();
            var enemies = enemies1.Concat(enemies2).ToList();
            
            return (movements, enemies);
        }
        
        /**
         * Return all movements in cross form
         * @occupied are all tiles that are occupied by another piece
         * @iRange are the columns count of the Board
         * @jRange are the rows count of the Board
         * @range is the piece max movement tiles
         */
        protected (List<Vector2Int> movements, List<Vector2Int> enemies) CMovement(List<Vector2Int> occupied, (int, int) dimensions, int range=8)
        {
            var (hMovement, enemies1) = HMovement(occupied, dimensions, range);
            var (vMovement,enemies2) = VMovement(occupied, dimensions, range);
            
            var movements = hMovement.Concat(vMovement).ToList();
            var enemies = enemies1.Concat(enemies2).ToList();
            
            return (movements, enemies);
        }
        
        /**
         * Return all movements in diagonal form
         * @occupied are all tiles that are occupied by another piece
         * @iRange are the columns count of the Board
         * @jRange are the rows count of the Board
         * @range is the piece max movement tiles
         */
        protected (List<Vector2Int> movements, List<Vector2Int> enemies) DMovement(List<Vector2Int> occupied, (int, int) dimensions, int range=8)
        {
            var function = new Vector2Int(1, 1);
            var (left, enemies1) = IterateVectors(occupied, function, dimensions, range);
            
            function = new Vector2Int(-1, 1);
            var (right, enemies2) = IterateVectors(occupied, function, dimensions, range);

            var movements = left.Concat(right).ToList();
            var enemies = enemies1.Concat(enemies2).ToList();
            
            return (movements, enemies);
        }

        // Return all possible movements in horizontal
        private (List<Vector2Int>, List<Vector2Int>) HMovement(ICollection<Vector2Int> occupied, (int, int) dimensions, int range)
        {
            var function = new Vector2Int(1, 0);
            return IterateVectors(occupied, function, dimensions, range);
        }
        
        // Return all possible movements in vertical
        private (List<Vector2Int>, List<Vector2Int>) VMovement(ICollection<Vector2Int> occupied, (int, int) dimensions, int range)
        {
            
            var function = new Vector2Int(0, 1);
            return IterateVectors(occupied, function, dimensions, range);
        }

        /**
         * Recursively applies the operator(+-) function to the Poss vector checking if doesnt collide with
         * other piece and keeping it inside the board range and max movements range
         */
        private (List<Vector2Int>, List<Vector2Int>) IterateVectors(ICollection<Vector2Int> occupied, Vector2Int function, (int, int) dimensions, int range)
        {
            var movements = new List<Vector2Int>();
            var enemies = new List<Vector2Int>();
            var i = poss.x + function.x;
            var j = poss.y + function.y;
            var steps = 0;
            
            #region Evaluates positives

            var (rows, columns) = dimensions;
            while (i < columns || j < rows)
            {
                if (IterateVectorsAux(i, j, occupied, steps, range, enemies, movements))
                    break;
                
                i += function.x;
                j += function.y;
                steps++;
            }
            
            #endregion

            i = poss.x - function.x;
            j = poss.y - function.y;
            steps = 0;
            
            #region Evaluates negatives
            
            while (i > 0 || j > 0)
            {
                if (IterateVectorsAux(i, j, occupied, steps, range, enemies, movements))
                    break;
                
                i -= function.x;
                j -= function.y;
                steps++;
            }
            
            #endregion

            return (movements, enemies);
        }

        // Auxiliary function that determines if the movement is valid
        private bool IterateVectorsAux(int i, int j, ICollection<Vector2Int> occupied, int steps, int range, ICollection<Vector2Int> enemies, ICollection<Vector2Int> movements)
        {
            var vect2 = new Vector2Int(i, j);
            if (occupied.Contains(vect2) || steps >= range) // Breaks if collides or max movements reached
            {
                if (PiecesManager.IsEnemy(vect2, team))
                    enemies.Add(vect2);
                return true;
            }
                
            movements.Add(vect2);
            return false;
        }
        
    }
}