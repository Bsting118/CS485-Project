/*-------------------------------------
 Custom script made by Brendan Sting
 Date: 7/21/2024
-------------------------------------*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Bsting.Ship.FX
{
    /// <summary>
    /// Class that auto-handles instantiating a dome of space dust particles, attached to a GameObject. 
    /// </summary>
    [ExecuteInEditMode]
    public class AddSpaceDust : MonoBehaviour
    {
        [SerializeField] private Material _spaceDustMaterial = null;

        private GameObject spaceDustInstance = null;

        #region MonoBehaviors
        void Awake()
        {
            spaceDustInstance = CreateNewSpaceDustObject(this.gameObject);
            SetupDustToDefaultSettings(spaceDustInstance);
        }

        // OnValidate() runs whenever there is a change to a property of the component script it is in
        void OnValidate()
        {
            if (spaceDustInstance != null)
            {
                SetupDustToDefaultSettings(spaceDustInstance);
            }
            else
            {
                spaceDustInstance = CreateNewSpaceDustObject(this.gameObject);
                SetupDustToDefaultSettings(spaceDustInstance);
            }
        }
        #endregion

        #region Helper Function(s)
        private GameObject CreateNewSpaceDustObject(GameObject underThisParent = null)
        {
            GameObject spaceDustObj = new GameObject("SpaceDust");

            ParticleSystem spaceDustParticles = spaceDustObj.AddComponent<ParticleSystem>();

            // Attach under passed parent object:
            if (underThisParent != null)
            {
                spaceDustObj.transform.parent = underThisParent.transform;
            }

            return spaceDustObj;
        }

        private void SetupDustToDefaultSettings(GameObject spaceDustObject)
        {
            ParticleSystem spaceDustParticles = spaceDustObject.GetComponent<ParticleSystem>();
            if (spaceDustParticles != null)
            {
                var rendererSettings = spaceDustObject.GetComponent<ParticleSystemRenderer>();
                var mainSettings = spaceDustParticles.main;
                var emissionSettings = spaceDustParticles.emission;
                var shapeSettings = spaceDustParticles.shape;

                spaceDustParticles.Stop(false, ParticleSystemStopBehavior.StopEmittingAndClear);

                // Basic settings:
                mainSettings.duration = 1000f;
                mainSettings.startLifetime = 4;
                mainSettings.startSize = 2;
                mainSettings.startColor = Color.white;
                mainSettings.simulationSpace = ParticleSystemSimulationSpace.World;
                mainSettings.playOnAwake = true;
                mainSettings.maxParticles = 10000;

                // Emission settings:
                emissionSettings.enabled = true;
                emissionSettings.rateOverTime = 1000;

                // Shape settings:
                shapeSettings.enabled = true;
                shapeSettings.shapeType = ParticleSystemShapeType.Sphere;
                shapeSettings.radius = 500;
                shapeSettings.radiusThickness = 0.5f;

                // Renderer settings:
                rendererSettings.enabled = true;
                if (_spaceDustMaterial != null)
                {
                    rendererSettings.material = _spaceDustMaterial;
                }

                spaceDustParticles.Play();
            }
        }
        #endregion
    }
}