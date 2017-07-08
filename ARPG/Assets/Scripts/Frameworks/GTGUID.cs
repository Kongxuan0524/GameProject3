using UnityEngine;
using System.Collections;

public class GTGUID
{
    static ulong GUID = 10000001;

    static GTGUID()
    {
        GUID = PlayerPrefs.GetString("GUID", "100000001").ToUInt64();
    }

    public static ulong NewGUID()
    {
        GUID++;
        PlayerPrefs.SetString("GUID", GUID.ToString());
        return GUID;
    }
}
