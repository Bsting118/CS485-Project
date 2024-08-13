using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using Cinemachine.PostFX;

namespace Bsting.Ship.FX
{
    public class HyperspeedVFXController : MonoBehaviour
    {
        [SerializeField] public CinemachineVolumeSettings volumeSettings;

        private ChromaticAberration chromaticAberration;

        private void Awake()
        {
            if (volumeSettings != null)
            {
                foreach (VolumeComponent volumeComponent in volumeSettings.m_Profile.components)
                {
                    if (volumeComponent.name == "ChromaticAberration") chromaticAberration = volumeComponent as ChromaticAberration;
                }
            }
        }

        public void EnableChromaticAbberation()
        {
            if (chromaticAberration)
            {
                chromaticAberration.intensity.value = 1f;
                chromaticAberration.intensity.overrideState = true;
            }
        }

        public void DisableChromaticAbberation()
        {
            if (chromaticAberration)
            {
                chromaticAberration.intensity.value = 0f;
                chromaticAberration.intensity.overrideState = true;
            }
        }
    }
}