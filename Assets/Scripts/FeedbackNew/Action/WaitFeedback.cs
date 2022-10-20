using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Hakim
{
    [Hakim.GameFeedback("Wait/WaitFeedback")]  //0, 59, 210, 
    public class WaitFeedback : GameFeedback
    {
        public float Timer;

        public override IEnumerator Execute(GameEvent gameEvent)
        {
            yield return new WaitForSeconds(Timer);
        }

        public override string ToString()
        {
            return $"Wait : { Timer}  s";
        }

        public override Color Coloration()
        {
            return Color.black;
        }
    }

}
