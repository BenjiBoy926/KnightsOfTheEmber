using DG.Tweening;
using UnityEngine;

[RequireComponent(typeof(BattleCamera))]
public class BattleCameraZoom : MonoBehaviour
{
    private float CurrentZoom => OrthoSizeToZoom(_camera.OrthographicSize);

    [SerializeField] private Vector2 _viewSizeRange = new(5, 15);
    [SerializeField] private float _zoomJump = 0.3f;
    [SerializeField] private float _zoomJumpDuration = 0.35f;
    [SerializeField] private Ease _zoomJumpEase = Ease.OutQuint;
    private BattleCamera _camera;
    private Tween _zoomJumpTween;

    private void Awake()
    {
        _camera = GetComponent<BattleCamera>();
    }

    public void ChangeZoom(float zoomDelta)
    {
        SetZoom(CurrentZoom + zoomDelta);
    }

    public void SetZoom(float zoom)
    {
        _camera.OrthographicSize = ZoomToOrthoSize(zoom);
    }

    public void ZoomIn()
    {
        ZoomJump(+_zoomJump);
    }

    public void ZoomOut()
    {
        ZoomJump(-_zoomJump);
    }

    private void ZoomJump(float jumpAmount)
    {
        float newZoom = CurrentZoom + jumpAmount;
        float newOrthoSize = ZoomToOrthoSize(newZoom);
        OrthoSizeJump(newOrthoSize);
    }

    private void OrthoSizeJump(float newOrthoSize)
    {
        if (!ShouldDoOrthoSizeJump(newOrthoSize))
        {
            return;
        }
        _camera.DOKill();
        _zoomJumpTween = _camera.Camera.DOOrthoSize(newOrthoSize, _zoomJumpDuration).SetEase(_zoomJumpEase);
    }

    private bool ShouldDoOrthoSizeJump(float newOrthoSize)
    {
        return !Mathf.Approximately(_camera.OrthographicSize, newOrthoSize) && !IsOrthoSizeTweening();
    }

    private bool IsOrthoSizeTweening()
    {
        return _zoomJumpTween.IsActive();
    }

    private float OrthoSizeToZoom(float orthoSize)
    {
        float invZoom = Mathf.InverseLerp(_viewSizeRange.x, _viewSizeRange.y, orthoSize);
        return 1 - invZoom;
    }

    private float ZoomToOrthoSize(float zoom)
    {
        float t = 1 - zoom;
        return Mathf.Lerp(_viewSizeRange.x, _viewSizeRange.y, t);
    }
}