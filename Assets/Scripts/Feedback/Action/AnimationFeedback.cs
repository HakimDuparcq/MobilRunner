using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Hakim
{
    [Hakim.GameFeedback("Animation/ChangeAnimation")]
    public class AnimationFeedback : GameFeedback
    {
        public string animationName;
        public bool isEndGame;
        public override IEnumerator Execute(GameEvent gameEvent, GameObject gameObject)
        {
            if (isEndGame)
            {
                if (Mathf.Abs( Skin.instance.skinNumber ) >= Skin.instance.skinNumberToWin)
                {
                    Player.instance.animator.SetTrigger("Victory");
                    yield break;
                }
                else
                {
                    Player.instance.animator.SetTrigger("Defeat");
                    yield break;
                }
            }

            Player.instance.animator.SetTrigger(animationName);
            yield break;
        }

        public override string ToString()
        {
            if (isEndGame)
            {
                return $"Player Animation EndGame";
            }
            else
            {
                return $"Player Animation :  { animationName}";

            }
        }

        public override Color Coloration()
        {
            return Color.black;
        }
    }

}
