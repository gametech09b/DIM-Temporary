using System.Net.Http.Headers;
using UnityEngine;

namespace DungeonGunner.AStarPathfinding
{
    public class Grid
    {
        private int _width;
        private int _height;

        private Node[,] _nodes;



        public Grid(int width, int height)
        {
            this._width = width;
            this._height = height;

            _nodes = new Node[width, height];

            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    _nodes[x, y] = new Node(new Vector2Int(x, y));
                }
            }
        }



        public Node GetNode(int xPosition, int yPosition)
        {
            if (xPosition < _width && yPosition < _height)
            {
                return _nodes[xPosition, yPosition];
            }
            else
            {
                Debug.Log("Position Out of Range");
                return null;
            }
        }
    }
}
