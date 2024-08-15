using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FracturedAsteroid : MonoBehaviour
{
    [SerializeField][Range(1f, 60f)] private float _duration = 2f;

    private void OnEnable()
    {
        Destroy(gameObject, _duration);
    }
}
