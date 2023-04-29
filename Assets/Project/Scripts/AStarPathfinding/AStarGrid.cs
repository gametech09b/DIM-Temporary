using UnityEngine;

namespace DungeonGunner.AStarPathfinding
{
    public class AStarGrid
    {
        private int width;
        private int height;

        private Node[,] nodes;



        public AStarGrid(int _width, int _height)
        {
            this.width = _width;
            this.height = _height;

            nodes = new Node[_width, _height];

            for (int x = 0; x < _width; x++)
            {
                for (int y = 0; y < _height; y++)
                {
                    nodes[x, y] = new Node(new Vector2Int(x, y));
                }
            }
        }



        public Node GetNode(int _x, int _y)
        {
            if (_x < width && _y < height)
            {
                return nodes[_x, _y];
            }
            else
            {
                Debug.Log("Position Out of Range");
                return null;
            }
        }
    }
}
