using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace LVL
{
    public class LevelUtil
    {
        public static string GetConfigPath(int mapID)
        {
            return string.Format("Text/Lvl/{0}", mapID);
        }
    }
}

