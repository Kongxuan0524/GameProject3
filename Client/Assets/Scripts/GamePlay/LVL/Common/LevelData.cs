﻿using UnityEngine;
using System.Collections;

namespace LVL
{
    public class LevelData
    {
        public static int       SceneID  { get; set; }
        public static int       Chapter  { get; set; }
        public static int       CopyID   { get; set; }
        public static ECopyType CopyType { get; set; }
        public static float     StTime   { get; set; }
        public static float     EdTime   { get; set; }
        public static bool      Win      { get; set; }
        public static int       Star     { get; set; }
    }
}