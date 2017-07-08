using UnityEngine;
using System.Collections;

namespace CinemaDirector
{
    [PlotTimeline("Animation", "Rewind")]
    public class PlotAnimationRewind : CinemaActorEvent
    {
        public string Animation = string.Empty;

        public override void Trigger(GameObject actor)
        {
            if (actor != null)
            {
                Animation animation = actor.GetComponent<Animation>();
                if (!animation)
                {
                    return;
                }

                animation.Rewind(Animation);
            }
        }

        public override void Reverse(GameObject actor)
        {
        }
    }
}