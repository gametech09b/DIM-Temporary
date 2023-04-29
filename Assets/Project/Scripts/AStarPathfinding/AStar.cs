using System.Collections.Generic;
using UnityEngine;

namespace DungeonGunner.AStarPathfinding
{
    public static class AStar
    {
        public static Stack<Vector3> BuildPath(Room _room, Vector3Int _startGridPosition, Vector3Int _endGridPosition)
        {
            _startGridPosition -= (Vector3Int)_room.templateLowerBounds;
            _endGridPosition -= (Vector3Int)_room.templateLowerBounds;

            List<Node> openNodeList = new List<Node>();
            HashSet<Node> closeNodeHashSet = new HashSet<Node>();

            AStarGrid grid = new AStarGrid(_room.templateUpperBounds.x - _room.templateLowerBounds.x + 1, _room.templateUpperBounds.y - _room.templateLowerBounds.y + 1);

            Node startNode = grid.GetNode(_startGridPosition.x, _startGridPosition.y);
            Node targetNode = grid.GetNode(_endGridPosition.x, _endGridPosition.y);

            Node endPathNode = FindShortestPath(startNode, targetNode, grid, openNodeList, closeNodeHashSet, _room.roomGameObject);

            if (endPathNode != null)
            {
                return CreatePathStack(endPathNode, _room);
            }

            return null;
        }



        private static Node FindShortestPath(Node _startNode, Node _targetNode, AStarGrid _grid, List<Node> _openNodeList, HashSet<Node> _closedNodeHashSet, RoomGameObject _roomGameObject)
        {
            _openNodeList.Add(_startNode);

            while (_openNodeList.Count > 0)
            {
                _openNodeList.Sort();

                Node currentNode = _openNodeList[0];
                _openNodeList.RemoveAt(0);

                if (currentNode == _targetNode)
                {
                    return currentNode;
                }

                _closedNodeHashSet.Add(currentNode);

                EvaluateCurrentNodeNeighbours(currentNode, _targetNode, _grid, _openNodeList, _closedNodeHashSet, _roomGameObject);
            }

            return null;
        }



        public static void EvaluateCurrentNodeNeighbours(Node _currentNode, Node _targetNode, AStarGrid _grid, List<Node> _openNodeList, HashSet<Node> _closedNodeHashSet, RoomGameObject _roomGameObject)
        {
            Vector2Int currentNodePosition = _currentNode.position;

            Node neighbourNode;

            for (int i = -1; i <= 1; i++)
            {
                for (int j = -1; j <= 1; j++)
                {
                    if (i == 0 && j == 0)
                    {
                        continue;
                    }

                    neighbourNode = GetValidNeighbourNode(currentNodePosition.x + i, currentNodePosition.y + j, _grid, _closedNodeHashSet, _roomGameObject);


                    if (neighbourNode != null)
                    {
                        int movementPenalty = _roomGameObject.GetAStarMovementPenalty(neighbourNode.position.x, neighbourNode.position.y);

                        int newCostToNeighbour = _currentNode.gCost + GetDistance(_currentNode, neighbourNode) + movementPenalty;

                        bool isNeighbourNodeInOpenList = _openNodeList.Contains(neighbourNode);

                        if (newCostToNeighbour < neighbourNode.gCost || !isNeighbourNodeInOpenList)
                        {
                            neighbourNode.gCost = newCostToNeighbour;
                            neighbourNode.hCost = GetDistance(neighbourNode, _targetNode);
                            neighbourNode.parentNode = _currentNode;

                            if (!isNeighbourNodeInOpenList)
                            {
                                neighbourNode.gCost = newCostToNeighbour;
                                neighbourNode.hCost = GetDistance(neighbourNode, _targetNode);
                                neighbourNode.parentNode = _currentNode;

                                if (!isNeighbourNodeInOpenList)
                                {
                                    _openNodeList.Add(neighbourNode);
                                }
                            }
                        }
                    }
                }
            }
        }



        public static Node GetValidNeighbourNode(int _x, int _y, AStarGrid _grid, HashSet<Node> _closedNodeHashSet, RoomGameObject _roomGameObject)
        {
            Vector2Int templateLowerBounds = _roomGameObject.room.templateLowerBounds;
            Vector2Int templateUpperBounds = _roomGameObject.room.templateUpperBounds;

            if (_x >= templateUpperBounds.x - templateLowerBounds.x
            || _x < 0
            || _y >= templateUpperBounds.y - templateLowerBounds.y
            || _y < 0)
            {
                return null;
            }

            Node neighbourNode = _grid.GetNode(_x, _y);

            int movementPenalty = _roomGameObject.GetAStarMovementPenalty(neighbourNode.position.x, neighbourNode.position.y);

            if (movementPenalty == 0 || _closedNodeHashSet.Contains(neighbourNode))
            {
                return null;
            }
            else
            {
                return neighbourNode;
            }
        }



        public static int GetDistance(Node _nodeA, Node _nodeB)
        {
            int distanceX = Mathf.Abs(_nodeA.position.x - _nodeB.position.x);
            int distanceY = Mathf.Abs(_nodeA.position.y - _nodeB.position.y);

            if (distanceX > distanceY)
            {
                return 14 * distanceY + 10 * (distanceX - distanceY);
            }

            return 14 * distanceX + 10 * (distanceY - distanceX);
        }



        public static Stack<Vector3> CreatePathStack(Node _targetNode, Room _room)
        {
            Stack<Vector3> pathStack = new Stack<Vector3>();

            Node nextNode = _targetNode;

            Vector3 cellMiddle = _room.roomGameObject.grid.cellSize * 0.5f;
            cellMiddle.z = 0;

            while (nextNode != null)
            {
                Vector3Int cellPosition = new Vector3Int(nextNode.position.x + _room.templateLowerBounds.x, nextNode.position.y + _room.templateLowerBounds.y, 0);

                Vector3 cellWorldPosition = _room.roomGameObject.grid.CellToWorld(cellPosition);

                cellWorldPosition += cellMiddle;

                pathStack.Push(cellWorldPosition);

                nextNode = nextNode.parentNode;
            }

            return pathStack;
        }
    }
}
