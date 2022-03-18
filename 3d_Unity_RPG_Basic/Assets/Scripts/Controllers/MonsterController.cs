using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterController : ActorController
{
    [SerializeField] //테스트용 타겟
    private Transform _target;

    private bool _IsSleeping = true;

    void Start()
    {
        Init();
    }

    void Update()
    {
        if (_IsSleeping)
            AwakeState();

        Move();
    }

    void Init()
    {
        _characterController = GetComponent<CharacterController>();
        _anim = GetComponent<Animator>();
        _anim.SetBool("Sleep", true);

        _moveSpeed = 5.0f;
        _jumpVelocity = 0.0f;
    }

    void Move()
    {
        float speed = _moveSpeed;
        Vector3 moveDist = _target.position - transform.position;
        Vector3 moveDir = Vector3.Normalize(moveDist);

        float smoothTime = _characterController.isGrounded ? _speedSmoothTime : _speedSmoothTime / _airControlPercent;
        speed = Mathf.SmoothDamp(_currentSpeed, speed, ref _speedSmoothVelocity, smoothTime);
        _currentVelocityY += Time.deltaTime * Physics.gravity.y;

        if (_IsSleeping)
        {
            Vector3 velocity = Vector3.up * _currentVelocityY;
            _characterController.Move(velocity * Time.deltaTime);
        }
        else
        {
            if (moveDist.magnitude > 1.0f)
            {
                Vector3 velocity = moveDir * speed + Vector3.up * _currentVelocityY;
                _characterController.Move(velocity * Time.deltaTime);
            }

            transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(moveDir), Time.deltaTime * 10.0f);
        }
    }

    void AwakeState()
    {
        Vector3 moveDist = _target.position - transform.position;

        if (moveDist.magnitude < 7.0f)
        {
            _IsSleeping = false;
            _anim.SetBool("Sleep", false);
        }
    }

    public void IdleStart()
    {

    }
}
