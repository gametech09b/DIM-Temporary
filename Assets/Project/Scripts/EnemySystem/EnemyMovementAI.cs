using System.Collections.Generic;
using UnityEngine;

using DungeonGunner.AStarPathfinding;
using System.Collections;

namespace DungeonGunner
{
    [DisallowMultipleComponent]
    #region Requirement Components
    [RequireComponent(typeof(Enemy))]
    #endregion
    public class EnemyMovementAI : MonoBehaviour
    {
        [SerializeField] private MovementDetailSO _movementDetail;

        private Enemy _enemy;
        private Stack<Vector3> _pathStack = new Stack<Vector3>();
        private Vector3 _referenceTargetPosition;

        private Coroutine _moveToPositionCoroutine;
        private WaitForFixedUpdate _waitForFixedUpdate;
        private float _rebuildPathCooldownTimer;

        [HideInInspector] public float moveSpeed;
        private bool _isChasing;



        private void Awake()
        {
            _enemy = GetComponent<Enemy>();

            moveSpeed = _movementDetail.GetMoveSpeed();
        }



        private void Start()
        {
            _waitForFixedUpdate = new WaitForFixedUpdate();

            _referenceTargetPosition = GameManager.Instance.GetCurrentPlayer().GetPosition();
        }



        private void Update()
        {
            Move();
        }



        private void Move()
        {
            _rebuildPathCooldownTimer -= Time.deltaTime;

            Vector3 currentTargetPosition = GameManager.Instance.GetCurrentPlayer().GetPosition();

            float distanceToTargetPosition = Vector3.Distance(transform.position, currentTargetPosition);

            if (!_isChasing && distanceToTargetPosition > _enemy.detail.chaseDistance)
            {
                _isChasing = true;
            }

            if (_isChasing) return;


            if (_rebuildPathCooldownTimer <= 0f || Vector3.Distance(_referenceTargetPosition, currentTargetPosition) > Settings.AStarPlayerDistanceToRebuildPath)
            {
                _rebuildPathCooldownTimer = Settings.AStarEnemyRebuildCooldown;

                _referenceTargetPosition = currentTargetPosition;

                CalculatePath();

                if (_pathStack != null)
                {
                    if (_moveToPositionCoroutine != null)
                    {
                        _enemy.idleEvent.CallOnIdleEvent();
                        StopCoroutine(_moveToPositionCoroutine);
                    }

                    _moveToPositionCoroutine = StartCoroutine(MoveToPositionCoroutine(_pathStack));
                }
            }
        }



        private void CalculatePath()
        {
            Room currentRoom = GameManager.Instance.GetCurrentRoom();

            UnityEngine.Grid grid = currentRoom.GetGrid();
            Vector3Int gridPosition = grid.WorldToCell(transform.position);
            Vector3Int targetGridPosition = GetNearestNonObstacleTargetPosition(currentRoom);

            _pathStack = AStar.BuildPath(currentRoom, gridPosition, targetGridPosition);

            if (_pathStack != null)
            {
                _pathStack.Pop();
            }
            else
            {
                _enemy.idleEvent.CallOnIdleEvent();
            }
        }



        private Vector3Int GetNearestNonObstacleTargetPosition(Room currentRoom)
        {
            Vector3 targetPosition = GameManager.Instance.GetCurrentPlayer().GetPosition();

            Vector3Int targetCellPosition = currentRoom.GetGrid().WorldToCell(targetPosition);

            Vector2Int adjustedTargetCellPosition = new Vector2Int(targetCellPosition.x - currentRoom.templateLowerBounds.x, targetCellPosition.y - currentRoom.templateLowerBounds.y);

            int penalty = currentRoom.roomGameObject.GetAStarMovementPenalty(adjustedTargetCellPosition.x, adjustedTargetCellPosition.y);

            if (penalty != 0)
            {
                return targetCellPosition;
            }

            return FindNonObstacleTargetPosition(currentRoom, targetCellPosition, adjustedTargetCellPosition);
        }



        private Vector3Int FindNonObstacleTargetPosition(Room currentRoom, Vector3Int targetCellPosition, Vector2Int adjustedTargetCellPosition)
        {
            for (int i = -1; i <= 1; i++)
            {
                for (int j = -1; i <= 1; j++)
                {
                    if (j == 0 && i == 0) continue;

                    try
                    {
                        int penalty = currentRoom.roomGameObject.GetAStarMovementPenalty(adjustedTargetCellPosition.x + i, adjustedTargetCellPosition.y + j);

                        if (penalty != 0)
                        {
                            return new Vector3Int(targetCellPosition.x + i, targetCellPosition.y + j, 0);
                        }
                    }
                    catch (System.Exception)
                    {
                        continue;
                    }
                }
            }

            return targetCellPosition;
        }



        private IEnumerator MoveToPositionCoroutine(Stack<Vector3> pathStack)
        {
            while (pathStack.Count > 0)
            {
                Vector3 nextPosition = pathStack.Pop();

                while (Vector3.Distance(nextPosition, transform.position) > 0.2f)
                {
                    Vector2 directionVector = (nextPosition - transform.position).normalized;

                    _enemy.moveToPositionEvent.CallOnMoveToPosition(transform.position, nextPosition, directionVector, moveSpeed);

                    yield return _waitForFixedUpdate;
                }

                yield return _waitForFixedUpdate;
            }

            _enemy.idleEvent.CallOnIdleEvent();
        }



        #region Validation
#if UNITY_EDITOR
        private void OnValidate()
        {
            HelperUtilities.CheckNullValue(this, nameof(_movementDetail), _movementDetail);
        }
#endif
        #endregion
    }
}
