using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Pieces
{
    public class King : Piece
    {
        private bool _check;
        private bool _checkmate;


        public override (List<Vector2Int> movements, List<Vector2Int> enemies) Movements()
        {
            var (movements, enemies) = AMovement(PiecesManager.GetOccupied(), PiecesManager.GetDimensions(), 1);
            var otherTeam = team.Equals(Team.White) ? Team.Black : Team.White;
            var validVectors = PiecesManager.GetValidVectors();

            // Movements following check rules
            var (movementsOther, enemiesOther) = PiecesManager.AllMovements(otherTeam);
            var movementsValid = movements.Except(movementsOther).Intersect(validVectors).ToList();

            // Verifies check or checkmate
            _check = enemiesOther.Contains(poss);
            _checkmate = _check && movementsValid.Count <= 0;

            return (movementsValid, enemies);
        }

        public override (List<Vector2Int> movements, List<Vector2Int> enemies) MovementsForCheck()
        {
            return AMovement(PiecesManager.GetOccupied(), PiecesManager.GetDimensions(), 1);
        }

        // Evaluates the King Status
        public (bool check, bool checkmate) IsCheck()
        {
            Movements();
            return (_check, _checkmate);
        }
        
    }
}