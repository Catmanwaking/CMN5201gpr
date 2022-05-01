//Author: Dominik Dohmeier
using UnityEngine;

public class CubeRotator : MonoBehaviour
{
    public void RotateCube(Vector2 input)
    {
        float rotationStrength = input.magnitude;
        Vector2 perp = new Vector2(input.y, -input.x).normalized;
        transform.Rotate(perp, perp.magnitude, Space.World);
    }
}