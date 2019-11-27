using System;
using Pieces;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
        public float speed;
        public Vector3 possWhite;
        public Vector3 possBlack;
        public Transform focalWhite;
        public Transform focalBlack;

        private Vector3 _target;
        private Transform _focalPoint;
        
        
        // Change target poss and focal point base on turn
        public void Flip(Piece.Team team)
        {
                _target = team.Equals(Piece.Team.Black) ? possBlack : possWhite;
                _focalPoint = team.Equals(Piece.Team.Black) ? focalBlack : focalWhite;
        }

        private void Start()
        {
                _focalPoint = focalWhite;
        }

        private void Update()
        {
                // Rotates the camera
                transform.LookAt(_focalPoint);
                
                // Moves the Camera to the target
                var position = transform.position;
                var dir = _target - position;
                var distance = Vector3.Distance(_target, position);
                if (_target.Equals(Vector3.zero) || distance < 0.1f) return;
                transform.Translate(Time.deltaTime * speed * dir.normalized, Space.World);
        }
}