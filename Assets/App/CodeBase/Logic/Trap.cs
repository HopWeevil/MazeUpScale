using System.Collections;
using CodeBase.Logic.Player;
using UnityEngine;
using DG.Tweening;

namespace CodeBase.Logic
{
    public class Trap : MonoBehaviour
    {
        [SerializeField] private ParticleSystem[] _fireWalls;
        [SerializeField] private float _activateDuration = 1f;
        [SerializeField] private float _deactivateDuration = 1f;
        [SerializeField] private float _activeTime = 5f;
        [SerializeField] private float _deactivateTime = 5f;
        [SerializeField] private int _damage = 10;
        [SerializeField] private float _damageTickInterval = 1f;

        private bool _isActive = false;
        private float _damageTickTimer = 0f;

        private void Start()
        {
            StartCoroutine(ActivateDeactivateCycle());
        }

        IEnumerator ActivateDeactivateCycle()
        {
            while (true)
            {
                ActivateFireWalls();
                _isActive = true;
                yield return new WaitForSeconds(_activeTime);

                DeactivateFireWalls();
                _isActive = false;
                yield return new WaitForSeconds(_deactivateTime);
            }
        }

        private void ActivateFireWalls()
        {
            foreach (ParticleSystem fireWall in _fireWalls)
            {
                var emission = fireWall.emission;
                fireWall.transform.DOScale(Vector3.one, _activateDuration).SetEase(Ease.OutSine).SetLink(gameObject);
                emission.enabled = true;
                fireWall.Play();
            }
        }

        private void DeactivateFireWalls()
        {
            foreach (ParticleSystem fireWall in _fireWalls)
            {
                var emission = fireWall.emission;
                fireWall.transform.DOScale(Vector3.zero, _deactivateDuration).SetEase(Ease.InSine).SetLink(gameObject).OnComplete(() =>
                {
                    emission.enabled = false;
                    fireWall.Stop();
                });
            }
        }

        private void OnTriggerStay(Collider other)
        {
            if (_isActive && other.TryGetComponent(out PlayerHealth health))
            {
                _damageTickTimer += Time.deltaTime;
                if (_damageTickTimer >= _damageTickInterval)
                {
                    health.TakeDamage(_damage);
                    _damageTickTimer = 0f;
                }
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.TryGetComponent(out PlayerHealth health))
            {
                _damageTickTimer = 0f;
            }
        }
    }
}