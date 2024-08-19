/*-------------------------------------
 Custom script made by Brendan Sting
 Date: 7/27/2024
-------------------------------------*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Bsting.Ship
{
    public enum VirtualCameras
    {
        NoCamera = -1,
        CockpitCamera = 0,
        FollowCamera = 1,
    }

    /// <summary>
    /// Base camera interface to declare the input toggle 
    /// property as well as the mapping of vcam indices in an enum.
    /// </summary>
    public interface IVirtualCameras
    {
        VirtualCameras CameraKeyPressed
        {
            get;
        }
    }
}
