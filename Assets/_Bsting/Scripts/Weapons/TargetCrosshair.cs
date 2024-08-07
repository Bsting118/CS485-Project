using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetCrosshair : MonoBehaviour
{
    [SerializeField] private float _distanceFromOrigin = 50.0f;

    private Ray _targetRay;

    // Update is called once per frame
    void Update()
    {
        _targetRay = new Ray(transform.position, transform.forward);
    }

    public Vector3 GetCrosshairPointToAimAt()
    {
        return _targetRay.GetPoint(_distanceFromOrigin);
    }

    void OnDrawGizmos()
    {
        Ray simulatedRay;
        if (Application.isPlaying)
        {
            simulatedRay = _targetRay;
        }
        else
        {
            simulatedRay = new Ray(transform.position, transform.forward);
        }

        // Draw a yellow line representing the ray
        Gizmos.color = Color.cyan;
        Gizmos.DrawLine(simulatedRay.origin, simulatedRay.GetPoint(_distanceFromOrigin));

        // Optionally, draw a sphere at the end of the ray for better visualization
        Gizmos.DrawSphere(simulatedRay.GetPoint(_distanceFromOrigin), 0.25f);
    }
}
