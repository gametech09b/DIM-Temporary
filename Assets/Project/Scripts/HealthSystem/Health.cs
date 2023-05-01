using System.Collections;
using UnityEngine;

namespace DungeonGunner
{
    [DisallowMultipleComponent]
    #region Requirement Components
    [RequireComponent(typeof(HealthEvent))]
    #endregion
    public class Health : MonoBehaviour
    {
        private int startingAmount;
        private int currentAmount;
        private HealthEvent healthEvent;

        private bool isImmuneAfterHit = false;
        private Coroutine immuneCoroutine;
        private float immuneDuration;

        private SpriteRenderer spriteRenderer;
        private const float spriteFlashInterval = 0.2f;
        private WaitForSeconds spriteFlashIntervalWaitForSeconds = new WaitForSeconds(spriteFlashInterval);

        private Player player;
        [HideInInspector] public Enemy enemy;

        [HideInInspector] public bool isDamageable = true;



        private void Awake()
        {
            healthEvent = GetComponent<HealthEvent>();
        }



        private void Start()
        {
            CallOnHealthChange(0);

            player = GetComponent<Player>();
            enemy = GetComponent<Enemy>();

            if (player != null)
            {
                if (player.playerDetail.isImmuneAfterHit)
                {
                    isImmuneAfterHit = true;
                    immuneDuration = player.playerDetail.immuneDuration;
                    spriteRenderer = player.spriteRenderer;
                }
            }
            else if (enemy != null)
            {
                if (enemy.enemyDetail.isImmuneAfterHit)
                {
                    isImmuneAfterHit = true;
                    immuneDuration = enemy.enemyDetail.immuneDuration;
                    spriteRenderer = enemy.spriteRendererArray[0];
                }
            }
        }



        private void CallOnHealthChange(int _damageAmount)
        {
            healthEvent.CallOnHealthChange(GetPercent(), currentAmount, _damageAmount);
        }



        public void TakeDamage(int _damageAmount)
        {
            bool isDashing = false;
            if (player != null)
                isDashing = player.controllerHandler.isDashing;

            if (isDamageable
            && !isDashing)
            {
                currentAmount -= _damageAmount;
                CallOnHealthChange(_damageAmount);

                PostHitImmune();
            }
        }



        private void PostHitImmune()
        {
            if (!gameObject.activeSelf)
                return;

            if (isImmuneAfterHit)
            {
                if (immuneCoroutine != null)
                    StopCoroutine(immuneCoroutine);

                immuneCoroutine = StartCoroutine(PostHitImmuneCoroutine(immuneDuration, spriteRenderer));
            }
        }



        private IEnumerator PostHitImmuneCoroutine(float _immuneDuration, SpriteRenderer _spriteRenderer)
        {
            int iterationAmount = Mathf.RoundToInt((_immuneDuration / spriteFlashInterval) * 0.5f);

            isDamageable = false;

            while (iterationAmount > 0)
            {
                _spriteRenderer.color = Color.red;
                yield return spriteFlashIntervalWaitForSeconds;
                _spriteRenderer.color = Color.white;
                yield return spriteFlashIntervalWaitForSeconds;

                iterationAmount--;

                yield return null;
            }

            isDamageable = true;
        }



        public void SetStartingAmount(int _amount)
        {
            startingAmount = _amount;
            currentAmount = _amount;
        }



        public void SetCurrentAmount(int _amount)
        {
            currentAmount = _amount;
        }



        public int GetStartingAmount()
        {
            return startingAmount;
        }



        public int GetCurrentAmount()
        {
            return currentAmount;
        }



        private float GetPercent()
        {
            return (float)currentAmount / (float)startingAmount;
        }
    }
}
