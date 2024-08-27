using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AsteroidUnitSpawner : MonoBehaviour
{
    public Transform _targetSource;
    public float _angleSpawnCone;
    public float _radiusToSpawnAwayFrom;
    public GameObject _prefabToSpawn;

    // Start is called before the first frame update
    void Start()
    {
        // ...
    }

    // Update is called once per frame
    void Update()
    {
        // ...
    }

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

    public void SpawnAsteroid(Transform fromThisObject, float aboutThisAngle, float usingRadius, GameObject chosenPrefab)
    {
        Vector3 finalVectorOffset = GetPointOnUnitSphereCap(fromThisObject.rotation, aboutThisAngle) * usingRadius;
        finalVectorOffset = fromThisObject.position + finalVectorOffset;

        // Spawn:
        Instantiate(chosenPrefab, finalVectorOffset, Quaternion.identity);
    }
}
