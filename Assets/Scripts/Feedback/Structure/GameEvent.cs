using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;




namespace Hakim
{
    [CreateAssetMenu(menuName = "GameEvent")]
    [Serializable]
    public class GameEvent : ScriptableObject
    {
        [SerializeReference] public string Name;

        public GameObject GameObject;

        [SerializeReference] public List<GameFeedback> Feedbacks = new List<GameFeedback>();

    
        public List<Color> ColorsType = new List<Color>()
                                             {Color.white, Color.black, Color.blue  };


        [SerializeReference] public List<Color> ColorFeedbacks = new List<Color>();


        public void OnEnable()
        {
            Name = this.name;
        }


        public IEnumerator Execute(GameObject gameObject)
        {
            foreach (GameFeedback item in Feedbacks)
            {
                yield return item.Execute(this, gameObject);    
            }
        }


    }
}
