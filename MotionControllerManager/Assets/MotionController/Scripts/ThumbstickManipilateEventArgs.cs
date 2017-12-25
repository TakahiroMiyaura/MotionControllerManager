// Copyright(c) 2017 Takahiro Miyaura
// Released under the MIT license
// http://opensource.org/licenses/mit-license.php

using UnityEngine;
using UnityEngine.XR.WSA.Input;

namespace Assets.MotionController.Scripts
{
    public class ThumbstickManipilateEventArgs : MotionControllerEventArgs
    {
        public ThumbstickManipilateEventArgs(InteractionSourceState sourceState, bool thumbstickPressed, Vector2 thumbstickPosition, InteractionSourceHandedness handedness) : base(sourceState,handedness)
        {
            ThumbstickPressed = thumbstickPressed;
            ThumbstickPosition = thumbstickPosition;
        }

        public bool ThumbstickPressed { get; private set; }
        public Vector2 ThumbstickPosition { get; private set; }
    }
}