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



        public Node(Vector2Int position)
        {
            this.position = position;
            this.parentNode = null;
        }



        public int CompareTo(Node node)
        {
            int compare = FCost.CompareTo(node.FCost);

            if (compare == 0)
            {
                compare = hCost.CompareTo(node.hCost);
            }

            return compare;
        }
    }
}
