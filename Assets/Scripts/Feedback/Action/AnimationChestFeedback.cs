using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Hakim
{
    [Hakim.GameFeedback("Animation/ChangeChestAnimation")]
    public class AnimationChestFeedback : GameFeedback
    {
        public string animationName;
        public override IEnumerator Execute(GameEvent gameEvent, GameObject gameObject)
        {
            GameObject chest = GameObject.FindGameObjectsWithTag("Chest")[0];
            if (animationName=="Surprise")
            {
                chest.GetComponent<Animator>().SetTrigger(animationName);

            }
            else
            {
                if (Mathf.Abs(Skin.instance.skinNumber) >= Skin.instance.skinNumberToWin)
                {
                    chest.GetComponent<Animator>().SetTrigger("Open");
                }
            }

            
            yield break;
        }

        public override string ToString()
        {
            return $"Chest Animation :  { animationName}";
        }

        public override Color Coloration()
        {
            return Color.black;
        }
    }

}
