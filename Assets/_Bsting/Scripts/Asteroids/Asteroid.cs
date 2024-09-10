using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Asteroid : MonoBehaviour, IDamageable
{
    [field: SerializeField] public Vector3 TargetToHurdleTowards { get; set; }
    [field: SerializeField] public bool HasTargetBeenSet { get; set; } = false;
    [field: SerializeField] public float SpeedToApplyToHurdlingAsteroid { get; set; } = 1.0f;
    [SerializeField] private FracturedAsteroid _fracAsteroidPrefab;
    [SerializeField] private Detonator _explosionPrefab;

    private Transform _transform;
    private bool _hasStartedToHurdle = false;

    void Awake()
    {
        _transform = transform;
    }

    void FixedUpdate()
    {
        //if (TargetToHurdleTowards != null)
        if (HasTargetBeenSet)
        {
            ThrowAsteroidAtTarget(TargetToHurdleTowards, atSpeed:SpeedToApplyToHurdlingAsteroid);
        }
    }

    #region Damage Asteroid Methods
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
    #endregion

    // ADDED:
    private void ThrowAsteroidAtTarget(Transform target, float atSpeed = 1.0f, float throwFactorToTarget = 2f)
    {
        // (this.gameObject = asteroid object)

        // Assuming that asteroid object has a RigidBody for applied physics:
        Rigidbody asteroidRB = this.gameObject.GetComponent<Rigidbody>();
        //asteroidRB.interpolation = RigidbodyInterpolation.Interpolate;

        /* --- Setting up Vector line to find the endpoint where the Asteroid should be moved/shot to --- */
        // Intersect and extend past the target
        Debug.Log("Target position: " + target.position);
        Debug.Log("Asteroid position: " + _transform.position);
        Vector3 headingOfAsteroid = (target.position - _transform.position) * throwFactorToTarget;
        Debug.Log("Heading of asteroid: " + headingOfAsteroid);

        // Debug.Log("Found heading vector: " + headingOfAsteroid);

        Vector3 endingSpot = _transform.position + headingOfAsteroid;

        // Debug.Log("Found ending spot position: " + endingSpot);

        //asteroidRB.MovePosition(_transform.position + endingSpot * Time.deltaTime * atSpeed);
        // Using AddForce() instead of  MovePosition() now:
        asteroidRB.AddForce(headingOfAsteroid);

    }

    private void ThrowAsteroidAtTarget(Vector3 target, float atSpeed = 1.0f, float throwFactorToTarget = 2f)
    {
        // (this.gameObject = asteroid object)

        // Assuming that asteroid object has a RigidBody for applied physics:
        Rigidbody asteroidRB = this.gameObject.GetComponent<Rigidbody>();
        //asteroidRB.interpolation = RigidbodyInterpolation.Interpolate;

        /* --- Setting up Vector line to find the endpoint where the Asteroid should be moved/shot to --- */
        // Intersect and extend past the target
        Vector3 headingOfAsteroid = (target - _transform.position) * throwFactorToTarget;

        // Debug.Log("Found heading vector: " + headingOfAsteroid);

        Vector3 endingSpot = _transform.position + headingOfAsteroid;

        // Debug.Log("Found ending spot position: " + endingSpot);

        //asteroidRB.MovePosition(_transform.position + endingSpot * Time.deltaTime * atSpeed);
        // Using AddForce() instead of  MovePosition() now:
        if (!_hasStartedToHurdle)
        {
            asteroidRB.AddForce(headingOfAsteroid * atSpeed);
            _hasStartedToHurdle = true;
        }

        Debug.Log("Velocity of asteroid: " + asteroidRB.velocity);

    }
}
