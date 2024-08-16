using Bsting.Ship.Managers;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class LevelManager : Manager<LevelManager>
{
    // Feature: Treadmill will be a sphere that will reset player back facing inside the playable zone
    // Teleport - should warp player to opposite edge but facing inside normal direction to playable field

    // Fields:
    [SerializeField] private Vector3 _sphereCenterPos = Vector3.zero;
    // Unity Forum posts says most game devs keep run distance under/at 10,000 units (> 100,000 = scuffed):
    [SerializeField][Range(50f, 10000f)] private float _sphereRadius = 100.0f;
    [SerializeField] private bool _isPlayerTeleportedToEdgeOfMap = true;
    [SerializeField] public UnityEvent OnTeleportPlayerReady = new UnityEvent();
    [SerializeField] public UnityEvent OnPlayerTeleported = new UnityEvent();

    // Private var(s):
    private Transform _connectedPlayerShipTransform = null;
    private Vector3 _playerPos = Vector3.zero;
    private Vector3 _directionFromCenter = Vector3.zero;


    protected override void Awake()
    {
        base.Awake();

        _sphereCenterPos = this.gameObject.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        CheckIfPlayerNeedsToBeTeleportedBack(_connectedPlayerShipTransform);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(_sphereCenterPos, _sphereRadius);
    }

    private void CheckIfPlayerNeedsToBeTeleportedBack(Transform currentPlayerTransform)
    {
        if (currentPlayerTransform != null)
        {
            _playerPos = currentPlayerTransform.position;
            _directionFromCenter = _playerPos - _sphereCenterPos;

            // Determine if player is out of bounds:
            // (Vector length to sphere center > radius of sphere)
            if (_directionFromCenter.magnitude > _sphereRadius)
            {
                // ...
                OnTeleportPlayerReady.Invoke();
                TeleportToOppositeEnd(currentPlayerTransform);
                OnPlayerTeleported.Invoke();
            }
        }
    }

    private void TeleportToOppositeEnd(Transform playerTransform)
    {
        _directionFromCenter.Normalize();

        if (_isPlayerTeleportedToEdgeOfMap)
        {
            // To opposite edge (Asteroids-style):
            playerTransform.position = _sphereCenterPos - _directionFromCenter * _sphereRadius;
        }
        else
        {
            // To center of treadmill sphere in same direction:
            playerTransform.position = _sphereCenterPos - _directionFromCenter;
        }
    }

    public void SetPlayerShipTransform(Transform updatedPlayerTransform)
    {
        if (updatedPlayerTransform != null)
        {
            _connectedPlayerShipTransform = updatedPlayerTransform;
        }
    }
}
