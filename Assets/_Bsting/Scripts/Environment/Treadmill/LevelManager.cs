using Bsting.Ship.Managers;
using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

namespace Bsting.Ship.Managers
{
    public class LevelManager : Manager<LevelManager>
    {
        // Fields:

        // Feature: Treadmill will be a sphere that will reset player back facing inside the playable zone
        // Teleport - should warp player to opposite edge but facing inside normal direction to playable field
        // NEW Teleport - should warp player to center of treadmill sphere but facing normal direction

        [Header("Spherical Teleport-Based Treadmill Settings")]
        [SerializeField] private Vector3 _sphereCenterPosition = Vector3.zero;
        // Unity Forum posts says most game devs keep run distance under/at 10,000 units (> 100,000 = scuffed):
        [SerializeField][Range(100f, 10000f)] private float _sphereRadius = 100.0f;
        [SerializeField][Tooltip("If unchecked, then Player will spawn at the center of the treadmill sphere.")] 
        private bool _isPlayerTeleportedToEdgeOfMap = true;
        [SerializeField] public UnityEvent OnTeleportPlayerReady = new UnityEvent();
        [SerializeField] public UnityEvent OnPlayerTeleported = new UnityEvent();
        [SerializeField] public UnityEvent OnPlayerOutOfTeleport = new UnityEvent();
        [SerializeField][Range(0f, 10f)] private float _teleportDelayTime = 2.0f;
        [SerializeField][Range(0f, 10f)] private float _teleportSicknessTime = 1.0f;

        [Header("Environment Shading Settings")]
        [field: SerializeField] public List<GameObject> TargetShadedObjectsToFacePlayerDirection = new List<GameObject>();
        [field: SerializeField] public GameObject FirstPerson3DCamera = null;

        // Private var(s):
        private Transform _connectedPlayerShipTransform = null;
        private Vector3 _playerPos = Vector3.zero;
        private Vector3 _directionFromCenter = Vector3.zero;
        private Coroutine _entryTeleportFXRoutine = null;
        private Coroutine _exitTeleportFXRoutine = null;
        private bool _canWarpPlayer = false;
        private bool _canPlayerExitThroughWarp = false;
        private bool _teleportProcessStarted = false;

        #region MonoBehaviors
        protected override void Awake()
        {
            base.Awake();
        }

        void OnEnable()
        {
            _sphereCenterPosition = this.gameObject.transform.position;
            _canPlayerExitThroughWarp = true;
            _teleportProcessStarted = false;
            if (_exitTeleportFXRoutine != null)
            {
                InterruptTeleportExitRoutine();
            }
        }

        void OnDisable()
        {
            if (_entryTeleportFXRoutine != null)
            {
                InterruptTeleportEnterRoutine();
            }
            if (_exitTeleportFXRoutine != null)
            {
                InterruptTeleportExitRoutine();
            }

            // Removed subscriber if Singleton is disabled or queued for destruction:
            SceneManager.activeSceneChanged -= ClearListOfCurrentShadedObjects;
        }

        void OnDestroy()
        {
            if (_entryTeleportFXRoutine != null)
            {
                InterruptTeleportEnterRoutine();
            }
            if (_exitTeleportFXRoutine != null)
            {
                InterruptTeleportExitRoutine();
            }
        }

        private void Start()
        {
            // Hook up listener to wipe out dropped game object references upon scene change:
            SceneManager.activeSceneChanged += ClearListOfCurrentShadedObjects;
        }

        // Update is called once per frame
        void Update()
        {
            // Treadmill Teleport Update:
            CheckIfPlayerNeedsToBeTeleportedBack(_connectedPlayerShipTransform);

            // Lit Environmental Shader Materials Update:
            UpdateLitMaterialsToFacePlayer();
        }

        void OnDrawGizmos()
        {
            // Draw playable area sphere:
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(_sphereCenterPosition, _sphereRadius);

            // Draw first-person in-range of view sphere (anything past this is culled w/o workarounds):
            if (FirstPerson3DCamera != null)
            {
                Color semiTransparentGizmoColor = Color.green;
                semiTransparentGizmoColor.a = 0.2f;
                Gizmos.color = semiTransparentGizmoColor;
                float renderRadius = FirstPerson3DCamera.GetComponent<CinemachineVirtualCamera>().m_Lens.FarClipPlane;
                Vector3 centerPos = GetCurrentPlayerPos();
                Gizmos.DrawSphere(centerPos, renderRadius);
                Gizmos.DrawWireSphere(centerPos, renderRadius);
            }
        }

        void OnApplicationQuit()
        {
            ResetAllLitMaterialsFacingPosition();
        }
        #endregion

        #region Coroutine(s)
        IEnumerator WaitOnTeleportEntryFX()
        {
            Debug.Log("Started Teleport Treadmill Entry Coroutine.");
            _canWarpPlayer = false;
            yield return new WaitForSeconds(_teleportDelayTime);
            _canWarpPlayer = true;
            Debug.Log("Ending Teleport Treadmill Entry Coroutine...");
            _entryTeleportFXRoutine = null;
        }

