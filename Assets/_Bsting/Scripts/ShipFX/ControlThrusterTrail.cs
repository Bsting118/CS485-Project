using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControlThrusterTrail : MonoBehaviour
{
    public TrailRenderer trailRenderer;
    public GameObject shipToGrabDirectionFrom;
    public Vector3 direction = Vector3.forward; // Set the desired direction
    private Vector3 lastPosition;
    public float threshold = 0.1f; // Threshold to determine if movement is in the desired direction

    void Start()
    {
        if (trailRenderer == null)
        {
            trailRenderer = GetComponent<TrailRenderer>();
        }

        if (shipToGrabDirectionFrom != null)
        {
            direction = shipToGrabDirectionFrom.transform.forward;
        }
        lastPosition = transform.position;
    }

    void Update()
    {
        // Use the dot product to determine if the movement is in the desired direction
        // If product is above a certain threshold, it is considered to be in the desired direction
        Vector3 movement = transform.position - lastPosition;
        float dotProduct = Vector3.Dot(movement.normalized, direction.normalized);

        if (dotProduct > threshold)
        {
            if (!trailRenderer.emitting)
            {
                trailRenderer.emitting = true;
            }
        }
        else
        {
            if (trailRenderer.emitting)
            {
                trailRenderer.emitting = false;
            }
        }

        lastPosition = transform.position;
    }
}

