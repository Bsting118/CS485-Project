/*-------------------------------------
 Custom script made by Brendan Sting
 Date: 7/21/2024
-------------------------------------*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Class that handles assigning an overriding skybox to a URP camera. 
/// </summary>
[RequireComponent(typeof(Skybox))] // Will add comp. type if not assigned
public class SkyboxHandler : MonoBehaviour
{
    // Serialized fields:
    [SerializeField] private List<Material> _skyboxMaterials;
    [SerializeField] private int _defaultMaterialIndex = 0;

    // Private var's:
    private Skybox _skybox;

    #region MonoBehaviors
    void Awake()
    {
       _skybox = GetComponent<Skybox>();
    }

    void OnEnable()
    {
        ChangeSkybox(_defaultMaterialIndex);
    }
    #endregion

    #region Helper Function(s)
    private void ChangeSkybox(int skyboxIndex)
    {
        if ((_skybox != null) &&  (skyboxIndex >= 0) && (skyboxIndex <= _skyboxMaterials.Count))
        {
            _skybox.material = _skyboxMaterials[skyboxIndex];
        }
    }
    #endregion
}
