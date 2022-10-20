using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Hakim
{
    [System.Serializable]
    public class GameFeedback 
    {

        public virtual IEnumerator Execute(GameEvent gameEvent)
        {
            yield break;
        }

        public virtual Color Coloration()
        {
            return Color.white;
        }
    }
}
