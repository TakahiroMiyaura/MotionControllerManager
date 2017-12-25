// Copyright(c) 2017 Takahiro Miyaura
// Released under the MIT license
// http://opensource.org/licenses/mit-license.php

using System;
using UnityEngine.XR.WSA.Input;

namespace Assets.MotionController.Scripts
{

    public class MotionControllerEventArgs : EventArgs
    {
        public MotionControllerEventArgs(InteractionSourceState sourceState, InteractionSourceHandedness handedness)
        {
            SourceState = sourceState;
            Handedness = handedness;
        }

        public InteractionSourceState SourceState { get; private set; }
        public InteractionSourceHandedness Handedness { get; private set; }
    }
}