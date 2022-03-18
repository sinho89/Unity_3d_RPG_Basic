using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : ActorController
{
    private Camera _followCam;
    private void Start()
    {
        Init();
    }

    private void Update()
    {
        UpdateAnimation();
        Moving();
    }

    private void Init()
    {
        Managers.Input._keyAction -= OnKeyBoard;
        Managers.Input._keyAction += OnKeyBoard; // [Input by Keyboard] OnKeyBoard()

        Managers.Input._mouseAction -= OnMouseKey;
        Managers.Input._mouseAction += OnMouseKey;

        _followCam = Camera.main;
        _characterController = GetComponent<CharacterController>();
        _anim = GetComponent<Animator>();

        _moveSpeed = 10.0f;
        _jumpVelocity = 5.0f;
    }

    private void OnKeyBoard(Defines.KeyboardEvents evt) // Input by Keyboard
    {
        switch (evt)
        {
            case Defines.KeyboardEvents.MoveButtonDown:
                StartMove();
                break;
            case Defines.KeyboardEvents.MoveButtonUp:
                EndMove();
                break;
            case Defines.KeyboardEvents.MoveButtonPressing:
                break;
            case Defines.KeyboardEvents.JumpButtonDown:
                StartJump();
                break;
        }
    }

    private void OnMouseKey(Defines.MouseEvents evt) // Input by Mouse
    {
        switch (evt)
        {
            case Defines.MouseEvents.LeftPointerDown:
                break;
        }
    }


    private void StartMove()
    {
    }


    private void Moving()
    {

        float speed = _moveSpeed * Managers.Input.MoveInput.magnitude;

        Vector3 camForwardDir = new Vector3(_followCam.transform.forward.x, 0, _followCam.transform.forward.z); 

        Vector3 moveDir = Vector3.Normalize(camForwardDir * Managers.Input.MoveInput.y
                            + _followCam.transform.right * Managers.Input.MoveInput.x);

        float smoothTime = _characterController.isGrounded ? _speedSmoothTime : _speedSmoothTime / _airControlPercent;
        speed = Mathf.SmoothDamp(_currentSpeed, speed, ref _speedSmoothVelocity, smoothTime);
        _currentVelocityY += Time.deltaTime * Physics.gravity.y;

        Vector3 velocity = moveDir * speed + Vector3.up * _currentVelocityY;
        _characterController.Move(velocity * Time.deltaTime);

        // 카메라 회전값 적용
        transform.rotation = Quaternion.Lerp(transform.rotation,
        Quaternion.LookRotation(new Vector3(_followCam.transform.forward.x, 0, _followCam.transform.forward.z)), Time.deltaTime * 10.0f);


        if (_characterController.isGrounded)
        {
            _currentVelocityY = 0;

            if(StateAction == Defines.ActorStates.Jump)
                StateAction = Defines.ActorStates.Idle;
        }
    }

    private void EndMove()
    {
    }

    private void StartJump()
    {
        StateAction = Defines.ActorStates.Jump;
        Jump();
    }

    private void Jump()
    {
        if (!_characterController.isGrounded)
            return;

        _currentVelocityY = _jumpVelocity;
    }

    private void UpdateAnimation()
    {
        _anim.SetFloat("InputX", Managers.Input.MoveInput.x);
        _anim.SetFloat("InputY", Managers.Input.MoveInput.y);

        switch (_stateAction)
        {
            case Defines.ActorStates.Idle:
                UpdateIdle();
                break;
            case Defines.ActorStates.Move:
                UpdateMove();
                break;
            case Defines.ActorStates.Jump:
                UpdateJump();
                break;
            case Defines.ActorStates.Attack:
                break;
            case Defines.ActorStates.Die:
                break;
        }
    }

    private void UpdateIdle()
    {
        // Idle 애니메이션 4가지 일정시간지난후 랜덤기준으로 뿌려줌
    }
    private void UpdateMove()
    {
    }

    private void UpdateJump()
    {

    }
}
