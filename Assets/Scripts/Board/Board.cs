using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Board
{
    [ExecuteInEditMode]
    public class Board : MonoBehaviour
    {
        public int rows;
        public int columns;

        // Returns the Tiles according to each vector
        public List<Tile> GetTiles(List<Vector2Int> vectors)
        {
            return (from row in Matrix from tile in row 
                let tmp = new Vector2Int(tile.I, tile.J) 
                where vectors.Contains(tmp) select tile).ToList();
        }

        // Returns all Tiles that currently has a piece on it
        public List<Vector2Int> GetOccupied()
        {
            return (from row in Matrix from tile in row where tile.Occupied select new Vector2Int(tile.I, tile.J)).ToList();
        }

        // Returns a pair of Vectors with the board dimensions ([iMin, iMax], [jMin, jMax])
        public (Vector2Int, Vector2Int) GetDimensions()
        {
            return (new Vector2Int(0, columns), new Vector2Int(0, rows));
        }

        public List<List<Tile>> Matrix { get; set; } = new List<List<Tile>>();
    }
}