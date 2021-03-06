﻿using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Board
{
    /**
     * Class that loads the graphic Grid in the Scene
     */
    [ExecuteInEditMode]
    public class Tilemap : MonoBehaviour
    {
        public float offset;
        public Transform tilePref;
        public Material blackMat;
        public Material whiteMat;

        private Board _board;
        private int _rows;
        private int _columns;
        
        private void Start()
        {
            _board = GetComponentInParent<Board>();
            _rows = _board.rows;
            _columns = _board.columns;
            LoadGrid();
        }

        // Reloads the Grid
        private void OnEnable()
        {
            DestroyGrid();
            LoadGrid();
        }

        // Clears the Grid
        private void OnDisable()
        {
            DestroyGrid();
        }

        // Instantiates each tile of the grid
        private void LoadGrid()
        {
            var matrix = new List<List<Tile>>();
            var vectors = new List<Vector2Int>();
            var tileSize = tilePref.GetComponent<MeshRenderer>().bounds.size;
            var xOffset = tileSize.x + offset;
            var zOffset = tileSize.z + offset;
            var count = 0;
            
            #region Creates the matrix
            
            for (var i = 0; i < _rows; i++)
            {
                var z = zOffset * i;
                var row = new List<Tile>();
                for (var j = 0; j < _columns; j++)
                {
                    var x = xOffset * j;
                    var poss = new Vector3(x, 0, z);
                    var tile = Instantiate(tilePref, poss, Quaternion.identity);
                    count++;
                    row.Add(SetupTile(tile, i, j, count));
                    vectors.Add(new Vector2Int(i, j));
                }

                matrix.Add(row);
                count--;
            }
            
            #endregion

            if (_board)
            {
                _board.Matrix = matrix;
                _board.Vectors = vectors;
            }
            Size = new Vector2(xOffset * _columns, zOffset * _rows);
        }
        
        // Deletes the Grid
        private static void DestroyGrid()
        {
            var tiles = GameObject.FindGameObjectsWithTag("Tile");
            foreach (var children in tiles)
                DestroyImmediate(children);
        }

        // Selects the color for the Tile and other properties
        private Tile SetupTile(Transform tile, int i, int j, int count)
        {
            tile.SetParent(transform);
            var sTile = tile.GetComponent<Tile>();
            sTile.Poss = new Vector2Int(i, j);
            
            // Material
            var material = blackMat;
            if (count % 2 == 0) material = whiteMat;
            tile.GetComponent<MeshRenderer>().sharedMaterial = material;
            return sTile;
        }

        
        // Properties
        
        public Vector2 Size { get; set; }
    }
}
