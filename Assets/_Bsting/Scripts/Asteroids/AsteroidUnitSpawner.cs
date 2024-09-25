using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;


public class AsteroidUnitSpawner : MonoBehaviour
{
    // Public properties:
    [field: SerializeField] public Transform TargetSource { get; private set; } = null;
    [field: SerializeField] public List<GameObject> ListOfPossiblePrefabsToSpawn { get; private set; } = null;
    [field: SerializeField] public ShipUIController ScoreController { get; private set; } = null; 
    [field: SerializeField] public float AngleSpawnCone { get; private set; }
    [field: SerializeField] public float RadiusToSpawnAwayFrom { get; private set; } 

    // Inspector-exposed fields:
    [SerializeField] private int _limitOfAsteroidsToSpawnInScene = 8;
    [SerializeField] private float _lifetimeForEachAsteroid = 25.0f;
    [SerializeField] private float _cooldownTimeBetweenEachSpawn = 1.5f;
    [SerializeField] private float _hurdlingSpeedForAsteroids = 1.0f;

    // Private var's:
    private int _countOfAsteroidsInScene = 0;
    private AsteroidUnitSpawner _instance = null;
    private bool _readyToSpawnNextAsteroid = false;
    private Coroutine _spawnCooldownRoutine = null;

    #region Monobehaviors
    void Awake()
    {
        LoadSpawnerInstance();
    }

    void OnEnable()
    {
        LoadSpawnerInstance();
    }

    void OnDisable()
    {
        if (_spawnCooldownRoutine != null)
        {
            InterruptSpawnerCooldownRoutine();
        }
    }

    void OnDestroy()
    {
        if (_spawnCooldownRoutine != null)
        {
            InterruptSpawnerCooldownRoutine();
        }
    }

    void FixedUpdate()
    {
        TryToSpawnNextAsteroid(ListOfPossiblePrefabsToSpawn, TargetSource, AngleSpawnCone, RadiusToSpawnAwayFrom);
    }
    #endregion

    /// <summary>
    ///  Takes a Quaternion to define the direction of the cone (spot) 
    ///  in the same way as the spotlight of unity. 
    ///  The cone always points the the local z-axis.
    /// </summary>
    /// <param name="targetDirection"></param>
    /// <param name="angle"></param>
    /// <returns>Vector3</returns>
    public Vector3 GetPointOnUnitSphereCap(Quaternion targetDirection, float angle)
    {
        float angleInRad = Random.Range(0.0f, angle) * Mathf.Deg2Rad;
        Vector2 pointOnCircle = (Random.insideUnitCircle.normalized) * Mathf.Sin(angleInRad);
        Vector3 crossSectionVector = new Vector3(pointOnCircle.x, pointOnCircle.y, Mathf.Cos(angleInRad));
        return targetDirection * crossSectionVector;
    }

    /// <summary>
    /// Creates a rotation from the forward and upward
    /// vectors found in the passed in Vector3 target 
    /// direction.
    /// </summary>
    /// <param name="targetDirection"></param>
    /// <param name="angle"></param>
    /// <returns>Vector3</returns>
    public Vector3 GetPointOnUnitSphereCap(Vector3 targetDirection, float angle)
    {
        return GetPointOnUnitSphereCap(Quaternion.LookRotation(targetDirection), angle);
    }

    /// <summary>
    /// Helper function to allow other classes to access this script 
    /// instance and decrement its counter.
    /// </summary>
    public void ReportAsteroidDestroyed(bool fromPlayerDamage=false)
    {
        if (_countOfAsteroidsInScene > 0)
        {
            _countOfAsteroidsInScene--;
        }

        if (fromPlayerDamage)
        {
            // Do something with score here:
            if (ScoreController != null)
            {
                ScoreController.UptickPlayerScore();
            }
        }
    }

    #region Coroutine(s)
    IEnumerator SpawnCooldown()
    {
        // Anything after the WaitForSeconds will be held in pause
        // (will not run until its timer expires):
        yield return new WaitForSeconds(_cooldownTimeBetweenEachSpawn);

        // Have flag variable after timer to use as cooldown enforcer/lock
        _readyToSpawnNextAsteroid = true;
    }
    #endregion

