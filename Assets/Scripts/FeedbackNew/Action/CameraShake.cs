using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Hakim
{

    public class CameraShake : GameFeedback
    {
        public override IEnumerator Execute(GameEvent gameEvent)
        {
            yield break;
        }

        public override string ToString()
        {
            return $"Shake : ";
        }

        public override Color Coloration()
        {
            return Color.magenta;
        }


    }


}
