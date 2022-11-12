using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Hakim
{
    [Hakim.GameFeedback("Hud/Animation")] 
    public class HudAnimator : GameFeedback
    {
        public override IEnumerator Execute(GameEvent gameEvent, GameObject gameObject)
        {
            UiManager.instance.Game.SetActive(false);
            UiManager.instance.ScoreView.SetActive(true);
            yield break;
        }

        public override string ToString()
        {
            return $"UI : Score";
        }

        public override Color Coloration()
        {
            return Color.black;
        }
    }

}
