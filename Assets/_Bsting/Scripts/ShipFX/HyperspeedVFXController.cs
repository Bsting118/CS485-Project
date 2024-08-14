using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using Cinemachine.PostFX;
using Cinemachine;
using Unity.Mathematics;

namespace Bsting.Ship.FX
{
    public class HyperspeedVFXController : MonoBehaviour
    {
        [SerializeField] public List<CinemachineVirtualCamera> ListOfVirtualCams;
        [SerializeField] public List<CinemachineVolumeSettings> VolumeSettings;
        [SerializeField] private float _amplitudeToApplyToCameraShake = 5.0f;
        [SerializeField] private float _frequencyToApplyToCameraShake = 5.0f;

        private List<ChromaticAberration> chromaticAberrations;
        private List<CinemachineBasicMultiChannelPerlin> perlinNoiseModules;

        private void Awake()
        {
            InitVcamPropertyLists();

            if (VolumeSettings != null)
            {
                foreach (CinemachineVolumeSettings vcamVolume in VolumeSettings)
                {
                    foreach (VolumeComponent volumeComponent in vcamVolume.m_Profile.components)
                    {
                        if (volumeComponent.name == "ChromaticAberration")
                        {
                            chromaticAberrations.Add(volumeComponent as ChromaticAberration);
                        }
                    }
                }
            }

            if (ListOfVirtualCams != null)
            {
                foreach (CinemachineVirtualCamera vcam in ListOfVirtualCams)
                {
                    perlinNoiseModules.Add(vcam.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>());
                }
            }
        }

        private void InitVcamPropertyLists()
        {
            chromaticAberrations = new List<ChromaticAberration>();
            perlinNoiseModules = new List<CinemachineBasicMultiChannelPerlin>();
        }

        private void ApplyShakeToNoise(CinemachineBasicMultiChannelPerlin toThisNoise, float amplitudeGain, float frequencyGain)
        {
            toThisNoise.m_AmplitudeGain = amplitudeGain;
            toThisNoise.m_FrequencyGain = frequencyGain;
        }

        public void EnableChromaticAbberation()
        {
            foreach (ChromaticAberration chromAberr in chromaticAberrations)
            {
                chromAberr.intensity.value = 1f;
                chromAberr.intensity.overrideState = true;
            }
        }

        public void DisableChromaticAbberation()
        {
            foreach (ChromaticAberration chromAberr in chromaticAberrations)
            {
                chromAberr.intensity.value = 0f;
                chromAberr.intensity.overrideState = true;
            }
        }

        public void EnableCameraShake()
        {
            foreach (CinemachineBasicMultiChannelPerlin noiseModule in perlinNoiseModules)
            {
                ApplyShakeToNoise(noiseModule, _amplitudeToApplyToCameraShake, _frequencyToApplyToCameraShake);
            }
        }

        public void DisableCameraShake()
        {
            foreach (CinemachineBasicMultiChannelPerlin noiseModule in perlinNoiseModules)
            {
                ApplyShakeToNoise(noiseModule, 0f, 0f);
            }
        }
    }
}