// Copyright(c) 2017 Takahiro Miyaura
// Released under the MIT license
// http://opensource.org/licenses/mit-license.php

using System;
using System.Collections.Generic;
using HoloToolkit.Unity;
using HoloToolkit.Unity.InputModule;
using UnityEngine;
using UnityEngine.XR.WSA.Input;
using Cursor = HoloToolkit.Unity.InputModule.Cursor;


namespace Assets.MotionController.Scripts
{
    /// <summary>
    ///     Motion Controller Manager is responsible for managing input sources and dispatching relevant events to the
    ///     appropriate input handlers.
    /// </summary>
    public partial class MotionControllerManager : SingleInstance<MotionControllerManager>
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
        [HideInInspector] public EventHandler<MotionControllerPositionEventArgs> MotionControllerPositionChanged;
        
        /// <summary>
        ///     Occurs when select button of motion controller is held.
        /// </summary>
        [HideInInspector] public EventHandler<SelectManipilateEventArgs> SelectHold;

        /// <summary>
        ///     Occurs when select button of motion controller is pressed.
        /// </summary>
        [HideInInspector] public EventHandler<SelectManipilateEventArgs> SelectDown;

        /// <summary>
        ///     Occurs when select button of motion controller is released.
        /// </summary>
        [HideInInspector] public EventHandler<SelectManipilateEventArgs> SelectUp;

        /// <summary>
        ///     Occurs when thumbstick button of motion controller is held.
        /// </summary>
        [HideInInspector] public EventHandler<ThumbstickManipilateEventArgs> ThumbstickHold;

        /// <summary>
        ///     Occurs when thumbstick button of motion controller is pressed.
        /// </summary>
        [HideInInspector] public EventHandler<ThumbstickManipilateEventArgs> ThumbstickDown;

        /// <summary>
        ///     Occurs when thumbstick button of motion controller is released.
        /// </summary>
        [HideInInspector] public EventHandler<ThumbstickManipilateEventArgs> ThumbstickUp;

        /// <summary>
        ///     Occurs when touchpad button of motion controller is held.
        /// </summary>
        [HideInInspector] public EventHandler<TouchpadManipilateEventArgs> TouchpadHold;

        /// <summary>
        ///     Occurs when touchpad button of motion controller is pressed.
        /// </summary>
        [HideInInspector] public EventHandler<TouchpadManipilateEventArgs> TouchpadDown;

        /// <summary>
        ///     Occurs when touchpad button of motion controller is released.
        /// </summary>
        [HideInInspector] public EventHandler<TouchpadManipilateEventArgs> TouchpadUp;

        /// <summary>
        ///     Occurs when touchpad button of motion controller is touched.
        /// </summary>
        [HideInInspector] public EventHandler<TouchpadManipilateEventArgs> TouchpadTouch;

        #endregion

        #region Unity Methods

        private void Awake()
        {
#if UNITY_WSA && UNITY_2017_2_OR_NEWER
            _controllers = new Dictionary<uint, MotionControllerState>();

            InteractionManager.InteractionSourceDetected += InteractionSourceDetected;

            InteractionManager.InteractionSourceLost += InteractionSourceLost;
            InteractionManager.InteractionSourceUpdated += InteractionSourceUpdated;
#endif
        }

