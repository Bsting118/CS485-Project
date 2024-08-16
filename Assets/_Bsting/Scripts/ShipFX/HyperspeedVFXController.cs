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
        [SerializeField] private float _amplitudeToApplyToFirstPersonCameraShake = 5.0f;
        [SerializeField] private float _frequencyToApplyToFirstPersonCameraShake = 5.0f;
        [SerializeField] private float _amplitudeToApplyToThirdPersonCameraShake = 0.5f;
        [SerializeField] private float _frequencyToApplyToThirdPersonCameraShake = 0.5f;

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

        void OnApplicationQuit()
        {
            DisableCameraShake();
            DisableChromaticAbberation();
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

        private GameObject GetGameObjectOfVirtualCameraComponent(CinemachineComponentBase vcamComp)
        {
            GameObject currentVCamGameObj = null;

            if (vcamComp != null)
            {
                currentVCamGameObj = vcamComp.gameObject;

                bool isInvisibleCMObj = (currentVCamGameObj.name == "cm");

                if (isInvisibleCMObj)
                {
                    currentVCamGameObj = vcamComp.transform.parent.gameObject;
                }
            }

            return currentVCamGameObj;
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
                GameObject vcamObject = GetGameObjectOfVirtualCameraComponent(noiseModule);
                bool isOnTPCamera = vcamObject.CompareTag("FollowCamera");
                bool isOnFPCamera = vcamObject.CompareTag("CockpitCamera");

                if (isOnTPCamera)
                {
                    ApplyShakeToNoise(noiseModule, _amplitudeToApplyToThirdPersonCameraShake, _frequencyToApplyToThirdPersonCameraShake);
                }
                else if (isOnFPCamera)
                {
                    ApplyShakeToNoise(noiseModule, _amplitudeToApplyToFirstPersonCameraShake, _frequencyToApplyToFirstPersonCameraShake);
                }

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