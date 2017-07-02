using UnityEngine;
using System.Collections;

namespace LVL
{
    public class LevelGroupBase : LevelElement
    {
        public void DelAllElements()
        {
            NGUITools.DestroyChildren(transform);
        }
    }
}