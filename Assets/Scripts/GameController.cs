using System;
using Board;
using Pieces;
using UnityEngine;

public class GameController : MonoBehaviour
{
        public GameObject board;
        public GameObject piecesManager;

        private Piece.Team _turn;
        private Camera _cam;

        // Flips the camera and enables the other team to move
        public void SwitchTurn()
        {
                // Switch turn
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