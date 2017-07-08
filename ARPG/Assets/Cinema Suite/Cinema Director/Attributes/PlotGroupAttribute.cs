using UnityEngine;
using System.Collections;
using System;

namespace CinemaDirector
{
    public class PlotGroupAttribute : Attribute
    {
        public string Label;
        public Type[] AllowTypes;
    }
}

