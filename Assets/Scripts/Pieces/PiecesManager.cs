using System.Collections;
using System.Collections.Generic;
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
                _gameController.SendMessage("SwitchTurn");
            
            _target.SendMessage("Move", possTo);
            _target = null;
            
            HighlightMovements(_targetMoves.movements, false, false);
            HighlightMovements(_targetMoves.enemies, false, true);

        }

        // Gets al positions that are occupied by pieces
        public List<Vector2Int> GetOccupied()
        {
            return _board.GetOccupied();
        }

        // Return a bool if the piece in the poss is an enemy
        public bool IsEnemy(Vector2Int vect, Piece.Team myTeam)
        {
            var result = false;
            
            foreach (var pieceObj in _pieces)
            {
                var piece = pieceObj.GetComponent<Piece>();
                if (piece.poss.x != vect.x || piece.poss.y != vect.y) continue;
                result = !piece.team.Equals(myTeam);
                break;

            }

            return result;
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

        // Reads the pieces alignment and creates each Piece 
        private void LoadPieces()
        {
            for (var i = 0; i < _alignment.Count; i++)
            {
                for (var j = 0; j < _alignment[i].Count; j++)
                {
                    var path = "Assets/Prefabs/Pieces/";
                    var letter = _alignment[i][j].Item1;
                    var color = _alignment[i][j].Item2;
                    
                    #region Decides witch piece to place
                    
                    switch (letter)
                    {
                        case 'P':
                            path += "Pawn.prefab";
                            break;
                        case 'R':
                            path += "Rook.prefab";
                            break;
                        case 'H':
                            path += "Knight.prefab";
                            break;
                        case 'B':
                            path += "Bishop.prefab";
                            break;
                        case 'K':
                            path += "King.prefab";
                            break;
                        case 'Q':
                            path += "Queen.prefab";
                            break;
                        default:
                            path = "";
                            break;
                    }
                    
                    #endregion

                    #region Decides the team

                    var team = Piece.Team.White;
                    if (color.Equals('B')) team = Piece.Team.Black;

                    #endregion
                    
                    if (path.Equals("")) continue;
                    var prefab = AssetDatabase.LoadAssetAtPath<GameObject>(path);
                    StartCoroutine(LoadPiece(i, j, prefab, team));
                }
            }
        }

        // Instantiates the Piece and sets its values
        private IEnumerator LoadPiece(int i, int j, GameObject prefab, Piece.Team team)
        {
            var poss = Vector3.zero;
            var piece = Instantiate(prefab, poss, prefab.transform.rotation);
            var script = piece.GetComponent<Piece>();
            piece.transform.SetParent(transform);
            _pieces.Add(piece);
            script.LoadTeamMaterial(team);

            yield return new WaitForSeconds(1);
            script.Spawn(new Vector2Int(i, j));
        }

        
        // Properties
        
        public List<Piece> Pieces { get; set; }
        
        public Piece.Team Turn { get; set; }

    }
}