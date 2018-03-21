// Copyright(c) 2017 Takahiro Miyaura
// Released under the MIT license
// http://opensource.org/licenses/mit-license.php

using System;
using System.Collections.Generic;
using HoloToolkit.Unity;
using UnityEngine;

#if UNITY_2017_2_OR_NEWER
using UnityEngine.XR.WSA.Input;
#endif

namespace Assets.MotionController.Scripts
{
    /// <summary>
    ///     Motion Controller Manager is responsible for managing input sources and dispatching relevant events to the
    ///     appropriate input handlers.
    /// </summary>
    public partial class MotionControllerManager : Singleton<MotionControllerManager>
    {
        #region private fields

        /// <summary>
        ///     Value with the state just before the controller.
        /// </summary>
        private Dictionary<uint, MotionControllerState> _controllers;

        /// <summary>
        ///     Line Renderer for the left controller.
        /// </summary>
        private LineRenderer _leftLaserPointer;

        /// <summary>
        ///     Line Renderer for the right controller.
        /// </summary>
        private LineRenderer _rightLaserPointer;

        #endregion

        #region public properties

        /// <summary>
        ///     Gets or sets the output debug log.
        /// </summary>
        public bool IsDebugLog = false;

        /// <summary>
        ///     Gets or sets the Line Renderer for the right controller.
        /// </summary>
        public LineRenderer LeftLaserPointer;

        /// <summary>
        ///     Gets or sets whether to display the laser pointer when the left controller grasped.
        /// </summary>
        public bool LeftGrasped;

        /// <summary>
        ///     Gets or sets whether to display the laser pointer when the menu button pressed of the left controller.
        /// </summary>
        public bool LeftMenuDown;

        /// <summary>
        ///     Gets or sets whether to display the laser pointer when the select button pressed of the left controller.
        /// </summary>
        public bool LeftSelectDown = true;

        /// <summary>
        ///     Gets or sets whether to display the laser pointer when the thumbstick button pressed of the left controller.
        /// </summary>
        public bool LeftThumbstickDown;

        /// <summary>
        ///     Gets or sets whether to display the laser pointer when the touchpad button pressed of the left controller.
        /// </summary>
        public bool LeftTouchpadDown;

        /// <summary>
        ///     Gets or sets whether to display the laser pointer when the touchpad button touched of the left controller.
        /// </summary>
        public bool LeftTouchpadTouched = true;


        /// <summary>
        ///     Gets or sets the Line Renderer for the right controller.
        /// </summary>
        public LineRenderer RightLaserPointer;

        /// <summary>
        ///     Gets or sets whether to display the laser pointer when the the right controller grasped.
        /// </summary>
        public bool RightGrasped;

        /// <summary>
        ///     Gets or sets whether to display the laser pointer when the menu button pressed of the right controller.
        /// </summary>
        public bool RightMenuDown;

        /// <summary>
        ///     Gets or sets whether to display the laser pointer when the select button pressed of the right controller.
        /// </summary>
        public bool RightSelectDown = true;

        /// <summary>
        ///     Gets or sets whether to display the laser pointer when the thumbstick button pressed of the right controller.
        /// </summary>
        public bool RightThumbstickDown;

        /// <summary>
        ///     Gets or sets whether to display the laser pointer when the touchpad button pressed of the right controller.
        /// </summary>
        public bool RightTouchpadDown;

        /// <summary>
        ///     Gets or sets whether to display the laser pointer when the touchpad button touched of the right controller.
        /// </summary>
        public bool RightTouchpadTouched = true;

        #endregion

        #region EventHandler

        /// <summary>
        ///     Occurs when grasp button of motion controller is pressed.
        /// </summary>
        [HideInInspector] public EventHandler Grasped;

        /// <summary>
        ///     Occurs when grasp button of motion controller is released.
        /// </summary>
        [HideInInspector] public EventHandler Released;

        /// <summary>
        ///     Occurs when a menu button of motion controller is pressed.
        /// </summary>
        [HideInInspector] public EventHandler MenuDown;

        /// <summary>
        ///     Occurs when a menu button of motion controller is released.
        /// </summary>
        [HideInInspector] public EventHandler MenuUp;

        /// <summary>
        ///     Occurs when motion controller is manipulated.
        /// </summary>
        [HideInInspector] public EventHandler<MotionControllerManipulateEventArgs> MotionControllerManipulate;

        /// <summary>
        ///     Occurs when select button of motion controller is held.
        /// </summary>
        [HideInInspector] public EventHandler<SelectManipulateEventArgs> SelectHold;

