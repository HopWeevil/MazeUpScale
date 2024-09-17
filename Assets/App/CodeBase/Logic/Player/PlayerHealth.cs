using System;
using UnityEngine;

namespace CodeBase.Logic.Player
{
    public class PlayerHealth : MonoBehaviour
    {

        [SerializeField] private float _current;
        [SerializeField] private float _max;

        [SerializeField] private AudioClip _hit;

        public event Action HealthChanged;

        public float Current
        {
            get => _current;
            set => _current = value;
        }

        public float Max
        {
            get => _max;
            set => _max = value;
        }

        public void TakeDamage(float damage)
        {
            Current -= damage;
            HealthChanged?.Invoke();

            if(Current > 0)
            {
                AudioSource.PlayClipAtPoint(_hit, transform.position);
            }
        }

        public float GetCurrent()
        {
            return _current;
        }

        public float GetMax()
        {
            return _max;
        }
    }
}
