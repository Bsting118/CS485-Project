//------------------------------------------------------------------------------
// <auto-generated>
//     This code was auto-generated by com.unity.inputsystem:InputActionCodeGenerator
//     version 1.7.0
//     from Assets/Input/PlayerInputSystem.inputactions
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Utilities;

public partial class @PlayerInputSystem: IInputActionCollection2, IDisposable
{
    public InputActionAsset asset { get; }
    public @PlayerInputSystem()
    {
        asset = InputActionAsset.FromJson(@"{
    ""name"": ""PlayerInputSystem"",
    ""maps"": [
        {
            ""name"": ""Player"",
            ""id"": ""50aced59-af28-40d5-b144-8dc54153ecc4"",
            ""actions"": [
                {
                    ""name"": ""Fire"",
                    ""type"": ""Button"",
                    ""id"": ""0a3c28f6-4c9b-4947-81b2-c546caa139a7"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""AircraftYaw"",
                    ""type"": ""Value"",
                    ""id"": ""12111d51-9403-4e57-8d9a-2ea48ababffc"",
                    ""expectedControlType"": ""Axis"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": true
                },
                {
                    ""name"": ""AircraftPitch"",
                    ""type"": ""Value"",
                    ""id"": ""4692dfdc-df18-475d-8005-6b8a66ec7045"",
                    ""expectedControlType"": ""Axis"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": true
                },
                {
                    ""name"": ""AircraftRoll"",
                    ""type"": ""Button"",
                    ""id"": ""095d846a-7cd8-42d6-864e-991b43b4572e"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""AircraftThrust"",
                    ""type"": ""Button"",
                    ""id"": ""e350aae0-f3a3-4577-8cf7-4ced91af980e"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""AircraftHyperSpeed"",
                    ""type"": ""Button"",
                    ""id"": ""ab8cf7af-cb58-4129-8da8-695ddeec9156"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""QuitGameOverride"",
                    ""type"": ""Button"",
                    ""id"": ""05f94904-cceb-4941-81bd-1a9d2670e019"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""ToggleCamera"",
                    ""type"": ""Button"",
                    ""id"": ""1f3fb0f5-4ded-4a0d-9cac-f04957040af2"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                }
            ],
            ""bindings"": [
                {
                    ""name"": """",
                    ""id"": ""143bb1cd-cc10-4eca-a2f0-a3664166fe91"",
                    ""path"": ""<Gamepad>/rightTrigger"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": "";Gamepad"",
                    ""action"": ""Fire"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""05f6913d-c316-48b2-a6bb-e225f14c7960"",
                    ""path"": ""<Mouse>/leftButton"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": "";Keyboard&Mouse"",
                    ""action"": ""Fire"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""ee3d0cd2-254e-47a7-a8cb-bc94d9658c54"",
                    ""path"": ""<Joystick>/trigger"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Joystick"",
                    ""action"": ""Fire"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""38634ddb-267e-49d5-91e8-beec7809db16"",
                    ""path"": ""<Mouse>/position/y"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard&Mouse"",
                    ""action"": ""AircraftPitch"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""47087731-29c2-4eb5-a49f-450a011e0f24"",
                    ""path"": ""<Mouse>/position/x"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard&Mouse"",
                    ""action"": ""AircraftYaw"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""RollAxis"",
                    ""id"": ""492a3e78-966e-4fcd-91d0-f99f6e59dfdc"",
                    ""path"": ""1DAxis"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""AircraftRoll"",
                    ""isComposite"": true,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""negative"",
                    ""id"": ""609d1003-5a75-4132-a5b6-82ff458b1559"",
                    ""path"": ""<Keyboard>/d"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard&Mouse"",
                    ""action"": ""AircraftRoll"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""positive"",
                    ""id"": ""42a9d207-6891-4b40-966a-7f6035444ffa"",
                    ""path"": ""<Keyboard>/a"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard&Mouse"",
                    ""action"": ""AircraftRoll"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""ThrustAxis"",
                    ""id"": ""07afd80f-a02e-44ac-b4af-595f3d8e768f"",
                    ""path"": ""1DAxis"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""AircraftThrust"",
                    ""isComposite"": true,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""negative"",
                    ""id"": ""65bae9ea-b2ac-46a2-9618-52a545410b79"",
                    ""path"": ""<Keyboard>/s"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard&Mouse"",
                    ""action"": ""AircraftThrust"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""positive"",
                    ""id"": ""4ed02a08-7b33-454b-9d9f-fc8126b58f23"",
                    ""path"": ""<Keyboard>/w"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard&Mouse"",
                    ""action"": ""AircraftThrust"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": """",
                    ""id"": ""382bddd4-b115-472d-9307-8764e9beb881"",
                    ""path"": ""<Keyboard>/escape"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard&Mouse"",
                    ""action"": ""QuitGameOverride"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""be8051a7-b964-41b8-aa5a-4680bb278229"",
                    ""path"": ""<Keyboard>/q"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard&Mouse"",
                    ""action"": ""ToggleCamera"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""90d55dd0-80ec-4ab1-b178-67313591ee9e"",
                    ""path"": ""<Keyboard>/leftShift"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard&Mouse"",
                    ""action"": ""AircraftHyperSpeed"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        },
        {
            ""name"": ""UI"",
            ""id"": ""24fe770d-5582-45b8-a05e-f62f90b68f39"",
            ""actions"": [],
            ""bindings"": []
        }
    ],
    ""controlSchemes"": [
        {
            ""name"": ""Keyboard&Mouse"",
            ""bindingGroup"": ""Keyboard&Mouse"",
            ""devices"": [
                {
                    ""devicePath"": ""<Keyboard>"",
                    ""isOptional"": false,
                    ""isOR"": false
                },
                {
                    ""devicePath"": ""<Mouse>"",
                    ""isOptional"": false,
                    ""isOR"": false
                }
            ]
        },
        {
            ""name"": ""Gamepad"",
            ""bindingGroup"": ""Gamepad"",
            ""devices"": [
                {
                    ""devicePath"": ""<Gamepad>"",
                    ""isOptional"": false,
                    ""isOR"": false
                }
            ]
        },
        {
            ""name"": ""Touch"",
            ""bindingGroup"": ""Touch"",
            ""devices"": [
                {
                    ""devicePath"": ""<Touchscreen>"",
                    ""isOptional"": false,
                    ""isOR"": false
                }
            ]
        },
        {
            ""name"": ""Joystick"",
            ""bindingGroup"": ""Joystick"",
            ""devices"": [
                {
                    ""devicePath"": ""<Joystick>"",
                    ""isOptional"": false,
                    ""isOR"": false
                }
            ]
        },
        {
            ""name"": ""XR"",
            ""bindingGroup"": ""XR"",
            ""devices"": [
                {
                    ""devicePath"": ""<XRController>"",
                    ""isOptional"": false,
                    ""isOR"": false
                }
            ]
        }
    ]
}");
        // Player
        m_Player = asset.FindActionMap("Player", throwIfNotFound: true);
        m_Player_Fire = m_Player.FindAction("Fire", throwIfNotFound: true);
        m_Player_AircraftYaw = m_Player.FindAction("AircraftYaw", throwIfNotFound: true);
        m_Player_AircraftPitch = m_Player.FindAction("AircraftPitch", throwIfNotFound: true);
        m_Player_AircraftRoll = m_Player.FindAction("AircraftRoll", throwIfNotFound: true);
        m_Player_AircraftThrust = m_Player.FindAction("AircraftThrust", throwIfNotFound: true);
        m_Player_AircraftHyperSpeed = m_Player.FindAction("AircraftHyperSpeed", throwIfNotFound: true);
        m_Player_QuitGameOverride = m_Player.FindAction("QuitGameOverride", throwIfNotFound: true);
        m_Player_ToggleCamera = m_Player.FindAction("ToggleCamera", throwIfNotFound: true);
        // UI
        m_UI = asset.FindActionMap("UI", throwIfNotFound: true);
    }

    public void Dispose()
    {
        UnityEngine.Object.Destroy(asset);
    }

    public InputBinding? bindingMask
    {
        get => asset.bindingMask;
        set => asset.bindingMask = value;
    }

    public ReadOnlyArray<InputDevice>? devices
    {
        get => asset.devices;
        set => asset.devices = value;
    }

    public ReadOnlyArray<InputControlScheme> controlSchemes => asset.controlSchemes;

    public bool Contains(InputAction action)
    {
        return asset.Contains(action);
    }

    public IEnumerator<InputAction> GetEnumerator()
    {
        return asset.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }

    public void Enable()
    {
        asset.Enable();
    }

    public void Disable()
    {
        asset.Disable();
    }

    public IEnumerable<InputBinding> bindings => asset.bindings;

    public InputAction FindAction(string actionNameOrId, bool throwIfNotFound = false)
    {
        return asset.FindAction(actionNameOrId, throwIfNotFound);
    }

    public int FindBinding(InputBinding bindingMask, out InputAction action)
    {
        return asset.FindBinding(bindingMask, out action);
    }

    // Player
    private readonly InputActionMap m_Player;
    private List<IPlayerActions> m_PlayerActionsCallbackInterfaces = new List<IPlayerActions>();
    private readonly InputAction m_Player_Fire;
    private readonly InputAction m_Player_AircraftYaw;
    private readonly InputAction m_Player_AircraftPitch;
    private readonly InputAction m_Player_AircraftRoll;
    private readonly InputAction m_Player_AircraftThrust;
    private readonly InputAction m_Player_AircraftHyperSpeed;
    private readonly InputAction m_Player_QuitGameOverride;
    private readonly InputAction m_Player_ToggleCamera;
    public struct PlayerActions
    {
        private @PlayerInputSystem m_Wrapper;
        public PlayerActions(@PlayerInputSystem wrapper) { m_Wrapper = wrapper; }
        public InputAction @Fire => m_Wrapper.m_Player_Fire;
        public InputAction @AircraftYaw => m_Wrapper.m_Player_AircraftYaw;
        public InputAction @AircraftPitch => m_Wrapper.m_Player_AircraftPitch;
        public InputAction @AircraftRoll => m_Wrapper.m_Player_AircraftRoll;
        public InputAction @AircraftThrust => m_Wrapper.m_Player_AircraftThrust;
        public InputAction @AircraftHyperSpeed => m_Wrapper.m_Player_AircraftHyperSpeed;
        public InputAction @QuitGameOverride => m_Wrapper.m_Player_QuitGameOverride;
        public InputAction @ToggleCamera => m_Wrapper.m_Player_ToggleCamera;
        public InputActionMap Get() { return m_Wrapper.m_Player; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(PlayerActions set) { return set.Get(); }
        public void AddCallbacks(IPlayerActions instance)
        {
            if (instance == null || m_Wrapper.m_PlayerActionsCallbackInterfaces.Contains(instance)) return;
            m_Wrapper.m_PlayerActionsCallbackInterfaces.Add(instance);
            @Fire.started += instance.OnFire;
            @Fire.performed += instance.OnFire;
            @Fire.canceled += instance.OnFire;
            @AircraftYaw.started += instance.OnAircraftYaw;
            @AircraftYaw.performed += instance.OnAircraftYaw;
            @AircraftYaw.canceled += instance.OnAircraftYaw;
            @AircraftPitch.started += instance.OnAircraftPitch;
            @AircraftPitch.performed += instance.OnAircraftPitch;
            @AircraftPitch.canceled += instance.OnAircraftPitch;
            @AircraftRoll.started += instance.OnAircraftRoll;
            @AircraftRoll.performed += instance.OnAircraftRoll;
            @AircraftRoll.canceled += instance.OnAircraftRoll;
            @AircraftThrust.started += instance.OnAircraftThrust;
            @AircraftThrust.performed += instance.OnAircraftThrust;
            @AircraftThrust.canceled += instance.OnAircraftThrust;
            @AircraftHyperSpeed.started += instance.OnAircraftHyperSpeed;
            @AircraftHyperSpeed.performed += instance.OnAircraftHyperSpeed;
            @AircraftHyperSpeed.canceled += instance.OnAircraftHyperSpeed;
            @QuitGameOverride.started += instance.OnQuitGameOverride;
            @QuitGameOverride.performed += instance.OnQuitGameOverride;
            @QuitGameOverride.canceled += instance.OnQuitGameOverride;
            @ToggleCamera.started += instance.OnToggleCamera;
            @ToggleCamera.performed += instance.OnToggleCamera;
            @ToggleCamera.canceled += instance.OnToggleCamera;
        }

        private void UnregisterCallbacks(IPlayerActions instance)
        {
            @Fire.started -= instance.OnFire;
            @Fire.performed -= instance.OnFire;
            @Fire.canceled -= instance.OnFire;
            @AircraftYaw.started -= instance.OnAircraftYaw;
            @AircraftYaw.performed -= instance.OnAircraftYaw;
            @AircraftYaw.canceled -= instance.OnAircraftYaw;
            @AircraftPitch.started -= instance.OnAircraftPitch;
            @AircraftPitch.performed -= instance.OnAircraftPitch;
            @AircraftPitch.canceled -= instance.OnAircraftPitch;
            @AircraftRoll.started -= instance.OnAircraftRoll;
            @AircraftRoll.performed -= instance.OnAircraftRoll;
            @AircraftRoll.canceled -= instance.OnAircraftRoll;
            @AircraftThrust.started -= instance.OnAircraftThrust;
            @AircraftThrust.performed -= instance.OnAircraftThrust;
            @AircraftThrust.canceled -= instance.OnAircraftThrust;
            @AircraftHyperSpeed.started -= instance.OnAircraftHyperSpeed;
            @AircraftHyperSpeed.performed -= instance.OnAircraftHyperSpeed;
            @AircraftHyperSpeed.canceled -= instance.OnAircraftHyperSpeed;
            @QuitGameOverride.started -= instance.OnQuitGameOverride;
            @QuitGameOverride.performed -= instance.OnQuitGameOverride;
            @QuitGameOverride.canceled -= instance.OnQuitGameOverride;
            @ToggleCamera.started -= instance.OnToggleCamera;
            @ToggleCamera.performed -= instance.OnToggleCamera;
            @ToggleCamera.canceled -= instance.OnToggleCamera;
        }

        public void RemoveCallbacks(IPlayerActions instance)
        {
            if (m_Wrapper.m_PlayerActionsCallbackInterfaces.Remove(instance))
                UnregisterCallbacks(instance);
        }

        public void SetCallbacks(IPlayerActions instance)
        {
            foreach (var item in m_Wrapper.m_PlayerActionsCallbackInterfaces)
                UnregisterCallbacks(item);
            m_Wrapper.m_PlayerActionsCallbackInterfaces.Clear();
            AddCallbacks(instance);
        }
    }
    public PlayerActions @Player => new PlayerActions(this);

    // UI
    private readonly InputActionMap m_UI;
    private List<IUIActions> m_UIActionsCallbackInterfaces = new List<IUIActions>();
    public struct UIActions
    {
        private @PlayerInputSystem m_Wrapper;
        public UIActions(@PlayerInputSystem wrapper) { m_Wrapper = wrapper; }
        public InputActionMap Get() { return m_Wrapper.m_UI; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(UIActions set) { return set.Get(); }
        public void AddCallbacks(IUIActions instance)
        {
            if (instance == null || m_Wrapper.m_UIActionsCallbackInterfaces.Contains(instance)) return;
            m_Wrapper.m_UIActionsCallbackInterfaces.Add(instance);
        }

        private void UnregisterCallbacks(IUIActions instance)
        {
        }

        public void RemoveCallbacks(IUIActions instance)
        {
            if (m_Wrapper.m_UIActionsCallbackInterfaces.Remove(instance))
                UnregisterCallbacks(instance);
        }

        public void SetCallbacks(IUIActions instance)
        {
            foreach (var item in m_Wrapper.m_UIActionsCallbackInterfaces)
                UnregisterCallbacks(item);
            m_Wrapper.m_UIActionsCallbackInterfaces.Clear();
            AddCallbacks(instance);
        }
    }
    public UIActions @UI => new UIActions(this);
    private int m_KeyboardMouseSchemeIndex = -1;
    public InputControlScheme KeyboardMouseScheme
    {
        get
        {
            if (m_KeyboardMouseSchemeIndex == -1) m_KeyboardMouseSchemeIndex = asset.FindControlSchemeIndex("Keyboard&Mouse");
            return asset.controlSchemes[m_KeyboardMouseSchemeIndex];
        }
    }
    private int m_GamepadSchemeIndex = -1;
    public InputControlScheme GamepadScheme
    {
        get
        {
            if (m_GamepadSchemeIndex == -1) m_GamepadSchemeIndex = asset.FindControlSchemeIndex("Gamepad");
            return asset.controlSchemes[m_GamepadSchemeIndex];
        }
    }
    private int m_TouchSchemeIndex = -1;
    public InputControlScheme TouchScheme
    {
        get
        {
            if (m_TouchSchemeIndex == -1) m_TouchSchemeIndex = asset.FindControlSchemeIndex("Touch");
            return asset.controlSchemes[m_TouchSchemeIndex];
        }
    }
    private int m_JoystickSchemeIndex = -1;
    public InputControlScheme JoystickScheme
    {
        get
        {
            if (m_JoystickSchemeIndex == -1) m_JoystickSchemeIndex = asset.FindControlSchemeIndex("Joystick");
            return asset.controlSchemes[m_JoystickSchemeIndex];
        }
    }
    private int m_XRSchemeIndex = -1;
    public InputControlScheme XRScheme
    {
        get
        {
            if (m_XRSchemeIndex == -1) m_XRSchemeIndex = asset.FindControlSchemeIndex("XR");
            return asset.controlSchemes[m_XRSchemeIndex];
        }
    }
    public interface IPlayerActions
    {
        void OnFire(InputAction.CallbackContext context);
        void OnAircraftYaw(InputAction.CallbackContext context);
        void OnAircraftPitch(InputAction.CallbackContext context);
        void OnAircraftRoll(InputAction.CallbackContext context);
        void OnAircraftThrust(InputAction.CallbackContext context);
        void OnAircraftHyperSpeed(InputAction.CallbackContext context);
        void OnQuitGameOverride(InputAction.CallbackContext context);
        void OnToggleCamera(InputAction.CallbackContext context);
    }
    public interface IUIActions
    {
    }
}