        /// <summary>
        ///     Occurs when select button of motion controller is pressed.
        /// </summary>
        [HideInInspector] public EventHandler<SelectManipulateEventArgs> SelectDown;

        /// <summary>
        ///     Occurs when select button of motion controller is released.
        /// </summary>
        [HideInInspector] public EventHandler<SelectManipulateEventArgs> SelectUp;

        /// <summary>
        ///     Occurs when thumbstick button of motion controller is manipulate.
        /// </summary>
        [HideInInspector] public EventHandler<ThumbstickManipulateEventArgs> ThumbstickManipulate;

        /// <summary>
        ///     Occurs when thumbstick button of motion controller is pressed.
        /// </summary>
        [HideInInspector] public EventHandler<ThumbstickManipulateEventArgs> ThumbstickDown;

        /// <summary>
        ///     Occurs when thumbstick button of motion controller is released.
        /// </summary>
        [HideInInspector] public EventHandler<ThumbstickManipulateEventArgs> ThumbstickUp;

        /// <summary>
        ///     Occurs when touchpad button of motion controller is held.
        /// </summary>
        [HideInInspector] public EventHandler<TouchpadManipulateEventArgs> TouchpadManipulate;

        /// <summary>
        ///     Occurs when touchpad button of motion controller is pressed.
        /// </summary>
        [HideInInspector] public EventHandler<TouchpadManipulateEventArgs> TouchpadDown;

        /// <summary>
        ///     Occurs when touchpad button of motion controller is released.
        /// </summary>
        [HideInInspector] public EventHandler<TouchpadManipulateEventArgs> TouchpadUp;

        /// <summary>
        ///     Occurs when touchpad button of motion controller is touched.
        /// </summary>
        [HideInInspector] public EventHandler<TouchpadManipulateEventArgs> TouchpadTouch;

        #endregion

        #region Unity Methods

        protected override void Awake()
        {
            base.Awake();
#if UNITY_WSA && UNITY_2017_2_OR_NEWER
            _controllers = new Dictionary<uint, MotionControllerState>();

            InteractionManager.InteractionSourceDetected += InteractionSourceDetected;

            InteractionManager.InteractionSourceLost += InteractionSourceLost;
            InteractionManager.InteractionSourceUpdated += InteractionSourceUpdated;
#endif
        }

        private void Start()
        {
            var obj = new GameObject("LaserPointers");
            var objL = new GameObject("LeftLaserPointers");
            objL.transform.SetParent(obj.transform);
            var objR = new GameObject("RightLaserPointers");
            objR.transform.SetParent(obj.transform);
            if (RightLaserPointer == null)
            {
                _rightLaserPointer = objR.AddComponent<LineRenderer>();
                InitializeLaserPointer(_rightLaserPointer);
            }
            else
            {
                _rightLaserPointer = RightLaserPointer;
            }
            if (LeftLaserPointer == null)
            {
                _leftLaserPointer = objL.AddComponent<LineRenderer>();
                InitializeLaserPointer(_leftLaserPointer);
            }
            else
            {
                _leftLaserPointer = LeftLaserPointer;
            }
        }

        #endregion

        #region Events

        /// <summary>
        ///     Raises the Grasped event.
        /// </summary>
        /// <param name="sender">
        ///     <see cref="InteractionSourceState" />
        /// </param>
        /// <param name="e">An <see cref="EventArgs" /> that contains the event data. </param>
        public virtual void OnGrasped(object sender, EventArgs e)
        {
            if (Grasped != null)
                Grasped(sender, e);
        }

        /// <summary>
        ///     Raises the Released event.
        /// </summary>
        /// <param name="sender">
        ///     <see cref="InteractionSourceState" />
        /// </param>
        /// <param name="e">An <see cref="EventArgs" /> that contains the event data. </param>
        public virtual void OnReleased(object sender, EventArgs e)
        {
            if (Released != null)
                Released(sender, e);
        }

        /// <summary>
        ///     Raises the MenuDown event.
        /// </summary>
        /// <param name="sender">
        ///     <see cref="InteractionSourceState" />
        /// </param>
        /// <param name="e">An <see cref="EventArgs" /> that contains the event data. </param>
        public virtual void OnMenuDown(object sender, EventArgs e)
        {
            if (MenuDown != null)
                MenuDown(sender, e);
        }

        /// <summary>
        ///     Raises the MenuUp event.
        /// </summary>
        /// <param name="sender">
        ///     <see cref="InteractionSourceState" />
        /// </param>
        /// <param name="e">An <see cref="EventArgs" /> that contains the event data. </param>
        public virtual void OnMenuUp(object sender, EventArgs e)
        {
            if (MenuUp != null)
                MenuUp(sender, e);
        }

