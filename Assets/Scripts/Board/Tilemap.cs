using UnityEngine;

namespace Board
{
    public class Tilemap : MonoBehaviour
    {
        public int rows;
        public int columns;
        public float offset;
        public Transform tilePref;
        
        // Start is called before the first frame update
        private void Start()
        {
            LoadGrid();
        }

        // Instantiates each tile of the grid
        private void LoadGrid()
        {
            var tileSize = tilePref.GetComponent<MeshRenderer>().bounds.size;
            var xOffset = tileSize.x + offset;
            var zOffset = tileSize.z + offset;
            
            for (var i = 0; i < rows; i++)
            {
                var z = zOffset * i;
                for (var j = 0; j < columns; j++)
                {
                    var x = xOffset * j;
                    var poss = new Vector3(x, 0, z);
                    var tile = Instantiate(tilePref, poss, Quaternion.identity);
                    tile.SetParent(transform);
                    tile.GetComponent<Tile>().Index(i, j);
                }
            }
        }
    }
}
