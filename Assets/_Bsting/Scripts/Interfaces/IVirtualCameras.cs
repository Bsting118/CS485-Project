using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum VirtualCameras
{
    NoCamera = -1,
    CockpitCamera = 0,
    FollowCamera = 1,
}

public interface IVirtualCameras
{
    VirtualCameras CameraKeyPressed 
    {
        get;
    }
}

