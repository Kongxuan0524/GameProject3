using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class GTGlobal
{
    public static int       CurPlayerID
    {
        get;
        set;
    }

    public static ulong     CurPlayerGUID
    {
        get;
        set;
    }

    public static int       CurSceneID
    {
        get;
        set;
    }

    public static bool      Along
    {
        get;
        set;
    }

    public static ELanguage Language
    {
        get;
        set;
    }

    public static float     TimeScale
    {
        get { return Time.timeScale; }
        set { Time.timeScale = value; }
    }

    public static string    LoadedLevelName
    {
        get { return SceneManager.GetActiveScene().name; }
    }

    public static int       LAST_CITY_ID
    {
        get { return GTSceneKey.SCENE_CITY_4; }
    }
}
