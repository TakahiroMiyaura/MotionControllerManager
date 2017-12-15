using System;
using System.Collections;
using System.Collections.Generic;
using Assets.MotionControl.Scripts;
using UnityEngine;
using UnityEngine.XR.WSA.Input;

public class MotionControllerTest : MonoBehaviour
{

    public TextMesh text;

    public GameObject GameObjectTouchpadTouch;
    public GameObject GameObjectGraspedPress;
    public GameObject GameObjectGraspedRelease;
    public GameObject GameObjectMenuPress;
    public GameObject GameObjectMenuRelease;
    public GameObject GameObjectSelectManupilate;
    public GameObject GameObjectSelectPress;
    public GameObject GameObjectSelectRelease;
    public GameObject GameObjectThumbstickManupilate;
    public GameObject GameObjectThumbstickPress;
    public GameObject GameObjectThumbstickRelease;
    public GameObject GameObjectTouchpadManupilate;
    public GameObject GameObjectTouchpadRelease;
    public GameObject GameObjectTouchpadPress;
    private Renderer _materialGameObjectTouchpadPress;
    private Renderer _materialGameObjectTouchpadRelease;
    private Renderer _materialGameObjectTouchpadManupilate;
    private Renderer _materialGameObjectThumbstickRelease;
    private Renderer _materialGameObjectThumbstickPress;
    private Renderer _materialGameObjectThumbstickManupilate;
    private Renderer _materialGameObjectSelectRelease;
    private Renderer _materialGameObjectSelectPress;
    private Renderer _materialGameObjectSelectManupilate;
    private Renderer _materialGameObjectMenuRelease;
    private Renderer _materialGameObjectMenuPress;
    private Renderer _materialGameObjectGraspedRelease;
    private Renderer _materialGameObjectGraspedPress;
    private Renderer _materialGameObjectTouchpadTouch;
    
    // Use this for initialization
    void Start () {

        _materialGameObjectTouchpadPress = GameObjectTouchpadPress.GetComponent<Renderer>();
        _materialGameObjectTouchpadRelease = GameObjectTouchpadRelease.GetComponent<Renderer>();
        _materialGameObjectTouchpadManupilate = GameObjectTouchpadManupilate.GetComponent<Renderer>();
        _materialGameObjectThumbstickRelease = GameObjectThumbstickRelease.GetComponent<Renderer>();
        _materialGameObjectThumbstickPress = GameObjectThumbstickPress.GetComponent<Renderer>();
        _materialGameObjectThumbstickManupilate = GameObjectThumbstickManupilate.GetComponent<Renderer>();
        _materialGameObjectSelectRelease = GameObjectSelectRelease.GetComponent<Renderer>();
        _materialGameObjectSelectPress = GameObjectSelectPress.GetComponent<Renderer>();
        _materialGameObjectSelectManupilate = GameObjectSelectManupilate.GetComponent<Renderer>();
        _materialGameObjectMenuRelease = GameObjectMenuRelease.GetComponent<Renderer>();
        _materialGameObjectMenuPress = GameObjectMenuPress.GetComponent<Renderer>();
        _materialGameObjectGraspedRelease = GameObjectGraspedRelease.GetComponent<Renderer>();
        _materialGameObjectGraspedPress = GameObjectGraspedPress.GetComponent<Renderer>();
        _materialGameObjectTouchpadTouch = GameObjectTouchpadTouch.GetComponent<Renderer>();

        MotionControllerManager.Instance.TouchpadTouch += TouchpadTouch;
        MotionControllerManager.Instance.Grasped += GraspedPress;
        MotionControllerManager.Instance.Released+= GraspedRelease;
        MotionControllerManager.Instance.MenuDown+= MenuPress;
        MotionControllerManager.Instance.MenuUp+= MenuRelease;
        MotionControllerManager.Instance.SelectHold+= SelectManupilate;
        MotionControllerManager.Instance.SelectDown+= SelectPress;
        MotionControllerManager.Instance.SelectUp+= SelectRelease;
        MotionControllerManager.Instance.ThumbstickHold+= ThumbstickManupilate;
        MotionControllerManager.Instance.ThumbstickDown+= ThumbstickPress;
        MotionControllerManager.Instance.ThumbstickUp+= ThumbstickRelease;
        MotionControllerManager.Instance.TouchpadHold+= TouchpadManupilate;
        MotionControllerManager.Instance.TouchpadUp+= TouchpadRelease;
        MotionControllerManager.Instance.TouchpadDown+= TouchpadPress;
        var material = new Material(Shader.Find("Diffuse"));
        material.color = Color.red;
        _materialGameObjectTouchpadTouch.material = material;
        material = new Material(Shader.Find("Diffuse"));
        material.color = Color.red;
        _materialGameObjectGraspedPress.material = material;
        material = new Material(Shader.Find("Diffuse"));
        material.color = Color.red;
        _materialGameObjectGraspedRelease.material = material;
        material = new Material(Shader.Find("Diffuse"));
        material.color = Color.red;
        _materialGameObjectMenuPress.material = material;
        material = new Material(Shader.Find("Diffuse"));
        material.color = Color.red;
        _materialGameObjectMenuRelease.material = material;
        material = new Material(Shader.Find("Diffuse"));
        material.color = Color.red;
        _materialGameObjectSelectManupilate.material = material;
        material = new Material(Shader.Find("Diffuse"));
        material.color = Color.red;
        _materialGameObjectSelectPress.material = material;
        material = new Material(Shader.Find("Diffuse"));
        material.color = Color.red;
        _materialGameObjectSelectRelease.material = material;
        material = new Material(Shader.Find("Diffuse"));
        material.color = Color.red;
        _materialGameObjectThumbstickManupilate.material = material;
        material = new Material(Shader.Find("Diffuse"));
        material.color = Color.red;
        _materialGameObjectThumbstickPress.material = material;
        material = new Material(Shader.Find("Diffuse"));
        material.color = Color.red;
        _materialGameObjectThumbstickRelease.material = material;
        material = new Material(Shader.Find("Diffuse"));
        material.color = Color.red;
        _materialGameObjectTouchpadManupilate.material = material;
        material = new Material(Shader.Find("Diffuse"));
        material.color = Color.red;
        _materialGameObjectTouchpadRelease.material = material;
        material = new Material(Shader.Find("Diffuse"));
        material.color = Color.red;
        _materialGameObjectTouchpadPress.material = material;
    }

