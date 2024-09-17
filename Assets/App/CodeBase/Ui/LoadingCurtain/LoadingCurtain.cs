using UnityEngine;
using DG.Tweening;

namespace CodeBase.UI.Curtain
{
    public class LoadingCurtain : MonoBehaviour, ILoadingCurtain
    {
        [SerializeField] private float _fadeDuration = 1f;
        [SerializeField] private CanvasGroup _curtain;

        private void Awake()
        {
            DontDestroyOnLoad(this);
        }

        public void Show()
        {
            gameObject.SetActive(true);
            _curtain.alpha = 1;
        }

        public void Hide() => FadeOut();

        private void FadeOut()
        {
            _curtain.DOFade(0, _fadeDuration).OnComplete(() => gameObject.SetActive(false));
        }
    }
}
