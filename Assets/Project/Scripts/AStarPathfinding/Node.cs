using System;
using UnityEngine;

namespace DungeonGunner.AStarPathfinding
{
    public class Node : IComparable<Node>
    {
        public Vector2Int position;
        public int gCost = 0;
        public int hCost = 0;

        public Node parentNode;

        public int FCost
        {
            get { return gCost + hCost; }
        }



        public Node(Vector2Int _position)
        {
            this.position = _position;
            this.parentNode = null;
        }



        public int CompareTo(Node _node)
        {
            int compare = FCost.CompareTo(_node.FCost);

            if (compare == 0)
            {
                compare = hCost.CompareTo(_node.hCost);
            }

            return compare;
        }
    }
}
