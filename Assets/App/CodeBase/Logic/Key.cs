using CodeBase.Services.PersistentProgress;
using CodeBase.Logic.Player;
using DG.Tweening;
using UnityEngine;
using Zenject;

namespace CodeBase.Logic
{
    public class Key : MonoBehaviour
    {
        [SerializeField] private float _floatDistance = 0.5f;
        [SerializeField] private float _floatDuration = 2f;
        [SerializeField] private float _rotationDuration = 4f;
        [SerializeField] private AudioClip _collectEffect;

        private IPersistentProgressService _progressService;

        [Inject]
        private void Construct(IPersistentProgressService progressService)
        {
            _progressService = progressService;
        }

        private void Start()
        {
            StartFloatingAnimation();
            StartRotatingAnimation();
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.TryGetComponent(out PlayerMover player))
            {
                _progressService.Progress.CollectKey();
                AudioSource.PlayClipAtPoint(_collectEffect, player.transform.position);
                Destroy(gameObject);
            }
        }

        private void StartFloatingAnimation()
        {
            transform.DOMoveY(transform.position.y + _floatDistance, _floatDuration)
                .SetLoops(-1, LoopType.Yoyo)
                .SetEase(Ease.InOutSine)
                .SetLink(gameObject);
        }

        private void StartRotatingAnimation()
        {
            transform.DORotate(new Vector3(0, 360, 0), _rotationDuration, RotateMode.FastBeyond360)
                .SetLoops(-1, LoopType.Restart)
                .SetEase(Ease.Linear)
                .SetLink(gameObject);
        }
    }
}