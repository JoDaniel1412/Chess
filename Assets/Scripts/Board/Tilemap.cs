using System;
using UnityEditor;
using UnityEngine;

namespace Board
{
    [ExecuteInEditMode]
    public class Tilemap : MonoBehaviour
    {
        public int rows;
        public int columns;
        public float offset;
        public Transform tilePref;
        
        
        private void Start()
        {
            LoadGrid();
        }

        private void OnEnable()
        {
            DestroyGrid();
            LoadGrid();
        }

        private void OnDisable()
        {
            DestroyGrid();
        }

        // Instantiates each tile of the grid
        private void LoadGrid()
        {
            var tileSize = tilePref.GetComponent<MeshRenderer>().bounds.size;
            var xOffset = tileSize.x + offset;
            var zOffset = tileSize.z + offset;
            var count = 0;
            
            for (var i = 0; i < rows; i++)
            {
                var z = zOffset * i;
                for (var j = 0; j < columns; j++)
                {
                    var x = xOffset * j;
                    var poss = new Vector3(x, 0, z);
                    var tile = Instantiate(tilePref, poss, Quaternion.identity);
                    count++;
                    SetupTile(tile, i, j, count);
                }

                count--;
            }
        }
        
        // Deletes the Grid
        private static void DestroyGrid()
        {
            var tiles = GameObject.FindGameObjectsWithTag("Tile");
            foreach (var children in tiles)
                DestroyImmediate(children);
        }

        // Selects the color for the Tile and other properties
        private void SetupTile(Transform tile, int i, int j, int count)
        {
            tile.SetParent(transform);
            tile.GetComponent<Tile>().Index(i, j);
            
            // Material
            const string materialsPath = "Assets/Materials/Board/";
            var path = materialsPath + "BlackTile.mat";
            if (count % 2 == 0) path = materialsPath + "WhiteTile.mat";
            var material = AssetDatabase.LoadAssetAtPath<Material>(path);
            tile.GetComponent<MeshRenderer>().sharedMaterial = material;
        }
    }
}
