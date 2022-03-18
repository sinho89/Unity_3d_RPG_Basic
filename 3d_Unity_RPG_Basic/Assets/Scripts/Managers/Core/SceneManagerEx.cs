using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneManagerEx
{
    public BaseScene CurrentScene { get { return GameObject.FindObjectOfType<BaseScene>(); } }

    public void LoadScene(Defines.Scenes type)
    {
        Managers.Clear();

        SceneManager.LoadScene(GetSceneName(type));
    }

    string GetSceneName(Defines.Scenes type)
    {
        string name = System.Enum.GetName(typeof(Defines.Scenes), type);
        return name;
    }

    public void Clear()
    {
        CurrentScene.Clear();
    }
}
