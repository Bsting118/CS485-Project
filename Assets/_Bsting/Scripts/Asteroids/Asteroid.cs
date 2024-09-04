using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Asteroid : MonoBehaviour, IDamageable
{
    [SerializeField] private FracturedAsteroid _fracAsteroidPrefab;
    [SerializeField] private Detonator _explosionPrefab;

    private Transform _transform;

    void Awake()
    {
        _transform = transform;
    }

    public void TakeDamage(int damage, Vector3 hitPosition)
    {
        FractureTheAsteroid(hitPosition);
    }

    private void FractureTheAsteroid(Vector3 hitPosition)
    {
        // Instance the fragmented pieces 
        if (_fracAsteroidPrefab != null)
        {
            Instantiate(_fracAsteroidPrefab, _transform.position, _transform.rotation);
        }

        // Instance the "explosion" where we hit the thing
        if (_explosionPrefab != null)
        {
            Instantiate(_explosionPrefab, hitPosition, Quaternion.identity);
        }

        // Destroy and report dstruction of Asteroid to allow a new spawn:
        AsteroidUnitSpawner.Instance.ReportAsteroidDestroyed();
        Destroy(this.gameObject);
    }
}
