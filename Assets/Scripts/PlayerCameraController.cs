using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerCameraController : MonoBehaviour
{
    enum WatchPoints
    {
        left = 0,
        right = 2,
        mid = 1
    }
    [Header("PlayerCameraInfo")]
    [SerializeField] float standerdDis = 10;
    [SerializeField] float maxiDis = 12;
    [SerializeField] float minDis = 5;
    [SerializeField] float disChangeSpeed = 0.25f;
    [SerializeField] CinemachineVirtualCamera cinemachineVirtualCamera;
    CinemachineComponentBase componentBase;
    private float _cameraSide = 0.5f;
    private byte nowPointNum;

    private void Awake()
    {
        componentBase = cinemachineVirtualCamera.GetCinemachineComponent(CinemachineCore.Stage.Body);
        if (componentBase is Cinemachine3rdPersonFollow)
        {
            (componentBase as Cinemachine3rdPersonFollow).CameraDistance = standerdDis;
        }

        _cameraSide = (componentBase as Cinemachine3rdPersonFollow).CameraSide;

       // Debug.Log(WatchPoints.left + 1);
        switch (_cameraSide)
        {
            case 0.25f:
                nowPointNum = (byte)WatchPoints.left;
                break;
            case 0.5f:
                nowPointNum = (byte)WatchPoints.mid;
                break;
            case 0.75f:
                nowPointNum = (byte)WatchPoints.right;
                break;
            default:
                break;
        }
    }

    public void OnCamera(InputValue value)
    {
        var cameraChangeDis = value.Get<Vector2>().normalized;
        (componentBase as Cinemachine3rdPersonFollow).CameraDistance -= disChangeSpeed * cameraChangeDis.y;
        if ((componentBase as Cinemachine3rdPersonFollow).CameraDistance >= maxiDis)
        {
            (componentBase as Cinemachine3rdPersonFollow).CameraDistance = maxiDis;
        }
        else if ((componentBase as Cinemachine3rdPersonFollow).CameraDistance <= minDis)
        {
            (componentBase as Cinemachine3rdPersonFollow).CameraDistance = minDis;
        }

    }

    public void OnViewChanger(InputValue value)
    {

        if (value.isPressed)
        {
            nowPointNum += 1;
            if (nowPointNum > 2)
            {
                nowPointNum = 0;
            }
            switch (nowPointNum)
            {
                case 0:
                    (componentBase as Cinemachine3rdPersonFollow).CameraSide=0.25f;
                    break;
                case 1:
                    (componentBase as Cinemachine3rdPersonFollow).CameraSide=0.5f;
                    break;
                case 2:
                    (componentBase as Cinemachine3rdPersonFollow).CameraSide=0.75f;
                    break;
                default:
                    break;
            }
        }
    }
}