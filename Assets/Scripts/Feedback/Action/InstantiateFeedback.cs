using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Hakim
{
    [Hakim.GameFeedback("Spawn/Instanciate")]
    public class InstantiateFeedback : GameFeedback
    {
        public GameObject Prefab;
        public override IEnumerator Execute(GameEvent gameEvent, GameObject gameObject)
        {
            GameObject.Instantiate(Prefab, gameObject.transform.position, Quaternion.identity);
            yield break;
        }

        public override string ToString()
        {
            return $"Instantiate : {(Prefab == null ? "???" : Prefab.name )}";
        }

        public override Color Coloration()
        {
            return Color.magenta;
        }
    }
}
