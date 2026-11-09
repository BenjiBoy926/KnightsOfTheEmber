using DG.Tweening;
using UnityEngine;

[RequireComponent(typeof(Camera))]
[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(BattleCameraGrab))]
[RequireComponent(typeof(BattleCameraFollow))]
[RequireComponent(typeof(BattleCameraGlide))]
[RequireComponent(typeof(BattleCameraZoom))]
public class BattleCamera : MonoBehaviour
{
    public bool IsGliding => _glide.IsActive;
    public bool IsGrabbed => _grab.IsActive;
    private float WorldHeight => _camera.orthographicSize * 2;
    private float WorldWidth => WorldHeight * _camera.aspect;
    public Vector2 WorldSize => new(WorldWidth, WorldHeight);
    private Vector2 WorldExtents => WorldSize / 2f;
    public Camera Camera => _camera;
    public float OrthographicSize
    {
        get => _camera.orthographicSize;
        set => _camera.orthographicSize = value;
    }
    public Vector2 Position
    {
        get => transform.position;
        set => transform.position = new(value.x, value.y, transform.position.z);
    }
    public Vector2 Velocity
    {
        get => _rigidbody.velocity;
        set => _rigidbody.velocity = value;
    }

    [SerializeField] private float _viewMargin = 1;

    private Camera _camera;
    private Rigidbody2D _rigidbody;
    private BattleCameraGrab _grab;
    private BattleCameraFollow _follow;
    private BattleCameraGlide _glide;
    private BattleCameraZoom _zoom;

    private void Awake()
    {
        _camera = GetComponent<Camera>();
        _rigidbody = GetComponent<Rigidbody2D>();
        _grab = GetComponent<BattleCameraGrab>();
        _follow = GetComponent<BattleCameraFollow>();
        _glide = GetComponent<BattleCameraGlide>();
        _zoom = GetComponent<BattleCameraZoom>();
    }

    // Grab

    public void Grab(Vector2 worldPosition)
    {
        _grab.Begin(worldPosition);
    }

    public void UpdateGrab(Vector2 screenPosition)
    {
        _grab.UpdateGrab(screenPosition);
    }

    public void Release()
    {
        _grab.End();
    }

    // Follow
    
    public Coroutine Follow(Transform target)
    {
        return _follow.Begin(target);
    }

    public void Unfollow()
    {
        _follow.End();
    }

    // Glide

    public Coroutine Glide(Transform transform)
    {
        return _glide.Begin(transform);
    }

    public Coroutine Glide(Vector3 position)
    {
        return _glide.Begin(position);
    }

    public void EndGlide()
    {
        _glide.End();
    }

    // Zoom

    public void ChangeZoom(float zoomDelta)
    {
        _zoom.ChangeZoom(zoomDelta);
    }

    public void SetZoom(float zoom)
    {
        _zoom.SetZoom(zoom);
    }

    public void ZoomIn()
    {
        _zoom.ZoomIn();
    }

    public void ZoomOut()
    {
        _zoom.ZoomOut();
    }

    // Self

    public void Shove(Vector2 point)
    {
        Rect worldRect = GetWorldRect();
        Vector2 direction = worldRect.Contains(point) ? Vector2.zero : (point - worldRect.center).normalized;
        Velocity = direction * 5f;
    }

    public Vector3 GetFramingPosition(Transform target)
    {
        return GetFramingPosition(target.position);
    }

    public Vector3 GetFramingPosition(Vector3 target)
    {
        Vector2 targetPosition = target;
        float z = transform.position.z;
        return new(targetPosition.x, targetPosition.y, z);
    }

    public Vector2 ScreenToWorld(Vector2 screen)
    {
        return _camera.ScreenToWorldPoint(screen);
    }

    public Vector2 WorldToScreen(Vector2 world)
    {
        return _camera.WorldToScreenPoint(world);
    }

    public void IncludeInView(Vector2 position)
    {
        Rect rect = GetWorldRect();
        Vector2 offset = OffsetOutsideEdge(rect, position);
        Position += offset;
    }

    private Rect GetWorldRect()
    {
        Vector2 marginVector = new(_viewMargin, _viewMargin);
        Vector2 extents = WorldExtents - (marginVector * 2);
        Vector2 center = Position;
        Vector2 min = center - extents;
        Vector2 max = center + extents;
        return Rect.MinMaxRect(min.x, min.y, max.x, max.y);
    }

    private static Vector2 OffsetOutsideEdge(Rect rect, Vector2 point)
    {
        float xOffset = OffsetOutsideRange(point.x, rect.xMin, rect.xMax);
        float yOffset = OffsetOutsideRange(point.y, rect.yMin, rect.yMax);
        return new(xOffset, yOffset);
    }

    private static float OffsetOutsideRange(float value, float min, float max)
    {
        if (value < min)
        {
            return value - min;
        }
        else if (value > max)
        {
            return value - max;
        }
        return 0;
    }
}