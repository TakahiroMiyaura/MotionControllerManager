// Copyright(c) 2017 Takahiro Miyaura
// Released under the MIT license
// http://opensource.org/licenses/mit-license.php

using UnityEngine;
using UnityEngine.XR.WSA.Input;

namespace Assets.MotionController.Scripts
{

    public class MotionControllerManipulateEventArgs : MotionControllerEventArgs
    {
        public MotionControllerManipulateEventArgs(InteractionSourceState sourceState, Vector3 pointerPosition, Quaternion pointerRotation, Vector3 controllerPosition, Quaternion controllerRotation, InteractionSourceHandedness handedness) : base(
            sourceState,handedness)
        {
            PointerPosition = pointerPosition;
            PointerRotation = pointerRotation;
            ControllerPosition = controllerPosition;
            ControllerRotation = controllerRotation;
        }

        public Vector3 PointerPosition { get; private set; }
        public Quaternion PointerRotation { get; private set; }
        public Vector3 ControllerPosition { get; private set; }
        public Quaternion ControllerRotation { get; private set; }
    }
}