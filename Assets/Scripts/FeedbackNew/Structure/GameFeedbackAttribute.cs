using System;
using UnityEngine;

namespace Hakim
{
    [AttributeUsage(AttributeTargets.Class, Inherited = false)]
    public class GameFeedbackAttribute : Attribute
    {
       // public Color Color => color;
        public string MenuName => menuName;

       // private Color color;
        private string menuName;

        public GameFeedbackAttribute(string menuName = "Skip") //int r, int g, int b, string menuName = "Skip")
        {
            //color = new Color(r / 255f, g / 255f, b / 255f);
            this.menuName = menuName;
        }


        /*public GameFeedbackAttribute()
        {
            color = Color.white;
            menuName = "Skip";
        }*/
    }
}