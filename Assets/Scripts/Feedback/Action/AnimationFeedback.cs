using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Hakim
{
    [Hakim.GameFeedback("Animation/ChangeAnimation")]
    public class AnimationFeedback : GameFeedback
    {
        public string animationName;
        public override IEnumerator Execute(GameEvent gameEvent, GameObject gameObject)
        {
            Player.instance.animator.SetTrigger(animationName);
            yield break;
        }

        public override string ToString()
        {
            return $"Animation :  { animationName}";
        }

        public override Color Coloration()
        {
            return Color.black;
        }
    }

}
