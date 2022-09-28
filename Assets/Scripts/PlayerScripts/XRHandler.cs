using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;

namespace PlayerScripts
{
    public class XRHandler
    {
        private static XRHandler _instance;
        
        private readonly InputDevice _leftController;
        private readonly InputDevice _rightController;
        private readonly InputDevice _headset;
        
        #region XR Input
        public InputDevice GetLeftController()
        {
            return _leftController;
        }
        
        public InputDevice GetRightController()
        {
            return _rightController;
        }
        
        public InputDevice GetHeadset()
        {
            return _headset;
        }

        public Vector3 GetHeadPosition()
        {
            _headset.TryGetFeatureValue(CommonUsages.devicePosition, out var position);
            return position;
        }

        public Quaternion GetHeadRotation()
        {
            _headset.TryGetFeatureValue(CommonUsages.deviceRotation, out var rotation);
            return rotation;
        }
        
        public float GetLeftTriggerValue()
        {
            return TriggerHelper(_leftController);
        }
        
        public float GetRightTriggerValue()
        {
            return TriggerHelper(_rightController);
        }
        
        public Vector2 GetLeftThumbstickValue()
        {
            return ThumbstickHelper(_leftController);
        }
        
        public Vector2 GetRightThumbstickValue()
        {
            return ThumbstickHelper(_rightController);
        }

        public Vector3 GetLeftControllerPosition()
        {
            return ControllerPositionHelper(_leftController);
        }
        
        public Vector3 GetRightControllerPosition()
        {
            return ControllerPositionHelper(_rightController);
        }
        
        #endregion

        #region Helpers
        private static float TriggerHelper(in InputDevice device)
        {
            device.TryGetFeatureValue(CommonUsages.trigger, out var triggerValue);
            return triggerValue;
        }
        
        private static Vector2 ThumbstickHelper(in InputDevice device)
        {
            device.TryGetFeatureValue(CommonUsages.primary2DAxis, out var thumbstickValue);
            return thumbstickValue;
        }

        private static Vector3 ControllerPositionHelper(in InputDevice device)
        {
            device.TryGetFeatureValue(CommonUsages.devicePosition, out var position);
            return position;
        }
        #endregion        
        
        #region Class initialization
        public static XRHandler GetInstance()
        {
            return _instance ??= new XRHandler();
        }
        
        private XRHandler()
        {
            var inputDevices = new List<InputDevice>();
            InputDevices.GetDevices(inputDevices);

            foreach (var device in inputDevices)
            {
                if ((device.characteristics & InputDeviceCharacteristics.Left) != 0)
                {
                    _leftController = device;
                }
                else if ((device.characteristics & InputDeviceCharacteristics.Right) != 0)
                {
                    _rightController = device;
                }
                else if ((device.characteristics & InputDeviceCharacteristics.HeadMounted) != 0)
                {
                    _headset = device;
                }
            }
        }
        #endregion
    }
}