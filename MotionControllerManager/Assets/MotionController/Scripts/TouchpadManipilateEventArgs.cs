// Copyright(c) 2017 Takahiro Miyaura
// Released under the MIT license
// http://opensource.org/licenses/mit-license.php

using UnityEngine;
using UnityEngine.XR.WSA.Input;

namespace Assets.MotionController.Scripts
{
    public class TouchpadManipilateEventArgs : MotionControllerEventArgs
    {
        public TouchpadManipilateEventArgs(InteractionSourceState sourceState, bool touchpadPressed, bool touchpadTouch, Vector2 touchpadPosition, InteractionSourceHandedness handedness) : base(sourceState,handedness)
        {
            TouchpadPressed = touchpadPressed;
            TouchpadTouch = touchpadTouch;
            TouchpadPosition = touchpadPosition;
        }

        public bool TouchpadPressed { get; private set; }
        public bool TouchpadTouch { get; private set; }
        public Vector2 TouchpadPosition { get; private set; }
    }
}