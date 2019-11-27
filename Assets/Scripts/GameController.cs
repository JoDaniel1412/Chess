using System;
using Pieces;
using UnityEngine;

public class GameController : MonoBehaviour
{
        public GameObject board;
        public GameObject piecesManager;

        private Piece.Team _turn;
        private Camera _cam;

        public void SwitchTurn()
        {
                _turn = _turn.Equals(Piece.Team.White) ? Piece.Team.Black : Piece.Team.White;
                piecesManager.GetComponent<PiecesManager>().Turn = _turn;
                _cam.SendMessage("Flip", _turn);
        }

        private void Start()
        {
                _turn = Piece.Team.White;
                _cam = FindObjectOfType<Camera>();
                piecesManager.GetComponent<PiecesManager>().Turn = _turn;
        }
}