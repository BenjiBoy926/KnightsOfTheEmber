using DG.Tweening;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Overlay))]
public class TutorialWindow : MonoBehaviour
{
    [SerializeField] private float _slideDuration = 0.35f;
    [SerializeField] private Ease _slideEase = Ease.OutQuint;
    [SerializeField] private InputActionReference _confirmAction;
    private Overlay _overlay;
    private RectTransform _rectTransform;
    private bool _confirmActionPressed = false;

    private void Awake()
    {
        _overlay = GetComponent<Overlay>();
        _rectTransform = transform.GetChild(0) as RectTransform;
        gameObject.SetActive(false);
    }

    private void OnEnable()
    {
        _confirmAction.action.performed += OnConfirm;
    }

    private void OnDisable()
    {
        _confirmAction.action.performed -= OnConfirm;
    }

    private void OnConfirm(InputAction.CallbackContext obj)
    {
        _confirmActionPressed = true;
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

        _confirmActionPressed = false;
        yield return new WaitUntil(() => _confirmActionPressed);
        
        yield return _rectTransform.DOAnchorPosY(Screen.height, _slideDuration)
            .SetEase(_slideEase)
            .WaitForCompletion();

        yield return _overlay.FadeOut();

        gameObject.SetActive(false);
    }
}