    #region Coroutine Helper(s)
    public void InterruptSpawnerCooldownRoutine()
    {
        StopCoroutine(_spawnCooldownRoutine);
        _spawnCooldownRoutine = null;
        _readyToSpawnNextAsteroid = true; // Default flag value
    }
    #endregion

    /// <summary>
    /// Helper function that spawns in an Asteroid from a prefab. 
    /// Its spawn position is based off of Steradian computations. 
    /// (Not a literal Steradian, but applies theory to its randomness.)
    /// </summary>
    /// <param name="fromThisObject">The origin of the spawning sphere and cone.</param>
    /// <param name="aboutThisAngle">How wide the cone should be to the sphere (360 = to the full sphere).</param>
    /// <param name="usingRadius">How far should the Asteroid spawn from the origin.</param>
    /// <param name="chosenPrefab">The prefab that represents the Asteroid object to spawn.</param>
    public GameObject SpawnAsteroid(Transform fromThisObject, float aboutThisAngle, float usingRadius, GameObject chosenPrefab)
    {
        Vector3 finalVectorOffset = GetPointOnUnitSphereCap(fromThisObject.rotation, aboutThisAngle) * usingRadius;
        finalVectorOffset = fromThisObject.position + finalVectorOffset;

        // Spawn:
        GameObject spawnedAsteroid = Instantiate(chosenPrefab, finalVectorOffset, Quaternion.identity);

        // Update Asteroid counter:
        _countOfAsteroidsInScene++;

        // Return a reference to what we instantiated in our scene:
        return spawnedAsteroid;
    }

    /// <summary>
    /// Simple accessor to access the centralized instance of this spawner script
    /// </summary>
    /// <returns>AsteroidUnitSpawner</returns>
    public AsteroidUnitSpawner GetSpawnerInstance()
    {
        return _instance;
    }

    /// <summary>
    /// Helper function to create a global point of access to this spawner:
    /// </summary>
    private void LoadSpawnerInstance()
    {
        if (_instance == null || _instance != this)
        {
            _instance = this;
        }

        _readyToSpawnNextAsteroid = true;
    }

    private void TryToSpawnNextAsteroid(List<GameObject> givenListOfAsteroidPrefabs, 
                                           Transform fromThisOrigin, 
                                           float angleOfSpawnCone, 
                                           float distanceFromOrigin)
    {
        if (_countOfAsteroidsInScene < _limitOfAsteroidsToSpawnInScene)
        {
            // We are able to fit another into the scene:

            if (givenListOfAsteroidPrefabs != null)
            {
                if (givenListOfAsteroidPrefabs.Count > 0 && _readyToSpawnNextAsteroid)
                {
                    // There ARE prefabs we can choose from:

                    // Get the chosen prefab via random selection:
                    int chosenPrefabIndex = Random.Range((int)0, (int)(givenListOfAsteroidPrefabs.Count - 1));
                    GameObject chosenPrefab = givenListOfAsteroidPrefabs[chosenPrefabIndex];

                    // Spawn the chosen prefab and report added Asteroid:
                    GameObject spawnedAsteroid = SpawnAsteroid(fromThisOrigin, angleOfSpawnCone, distanceFromOrigin, chosenPrefab);

                    // Setup spawn settings on spawn:
                    spawnedAsteroid.GetComponent<Asteroid>()._parentSpawner = _instance;
                    spawnedAsteroid.GetComponent<Asteroid>().LifetimeOfAsteroid = _lifetimeForEachAsteroid;

                    // Look at the origin point to aim slingshot vector at it:
                    spawnedAsteroid.transform.LookAt(fromThisOrigin);

                    // Setup throw settings!
                    spawnedAsteroid.GetComponent<Asteroid>().TargetToHurdleTowards = fromThisOrigin.position;
                    spawnedAsteroid.GetComponent<Asteroid>().HasTargetBeenSet = true;
                    spawnedAsteroid.GetComponent<Asteroid>().SpeedToApplyToHurdlingAsteroid = _hurdlingSpeedForAsteroids;

                    _readyToSpawnNextAsteroid = false;
                    _spawnCooldownRoutine = StartCoroutine(SpawnCooldown());
                }
            }
        }
    }
}
