using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Hakim
{
    [Hakim.GameFeedback("CameraShake")]
    public class CameraShake : GameFeedback
    {
        public override IEnumerator Execute(GameEvent gameEvent, GameObject gameObject)
        {
            yield break;
        }

        public override string ToString()
        {
            return $"Shake : ";
        }

        public override Color Coloration()
        {
            return Color.blue;
        }


    }


}