        /// <summary>
        ///     Raises the MotionControllerManipulate event.
        /// </summary>
        /// <param name="sender">
        ///     <see cref="InteractionSourceState" />
        /// </param>
        /// <param name="e">An <see cref="EventArgs" /> that contains the event data. </param>
        public virtual void OnMotionControllerManipulate(object sender, MotionControllerManipulateEventArgs e)
        {
            if (MotionControllerManipulate != null)
                MotionControllerManipulate(sender, e);
        }

        /// <summary>
        ///     Raises the SelectDown event.
        /// </summary>
        /// <param name="sender">
        ///     <see cref="InteractionSourceState" />
        /// </param>
        /// <param name="e">An <see cref="SelectManipulateEventArgs" /> that contains the event data. </param>
        public virtual void OnSelectDown(object sender, SelectManipulateEventArgs e)
        {
            if (SelectDown != null)
                SelectDown(sender, e);
        }

        /// <summary>
        ///     Raises the SelectHold event.
        /// </summary>
        /// <param name="sender">
        ///     <see cref="InteractionSourceState" />
        /// </param>
        /// <param name="e">An <see cref="SelectManipulateEventArgs" /> that contains the event data. </param>
        public virtual void OnSelectHold(object sender, SelectManipulateEventArgs e)
        {
            if (SelectHold != null)
                SelectHold(sender, e);
        }

        /// <summary>
        ///     Raises the SelectUp event.
        /// </summary>
        /// <param name="sender">
        ///     <see cref="InteractionSourceState" />
        /// </param>
        /// <param name="e">An <see cref="SelectManipulateEventArgs" /> that contains the event data. </param>
        public virtual void OnSelectUp(object sender, SelectManipulateEventArgs e)
        {
            if (SelectUp != null)
                SelectUp(sender, e);
        }

        /// <summary>
        ///     Raises the ThumbstickDown event.
        /// </summary>
        /// <param name="sender">
        ///     <see cref="InteractionSourceState" />
        /// </param>
        /// <param name="e">An <see cref="ThumbstickManipulateEventArgs" /> that contains the event data. </param>
        public virtual void OnThumbstickDown(object sender, ThumbstickManipulateEventArgs e)
        {
            if (ThumbstickDown != null)
                ThumbstickDown(sender, e);
        }

        /// <summary>
        ///     Raises the ThumbstickManipulate event.
        /// </summary>
        /// <param name="sender">
        ///     <see cref="InteractionSourceState" />
        /// </param>
        /// <param name="e">An <see cref="ThumbstickManipulateEventArgs" /> that contains the event data. </param>
        public virtual void OnThumbstickManipulate(object sender, ThumbstickManipulateEventArgs e)
        {
            if (ThumbstickManipulate != null)
                ThumbstickManipulate(sender, e);
        }

        /// <summary>
        ///     Raises the ThumbstickUp event.
        /// </summary>
        /// <param name="sender">
        ///     <see cref="InteractionSourceState" />
        /// </param>
        /// <param name="e">An <see cref="ThumbstickManipulateEventArgs" /> that contains the event data. </param>
        public virtual void OnThumbstickUp(object sender, ThumbstickManipulateEventArgs e)
        {
            if (ThumbstickUp != null)
                ThumbstickUp(sender, e);
        }

        /// <summary>
        ///     Raises the TouchpadDown event.
        /// </summary>
        /// <param name="sender">
        ///     <see cref="InteractionSourceState" />
        /// </param>
        /// <param name="e">An <see cref="TouchpadManipulateEventArgs" /> that contains the event data. </param>
        public virtual void OnTouchpadDown(object sender, TouchpadManipulateEventArgs e)
        {
            if (TouchpadDown != null)
                TouchpadDown(sender, e);
        }

        /// <summary>
        ///     Raises the TouchpadUp event.
        /// </summary>
        /// <param name="sender">
        ///     <see cref="InteractionSourceState" />
        /// </param>
        /// <param name="e">An <see cref="TouchpadManipulateEventArgs" /> that contains the event data. </param>
        public virtual void OnTouchpadUp(object sender, TouchpadManipulateEventArgs e)
        {
            if (TouchpadUp != null)
                TouchpadUp(sender, e);
        }

