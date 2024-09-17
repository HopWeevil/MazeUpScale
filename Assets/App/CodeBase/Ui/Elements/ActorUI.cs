using CodeBase.Logic.Player;
using UnityEngine;

namespace CodeBase.UI.Elements
{
    public class ActorUI : MonoBehaviour
    {
        public HpBar HpBar;

        private PlayerHealth _health;

        public void Construct(PlayerHealth health)
        {
            _health = health;
            UpdateHpBar();
        }

        private void Start()
        {
            _health.HealthChanged += UpdateHpBar;

        }

        private void OnDestroy()
        {
            _health.HealthChanged -= UpdateHpBar;
        }
    
        private void UpdateHpBar()
        {
            HpBar.SetValue(_health.GetCurrent(), _health.GetMax());
        }
    }
}