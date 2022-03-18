using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField]
    private Transform _centralAxis;
    [SerializeField]
    private Transform _followTarget;

    private float _camSpeed;
    private float _wheelSpeed;

    private float _rotAxisX;
    private float _rotAxisY;

    private float _followDistX;
    private float _followDistY;

    void Start()
    {
        Init();
    }

    void LateUpdate()
    {
        CamZoom();
        CamRotate();
        CamMove();
    }

    void Init()
    {

        _camSpeed   = 5.0f;
        _wheelSpeed = 2.0f;
        _followDistX = 0.8f;
        _followDistY = 1.3f;
    }

    void CamZoom()
    {
        transform.localPosition = new Vector3(0, 0, Managers.Input.MouseWheelValue * _wheelSpeed);

    }
    void CamRotate()
    {
        _rotAxisX = _centralAxis.rotation.x + Managers.Input.MouseRotInputY;
        _rotAxisY = _centralAxis.rotation.y + Managers.Input.MouseRotInputX;

        _centralAxis.rotation = Quaternion.Euler(new Vector3(_rotAxisX, _rotAxisY, 0) * _camSpeed);
     }

    void CamMove()
    {
        _centralAxis.position = _followTarget.position + (Vector3.up * _followDistY) + (_followTarget.transform.right * _followDistX); ;
    }
}
