using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Pieces;
using UnityEngine;

public class GameController : MonoBehaviour
{
        public GameObject board;
        public GameObject piecesManager;
        public GameObject uiController;
        public bool animations;
        public bool cameraFlip;

        private Piece.Team _turn;
        private Camera _cam;

        // Flips the camera and enables the other team to move
        public void SwitchTurn()
        {
                // Switch turn
                _turn = _turn.Equals(Piece.Team.White) ? Piece.Team.Black : Piece.Team.White;
                _cam.SendMessage("Flip", _turn);
        }

        public void Animations() => animations = !animations;

        public void CameraFlip() => cameraFlip = !cameraFlip;

        private void Start()
        {
                _turn = Piece.Team.None;
                _cam = FindObjectOfType<Camera>();

                StartCoroutine(DelayStart());
        }

        private void Update()
        {
                // Win the Game
                if (_turn.Equals(Piece.Team.None)) return;
                var (checkMate, king) = CheckMate();
                if (checkMate) WinGame(king.GetComponent<Piece>().team);
        }

        private IEnumerator DelayStart()
        {
                yield return new WaitForSeconds(2);
                _turn = Piece.Team.White;
        }

        // Returns if only one king its alive
        private (bool checkMate, GameObject king) CheckMate()
        {
                var kings = piecesManager.GetComponent<PiecesManager>().FindKings();
                return (kings.Count == 1, kings.First());
        }

        // Locks mouse events in the Board and displays the winner team
        private void WinGame(Piece.Team team)
        {
                var message = $"Winner: {team.ToString()}";
                _turn = Piece.Team.None;
                
                uiController.SendMessage("Message", (message, team));
                Debug.LogFormat(message);
        }
        
        
        // Properties
        
        public Piece.Team Turn
        {
                get => _turn;
                set => _turn = value;
        }
}