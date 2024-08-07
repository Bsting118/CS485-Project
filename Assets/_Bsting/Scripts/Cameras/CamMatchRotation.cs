using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Bsting.Ship.Player.Camera
{
    public class CamMatchRotation : MonoBehaviour
    {
        [field: SerializeField] public Transform TargetCamera;

        void LateUpdate()
        {
            MatchRotationOfThisCamera(TargetCamera);
        }

        private void MatchRotationOfThisCamera(Transform toTarget)
        {
            this.gameObject.transform.rotation = toTarget.rotation;
        }
    }
}