        IEnumerator WaitOnTeleportExitFX()
        {
            Debug.Log("Started Teleport Treadmill Exit Coroutine.");
            _canPlayerExitThroughWarp = false;
            yield return new WaitForSeconds(_teleportSicknessTime);
            _canPlayerExitThroughWarp = true;
            Debug.Log("Ending Teleport Treadmill Exit Coroutine...");
            _exitTeleportFXRoutine = null;
        }
        #endregion

        #region Helper Function(s)
        private void CheckIfPlayerNeedsToBeTeleportedBack(Transform currentPlayerTransform)
        {
            if (currentPlayerTransform != null)
            {
                _playerPos = GetCurrentPlayerPos(currentPlayerTransform);
                _directionFromCenter = _playerPos - _sphereCenterPosition;

                // Determine if player is out of bounds:
                // (Vector length to sphere center > radius of sphere)
                if ((_directionFromCenter.magnitude > _sphereRadius) && _canPlayerExitThroughWarp)
                {
                    // ...

                    if (_entryTeleportFXRoutine == null && _exitTeleportFXRoutine == null && !_teleportProcessStarted)
                    {
                        OnTeleportPlayerReady.Invoke();
                        _teleportProcessStarted = true;
                        _entryTeleportFXRoutine = StartCoroutine(WaitOnTeleportEntryFX());
                    }
                    if (_canWarpPlayer)
                    {
                        TeleportToOppositeEnd(currentPlayerTransform);

                        if (_exitTeleportFXRoutine == null)
                        {
                            OnPlayerTeleported.Invoke();
                            _exitTeleportFXRoutine = StartCoroutine(WaitOnTeleportExitFX());
                            _teleportProcessStarted = false;
                            OnPlayerOutOfTeleport.Invoke();
                        }
                    }
                }
            }
        }

        private void TeleportToOppositeEnd(Transform playerTransform)
        {
            _directionFromCenter.Normalize();

            if (_isPlayerTeleportedToEdgeOfMap)
            {
                // To opposite edge (Asteroids-style):
                playerTransform.position = _sphereCenterPosition - _directionFromCenter * _sphereRadius;
            }
            else
            {
                // To center of treadmill sphere in same direction:
                playerTransform.position = _sphereCenterPosition - _directionFromCenter;
            }
        }

        private void UpdateLitMaterialsToFacePlayer()
        {
            Vector3 playerPos = GetCurrentPlayerPos();

            foreach (GameObject shadedObj in TargetShadedObjectsToFacePlayerDirection)
            {
                if (shadedObj != null)
                {
                    // Get Unity properties:
                    Material targetMat = shadedObj.GetComponent<Renderer>().material;
                    Transform targetTransform = shadedObj.transform;

                    if ((targetMat != null) && (targetTransform != null))
                    {
                        // Process params:
                        Vector3 targetObjPos = targetTransform.position;

                        // Set shader properties on this shaded object:
                        targetMat.SetVector("_PlayerPositionToIlluminate", playerPos);
                        targetMat.SetVector("_CenterWSPosition", targetObjPos);
                    }
                }
            }
        }

        private void ResetAllLitMaterialsFacingPosition()
        {
            foreach (GameObject shadedObj in TargetShadedObjectsToFacePlayerDirection)
            {
                if (shadedObj != null)
                {
                    Material targetMat = shadedObj.GetComponent<Renderer>().material;

                    if (targetMat != null)
                    {
                        targetMat.SetVector("_PlayerPositionToIlluminate", Vector3.zero);
                    }
                }
            }
        }

        private Vector3 GetCurrentPlayerPos(Transform usingThisTransform = null)
        {
            Vector3 foundPos = Vector3.zero;

            if (usingThisTransform != null)
            {
                foundPos = usingThisTransform.position;
            }
            else if (_connectedPlayerShipTransform != null)
            {
                foundPos = _connectedPlayerShipTransform.position;
            }

            return foundPos;
        }

        private void ClearListOfCurrentShadedObjects(Scene current, Scene next)
        {
            // Wipe the dropped reference list from the scene change event:
            if (TargetShadedObjectsToFacePlayerDirection != null && TargetShadedObjectsToFacePlayerDirection.Count > 0)
            {
                TargetShadedObjectsToFacePlayerDirection.Clear();
            }

            // Force the connected FP camera to be cleared:
            FirstPerson3DCamera = null;
        }
        #endregion

        #region Coroutine Helper(s)
        public void InterruptTeleportEnterRoutine()
        {
            StopCoroutine(_entryTeleportFXRoutine);
            _entryTeleportFXRoutine = null;
            _canWarpPlayer = false;
        }

        public void InterruptTeleportExitRoutine()
        {
            StopCoroutine(_exitTeleportFXRoutine);
            _exitTeleportFXRoutine = null;
            _canPlayerExitThroughWarp = true;
        }
        #endregion

        #region Public Mutator(s)
        public void SetPlayerShipTransform(Transform updatedPlayerTransform)
        {
            if (updatedPlayerTransform != null)
            {
                _connectedPlayerShipTransform = updatedPlayerTransform;
            }
        }

        public void SetFirstPerson3DCamera(GameObject newFPCamera)
        {
            FirstPerson3DCamera = newFPCamera;
        }
        #endregion

        #region Public Accessor(s)
        public GameObject GetFirstPerson3DCamera()
        {
            return FirstPerson3DCamera;
        }
        #endregion
    }
}