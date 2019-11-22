using System;
using System.Collections;
using UnityEngine;

namespace Board
{
    public class Tile : MonoBehaviour
    {
        public Material highlight; 
            
        private int _i;
        private int _j;
        private Material _defaultMaterial;
        
        
        public void Index(int i, int j)
        {
            _i = i;
            _j = j;
        }
        
        // Highlights the Tile when the mouse enters
        private void OnMouseEnter()
        {
            var mesh = GetComponent<MeshRenderer>();
            _defaultMaterial = mesh.material;
            mesh.material = highlight;
        }

        // Removes the highlight of the Tile when mouse exit
        private void OnMouseExit()
        {
            var mesh = GetComponent<MeshRenderer>();
            mesh.material = _defaultMaterial;
        }

        public int I => _i;

        public int J => _j;
    }
}