        /// <summary>
        ///     Raises the TouchpadTouch event.
        /// </summary>
        /// <param name="sender">
        ///     <see cref="InteractionSourceState" />
        /// </param>
        /// <param name="e">An <see cref="TouchpadManipulateEventArgs" /> that contains the event data. </param>
        public virtual void OnTouchpadTouch(object sender, TouchpadManipulateEventArgs e)
        {
            if (TouchpadTouch != null)
                TouchpadTouch(sender, e);
        }

        /// <summary>
        ///     Raises the TouchpadManipulate event.
        /// </summary>
        /// <param name="sender">
        ///     <see cref="InteractionSourceState" />
        /// </param>
        /// <param name="e">An <see cref="TouchpadManipulateEventArgs" /> that contains the event data. </param>
        public virtual void OnTouchpadManipulate(object sender, TouchpadManipulateEventArgs e)
        {
            if (TouchpadManipulate != null)
                TouchpadManipulate(sender, e);
        }

        #endregion

#if UNITY_WSA && UNITY_2017_2_OR_NEWER
        private void InteractionSourceDetected(InteractionSourceDetectedEventArgs obj)
        {
            if (obj.state.source.kind == InteractionSourceKind.Controller &&
                !_controllers.ContainsKey(obj.state.source.id))
                _controllers.Add(obj.state.source.id, new MotionControllerState
                {
                    Handedness =
                        obj.state.source.handedness
                });
        }

        private void InteractionSourceLost(InteractionSourceLostEventArgs obj)
        {
            _controllers.Remove(obj.state.source.id);
        }

        private void InteractionSourceUpdated(InteractionSourceUpdatedEventArgs obj)
        {
            MotionControllerState motionControllerState;
            if (_controllers.TryGetValue(obj.state.source.id, out motionControllerState))
            {
                
                var handedness = obj.state.source.handedness;
                Vector3 pointerPosition;
                Vector3 controllerPosition;
                Quaternion pointerRotation;
                Quaternion controllerRotation;
                obj.state.sourcePose.TryGetPosition(out pointerPosition, InteractionSourceNode.Pointer);
                obj.state.sourcePose.TryGetRotation(out pointerRotation, InteractionSourceNode.Pointer);
                obj.state.sourcePose.TryGetPosition(out controllerPosition, InteractionSourceNode.Grip);
                obj.state.sourcePose.TryGetRotation(out controllerRotation, InteractionSourceNode.Grip);
        
                SetLaserPointer(obj, handedness, controllerPosition, pointerPosition);

                if (obj.state.grasped && !motionControllerState.Grasped)
                {
                    OutputLog("OnGrasped");
                    OnGrasped(obj.state, new EventArgs());
                }
                else if (!obj.state.grasped && motionControllerState.Grasped)
                {
                    OutputLog("OnReleased");
                    OnReleased(obj.state, new EventArgs());
                }
                motionControllerState.Grasped = obj.state.grasped;


                if (obj.state.menuPressed && !motionControllerState.MenuPressed)
                {
                    OutputLog("OnMenuDown");
                    OnMenuDown(obj.state, new EventArgs());
                }
                else if (!obj.state.menuPressed && motionControllerState.MenuPressed)
                {
                    OutputLog("OnMenuUp");
                    OnMenuUp(obj.state, new EventArgs());
                }
                motionControllerState.MenuPressed = obj.state.menuPressed;


                var selectManipulateEventArgs = new SelectManipulateEventArgs(obj.state
                    , obj.state.selectPressed
                    , obj.state.selectPressedAmount
                    , handedness);
                if (obj.state.selectPressed && !motionControllerState.SelectPressed)
                {
                    OutputLog("OnSelectDown");
                    OnSelectDown(obj.state, selectManipulateEventArgs);
                }
                else if (!obj.state.selectPressed && motionControllerState.SelectPressed)
                {
                    OutputLog("OnSelectUp");
                    OnSelectUp(obj.state, selectManipulateEventArgs);
                }
                else if (obj.state.selectPressed && motionControllerState.SelectPressed)
                {
                    OutputLog("OnSelectHold");
                    OnSelectHold(obj.state, selectManipulateEventArgs);
                }
                motionControllerState.SelectPressed = obj.state.selectPressed;

                var thumbstickManipulateEventArgs = new ThumbstickManipulateEventArgs(obj.state
                    , obj.state.thumbstickPressed
                    , obj.state.thumbstickPosition
                    , handedness);
                if (obj.state.thumbstickPressed && !motionControllerState.ThumbstickPressed)
                {
                    OutputLog("OnThumbstickDown");
                    OnThumbstickDown(obj.state, thumbstickManipulateEventArgs);
                }
                else if (!obj.state.thumbstickPressed && motionControllerState.ThumbstickPressed)
                {
                    OutputLog("OnThumbstickUp");
                    OnThumbstickUp(obj.state, thumbstickManipulateEventArgs);
                }
                
                OnThumbstickManipulate(obj.state, thumbstickManipulateEventArgs);
                motionControllerState.ThumbstickPressed = obj.state.thumbstickPressed;


                var touchpadManipulateEventArgs = new TouchpadManipulateEventArgs(obj.state
                    , obj.state.touchpadPressed
                    , obj.state.touchpadTouched
                    , obj.state.touchpadPosition
                    , handedness);
                if (obj.state.touchpadPressed && !motionControllerState.TouchpadPressed)
                {
                    OutputLog("OnTouchpadDown");
                    OnTouchpadDown(obj.state, touchpadManipulateEventArgs);
                }
                else if (!obj.state.touchpadPressed && motionControllerState.TouchpadPressed)
                {
                    OutputLog("OnThumbstickUp");
                    OnTouchpadUp(obj.state, touchpadManipulateEventArgs);
                }
                OnTouchpadManipulate(obj.state, touchpadManipulateEventArgs);
                motionControllerState.TouchpadPressed = obj.state.touchpadPressed;

                if (obj.state.touchpadTouched && !motionControllerState.TouchpadTouched)
                {
                    OutputLog("OnTouchpadTouch");
                    OnTouchpadTouch(obj.state, touchpadManipulateEventArgs);
                }
                else if (!obj.state.touchpadTouched && motionControllerState.TouchpadTouched)
                {
                    OutputLog("OnTouchpadUp");
                    OnTouchpadUp(obj.state, touchpadManipulateEventArgs);
                }
                else if (motionControllerState.TouchpadTouched && obj.state.touchpadTouched)
                {
                    OutputLog("OnTouchpadManipulate");
                    OnTouchpadManipulate(obj.state, touchpadManipulateEventArgs);
                }
                motionControllerState.TouchpadTouched = obj.state.touchpadTouched;

                OnMotionControllerManipulate(obj.state, new MotionControllerManipulateEventArgs(obj.state,
                    pointerPosition
                    , pointerRotation, controllerPosition
                    , controllerRotation
                    , handedness));
            }
        }

#endif
        private void InitializeLaserPointer(LineRenderer lineRenderer)
        {
            lineRenderer.positionCount = 0;
            lineRenderer.startWidth = 0.005f;
            lineRenderer.endWidth = 0.005f;
            var material = new Material(Shader.Find("Diffuse"));
            material.color = Color.white;
            lineRenderer.material = material;
        }

