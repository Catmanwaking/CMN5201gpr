using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem.EnhancedTouch;
using Touch = UnityEngine.InputSystem.EnhancedTouch.Touch;

public class SwipeControlManager : MonoBehaviour
{
    [SerializeField] private float minDist = 50.0f;
    [SerializeField, Range(0.0f, 1.5f)] private float maxTime = 1.0f;
    [SerializeField, Range(0.71f, 1.0f)] private float directionThreshhold = 0.9f; // 1/sqrt(2) ~ 0.707 => ~45°

    public UnityEvent<SwipeDirection> OnSwipeInput;
    public UnityEvent<Vector2> OnDragInput;

    private float minSqrDist;

    private void Awake()
    {
        EnhancedTouchSupport.Enable();
        minSqrDist = minDist * minDist;
    }

    private void Update()
    {
        DetectSwipe();
    }

    private void DetectSwipe()
    {
        if (Touch.activeTouches.Count != 1)
            return;

        Touch activeTouch = Touch.activeFingers[0].currentTouch;
        if (activeTouch.phase == UnityEngine.InputSystem.TouchPhase.Moved)
            OnDragInput.Invoke(activeTouch.delta);

        if(activeTouch.phase == UnityEngine.InputSystem.TouchPhase.Ended)
        {
            Vector2 swipe = activeTouch.screenPosition - activeTouch.startScreenPosition;
            double time = activeTouch.time - activeTouch.startTime;
            if (swipe.magnitude >= minSqrDist && time <= maxTime)
                DetermineSwipeDirection(swipe);
        }
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

        OnSwipeInput.Invoke(swipeDir);
    }
}