using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class ToggleAsteroidsCam : MonoBehaviour
{
    //[SerializeField] private UniversalRendererData _currentRendererData;
    public UniversalRenderPipelineAsset pipelineAsset;
    private FullScreenPassRendererFeature _outlineFullscreenShader; // Toggle this ON when in the air
    [SerializeField] private GameObject _spaceDustDome; //Toggle this OFF when in the air

    void Start()
    {
        pipelineAsset.GetRenderer(0);
        
        // Access the Renderer Data using Reflection
        var renderer = (GraphicsSettings.currentRenderPipeline as UniversalRenderPipelineAsset).GetRenderer(0);
        var property = typeof(ScriptableRenderer).GetProperty("rendererFeatures", BindingFlags.NonPublic | BindingFlags.Instance);
        List<ScriptableRendererFeature> features = property.GetValue(renderer) as List<ScriptableRendererFeature>;

        // Find the FullScreenPassRendererFeature
        foreach (var feature in features)
        {
            if (feature is FullScreenPassRendererFeature)
            {
                _outlineFullscreenShader = (FullScreenPassRendererFeature)feature;
                break;
            }
        }
    }

    void OnApplicationQuit()
    {
        ToggleAsteroidsDisplayOff();
    }

    public void ToggleAsteroidsDisplayOn()
    {
        TryToTurnOnOutline();
        TryToTurnOffSpaceDust();
    }

    public void ToggleAsteroidsDisplayOff()
    {
        TryToTurnOffOutline();
        TryToTurnOnSpaceDust();
    }

    private void TryToTurnOnOutline()
    {
        if (_outlineFullscreenShader != null)
        {
            _outlineFullscreenShader.SetActive(true);
        }
    }

    private void TryToTurnOffOutline()
    {
        if (_outlineFullscreenShader != null)
        {
            _outlineFullscreenShader.SetActive(false);
        }
    }

    private void TryToTurnOnSpaceDust()
    {
        if (_spaceDustDome != null)
        {
            _spaceDustDome.SetActive(true);
        }
    }

    private void TryToTurnOffSpaceDust()
    {
        if (_spaceDustDome != null)
        {
            _spaceDustDome.SetActive(false);
        }
    }
}
