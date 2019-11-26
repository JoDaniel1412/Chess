using System;
using System.Collections.Generic;
using System.Linq;
using Board;
using UnityEditor;
using UnityEngine;

namespace Pieces
{
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


        public void SelectTarget(GameObject target, (List<Vector2Int> movements, List<Vector2Int> enemies) moves )
        {
            _target = target;
            _targetMoves = moves;
            HighlightMovements(moves.movements, true, false);
            HighlightMovements(moves.enemies, true, true);
        }

        public void DropTarget()
        {
            var possTo = _tilesController.GetTileOnMouse();
            _target.SendMessage("Move", possTo);
            _target = null;
            HighlightMovements(_targetMoves.movements, false, false);
            HighlightMovements(_targetMoves.enemies, false, true);
        }

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

        public Vector3 GetTarget(Vector2Int poss)
        {
            return _board.GetTilePoss(poss.x, poss.y);
        }

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

        private void HighlightMovements(List<Vector2Int> movements, bool state, bool enemies)
        {
            _board.GetComponentInChildren<TilesController>().SendMessage("HighlightMovements", (movements, state, enemies));
        }

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
                    LoadPiece(i, j, prefab, team);
                }
            }
        }

        private void LoadPiece(int i, int j, GameObject prefab, Piece.Team team)
        {
            var poss = _board.GetTilePoss(i, j);
            var piece = Instantiate(prefab, poss, prefab.transform.rotation);
            var script = piece.GetComponent<Piece>();
            piece.transform.SetParent(transform);
            _pieces.Add(piece);
            script.SetTeam(team);
            script.poss = new Vector2Int(i, j);
        }

        public List<Piece> Pieces { get; set; }

    }
}