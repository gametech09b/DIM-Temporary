using System.Collections.Generic;
using UnityEngine;

using DungeonGunner.AStarPathfinding;
using System.Collections;

namespace DungeonGunner {
    [DisallowMultipleComponent]
    #region Requirement Components
    [RequireComponent(typeof(Enemy))]
    #endregion
    public class EnemyMovementAI : MonoBehaviour {
        [SerializeField] private MovementDetailSO _movementDetail;

        private Enemy enemy;
        private Stack<Vector3> pathStack = new Stack<Vector3>();
        private Vector3 referenceTargetPosition;

        private Coroutine moveToPositionCoroutine;
        private WaitForFixedUpdate waitForFixedUpdate;
        private float rebuildPathCooldownTimer;

        [HideInInspector] public float moveSpeed;
        private bool isChasing;

        [HideInInspector] public int updateAtFrame = 1;

        private List<Vector2Int> surroundingPositionList = new List<Vector2Int>();



        private void Awake() {
            enemy = GetComponent<Enemy>();

            moveSpeed = _movementDetail.GetMoveSpeed();
        }



        private void Start() {
            waitForFixedUpdate = new WaitForFixedUpdate();

            referenceTargetPosition = GameManager.Instance.GetCurrentPlayer().GetPosition();
        }



        private void Update() {
            Move();
        }



        private void Move() {
            rebuildPathCooldownTimer -= Time.deltaTime;

            Vector3 currentTargetPosition = GameManager.Instance.GetCurrentPlayer().GetPosition();

            float distanceToTargetPosition = Vector3.Distance(transform.position, currentTargetPosition);

            if (!isChasing
            && distanceToTargetPosition > enemy.enemyDetail.chaseDistance)
                isChasing = true;

            if (isChasing)
                return;

            if (Time.frameCount % Settings.AStarTargetFrameRate != updateAtFrame)
                return;

            if (rebuildPathCooldownTimer <= 0f
            || Vector3.Distance(referenceTargetPosition, currentTargetPosition) > Settings.AStarPlayerDistanceToRebuildPath) {
                rebuildPathCooldownTimer = Settings.AStarEnemyRebuildCooldown;

                referenceTargetPosition = currentTargetPosition;

                CalculatePath();

                if (pathStack != null) {
                    if (moveToPositionCoroutine != null) {
                        enemy.idleEvent.CallOnIdleEvent();
                        StopCoroutine(moveToPositionCoroutine);
                    }

                    moveToPositionCoroutine = StartCoroutine(MoveToPositionCoroutine(pathStack));
                }
            }
        }



        private void CalculatePath() {
            Room currentRoom = GameManager.Instance.GetCurrentRoom();

            UnityEngine.Grid grid = currentRoom.GetGrid();
            Vector3Int gridPosition = grid.WorldToCell(transform.position);
            Vector3Int targetGridPosition = GetNearestNonObstacleTargetPosition(currentRoom);

            pathStack = AStar.BuildPath(currentRoom, gridPosition, targetGridPosition);

            if (pathStack != null)
                pathStack.Pop();
            else
                enemy.idleEvent.CallOnIdleEvent();
        }



        public void SetUpdateAtFrame(int _updateAtFrame) {
            this.updateAtFrame = _updateAtFrame;
        }



        private Vector3Int GetNearestNonObstacleTargetPosition(Room _currentRoom) {
            Vector3 targetPosition = GameManager.Instance.GetCurrentPlayer().GetPosition();

            Vector3Int targetCellPosition = _currentRoom.GetGrid().WorldToCell(targetPosition);

            Vector2Int adjustedTargetCellPosition = new Vector2Int(targetCellPosition.x - _currentRoom.templateLowerBounds.x, targetCellPosition.y - _currentRoom.templateLowerBounds.y);

            int movementPenalty = _currentRoom.roomGameObject.GetAStarMovementPenalty(adjustedTargetCellPosition.x, adjustedTargetCellPosition.y);
            int itemObstaclePenalty = _currentRoom.roomGameObject.GetAStarItemObstaclePenalty(adjustedTargetCellPosition.x, adjustedTargetCellPosition.y);
            int penalty = Mathf.Min(movementPenalty, itemObstaclePenalty);

            if (penalty != 0) {
                return targetCellPosition;
            } else {
                surroundingPositionList.Clear();

                for (int x = -1; x <= 1; x++) {
                    for (int y = -1; y <= 1; y++) {

                        if (y == 0 && x == 0)
                            continue;

                        surroundingPositionList.Add(new Vector2Int(x, y));
                    }
                }

                for (int iterator = 0; iterator < 8; iterator++) {
                    int index = Random.Range(0, surroundingPositionList.Count);

                    try {

                        movementPenalty = _currentRoom.roomGameObject.GetAStarMovementPenalty(adjustedTargetCellPosition.x + surroundingPositionList[index].x, adjustedTargetCellPosition.y + surroundingPositionList[index].y);
                        itemObstaclePenalty = _currentRoom.roomGameObject.GetAStarItemObstaclePenalty(adjustedTargetCellPosition.x + surroundingPositionList[index].x, adjustedTargetCellPosition.y + surroundingPositionList[index].y);

                        penalty = Mathf.Min(movementPenalty, itemObstaclePenalty);

                        if (penalty != 0)
                            return new Vector3Int(targetCellPosition.x + surroundingPositionList[index].x, targetCellPosition.y + surroundingPositionList[index].y, 0);
                    } catch (System.Exception) { }

                    surroundingPositionList.RemoveAt(index);
                }

                return (Vector3Int)_currentRoom.spawnPositionArray[Random.Range(0, _currentRoom.spawnPositionArray.Length)];
            }
        }



        private IEnumerator MoveToPositionCoroutine(Stack<Vector3> _pathStack) {
            while (_pathStack.Count > 0) {
                Vector3 nextPosition = _pathStack.Pop();

                while (Vector3.Distance(nextPosition, transform.position) > 0.2f) {
                    Vector2 directionVector = (nextPosition - transform.position).normalized;

                    enemy.moveToPositionEvent.CallOnMoveToPosition(transform.position, nextPosition, directionVector, moveSpeed);

                    yield return waitForFixedUpdate;
                }

                yield return waitForFixedUpdate;
            }

            enemy.idleEvent.CallOnIdleEvent();
        }



        #region Validation
#if UNITY_EDITOR
        private void OnValidate() {
            HelperUtilities.CheckNullValue(this, nameof(_movementDetail), _movementDetail);
        }
#endif
        #endregion
    }
}
