using Bsting.Ship.Managers;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : Manager<LevelManager>
{
    // Feature: Treadmill will be a sphere that will reset player back facing inside the playable zone
    // Teleport - should warp player to opposite edge but facing inside normal direction to playable field

    // Fields:
    [SerializeField] private Vector3 sphereCenterPos = Vector3.zero;
    // Unity Forum posts says most game devs keep run distance under/at 10,000 units (> 100,000 = scuffed):
    [SerializeField][Range(50f, 10000f)] private float sphereRadius = 100.0f;

    // Private var(s):
    private Transform _connectedPlayerShipTransform = null;
    private Vector3 _playerPos = Vector3.zero;
    private Vector3 _directionFromCenter = Vector3.zero;


    protected override void Awake()
    {
        base.Awake();

        sphereCenterPos = this.gameObject.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        CheckIfPlayerNeedsToBeTeleportedBack(_connectedPlayerShipTransform);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(sphereCenterPos, sphereRadius);
    }

    private void CheckIfPlayerNeedsToBeTeleportedBack(Transform currentPlayerTransform)
    {
        if (currentPlayerTransform != null)
        {
            _playerPos = currentPlayerTransform.position;
            _directionFromCenter = _playerPos - sphereCenterPos;

            // Determine if player is out of bounds:
            // (Vector length to sphere center > radius of sphere)
            if (_directionFromCenter.magnitude > sphereRadius)
            {
                // ...
                TeleportToOppositeEnd(currentPlayerTransform);
            }
        }
    }

    private void TeleportToOppositeEnd(Transform playerTransform)
    {
        _directionFromCenter.Normalize();

        playerTransform.position = sphereCenterPos - _directionFromCenter * sphereRadius;
    }

    public void SetPlayerShipTransform(Transform updatedPlayerTransform)
    {
        if (updatedPlayerTransform != null)
        {
            _connectedPlayerShipTransform = updatedPlayerTransform;
        }
    }
}
