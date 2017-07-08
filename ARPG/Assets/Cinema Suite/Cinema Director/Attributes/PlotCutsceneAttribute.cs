using UnityEngine;
using System.Collections;
using System;

namespace CinemaDirector
{
    public class PlotCutsceneAttribute : Attribute
    {
        public Type[] AllowTypes;
    }
}

