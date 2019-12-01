using System;
using Pieces;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace UI
{
    public class UiController : MonoBehaviour
    {
        public GameObject messengerWhite;
        public GameObject messengerBlack;

    
        // Displays the message for the current team in the UI
        public void Message((string message, Piece.Team team) received)
        {
            switch (received.team)
            {
                case Piece.Team.Black:
                    messengerBlack.SendMessage("DisplayText", received.message);
                    break;
                case Piece.Team.White:
                    messengerWhite.SendMessage("DisplayText", received.message);
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(received.team), received.team, null);
            }
        }

        public void Back()
        {
            SceneManager.UnloadSceneAsync("Scenes/GameScene");
            SceneManager.LoadScene("Scenes/MainScene");
        }

    }
}
