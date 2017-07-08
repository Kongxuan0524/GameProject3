using UnityEngine;
using System.Collections;

namespace CinemaDirector
{
    [PlotTimeline("Animation", "Stop")]
    public class PlotAnimationStop : CinemaActorEvent
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

                animation.Stop(Animation);
            }
        }

        public override void Reverse(GameObject actor)
        {

        }
    }
}