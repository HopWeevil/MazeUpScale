using CodeBase.Infrastructure.Factories;
using UnityEngine;
using Zenject;

namespace CodeBase.Logic.Player
{
    public class PlayerDeath : MonoBehaviour
    {
        [SerializeField] private PlayerHealth _health;
        [SerializeField] private AudioClip _death;

        private IUIFactory _factory;

        [Inject]
        private void Construct(IUIFactory factory)
        {
            _factory = factory;
        }

        private bool _isDead;

        private void Start()
        {
            _health.HealthChanged += HealthChanged;
        }

        private void OnDestroy()
        {
            _health.HealthChanged -= HealthChanged;
        }

        private void HealthChanged()
        {
            if (!_isDead && _health.Current <= 0)
            {
                Die();
            }
        }

        private void Die()
        {
            _isDead = true;
            AudioSource.PlayClipAtPoint(_death, transform.position);
            _factory.CreateLevelFailedPanel();
        }
    }
}