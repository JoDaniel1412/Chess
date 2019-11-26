using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Pieces;
using UnityEngine;

namespace Board
{
    public class Tile : MonoBehaviour
    {
        public GameObject highlight;
        public Material selectedMaterial;
        public Material movementMaterial; 
        public Material enemyMaterial; 
            
        private int _i;
        private int _j;
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

        public void Index(int i, int j)
        {
            _i = i;
            _j = j;
        }

        public void OnCollisionEnter(Collision other)
        {
            // Checks if the Tile currently has a Piece on it
            if (!other.gameObject.CompareTag("Piece")) return;
            _colliders.Add(other.gameObject);
        }

        public void OnCollisionExit(Collision other)
        {
            if (!other.gameObject.CompareTag("Piece")) return;
            _colliders.Remove(other.gameObject);
        }

        private void Start()
        {
            _defaultScale = highlight.transform.localScale;
            _tilesController = FindObjectOfType<TilesController>();
        }

        private void Update()
        {
            _piece = _colliders.Count > 0 ? _colliders.First() : null;
        }

        // Moves the current Piece
        private void OnMouseDown()
        {
            if (!_piece) return;
            _piece.SendMessage("Selected");
        }

        // Drops the current Piece
        private void OnMouseUp()
        {
            if (!_piece) return;
            _piece.SendMessage("Dropped");
        }

        private void OnMouseEnter()
        {
            _tilesController.TileTarget = gameObject;
            
            // Highlights the Tile when the mouse enters
            if (_moving)
            {
                Elevate(true);
                return;
            }
            Highlight(selectedMaterial);
        }

        // Removes the highlight of the Tile when mouse exit
        private void OnMouseExit()
        {
            if (_moving)
            {
                Elevate(false);
                return;
            }
            UnHighlight();
        }

        private void Highlight(Material material)
        {
            if (_moving) return;
            highlight.SetActive(true);
            highlight.GetComponent<MeshRenderer>().material = material;
        }

        private void UnHighlight()
        {
            if (_moving) return;
            highlight.SetActive(false);
            highlight.transform.localScale = _defaultScale;
            Elevated = false;
        }

        // Raises the Highlight of the tile
        private void Elevate(bool state)
        {
            Elevated = state;
            var size = -1;
            if (state) size = 1;
            var scale = highlight.transform.localScale;
            scale.y += size;
            highlight.transform.localScale = scale;
        }
        

        public int I => _i;

        public int J => _j;

        public bool Occupied => _colliders.Count > 0;
        
        public bool Elevated { get; set; }
    }
}