        private void SetLaserPointer(InteractionSourceUpdatedEventArgs obj, InteractionSourceHandedness handedness,
            Vector3 controllerPosition, Vector3 pointerPosition)
        {
            if (handedness == InteractionSourceHandedness.Left)
                if (LeftGrasped && obj.state.grasped
                    || LeftMenuDown && obj.state.menuPressed
                    || LeftSelectDown && obj.state.selectPressed
                    || LeftThumbstickDown && obj.state.thumbstickPressed
                    || LeftTouchpadTouched && obj.state.touchpadTouched)
                {
                    _leftLaserPointer.positionCount = 2;
                    _leftLaserPointer.SetPosition(0, controllerPosition);
                    _leftLaserPointer.SetPosition(1, pointerPosition);
                }
                else
                {
                    _leftLaserPointer.positionCount = 0;
                }
            else if (handedness == InteractionSourceHandedness.Right)
                if (RightGrasped && obj.state.grasped
                    || RightMenuDown && obj.state.menuPressed
                    || RightSelectDown && obj.state.selectPressed
                    || RightThumbstickDown && obj.state.thumbstickPressed
                    || RightTouchpadTouched && obj.state.touchpadTouched)
                {
                    _rightLaserPointer.positionCount = 2;
                    _rightLaserPointer.SetPosition(0, controllerPosition);
                    _rightLaserPointer.SetPosition(1, pointerPosition);
                }
                else
                {
                    _rightLaserPointer.positionCount = 0;
                }
        }

        private void OutputLog(string message)
        {
            if (IsDebugLog)
            {
                Debug.Log(message);
            }
        }

        /// <summary>
        ///  Motion Controller State hold the state of motion controller.
        /// </summary>
        public class MotionControllerState
        {
            public bool Grasped;
            public InteractionSourceHandedness Handedness;
            public bool MenuPressed;
            public bool SelectPressed;
            public bool ThumbstickPressed;
            public bool TouchpadPressed;
            public bool TouchpadTouched;
        }


    }
}