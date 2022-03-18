using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Defines
{
    public enum Scenes
    {
        Unknown,
        Login,
        Game
    }

    public enum ActorStates
    {
        Idle = 0,
        Move,
        Attack,
        Die,
        Jump,
    }

    public enum KeyboardEvents
    {
        MoveButtonDown = 0,
        MoveButtonUp,
        MoveButtonPressing,
        JumpButtonDown,
        JumpButtonUp,
        JumpButtonPressing,
    }

    public enum MouseEvents
    {
        LeftPress = 0,
        LeftPointerDown,
        LeftPonterUp,
        LeftClick,
        RightPress,
        RightPointerDown,
        RightPonterUp,
        RightClick,
        BeginDrag,
        Drag
    }

    public enum Sounds
    {
        Bgm,
        Effect,
        MaxCount,
    }

    public enum UIEvents
    {
        Click,
        Drag,
    }
}
