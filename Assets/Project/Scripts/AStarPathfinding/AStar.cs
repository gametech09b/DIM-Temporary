using System.Collections.Generic;
using UnityEngine;

namespace DungeonGunner.AStarPathfinding
{
    public static class AStar
    {
        public static Stack<Vector3> BuildPath(Room room, Vector3Int startGridPosition, Vector3Int endGridPosition)
        {
            startGridPosition -= (Vector3Int)room.templateLowerBounds;
            endGridPosition -= (Vector3Int)room.templateLowerBounds;

            List<Node> openNodeList = new List<Node>();
            HashSet<Node> closeNodeHashSet = new HashSet<Node>();

            Grid grid = new Grid(room.templateUpperBounds.x - room.templateLowerBounds.x + 1, room.templateUpperBounds.y - room.templateLowerBounds.y + 1);

            Node startNode = grid.GetNode(startGridPosition.x, startGridPosition.y);
            Node targetNode = grid.GetNode(endGridPosition.x, endGridPosition.y);

            Node endPathNode = FindShortestPath(startNode, targetNode, grid, openNodeList, closeNodeHashSet, room.roomGameObject);

            if (endPathNode != null)
            {
                return CreatePathStack(endPathNode, room);
            }

            return null;
        }



        private static Node FindShortestPath(Node startNode, Node targetNode, Grid grid, List<Node> openNodeList, HashSet<Node> closedNodeHashSet, RoomGameObject roomGameObject)
        {
            openNodeList.Add(startNode);

            while (openNodeList.Count > 0)
            {
                openNodeList.Sort();

                Node currentNode = openNodeList[0];
                openNodeList.RemoveAt(0);

                if (currentNode == targetNode)
                {
                    return currentNode;
                }

                closedNodeHashSet.Add(currentNode);

                EvaluateCurrentNodeNeighbours(currentNode, targetNode, grid, openNodeList, closedNodeHashSet, roomGameObject);
            }

            return null;
        }



        public static void EvaluateCurrentNodeNeighbours(Node currentNode, Node targetNode, Grid grid, List<Node> openNodeList, HashSet<Node> closedNodeHashSet, RoomGameObject roomGameObject)
        {
            Vector2Int currentNodePosition = currentNode.position;

            Node neighbourNode;

            for (int i = -1; i <= 1; i++)
            {
                for (int j = -1; j <= 1; j++)
                {
                    if (i == 0 && j == 0)
                    {
                        continue;
                    }

                    neighbourNode = GetValidNeighbourNode(currentNodePosition.x + i, currentNodePosition.y + j, grid, closedNodeHashSet, roomGameObject);


                    if (neighbourNode != null)
                    {
                        int movementPenalty = roomGameObject.GetAStarMovementPenalty(neighbourNode.position.x, neighbourNode.position.y);

                        int newCostToNeighbour = currentNode.gCost + GetDistance(currentNode, neighbourNode) + movementPenalty;

                        bool isNeighbourNodeInOpenList = openNodeList.Contains(neighbourNode);

                        if (newCostToNeighbour < neighbourNode.gCost || !isNeighbourNodeInOpenList)
                        {
                            neighbourNode.gCost = newCostToNeighbour;
                            neighbourNode.hCost = GetDistance(neighbourNode, targetNode);
                            neighbourNode.parentNode = currentNode;

                            if (!isNeighbourNodeInOpenList)
                            {
                                neighbourNode.gCost = newCostToNeighbour;
                                neighbourNode.hCost = GetDistance(neighbourNode, targetNode);
                                neighbourNode.parentNode = currentNode;

                                if (!isNeighbourNodeInOpenList)
                                {
                                    openNodeList.Add(neighbourNode);
                                }
                            }
                        }
                    }
                }
            }
        }



        public static Node GetValidNeighbourNode(int x, int y, Grid grid, HashSet<Node> closedNodeHashSet, RoomGameObject roomGameObject)
        {
            Vector2Int templateLowerBounds = roomGameObject.room.templateLowerBounds;
            Vector2Int templateUpperBounds = roomGameObject.room.templateUpperBounds;

            if (x >= templateUpperBounds.x - templateLowerBounds.x
            || x < 0
            || y >= templateUpperBounds.y - templateLowerBounds.y
            || y < 0)
            {
                return null;
            }

            Node neighbourNode = grid.GetNode(x, y);

            int movementPenalty = roomGameObject.GetAStarMovementPenalty(neighbourNode.position.x, neighbourNode.position.y);

            if (movementPenalty == 0 || closedNodeHashSet.Contains(neighbourNode))
            {
                return null;
            }
            else
            {
                return neighbourNode;
            }
        }



        public static int GetDistance(Node nodeA, Node nodeB)
        {
            int distanceX = Mathf.Abs(nodeA.position.x - nodeB.position.x);
            int distanceY = Mathf.Abs(nodeA.position.y - nodeB.position.y);

            if (distanceX > distanceY)
            {
                return 14 * distanceY + 10 * (distanceX - distanceY);
            }

            return 14 * distanceX + 10 * (distanceY - distanceX);
        }



        public static Stack<Vector3> CreatePathStack(Node targetNode, Room room)
        {
            Stack<Vector3> pathStack = new Stack<Vector3>();

            Node nextNode = targetNode;

            Vector3 cellMiddle = room.roomGameObject.grid.cellSize * 0.5f;
            cellMiddle.z = 0;

            while (nextNode != null)
            {
                Vector3Int cellPosition = new Vector3Int(nextNode.position.x + room.templateLowerBounds.x, nextNode.position.y + room.templateLowerBounds.y, 0);

                Vector3 cellWorldPosition = room.roomGameObject.grid.CellToWorld(cellPosition);

                cellWorldPosition += cellMiddle;

                pathStack.Push(cellWorldPosition);

                nextNode = nextNode.parentNode;
            }

            return pathStack;
        }
    }
}
