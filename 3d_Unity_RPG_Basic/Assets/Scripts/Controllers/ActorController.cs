using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ActorController : MonoBehaviour
{
    protected CharacterController _characterController;
    protected Animator _anim;

    protected Defines.ActorStates _stateAction = Defines.ActorStates.Idle;
    protected Defines.ActorStates StateAction
    {
        get { return _stateAction; }
        set
        {
            _stateAction = value;
            _anim.SetInteger("ActionStateType", (int)_stateAction);
        }
    }

    protected float _moveSpeed;
    protected float _jumpVelocity;
    [Range(0.01f, 1.0f)] protected float _airControlPercent;

    protected float _speedSmoothTime = 0.1f;
    protected float _turnSmoothTime = 0.1f;

    protected float _speedSmoothVelocity;
    protected float _turnSmoothVelocity;

    protected float _currentVelocityY;

    protected float _currentSpeed =>
        new Vector2(_characterController.velocity.x, _characterController.velocity.z).magnitude;
}
