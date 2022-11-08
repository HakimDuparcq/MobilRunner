using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Hakim
{
    public class Test : MonoBehaviour
    {
        public GameObject soldier;
        public GameObject soldier2;
        void Start()
        {
            GameEventsManager.PlayEvent("NewAction", soldier);
            GameEventsManager.PlayEvent("NewAction", soldier2);

            //GameEventsManager.PlayEvent(GameEventsManager.instance.GameEvents[0], soldier2);

        }


    }

}