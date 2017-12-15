// Copyright(c) 2017 Takahiro Miyaura
// Released under the MIT license
// http://opensource.org/licenses/mit-license.php

using UnityEngine.XR.WSA.Input;

namespace Assets.MotionControl.Scripts
{
    public partial class MotionControllerManager
    {
        public class SelectManipilateEventArgs : MotionControllerEventArgs
        {
            public SelectManipilateEventArgs(InteractionSourceState sourceState, bool selectPressed, float selectPressedAmount, InteractionSourceHandedness handedness) : base(sourceState,handedness)
            {
                SelectPressed = selectPressed;
                SelectPressedAmount = selectPressedAmount;
            }

            public bool SelectPressed { get; private set; }
            public float SelectPressedAmount { get; private set; }
        }
    }
}