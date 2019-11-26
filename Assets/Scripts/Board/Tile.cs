using System;
using System.Collections;
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
        private bool _occupied;

        // Changes the Tile color to Movement material
        public void HighlightMovement(bool state)
        {
            if (state) Highlight(movementMaterial);
            else UnHighlight();
        }
        
        // Changes the Tile color to Enemy material
        public void HighlightEnemy(bool state)
        {
            if (state) Highlight(enemyMaterial);
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

        // Highlights the Tile when the mouse enters
        private void OnMouseEnter()
        {
            Highlight(selectedMaterial);
        }

        // Removes the highlight of the Tile when mouse exit
        private void OnMouseExit()
        {
            UnHighlight();
        }

        private void Highlight(Material material)
        {
            highlight.SetActive(true);
            highlight.GetComponent<MeshRenderer>().material = material;
        }

        public void UnHighlight()
        {
            highlight.SetActive(false);
        }
        

        public int I => _i;

        public int J => _j;
        
        public bool Occupied => _occupied;
    }
}
