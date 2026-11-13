using DG.Tweening;
using System.Collections;
using UnityEngine;

[RequireComponent(typeof(RectTransform))]
public class TutorialWindow : MonoBehaviour
{
    [SerializeField] private float _slideDuration = 0.35f;
    private RectTransform _rectTransform;

    private void Awake()
    {
        _rectTransform = GetComponent<RectTransform>();
        _rectTransform.anchoredPosition = new Vector2(0, Screen.height);
    }

    public Coroutine Show()
    {
        return StartCoroutine(GetDisplaySequence());
    }

    private IEnumerator GetDisplaySequence()
    {
        yield return _rectTransform.DOAnchorPosY(0, _slideDuration).WaitForCompletion();
        yield return new WaitForSeconds(2);
        yield return _rectTransform.DOAnchorPosY(Screen.height, _slideDuration).WaitForCompletion();
    }
}