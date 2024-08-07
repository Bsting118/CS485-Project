using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Bsting.Ship.Managers
{
    public class CameraManager : Manager<CameraManager>, IVirtualCameras
    {
        [Header("List Of Virtual Cameras To Activate")]
        [SerializeField] private List<GameObject> _virtualCameras;

        private PlayerInputSystem _currentPlayerInputSystem;
        private int _virtualCamerasIndex = -1;

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


        // Start is called before the first frame update
        void Start()
        {
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

        public void SetPlayerInputInstance(PlayerInputSystem newInputInstance)
        {
            _currentPlayerInputSystem = newInputInstance;
        }
    }
}