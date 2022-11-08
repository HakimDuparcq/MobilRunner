using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Hakim
{
    public class GameEventsManager : MonoBehaviour
    {
        public static GameEventsManager instance;

        public List<GameEvent> GameEvents;

        static Dictionary<string, GameEvent> _events;

        private void Awake()
        {
            instance = this;
            _events = new Dictionary<string, GameEvent>(GameEvents.Count);
            foreach (var gameEvent in GameEvents)
            {
                _events.Add(gameEvent.Name, gameEvent);
            }
        }

        public static void PlayEvent(string eventName, GameObject gameObject)
        {
            instance.StartCoroutine( _events[eventName].Execute(gameObject) ); 
        }

        public static void PlayEvent(GameEvent _event, GameObject gameObject)
        {
            instance.StartCoroutine(_event.Execute(gameObject));

        }
    }
}
