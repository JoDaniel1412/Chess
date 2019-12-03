using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Board;
using UnityEditor;
using UnityEngine;

namespace Pieces
{
    /**
     * Class that interacts with the GameController and Board so the Pieces can move
     */
    public class PiecesManager : MonoBehaviour
    {
        
        public GameObject pawn;
        public GameObject rook;
        public GameObject knight;
        public GameObject bishop;
        public GameObject king;
        public GameObject queen;
            
        private GameController _gameController;
        private Board.Board _board;
        private TilesController _tilesController;
        private List<GameObject> _pieces = new List<GameObject>();
        private GameObject _target;
        private (List<Vector2Int> movements, List<Vector2Int> enemies) _targetMoves;
        private readonly List<List<(char, char)>> _alignment = new List<List<(char, char)>>
        {
            new List<(char, char)> {('R','W'), ('H','W'), ('B','W'), ('K','W'), ('Q','W'), ('B','W'), ('H','W'), ('R','W')},
            new List<(char, char)> {('P','W'), ('P','W'), ('P','W'), ('P','W'), ('P','W'), ('P','W'), ('P','W'), ('P','W')},
            new List<(char, char)> (),
            new List<(char, char)> (),
            new List<(char, char)> (),
            new List<(char, char)> (),
            new List<(char, char)> {('P','B'), ('P','B'), ('P','B'), ('P','B'), ('P','B'), ('P','B'), ('P','B'), ('P','B')},
            new List<(char, char)> {('R','B'), ('H','B'), ('B','B'), ('Q','B'), ('K','B'), ('B','B'), ('H','B'), ('R','B')},
        };


        // Function used while a Piece is selected
        public void SelectTarget(GameObject target, (List<Vector2Int> movements, List<Vector2Int> enemies) moves )
        {
            _target = target;
            _targetMoves = moves;
            HighlightMovements(moves.movements, true, false);
            HighlightMovements(moves.enemies, true, true);
        }

        // Function used when a Piece is deselected
        public void DropTarget()
        {
            var possTo = _tilesController.GetTileOnMouse();
            
            // If the Piece moves
            if (!possTo.Equals(new Vector2Int(-1, -1)))
            {
                // If target poss has an enemy
                if (_targetMoves.enemies.Contains(possTo)) 
                {
                    _target.SendMessage("Attack");
                    var enemy = GetPieceAt(possTo);
                    enemy.SendMessage("Died");
                    _pieces.RemoveAll(enemy.gameObject.Equals);
                }
                
                _target.SendMessage("Move", possTo);
                StartCoroutine(SwitchTurn(_target.GetComponent<Piece>()));
            }
            
            HighlightMovements(_targetMoves.movements, false, false);
            HighlightMovements(_targetMoves.enemies, false, true);
            _target = null;
        }
        
        // Search for the kings
        public List<GameObject> FindKings()
        {
            var kings = (from pieceObj in Pieces 
                select pieceObj.GetComponent<Piece>() into piece 
                let type = piece.type where type.Equals(Piece.Type.King) 
                select piece.gameObject).ToList();
            
            return kings;
        }

        // Search for the king of the given team
        public GameObject FindKing(Piece.Team team)
        {
            var kings = FindKings();
            GameObject result = null;
            
            foreach (var kingObj in from kingObj in kings 
                let king = kingObj.GetComponent<Piece>() 
                where king.team.Equals(team) select kingObj)
            {
                result = kingObj;
            }

            return result;
        }

        // Return all possible movement a team can do
        public (List<Vector2Int> movements, List<Vector2Int> enemies) AllMovements(Piece.Team team)
        {
            var movements = new List<Vector2Int>();
            var enemies = new List<Vector2Int>(); 
            
            foreach (var piece in _pieces.Select(pieceObj => pieceObj.GetComponent<Piece>()).Where(piece => piece.team.Equals(team)))
            {
                List<Vector2Int> movementsSub, enemiesSub;
                
                // King and Pawn have different Check patterns
                if (piece.type.Equals(Piece.Type.King) || piece.type.Equals(Piece.Type.Pawn))
                    (movementsSub, enemiesSub) = piece.MovementsForCheck();
                else (movementsSub, enemiesSub) = piece.Movements();

                // Concatenates the lists
                movements = movements.Concat(movementsSub).ToList();
                enemies = enemies.Concat(enemiesSub).ToList();
            }

            return (movements, enemies);
        }

        // Highlights the Tile also when mouse its over a piece
        public void HighlightTile(bool state, Vector2Int poss)
        {
            var tile = _board.GetTile(poss);
            if (state) tile.Selected();
            else tile.UnSelected();
        }

