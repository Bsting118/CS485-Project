using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Bsting.Ship.Player.Camera
{
    public class CamMatchPosition : MonoBehaviour
    {
        [field: SerializeField] public Transform TargetCamera;

        void Awake()
        {
            MatchPositionOfThisCamera(TargetCamera);
        }

        void LateUpdate()
        {
            MatchPositionOfThisCamera(TargetCamera);
        }

        private void MatchPositionOfThisCamera(Transform toTarget)
        {
            this.gameObject.transform.position = toTarget.position;
        }
    }
}
