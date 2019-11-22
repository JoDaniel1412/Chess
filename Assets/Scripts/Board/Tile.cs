using UnityEngine;

namespace Board
{
    public class Tile : MonoBehaviour
    {
        private int _i;
        private int _j;
        
        public void Index(int i, int j)
        {
            _i = i;
            _j = j;
        }

        public int I => _i;

        public int J => _j;
    }
}