        // Gets al positions that are occupied by pieces
        public List<Vector2Int> GetOccupied()
        {
            return _board.GetOccupied();
        }
        
        // Gets al positions that are occupied by pieces of the team
        public List<Vector2Int> GetOccupied(Piece.Team team)
        {
            var occupied = _board.GetOccupied();
            return occupied.Where(vect => !IsEnemy(vect, team)).ToList();
        }

        // Return a bool if the piece in the poss is an enemy
        public bool IsEnemy(Vector2Int vect, Piece.Team myTeam)
        {
            var piece = GetPieceAt(vect);
            if (piece)
                return !piece.team.Equals(myTeam);
            return false;
        }

        // Search the Tile in the Poss and returns the center
        public Vector3 GetTarget(Vector2Int poss)
        {
            return _board.GetTileCenter(poss.x, poss.y);
        }

        // Search for the Board dimensions
        public (int, int) GetDimensions()
        {
            return (_board.rows, _board.columns);
        }
            
        private void Start()
        {
            _gameController = FindObjectOfType<GameController>();
            _board = FindObjectOfType<Board.Board>();
            _tilesController = _board.GetComponentInChildren<TilesController>();
            LoadPieces();
        }

        // Uses the TilesController to highlight the possible movements and enemies
        private void HighlightMovements(List<Vector2Int> movements, bool state, bool enemies)
        {
            _board.GetComponentInChildren<TilesController>().SendMessage("HighlightMovements", (movements, state, enemies));
        }

        // Returns the piece that matches the Poss
        private Piece GetPieceAt(Vector2Int poss)
        {
            Piece result = null;
            
            foreach (var pieceObj in _pieces)
            {
                var piece = pieceObj.GetComponent<Piece>();
                if (piece.poss != poss) continue;
                result = piece;
                break;
            }

            return result;
        }

        // Switches the turn until the piece arrives destination
        private IEnumerator SwitchTurn(Piece piece)
        {
            if (!piece.ArrivedDestination)
            {
                yield return new WaitForSeconds(0.2f);
                StartCoroutine(SwitchTurn(piece));
            }
            else _gameController.SendMessage("SwitchTurn");
        }

        // Reads the pieces alignment and creates each Piece 
        private void LoadPieces()
        {
            for (var i = 0; i < _alignment.Count; i++)
            {
                for (var j = 0; j < _alignment[i].Count; j++)
                {
                    var letter = _alignment[i][j].Item1;
                    var color = _alignment[i][j].Item2;
                    
                    #region Decides witch piece to place
                    
                    Piece.Type type;
                    GameObject prefab;
                    switch (letter)
                    {
                        case 'P':
                            prefab = pawn;
                            type = Piece.Type.Pawn;
                            break;
                        case 'R':
                            prefab = rook;
                            type = Piece.Type.Rook;
                            break;
                        case 'H':
                            prefab = knight;
                            type = Piece.Type.Knight;
                            break;
                        case 'B':
                            prefab = bishop;
                            type = Piece.Type.Bishop;
                            break;
                        case 'K':
                            prefab = king;
                            type = Piece.Type.King;
                            break;
                        case 'Q':
                            prefab = queen;
                            type = Piece.Type.Queen;
                            break;
                        default:
                            prefab = null;
                            type = Piece.Type.None;
                            break;
                    }
                    
                    #endregion

                    #region Decides the team

                    var team = Piece.Team.White;
                    if (color.Equals('B')) team = Piece.Team.Black;

                    #endregion
                    
                    if (!prefab) continue;
                    StartCoroutine(LoadPiece(i, j, prefab, team, type));
                }
            }
        }

        // Instantiates the Piece and sets its values
        private IEnumerator LoadPiece(int i, int j, GameObject prefab, Piece.Team team, Piece.Type type)
        {
            var poss = Vector3.zero;
            var piece = Instantiate(prefab, poss, prefab.transform.rotation);
            var script = piece.GetComponent<Piece>();
            piece.transform.SetParent(transform);
            _pieces.Add(piece);
            script.LoadTeamMaterial(team);
            script.type = type;

            yield return new WaitForSeconds(1);
            script.Spawn(new Vector2Int(i, j));
        }

        
        // Properties

        public List<GameObject> Pieces
        {
            get => _pieces;
            set => _pieces = value;
        }

        public Piece.Team Turn() => _gameController.Turn;

        public bool Animations() => _gameController.animations;

    }
}