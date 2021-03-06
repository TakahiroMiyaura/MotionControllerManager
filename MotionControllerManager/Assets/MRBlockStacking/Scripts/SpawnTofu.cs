﻿// Copyright(c) 2018 Takahiro Miyaura
// Released under the MIT license
// http://opensource.org/licenses/mit-license.php

using System;
using System.Collections.Generic;
using Assets.MotionController.Scripts;
using HoloToolkit.Unity;
using UnityEngine;
using UnityEngine.XR.WSA.Input;

public class SpawnTofu : MonoBehaviour
{
    public GameObject Tofu;

    public int MaxCreateTofus;

    private Queue<GameObject> _genObject;

    // Use this for initialization
    private int _count;
    private GameObject _manipulateObj;
    private float _distance;

    private void Start()
    {
        _genObject = new Queue<GameObject>(MaxCreateTofus);
        MotionControllerManager.Instance.SelectHold += SelectHold;
        MotionControllerManager.Instance.SelectDown += SelectDown;
        MotionControllerManager.Instance.SelectUp += SelectUp;
    }

    private void Update()
    {
    }

    private void SelectUp(object sender, EventArgs e)
    {
        var state = (InteractionSourceState) sender;
        if (state.source.handedness == InteractionSourceHandedness.Right)
            if (_manipulateObj != null)
            {
                _manipulateObj.GetComponent<Rigidbody>().useGravity = true;
                _manipulateObj.GetComponent<Rigidbody>().isKinematic = false;
                _manipulateObj = null;
            }
    }

    private void SelectHold(object sender, SelectManipulateEventArgs e)
    {
        var state = (InteractionSourceState) sender;
        if (state.source.handedness == InteractionSourceHandedness.Left)
        {
            var f = 20 - state.selectPressedAmount * 10;
            if (_count > f)
            {
                _count = 0;
                GenerateTofu(sender);
            }

            _count++;
        }
        else if (state.source.handedness == InteractionSourceHandedness.Right)
        {
            Vector3 gripPosition;
            Vector3 cursorPosition;
            state.sourcePose.TryGetPosition(out gripPosition, InteractionSourceNode.Grip);
            state.sourcePose.TryGetPosition(out cursorPosition, InteractionSourceNode.Pointer);

            var forward = (cursorPosition - gripPosition).normalized;

            if (_manipulateObj == null)
            {
                RaycastHit hit;
                if (Physics.Raycast(gripPosition, forward, out hit, Mathf.Infinity))
                {
                    Debug.Log("Hit!!");
                    if (_genObject.Contains(hit.transform.gameObject))
                    {
                        _manipulateObj = hit.transform.gameObject;
                        _manipulateObj.GetComponent<Rigidbody>().useGravity = false;
                        _manipulateObj.GetComponent<Rigidbody>().isKinematic = true;
                        Debug.Log("WWWW" + gripPosition + ":" + hit.transform.position);
                        _distance = Vector3.Distance(gripPosition, hit.transform.position);
                    }
                }
            }
            else
            {
                var newPos = gripPosition + forward * _distance;
                _manipulateObj.transform.position = newPos;
            }
        }
    }

    private void SelectDown(object sender, EventArgs e)
    {
        var state = (InteractionSourceState) sender;
        if (state.source.handedness == InteractionSourceHandedness.Left) GenerateTofu(sender);
    }

    private void GenerateTofu(object sender)
    {
        if (_genObject.Count >= MaxCreateTofus)
        {
            var dequeue = _genObject.Dequeue();
            Destroy(dequeue);
        }

        var instantiate = Instantiate(Tofu);
        _genObject.Enqueue(instantiate);
        var rigid = instantiate.AddComponent<Rigidbody>();
        rigid.collisionDetectionMode = CollisionDetectionMode.ContinuousDynamic;
        var state = (InteractionSourceState) sender;

        Vector3 gripPosition;
        state.sourcePose.TryGetPosition(out gripPosition, InteractionSourceNode.Grip);
        if (CameraCache.Main.transform.parent != null)
            gripPosition = CameraCache.Main.transform.parent.TransformPoint(gripPosition);
        instantiate.transform.position = gripPosition;

        Vector3 forward;
        state.sourcePose.TryGetForward(out forward, InteractionSourceNode.Grip);

        rigid.velocity = forward * 2f;
    }

}