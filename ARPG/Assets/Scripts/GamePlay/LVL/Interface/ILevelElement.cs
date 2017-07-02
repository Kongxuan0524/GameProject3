using UnityEngine;
using System.Collections;

namespace LVL
{
    public interface ILevelElement
    {
        void Import(DCFG cfg);
        DCFG Export();
    }
}

