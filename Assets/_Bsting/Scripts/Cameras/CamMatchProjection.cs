using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Bsting.Ship.Player.Camera
{
    using Camera = UnityEngine.Camera;

    public class CamMatchProjection : MonoBehaviour
    {
        [field: SerializeField] public Transform TargetCamera;

        void Awake()
        {
            MatchProjectionOfThisCamera(TargetCamera.gameObject);
        }

        void LateUpdate()
        {
            MatchProjectionOfThisCamera(TargetCamera.gameObject);
        }

        private void MatchProjectionOfThisCamera(GameObject toTarget)
        {
            if (toTarget.GetComponent<Camera>() != null)
            {
                Camera toCamera = toTarget.GetComponent<Camera>();
                Camera srcCamera = this.gameObject.GetComponent<Camera>();

                DetermineProjectionType(srcCamera, toCamera);
                srcCamera.fieldOfView = toCamera.fieldOfView;
                srcCamera.farClipPlane = toCamera.farClipPlane;
                srcCamera.nearClipPlane = toCamera.nearClipPlane;
                DetermineIfPhysicalCamera(srcCamera, toCamera);
            }
        }

        private void DetermineProjectionType(Camera forThisCam, Camera usingThisCam)
        {
            if (usingThisCam.orthographic)
            {
                forThisCam.orthographic = true;
            }
            else
            {
                forThisCam.orthographic = false;
            }
        }

        private void DetermineIfPhysicalCamera(Camera forThisCam, Camera usingThisCam)
        {
            if (usingThisCam.usePhysicalProperties)
            {
                forThisCam.usePhysicalProperties = true;
            }
            else
            {
                forThisCam.usePhysicalProperties = false;
            }
        }
    }
}