    private void TouchpadPress(object sender, EventArgs eventArgs)
    {
        _materialGameObjectTouchpadPress.material.color = Color.blue;
        _materialGameObjectTouchpadRelease.material.color = Color.red;
        var state = (InteractionSourceState)sender;
        text.text = "Handedness:" + state.source.handedness + " - TouchpadDown";
    }

    private void TouchpadRelease(object sender, EventArgs eventArgs)
    {

        _materialGameObjectTouchpadPress.material.color = Color.red;
        _materialGameObjectTouchpadManupilate.material.color = Color.red;
        _materialGameObjectTouchpadRelease.material.color = Color.blue;
        _materialGameObjectTouchpadTouch.material.color = Color.red;
        var state = (InteractionSourceState)sender;
        text.text = "Handedness:" + state.source.handedness + " - TouchpadUp";
    }

    private void TouchpadManupilate(object sender, EventArgs eventArgs)
    {
        _materialGameObjectTouchpadManupilate.material.color = Color.blue;
        var state = (InteractionSourceState)sender;
        text.text = "Handedness:" + state.source.handedness + " - TouchpadHold";
    }

    private void ThumbstickRelease(object sender, EventArgs eventArgs)
    {
        _materialGameObjectThumbstickPress.material.color = Color.red;
        _materialGameObjectThumbstickManupilate.material.color = Color.red;
        _materialGameObjectThumbstickRelease.material.color = Color.blue;
        var state = (InteractionSourceState)sender;
        text.text = "Handedness:" + state.source.handedness + " - ThumbstickUp";
    }
    
    private void ThumbstickPress(object sender, EventArgs eventArgs)
    {
        _materialGameObjectThumbstickPress.material.color = Color.blue;
        _materialGameObjectThumbstickRelease.material.color = Color.red;
        var state = (InteractionSourceState) sender;
        text.text = "Handedness:" + state.source.handedness + " - ThumbstickDown";
    }

    private void ThumbstickManupilate(object sender, EventArgs eventArgs)
    {
        _materialGameObjectThumbstickManupilate.material.color = Color.blue;
        var state = (InteractionSourceState) sender;
        text.text = "Handedness:" + state.source.handedness + " - ThumbstickHold";
    }

    private void SelectRelease(object sender, EventArgs eventArgs)
    {
        _materialGameObjectSelectPress.material.color = Color.red;
        _materialGameObjectSelectManupilate.material.color = Color.red;
        _materialGameObjectSelectRelease.material.color = Color.blue;
        var state = (InteractionSourceState) sender;
        text.text = "Handedness:" + state.source.handedness + " - SelectUp";
    }

    private void SelectPress(object sender, EventArgs eventArgs)
    {
        _materialGameObjectSelectRelease.material.color = Color.red;
        _materialGameObjectSelectPress.material.color = Color.blue;
        var state = (InteractionSourceState) sender;
        text.text = "Handedness:" + state.source.handedness + " - SelectDown";
    }

    private void SelectManupilate(object sender, EventArgs eventArgs)
    {
        _materialGameObjectSelectManupilate.material.color = Color.blue;
        var state = (InteractionSourceState) sender;
        text.text = "Handedness:" + state.source.handedness + " - SelectHold";
    }

    private void MenuRelease(object sender, EventArgs eventArgs)
    {
        _materialGameObjectMenuRelease.material.color = Color.blue;
        _materialGameObjectMenuPress.material.color = Color.red;
        var state = (InteractionSourceState) sender;
        text.text = "Handedness:" + state.source.handedness + " - MenuUp";
    }

    private void MenuPress(object sender, EventArgs eventArgs)
    {
        _materialGameObjectMenuRelease.material.color = Color.red;
        _materialGameObjectMenuPress.material.color = Color.blue;
        var state = (InteractionSourceState) sender;
        text.text = "Handedness:" + state.source.handedness + " - MenuDown";
    }

    private void GraspedRelease(object sender, EventArgs eventArgs)
    {
        _materialGameObjectGraspedRelease.material.color = Color.blue;
        _materialGameObjectGraspedPress.material.color = Color.red;
        var state = (InteractionSourceState) sender;
        text.text = "Handedness:" + state.source.handedness + " - Released";
    }

    private void GraspedPress(object sender, EventArgs eventArgs)
    {
        _materialGameObjectGraspedRelease.material.color = Color.red;
        _materialGameObjectGraspedPress.material.color = Color.blue;
        var state = (InteractionSourceState) sender;
        text.text = "Handedness:" + state.source.handedness + " - Grasped";
    }

    private void TouchpadTouch(object sender, EventArgs eventArgs)
    {
        _materialGameObjectTouchpadTouch.material.color = Color.blue;
        var state = (InteractionSourceState) sender;
        text.text = "Handedness:" + state.source.handedness + " - TouchpadTouch";
    }

    // Update is called once per frame
	void Update () {
		
	}
}
