using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Hakim
{
    [Hakim.GameFeedback("Spawn/ParticleEndGame")]
    public class SpawnParticleFeedback : GameFeedback
    {
        public override IEnumerator Execute(GameEvent gameEvent, GameObject gameObject)
        {
            GameManager.instance.SpawnParticleEvent();
            yield break;
        }


        

        public override string ToString()
        {
           return $"Spawn Particle End Game ";
        }

        public override Color Coloration()
        {
            return Color.black;
        }
    }

}
