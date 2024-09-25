/*-------------------------------------
 Custom script made by Brendan Sting
 Date: 7/27/2024
-------------------------------------*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Bsting.Ship.Managers
{
    /// <summary>
    /// A camera manager that goes through a list of 
    /// virtual cameras and decides which one to activate. 
    /// </summary>
    public class CameraManager : Manager<CameraManager>, IVirtualCameras
    {
        [Header("List Of Virtual Cameras To Activate")]
        [SerializeField] private List<GameObject> _virtualCameras;

        private PlayerInputSystem _currentPlayerInputSystem;
        private int _virtualCamerasIndex = -1;
        private bool _hasSubscribedToSceneChange = false;

        public VirtualCameras CameraKeyPressed
        {
            get
            {
                if (_virtualCameras != null)
                {
                    if (_virtualCameras.Count > 0)
                    {
                        if (_virtualCamerasIndex == -1) { _virtualCamerasIndex = 0; } // Default starting index

                        if (_currentPlayerInputSystem != null
                            &&
                            _currentPlayerInputSystem.Player.ToggleCamera.WasPressedThisFrame())
                        {
                            ToggleThruCameraIndices(_virtualCameras.Count - 1);
                        }
                    }
                }

                return (VirtualCameras)_virtualCamerasIndex;
            }
        }

        protected override void Awake()
        {
            base.Awake();
        }

        void OnDisable()
        {
            if (_hasSubscribedToSceneChange)
            {
                // Removed subscriber if Singleton is disabled or queued for destruction:
                SceneManager.activeSceneChanged -= ClearListOfCurrentPlayerCameras;
                _hasSubscribedToSceneChange = false;
                Debug.Log("MSG: Removed CameraManager subscriber to active scene changed event.");
            }
        }

        // Start is called before the first frame update
        void Start()
        {
            // Adding flag lock:
            if (!_hasSubscribedToSceneChange)
            {
                // Hook up listener to wipe out dropped game object references upon scene change:
                SceneManager.activeSceneChanged += ClearListOfCurrentPlayerCameras;
                _hasSubscribedToSceneChange = true;
                Debug.Log("MSG: Added CameraManager subscriber to active scene changed event.");
            }

            // Set CockpitCamera to be active camera by startup:
            SetActiveCamera(VirtualCameras.CockpitCamera);
        }

        // Update is called once per frame
        void Update()
        {
            VirtualCameras toggledCam = CameraKeyPressed;
            SetActiveCamera(toggledCam);
        }

        private void SetActiveCamera(VirtualCameras newActiveCamera)
        {
            if (newActiveCamera == VirtualCameras.NoCamera)
            {
                Debug.LogWarning("No camera available to toggle to.");
                return;
            }

            foreach (GameObject camera in _virtualCameras)
            {
                string strNewCamera = newActiveCamera.ToString();
                camera.SetActive(camera.CompareTag(strNewCamera));
            }
        }

        private void ToggleThruCameraIndices(int tailIndex)
        {
            if (_virtualCamerasIndex < tailIndex)
            {
                _virtualCamerasIndex++;
            }
            else
            {
                _virtualCamerasIndex = VirtualCameras.CockpitCamera.GetHashCode();
            }
        }

        private void ClearListOfCurrentPlayerCameras(Scene current, Scene next)
        {
            if (_virtualCameras != null && _virtualCameras.Count > 0)
            {
                _virtualCameras.Clear();
            }
        }

        public void SetPlayerInputInstance(PlayerInputSystem newInputInstance)
        {
            _currentPlayerInputSystem = newInputInstance;
        }

        public List<GameObject> GetListOfManagedCameras()
        {
            List<GameObject> managedCameras = _virtualCameras;
            return managedCameras;
        }

        public void SetListOfManagedCameras(List<GameObject> newListOfCameras)
        {
            _virtualCameras = newListOfCameras;
        }
    }
}