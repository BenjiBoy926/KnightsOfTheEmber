using DG.Tweening;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

[RequireComponent(typeof(Graphic))]
public class ContinueMarker : MonoBehaviour
{
    [SerializeField] private InputActionReference _continueAction;
    [SerializeField] private float _fadeDuration = 0.5f;
    private Graphic _graphic;
    private bool _continue = false;

    private void Awake()
    {
        _graphic = GetComponent<Graphic>();
        SetAlpha(0);
    }

    private void OnEnable()
    {
        _continueAction.action.performed += OnContinue;
    }

    private void OnDisable()
    {
        _continueAction.action.performed -= OnContinue;
    }

    private void OnContinue(InputAction.CallbackContext obj)
    {
        _continue = true;
    }

    public Coroutine Wait()
    {
        CoroutineRunner.StopAll(this);
        return CoroutineRunner.Play(this, WaitSequence());
    }

    private IEnumerator WaitSequence()
    {
        SetAlpha(0);
        _graphic.DOFade(1, _fadeDuration).SetLoops(-1, LoopType.Yoyo);

        _continue = false;
        while (!_continue)
        {
            yield return null;
        }

        _graphic.DOKill();
        SetAlpha(0);
    }

    private void SetAlpha(float alpha)
    {
       _graphic.color = _graphic.color.SetAlpha(alpha);
    }
}