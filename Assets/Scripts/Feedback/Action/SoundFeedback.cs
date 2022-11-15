using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Hakim
{
    [Hakim.GameFeedback("Audio/PlaySound")]
    public class SoundFeedback : GameFeedback
    {
        public string soundName;
        public override IEnumerator Execute(GameEvent gameEvent, GameObject gameObject)
        {
            if (soundName != ""){ AudioManager.instance.Play(soundName); }
            else{
                if (Mathf.Abs(Skin.instance.skinNumber) >= Skin.instance.skinNumberToWin) { AudioManager.instance.Play("Win");}
                else{
                    AudioManager.instance.Play("Loose");}
            }
            yield break;
        }

        public override string ToString()
        {
            return $"Play Sound : {soundName}";
        }

        public override Color Coloration()
        {
            return Color.magenta;
        }
    }
}
