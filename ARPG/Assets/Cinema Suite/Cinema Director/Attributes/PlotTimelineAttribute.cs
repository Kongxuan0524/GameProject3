using UnityEngine;
using System.Collections;
using System;

namespace CinemaDirector
{
    public class PlotTimelineAttribute : Attribute
    {
        public string Category;
        public string Label;

        public PlotTimelineAttribute(string category, string label)
        {
            this.Category = category;
            this.Label    = label;
        }
    }
}

