using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem.EnhancedTouch;
using Touch = UnityEngine.InputSystem.EnhancedTouch.Touch;
using TouchPhase = UnityEngine.InputSystem.TouchPhase;

public class SwipeControlManager : MonoBehaviour
{
    [SerializeField] private float minDist = 50.0f;
    [SerializeField, Range(0.0f, 1.5f)] private float maxTime = 1.0f;
    [SerializeField, Range(0.71f, 1.0f)] private float directionThreshhold = 0.9f; // 1/sqrt(2) ~ 0.707 => ~45°

    public UnityEvent<SwipeDirection> OnSwipeInput;
    public UnityEvent<Vector2> OnDragInput;
    public UnityEvent<Vector2> OnTouchDown;

    private float minSqrDist;

    private void Awake()
    {
        EnhancedTouchSupport.Enable();
        minSqrDist = minDist * minDist;
    }

    private void Update()
    {
        DetectTouch();
    }

    private void OnDestroy()
    {
        EnhancedTouchSupport.Disable();
    }

    private void DetectTouch()
    {
        if (Touch.activeTouches.Count != 1)
            return;

        Touch activeTouch = Touch.activeTouches[0];

        switch (activeTouch.phase)
        {
            case TouchPhase.Began:
                OnTouchDown?.Invoke(activeTouch.screenPosition);
                break;
            case TouchPhase.Moved:
                OnDragInput?.Invoke(activeTouch.delta);
                break;
            case TouchPhase.Ended:
                DetectSwipe(activeTouch);
                break;
        }            
    }

    private void DetectSwipe(Touch activeTouch)
    {
        Vector2 swipe = activeTouch.screenPosition - activeTouch.startScreenPosition;
        double time = activeTouch.time - activeTouch.startTime;
        if (swipe.sqrMagnitude >= minSqrDist && time <= maxTime)
            DetermineSwipeDirection(swipe);
    }

    private void DetermineSwipeDirection(Vector2 direction)
    {
        direction.Normalize();
        SwipeDirection swipeDir;
        float upDot = Vector2.Dot(Vector2.up, direction);
        float leftDot = Vector2.Dot(Vector2.left, direction);

        if (upDot > directionThreshhold)
            swipeDir = SwipeDirection.Up;
        else if (-upDot > directionThreshhold)
            swipeDir = SwipeDirection.Down;
        else if (leftDot > directionThreshhold)
            swipeDir = SwipeDirection.Left;
        else if (-leftDot > directionThreshhold)
            swipeDir = SwipeDirection.Right;
        else
            return;
        OnSwipeInput?.Invoke(swipeDir);
    }
}

public enum SwipeDirection
{
    Up,
    Down,
    Left,
    Right,
}