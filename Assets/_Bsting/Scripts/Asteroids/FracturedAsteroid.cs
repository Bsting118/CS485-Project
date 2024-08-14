using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FracturedAsteroid : MonoBehaviour
{
    [SerializeField][Range(1f, 5f)] private float _duration = 2f;

    private void OnEnable()
    {
        Debug.Log("==== Fractured Asteroid Enabled! ====");
        Destroy(gameObject, _duration);
    }
}
