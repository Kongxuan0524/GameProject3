using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;

namespace CinemaDirector
{
    public class PlotTrackAttribute : Attribute
    {
        public string Label;
        public Type[] AllowTypes;
    }
}

