using DG.Tweening;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Overlay))]
public class TutorialWindow : MonoBehaviour
{
    [SerializeField] private float _slideDuration = 0.35f;
    [SerializeField] private Ease _slideEase = Ease.OutQuint;
    private Overlay _overlay;
    private RectTransform _rectTransform;
    private ContinueMarker _continueMarker;

    private void Awake()
    {
        _overlay = GetComponent<Overlay>();
        _rectTransform = transform.GetChild(0) as RectTransform;
        _continueMarker = GetComponentInChildren<ContinueMarker>(true);
        gameObject.SetActive(false);
    }

    public Coroutine Show()
    {
        gameObject.SetActive(true);
        return CoroutineRunner.Play(this, GetDisplaySequence());
    }

    private IEnumerator GetDisplaySequence()
    {
        _rectTransform.anchoredPosition = new Vector2(0, Screen.height);

        yield return _overlay.FadeIn();

        yield return _rectTransform.DOAnchorPosY(0, _slideDuration)
            .SetEase(_slideEase)
            .WaitForCompletion();

        yield return _continueMarker.Wait();
        
        yield return _rectTransform.DOAnchorPosY(Screen.height, _slideDuration)
            .SetEase(_slideEase)
            .WaitForCompletion();

        yield return _overlay.FadeOut();

        gameObject.SetActive(false);
    }
}