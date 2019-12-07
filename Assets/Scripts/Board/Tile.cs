using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Board
{
    public class Tile : MonoBehaviour
    {
        public GameObject highlight;
        public Material selectedMaterial;
        public Material movementMaterial; 
        public Material enemyMaterial;

        private bool _moving;
        private GameObject _piece;
        private Vector3 _defaultScale;
        private TilesController _tilesController;
        private List<GameObject> _colliders = new List<GameObject>();

        // Changes the Tile color to Movement material
        public void HighlightMovement(bool state)
        {
            if (state) Highlight(movementMaterial);
            _moving = state;
            if (!state) UnHighlight();
        }
        
        // Changes the Tile color to Enemy material
        public void HighlightEnemy(bool state)
        {
            if (state) Highlight(enemyMaterial);
            _moving = state;
            if (!state) UnHighlight();
        }
        
        // Highlights the Tile when the mouse enters
        public void Selected()
        {
            _tilesController.TileTarget = gameObject;
            
            if (_moving) Elevate(true);
            else Highlight(selectedMaterial);
        }

        // Removes the highlight of the Tile when mouse exit
        public void UnSelected()
        {
            if (_moving) Elevate(false);
            else UnHighlight();
        }

        // Checks when a Piece enters the Tile
        public void OnCollisionEnter(Collision other)
        {
            if (!other.gameObject.CompareTag("Piece")) return;
            _colliders.Add(other.gameObject);
        }

        // Checks when a Piece exits the Tile
        public void OnCollisionExit(Collision other)
        {
            if (!other.gameObject.CompareTag("Piece")) return;
            _colliders.RemoveAll(other.gameObject.Equals);
        }

        public void PieceDead(GameObject pieceObject)
        {
            _colliders.RemoveAll(pieceObject.Equals);
        }

        private void Start()
        {
            _defaultScale = highlight.transform.localScale;
            _tilesController = FindObjectOfType<TilesController>();
        }

        private void Update()
        {
            // Updates the current piece in the Tile
            _piece = _colliders.Count > 0 ? _colliders.First() : null;
        }

        // Moves the current Piece
        private void OnMouseDown()
        {
            Debug.LogFormat("[INFO] Tile: Occupied: {0}, Colliders: {1}, Poss: {2}", Occupied, _colliders.Count, Poss);
            if (!_piece) return;
            _piece.SendMessage("Selected");
        }

        // Drops the current Piece
        private void OnMouseUp()
        {
            if (!_piece) return;
            _piece.SendMessage("Dropped");
        }

        // Turns on the Highlight of the Tile to the given material
        private void Highlight(Material material)
        {
            if (_moving) return;
            highlight.SetActive(true);
            highlight.GetComponent<MeshRenderer>().material = material;
        }
        
        // Turns off the Highlight of the Tile
        private void UnHighlight()
        {
            if (_moving) return;
            highlight.SetActive(false);
            highlight.transform.localScale = _defaultScale;
            Elevated = false;
        }

        // Raises and resets the Highlight of the Tile
        private void Elevate(bool state)
        {
            Elevated = state;
            var size = -1;
            if (state) size = 1;
            var scale = highlight.transform.localScale;
            scale.y += size;
            highlight.transform.localScale = scale;
        }

        private void OnMouseEnter() => Selected();

        private void OnMouseExit() => UnSelected();


        // Properties
        
        public Vector2Int Poss { get; set; }

        public bool Occupied => _colliders.Count > 0;
        
        public bool Elevated { get; set; }
    }
}
