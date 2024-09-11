using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Asteroid : MonoBehaviour, IDamageable
{
    // Public properties:
    [field: SerializeField, HideInInspector] public AsteroidUnitSpawner _parentSpawner { get; set; } = null;
    [field: SerializeField] public Vector3 TargetToHurdleTowards { get; set; }
    [field: SerializeField] public bool HasTargetBeenSet { get; set; } = false;
    [field: SerializeField] public float SpeedToApplyToHurdlingAsteroid { get; set; } = 1.0f;
    [field: SerializeField] public float LifetimeOfAsteroid { get; set; } = 20.0f;

    // Inspector-exposed fields:
    [SerializeField] private FracturedAsteroid _fracAsteroidPrefab;
    [SerializeField] private Detonator _explosionPrefab;

    // Private var's:
    private Transform _transform;
    private Rigidbody _rigidBody;
    private Coroutine _lifeTimerRoutine = null;
    private Vector3 _eulerAngleRotation = new Vector3(45, 0, 0); // Rotating the X axis, 45 deg/sec
    private bool _hasStartedToHurdle = false;
    private bool _hasStartedLifetime = false;

    #region Monobehaviors
    void Awake()
    {
        InitAsteroidComponents();
    }

    void OnEnable()
    {
        InitAsteroidComponents();
        _lifeTimerRoutine = StartCoroutine(UpdateLifetimeOfAsteroid());
    }

    void OnDisable()
    {
        if (_lifeTimerRoutine != null)
        {
            InterruptAsteroidLifetimeRoutine();
        }
    }

    void FixedUpdate()
    {
        if (HasTargetBeenSet && !_hasStartedToHurdle)
        {
            ThrowAsteroidAtTarget(TargetToHurdleTowards, atSpeed:SpeedToApplyToHurdlingAsteroid);
        }

        MakeAsteroidSpin();
    }
    #endregion

    IEnumerator UpdateLifetimeOfAsteroid()
    {
        if (!_hasStartedLifetime)
        {
            float lifeTimer = LifetimeOfAsteroid;
            _hasStartedLifetime = true;

            // Wait X seconds where X is the lifetime:
            yield return new WaitForSeconds(lifeTimer);

            // Once timer is up, this is executed:
            DespawnTheAsteroid();
        }
    }

    #region Coroutine Helper(s)
    public void InterruptAsteroidLifetimeRoutine()
    {
        StopCoroutine(_lifeTimerRoutine);
        _lifeTimerRoutine = null;
        _hasStartedLifetime = false;
    }
    #endregion

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
        Destroy(this.gameObject);
        _parentSpawner.ReportAsteroidDestroyed();
    }

    private void DespawnTheAsteroid()
    {
        Destroy(this.gameObject);
        // We still want to report it to get a new one:
        _parentSpawner.ReportAsteroidDestroyed();
    }
    #endregion

    #region Throw Asteroid Methods
    private void ThrowAsteroidAtTarget(Transform target, float atSpeed = 1.0f, float throwFactorToTarget = 2f)
    {
        // (this.gameObject = asteroid object)
        // (_rigidBody = this asteroid's RB)

        /* --- Setting up Vector line to find the endpoint where the Asteroid should be moved/shot to --- */
        // Intersect and extend past the target
        Vector3 headingOfAsteroid = (target.position - _transform.position) * throwFactorToTarget;

        // Using AddForce() instead of MovePosition() now:
        if (!_hasStartedToHurdle)
        {
            _rigidBody.AddForce(headingOfAsteroid * atSpeed);
            _hasStartedToHurdle = true;
        }

    }

    private void ThrowAsteroidAtTarget(Vector3 target, float atSpeed = 1.0f, float throwFactorToTarget = 2f)
    {
        // (this.gameObject = asteroid object)
        // (_rigidBody = this asteroid's RB)

        /* --- Setting up Vector line to find the endpoint where the Asteroid should be moved/shot to --- */
        // Intersect and extend past the target
        Vector3 headingOfAsteroid = (target - _transform.position) * throwFactorToTarget;

        // Using AddForce() instead of MovePosition() now:
        if (!_hasStartedToHurdle)
        {
            _rigidBody.AddForce(headingOfAsteroid * atSpeed);
            _hasStartedToHurdle = true;
        }

    }
    #endregion

    #region Rotate Asteroid Method(s)
    private void MakeAsteroidSpin()
    {
        if (_hasStartedToHurdle && _rigidBody != null)
        {
            Quaternion deltaRotation = Quaternion.Euler(_eulerAngleRotation * Time.fixedDeltaTime);
            _rigidBody.MoveRotation(_rigidBody.rotation * deltaRotation);
        }
    }
    #endregion

    #region Helper Method(s)
    private void InitAsteroidComponents()
    {
        if (_transform == null || _transform != transform)
        {
            _transform = transform;
        }

        if (_rigidBody == null)
        {
            _rigidBody = this.gameObject.GetComponent<Rigidbody>();
        }
    }
    #endregion
}
