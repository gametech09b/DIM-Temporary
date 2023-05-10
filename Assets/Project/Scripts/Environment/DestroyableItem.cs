using System.Collections;
using UnityEngine;

namespace DungeonGunner
{
    [DisallowMultipleComponent]
    #region Requirement Components
    [RequireComponent(typeof(Animator))]
    [RequireComponent(typeof(BoxCollider2D))]

    [RequireComponent(typeof(Health))]
    [RequireComponent(typeof(HealthEvent))]
    [RequireComponent(typeof(TakeContactDamage))]
    #endregion
    public class DestroyableItem : MonoBehaviour
    {
        [SerializeField] private int startingHealthAmount = 1;
        [SerializeField] private SoundEffectSO destroySoundEffect;

        private Animator animator;
        private BoxCollider2D boxCollider2D;

        private Health health;
        private HealthEvent healthEvent;
        private TakeContactDamage takeContactDamage;



        private void Awake()
        {
            animator = GetComponent<Animator>();
            boxCollider2D = GetComponent<BoxCollider2D>();

            health = GetComponent<Health>();
            healthEvent = GetComponent<HealthEvent>();
            takeContactDamage = GetComponent<TakeContactDamage>();
        }



        private void Start()
        {
            health.SetStartingAmount(startingHealthAmount);
        }



        private void OnEnable()
        {
            healthEvent.OnHealthChanged += HealthEvent_OnHealthChanged;
        }



        private void OnDisable()
        {
            healthEvent.OnHealthChanged -= HealthEvent_OnHealthChanged;
        }



        private void HealthEvent_OnHealthChanged(HealthEvent _sender, OnHealthChangedEventArgs _args)
        {
            if (_args.healthAmount <= 0f)
            {
                StartCoroutine(DestroyedAnimationCoroutine());
            }
        }



        private IEnumerator DestroyedAnimationCoroutine()
        {
            Destroy(boxCollider2D);

            if (destroySoundEffect != null)
            {
                SoundEffectManager.Instance.PlaySoundEffect(destroySoundEffect);
            }

            animator.SetBool(Settings.Destroy, true);

            while (!animator.GetCurrentAnimatorStateInfo(0).IsName(Settings.DestroyedState))
            {
                yield return null;
            }

            Destroy(animator);
            Destroy(health);
            Destroy(healthEvent);
            Destroy(takeContactDamage);
            Destroy(this);
        }
    }
}
