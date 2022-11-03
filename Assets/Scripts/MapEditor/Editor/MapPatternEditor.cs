using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(Pattern))]
public class MapPatternEditor : Editor
{
    Pattern pattern;
    private float coeffSize = 0.2f;

    private void OnEnable()
    {
        pattern = target as Pattern;
    }

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        //Convert the weaponSprite (see SO script) to Texture
        //Texture2D texture = AssetPreview.GetAssetPreview(pattern.preview2D);

        Texture2D texture = (Texture2D)AssetDatabase.LoadAssetAtPath("Assets/Scripts/MapEditor/Data/preview/" + pattern.name + "preview.asset", typeof(Texture2D));

        if (texture == null)
        {
            Debug.LogWarning("No Image Preview");
            return;
        }
        //We crate empty space 80x80 (you may need to tweak it to scale better your sprite
        //This allows us to place the image JUST UNDER our default inspector
        GUILayout.Label("", GUILayout.Height(texture.height*coeffSize), GUILayout.Width(texture.width*coeffSize));
        //Draws the texture where we have defined our Label (empty space)
        GUI.DrawTexture(GUILayoutUtility.GetLastRect(), texture, ScaleMode.ScaleAndCrop);
    }
}