        private void Start()
        {
            var parent = new GameObject("LaserPointers");
            if (RightLaserPointer == null)
            {
                var obj = new GameObject("Right");
                obj.transform.SetParent(parent.transform);
                _rightLaserPointer = obj.AddComponent<LineRenderer>();
                InitializeLaserPointer(_rightLaserPointer);
            }
            else
            {
                _rightLaserPointer = RightLaserPointer;
            }
            if (LeftLaserPointer == null)
            {
                var obj = new GameObject("Left");
                obj.transform.SetParent(parent.transform);
                _leftLaserPointer = obj.AddComponent<LineRenderer>();
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
        ///     Raises the MotionControllerPositionChanged event.
        /// </summary>
        /// <param name="sender">
        ///     <see cref="InteractionSourceState" />
        /// </param>
        /// <param name="e">An <see cref="EventArgs" /> that contains the event data. </param>
        public virtual void OnMotionControllerPositionChanged(object sender, MotionControllerPositionEventArgs e)
        {
            if (MotionControllerPositionChanged != null)
                MotionControllerPositionChanged(sender, e);
        }

        /// <summary>
        ///     Raises the SelectDown event.
        /// </summary>
        /// <param name="sender">
        ///     <see cref="InteractionSourceState" />
        /// </param>
        /// <param name="e">An <see cref="SelectManipilateEventArgs" /> that contains the event data. </param>
        public virtual void OnSelectDown(object sender, SelectManipilateEventArgs e)
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
        /// <param name="e">An <see cref="SelectManipilateEventArgs" /> that contains the event data. </param>
        public virtual void OnSelectHold(object sender, SelectManipilateEventArgs e)
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
        /// <param name="e">An <see cref="SelectManipilateEventArgs" /> that contains the event data. </param>
        public virtual void OnSelectUp(object sender, SelectManipilateEventArgs e)
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
        /// <param name="e">An <see cref="ThumbstickManipilateEventArgs" /> that contains the event data. </param>
        public virtual void OnThumbstickDown(object sender, ThumbstickManipilateEventArgs e)
        {
            if (ThumbstickDown != null)
                ThumbstickDown(sender, e);
        }

        /// <summary>
        ///     Raises the ThumbstickHold event.
        /// </summary>
        /// <param name="sender">
        ///     <see cref="InteractionSourceState" />
        /// </param>
        /// <param name="e">An <see cref="ThumbstickManipilateEventArgs" /> that contains the event data. </param>
        public virtual void OnThumbstickHold(object sender, ThumbstickManipilateEventArgs e)
        {
            if (ThumbstickHold != null)
                ThumbstickHold(sender, e);
        }

        /// <summary>
        ///     Raises the ThumbstickUp event.
        /// </summary>
        /// <param name="sender">
        ///     <see cref="InteractionSourceState" />
        /// </param>
        /// <param name="e">An <see cref="ThumbstickManipilateEventArgs" /> that contains the event data. </param>
        public virtual void OnThumbstickUp(object sender, ThumbstickManipilateEventArgs e)
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
        /// <param name="e">An <see cref="TouchpadManipilateEventArgs" /> that contains the event data. </param>
        public virtual void OnTouchpadDown(object sender, TouchpadManipilateEventArgs e)
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
        /// <param name="e">An <see cref="TouchpadManipilateEventArgs" /> that contains the event data. </param>
        public virtual void OnTouchpadUp(object sender, TouchpadManipilateEventArgs e)
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
        /// <param name="e">An <see cref="TouchpadManipilateEventArgs" /> that contains the event data. </param>
        public virtual void OnTouchpadTouch(object sender, TouchpadManipilateEventArgs e)
        {
            if (TouchpadTouch != null)
                TouchpadTouch(sender, e);
        }

        /// <summary>
        ///     Raises the TouchpadHold event.
        /// </summary>
        /// <param name="sender">
        ///     <see cref="InteractionSourceState" />
        /// </param>
        /// <param name="e">An <see cref="TouchpadManipilateEventArgs" /> that contains the event data. </param>
        public virtual void OnTouchpadHold(object sender, TouchpadManipilateEventArgs e)
        {
            if (TouchpadHold != null)
                TouchpadHold(sender, e);
        }

        #endregion


        public Cursor PointerCursor
        {
            get
            {
                var simpleSinglePointerSelector = FindObjectsOfType<SimpleSinglePointerSelector>();
                if (simpleSinglePointerSelector != null && simpleSinglePointerSelector.Length == 1)
                {
                    return simpleSinglePointerSelector[0].Cursor;
                }
                return null;
            }
        }

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
                Debug.Log("Event!");

                var handedness = obj.state.source.handedness;
                Vector3 pointerPosition;
                Vector3 controllerPosition;
                Quaternion pointerRotation;
                Quaternion controllerRotation;
                obj.state.sourcePose.TryGetPosition(out pointerPosition, InteractionSourceNode.Pointer);
                obj.state.sourcePose.TryGetRotation(out pointerRotation, InteractionSourceNode.Pointer);
                obj.state.sourcePose.TryGetPosition(out controllerPosition, InteractionSourceNode.Grip);
                obj.state.sourcePose.TryGetRotation(out controllerRotation, InteractionSourceNode.Grip);

                if (CameraCache.Main.transform.parent != null)
                {
                    pointerPosition = CameraCache.Main.transform.parent.TransformPoint(pointerPosition);
                    controllerPosition = CameraCache.Main.transform.parent.TransformPoint(controllerPosition);
                }

                if (PointerCursor != null)
                { 
                    SetLaserPointer(obj, handedness, controllerPosition, PointerCursor.Position);
                }
                if (obj.state.grasped && !motionControllerState.Grasped)
                {
                    Debug.Log("OnGrasped");
                    OnGrasped(obj.state, new EventArgs());
                }
                else if (!obj.state.grasped && motionControllerState.Grasped)
                {
                    Debug.Log("OnReleased");
                    OnReleased(obj.state, new EventArgs());
                }
                motionControllerState.Grasped = obj.state.grasped;


                if (obj.state.menuPressed && !motionControllerState.MenuPressed)
                {
                    Debug.Log("OnMenuDown");
                    OnMenuDown(obj.state, new EventArgs());
                }
                else if (!obj.state.menuPressed && motionControllerState.MenuPressed)
                {
                    Debug.Log("OnMenuUp");
                    OnMenuUp(obj.state, new EventArgs());
                }
                motionControllerState.MenuPressed = obj.state.menuPressed;


                var selectManipilateEventArgs = new SelectManipilateEventArgs(obj.state
                    , obj.state.selectPressed
                    , obj.state.selectPressedAmount
                    , handedness);
                if (obj.state.selectPressed && !motionControllerState.SelectPressed)
                {
                    Debug.Log("OnSelectDown");
                    OnSelectDown(obj.state, selectManipilateEventArgs);
                }
                else if (!obj.state.selectPressed && motionControllerState.SelectPressed)
                {
                    Debug.Log("OnSelectUp");
                    OnSelectUp(obj.state, selectManipilateEventArgs);
                }
                else if (obj.state.selectPressed && motionControllerState.SelectPressed)
                {
                    Debug.Log("OnSelectHold");
                    OnSelectHold(obj.state, selectManipilateEventArgs);
                }
                motionControllerState.SelectPressed = obj.state.selectPressed;

                var thumbstickManipilateEventArgs = new ThumbstickManipilateEventArgs(obj.state
                    , obj.state.thumbstickPressed
                    , obj.state.thumbstickPosition
                    , handedness);
                if (obj.state.thumbstickPressed && !motionControllerState.ThumbstickPressed)
                {
                    Debug.Log("OnThumbstickDown");
                    OnThumbstickDown(obj.state, thumbstickManipilateEventArgs);
                }
                else if (!obj.state.thumbstickPressed && motionControllerState.ThumbstickPressed)
                {
                    Debug.Log("OnThumbstickUp");
                    OnThumbstickUp(obj.state, thumbstickManipilateEventArgs);
                }
                else if (motionControllerState.ThumbstickPressed && obj.state.thumbstickPressed)
                {
                    Debug.Log("OnThumbstickHold");
                    OnThumbstickHold(obj.state, thumbstickManipilateEventArgs);
                }
                motionControllerState.ThumbstickPressed = obj.state.thumbstickPressed;


                var touchpadManipilateEventArgs = new TouchpadManipilateEventArgs(obj.state
                    , obj.state.touchpadPressed
                    , obj.state.touchpadTouched
                    , obj.state.touchpadPosition
                    , handedness);
                if (obj.state.touchpadPressed && !motionControllerState.TouchpadPressed)
                {
                    Debug.Log("OnTouchpadDown");
                    OnTouchpadDown(obj.state, touchpadManipilateEventArgs);
                }
                else if (!obj.state.touchpadPressed && motionControllerState.TouchpadPressed)
                {
                    Debug.Log("OnThumbstickHold");
                    OnTouchpadUp(obj.state, touchpadManipilateEventArgs);
                }
                else if (motionControllerState.TouchpadPressed && obj.state.touchpadPressed)
                {
                    Debug.Log("OnTouchpadHold");
                    OnTouchpadHold(obj.state, touchpadManipilateEventArgs);
                }
                motionControllerState.TouchpadPressed = obj.state.touchpadPressed;

                if (obj.state.touchpadTouched && !motionControllerState.TouchpadTouched)
                {
                    Debug.Log("OnTouchpadTouch");
                    OnTouchpadTouch(obj.state, touchpadManipilateEventArgs);
                }
                else if (!obj.state.touchpadTouched && motionControllerState.TouchpadTouched)
                {
                    Debug.Log("OnTouchpadUp");
                    OnTouchpadUp(obj.state, touchpadManipilateEventArgs);
                }
                else if (motionControllerState.TouchpadTouched && obj.state.touchpadTouched)
                {
                    Debug.Log("OnTouchpadHold");
                    OnTouchpadHold(obj.state, touchpadManipilateEventArgs);
                }
                motionControllerState.TouchpadTouched = obj.state.touchpadTouched;

                OnMotionControllerPositionChanged(obj.state, new MotionControllerPositionEventArgs(obj.state,
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
            var material = new Material(Shader.Find("Sprites/Default"));
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