using System;
using System.Collections;
using UnityEngine;

namespace Board
{
    public class Tile : MonoBehaviour
    {
        public Material highlight;
        public Material movement; 
            
        private int _i;
        private int _j;
        private bool _occupied;
        private Material _defaultMaterial;

        // Changes the Tile color to Movement material
        public void HighlightMovement(bool state)
        {
            if (state) Highlight(movement);
            else UnHighlight();
        }

        public void Index(int i, int j)
        {
            _i = i;
            _j = j;
        }

        public void OnCollisionStay(Collision other)
        {
            // Checks if the Tile currently has a Piece on it
            if (other.gameObject.CompareTag("Piece"))
                _occupied = true;
        }

        private void Start()
        {
            _defaultMaterial = GetComponent<MeshRenderer>().material;
        }

        // Highlights the Tile when the mouse enters
        private void OnMouseEnter()
        {
            Highlight(highlight);
        }

        // Removes the highlight of the Tile when mouse exit
        private void OnMouseExit()
        {
            UnHighlight();
        }

        private void Highlight(Material material)
        {
            GetComponent<MeshRenderer>().material = material;
        }

        public void UnHighlight()
        {
            GetComponent<MeshRenderer>().material = _defaultMaterial;
        }
        

        public int I => _i;

        public int J => _j;
        
        public bool Occupied => _occupied;
    }
}
