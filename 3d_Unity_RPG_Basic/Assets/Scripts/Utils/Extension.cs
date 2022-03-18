using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Extension // 확장 메서드 ( Object의 메서드 처럼 사용할 수 있도록 구현 )
{
   public static T GetOrAddComponent<T>(this GameObject go) where T : UnityEngine.Component
    {
        return Utils.GetOrAddComponent<T>(go);
    }

   /*public static void BindEvent(this GameObject go, Action<PointerEventData> action, Defines.UIEvent type = Defines.UIEvent.Click)
    {
        UI_Base.BindEvent(go, action, type);
    }*/
}
