// GENERATED AUTOMATICALLY FROM 'Assets/Content/ControlSchemes/Player Controls.inputactions'

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Utilities;

public class @DefaultplayerControls : IInputActionCollection, IDisposable
{
    public InputActionAsset asset { get; }
    public @DefaultplayerControls()
    {
        asset = InputActionAsset.FromJson(@"{
    ""name"": ""Player Controls"",
    ""maps"": [
        {
            ""name"": ""Default"",
            ""id"": ""f9f9af1a-0c95-4ee8-acdb-d183e9b03ae3"",
            ""actions"": [
                {
                    ""name"": ""Horizontal"",
                    ""type"": ""Value"",
                    ""id"": ""9c1e6997-7e0a-407a-bbb9-dbc491738447"",
                    ""expectedControlType"": ""Axis"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""JumpAction"",
                    ""type"": ""Button"",
                    ""id"": ""bc37d5b7-cd01-48ff-913c-b1352e1d5af4"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Action1"",
                    ""type"": ""Button"",
                    ""id"": ""35440359-68d6-41fa-8f15-b066738ec558"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Action2"",
                    ""type"": ""Button"",
                    ""id"": ""6c629074-3827-4e89-ab57-06896493cc2e"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                }
            ],
            ""bindings"": [
                {
                    ""name"": """",
                    ""id"": ""f49b7c39-864e-45a1-8a10-9b020977c632"",
                    ""path"": ""<Keyboard>/a"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Horizontal"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""a8052e13-6616-4fc0-aaad-af5412e05959"",
                    ""path"": ""<Gamepad>/dpad/left"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Horizontal"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""b154aff9-1414-4b19-8e33-e39bd5568ea8"",
                    ""path"": ""<Gamepad>/dpad/right"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Horizontal"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""8c8e26f7-89f7-4b4f-b22b-6e1fa0f0c7bb"",
                    ""path"": ""<Keyboard>/d"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Horizontal"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""1D Axis"",
                    ""id"": ""01ad2d48-0957-42e8-bd94-800e1b0a3fa4"",
                    ""path"": ""1DAxis"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Horizontal"",
                    ""isComposite"": true,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""negative"",
                    ""id"": ""4eef9aa5-791a-4dd0-a29b-22c7288b2d5c"",
                    ""path"": ""<Gamepad>/leftStick"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Horizontal"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""negative"",
                    ""id"": ""bc2e1873-4f00-4966-a84d-dadc6e00a705"",
                    ""path"": ""<DualShockGamepad>/leftStick/left"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Horizontal"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""positive"",
                    ""id"": ""8a8b1631-f3b0-47ee-9c9f-9003b8f9de21"",
                    ""path"": ""<Gamepad>/leftStick"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Horizontal"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""positive"",
                    ""id"": ""7881d50b-bbd1-4fbf-b094-b886be77da91"",
                    ""path"": ""<DualShockGamepad>/leftStick/right"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Horizontal"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": """",
                    ""id"": ""b89676ed-a059-477d-9998-3db494a5b919"",
                    ""path"": ""<Keyboard>/space"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""JumpAction"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""ab49ff77-882f-4a63-8d58-da5092a0d81e"",
                    ""path"": ""<DualShockGamepad>/buttonSouth"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""JumpAction"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""9125552c-3abe-47c8-9e51-f4e98fe8ddd3"",
                    ""path"": ""<Gamepad>/leftShoulder"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Action2"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""53ca2dbe-8041-4560-89cd-d3b9453e764c"",
                    ""path"": ""<Gamepad>/rightShoulder"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Action1"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        }
    ],
    ""controlSchemes"": []
}");
        // Default
        m_Default = asset.FindActionMap("Default", throwIfNotFound: true);
        m_Default_Horizontal = m_Default.FindAction("Horizontal", throwIfNotFound: true);
        m_Default_JumpAction = m_Default.FindAction("JumpAction", throwIfNotFound: true);
        m_Default_Action1 = m_Default.FindAction("Action1", throwIfNotFound: true);
        m_Default_Action2 = m_Default.FindAction("Action2", throwIfNotFound: true);
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

    // Default
    private readonly InputActionMap m_Default;
    private IDefaultActions m_DefaultActionsCallbackInterface;
    private readonly InputAction m_Default_Horizontal;
    private readonly InputAction m_Default_JumpAction;
    private readonly InputAction m_Default_Action1;
    private readonly InputAction m_Default_Action2;
    public struct DefaultActions
    {
        private @DefaultplayerControls m_Wrapper;
        public DefaultActions(@DefaultplayerControls wrapper) { m_Wrapper = wrapper; }
        public InputAction @Horizontal => m_Wrapper.m_Default_Horizontal;
        public InputAction @JumpAction => m_Wrapper.m_Default_JumpAction;
        public InputAction @Action1 => m_Wrapper.m_Default_Action1;
        public InputAction @Action2 => m_Wrapper.m_Default_Action2;
        public InputActionMap Get() { return m_Wrapper.m_Default; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(DefaultActions set) { return set.Get(); }
        public void SetCallbacks(IDefaultActions instance)
        {
            if (m_Wrapper.m_DefaultActionsCallbackInterface != null)
            {
                @Horizontal.started -= m_Wrapper.m_DefaultActionsCallbackInterface.OnHorizontal;
                @Horizontal.performed -= m_Wrapper.m_DefaultActionsCallbackInterface.OnHorizontal;
                @Horizontal.canceled -= m_Wrapper.m_DefaultActionsCallbackInterface.OnHorizontal;
                @JumpAction.started -= m_Wrapper.m_DefaultActionsCallbackInterface.OnJumpAction;
                @JumpAction.performed -= m_Wrapper.m_DefaultActionsCallbackInterface.OnJumpAction;
                @JumpAction.canceled -= m_Wrapper.m_DefaultActionsCallbackInterface.OnJumpAction;
                @Action1.started -= m_Wrapper.m_DefaultActionsCallbackInterface.OnAction1;
                @Action1.performed -= m_Wrapper.m_DefaultActionsCallbackInterface.OnAction1;
                @Action1.canceled -= m_Wrapper.m_DefaultActionsCallbackInterface.OnAction1;
                @Action2.started -= m_Wrapper.m_DefaultActionsCallbackInterface.OnAction2;
                @Action2.performed -= m_Wrapper.m_DefaultActionsCallbackInterface.OnAction2;
                @Action2.canceled -= m_Wrapper.m_DefaultActionsCallbackInterface.OnAction2;
            }
            m_Wrapper.m_DefaultActionsCallbackInterface = instance;
            if (instance != null)
            {
                @Horizontal.started += instance.OnHorizontal;
                @Horizontal.performed += instance.OnHorizontal;
                @Horizontal.canceled += instance.OnHorizontal;
                @JumpAction.started += instance.OnJumpAction;
                @JumpAction.performed += instance.OnJumpAction;
                @JumpAction.canceled += instance.OnJumpAction;
                @Action1.started += instance.OnAction1;
                @Action1.performed += instance.OnAction1;
                @Action1.canceled += instance.OnAction1;
                @Action2.started += instance.OnAction2;
                @Action2.performed += instance.OnAction2;
                @Action2.canceled += instance.OnAction2;
            }
        }
    }
    public DefaultActions @Default => new DefaultActions(this);
    public interface IDefaultActions
    {
        void OnHorizontal(InputAction.CallbackContext context);
        void OnJumpAction(InputAction.CallbackContext context);
        void OnAction1(InputAction.CallbackContext context);
        void OnAction2(InputAction.CallbackContext context);
    }
}
