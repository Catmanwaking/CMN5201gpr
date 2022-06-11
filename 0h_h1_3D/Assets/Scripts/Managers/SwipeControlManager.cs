using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem.EnhancedTouch;
using Touch = UnityEngine.InputSystem.EnhancedTouch.Touch;
using TouchPhase = UnityEngine.InputSystem.TouchPhase;

public class SwipeControlManager : MonoBehaviour
{
    [SerializeField] private float minSwipeDist = 50.0f;
    [SerializeField, Range(0.0f, 1.5f)] private float maxSwipeTime = 1.0f;
    [SerializeField, Range(0.0f, 0.3f)] private float maxTapTime = 0.2f;
    [SerializeField, Range(0.71f, 1.0f)] private float directionThreshhold = 0.9f; // 1/sqrt(2) ~ 0.707 => ~45°

    public UnityEvent<SwipeDirection> OnSwipeInput;
    public UnityEvent<Vector2> OnDragInput;
    public UnityEvent<Vector2> OnTapInput;
    public UnityEvent<Vector2> OnTouchBegin;
    public UnityEvent<Vector2> OnTouchEnd;

    private float minSwipeSqrDist;

    private void Awake()
    {
        EnhancedTouchSupport.Enable();
        minSwipeSqrDist = minSwipeDist * minSwipeDist;
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
                OnTouchBegin?.Invoke(activeTouch.screenPosition);
                break;
            case TouchPhase.Moved:
                OnDragInput?.Invoke(activeTouch.delta);
                break;
            case TouchPhase.Ended:
                OnTouchEnd?.Invoke(activeTouch.screenPosition);
                DetectAction(activeTouch);
                break;
        }            
    }

    private void DetectAction(Touch activeTouch)
    {
        Vector2 swipe = activeTouch.screenPosition - activeTouch.startScreenPosition;
        double time = activeTouch.time - activeTouch.startTime;
        if (swipe.sqrMagnitude >= minSwipeSqrDist && time <= maxSwipeTime)
            DetermineSwipeDirection(swipe);
        if(swipe.sqrMagnitude < minSwipeSqrDist && time <maxTapTime)
            OnTapInput?.Invoke(activeTouch.startScreenPosition);
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