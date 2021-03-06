﻿using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

/**
 * Class that keeps the logic Matrix and can access each Tile and their properties
 */
namespace Board
{
    [ExecuteInEditMode]
    public class Board : MonoBehaviour
    {
        public int rows;
        public int columns;
        public float yOffset;

        // Returns the Tiles according to each vector
        public List<Tile> GetTiles(List<Vector2Int> vectors)
        {
            return (from row in Matrix from tile in row 
                where vectors.Contains(tile.Poss) select tile).ToList();
        }

        // Returns the Tile according to the vector
        public Tile GetTile(Vector2Int poss)
        {
            return Matrix[poss.x][poss.y];
        }
        
        // Return the center of the Tile in the given (i, j)
        public Vector3 GetTileCenter(int i, int j)
        {
            var tile = Matrix[i][j];
            var center = tile.GetComponent<MeshRenderer>().bounds.center;
            center.y = yOffset;
            return center;
        }

        // Returns all Tiles that currently has a piece on it
        public List<Vector2Int> GetOccupied()
        {
            return (from row in Matrix from tile in row where tile.Occupied select tile.Poss).ToList();
        }

        // Properties
        
        public List<List<Tile>> Matrix { get; set; } = new List<List<Tile>>();
        
        public List<Vector2Int> Vectors { get; set; }

    }
}