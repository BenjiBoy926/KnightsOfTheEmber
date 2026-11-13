using DG.Tweening;
using System.Collections;
using UnityEngine;

[RequireComponent(typeof(RectTransform))]
public class TutorialWindow : MonoBehaviour
{
    [SerializeField] private float _slideDuration = 0.35f;
    [SerializeField] private Ease _slideEase = Ease.OutQuint;
    private RectTransform _rectTransform;

    private void Awake()
    {
        _rectTransform = GetComponent<RectTransform>();
        _rectTransform.anchoredPosition = new Vector2(0, Screen.height);
        gameObject.SetActive(false);
    }

    public Coroutine Show()
    {
        gameObject.SetActive(true);
        return StartCoroutine(GetDisplaySequence());
    }

    private IEnumerator GetDisplaySequence()
    {
        yield return _rectTransform.DOAnchorPosY(0, _slideDuration)
            .SetEase(_slideEase)
            .WaitForCompletion();
        yield return new WaitForSeconds(2);
        yield return _rectTransform.DOAnchorPosY(Screen.height, _slideDuration)
            .SetEase(_slideEase)
            .WaitForCompletion();
        gameObject.SetActive(false);
    }
}