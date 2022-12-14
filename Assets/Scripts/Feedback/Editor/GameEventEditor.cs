using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEditorInternal;
using UnityEngine;
using System.Reflection;


namespace Hakim
{
    [CanEditMultipleObjects]
    [CustomEditor(typeof(GameEvent), true)]
    public class GameEventEditor : Editor
    {
        #region Private Attributes

        private static readonly Color ProSkinTextColor = new Color(0.8f, 0.8f, 0.8f, 1.0f);
        private static readonly Color PersonalSkinTextColor = new Color(0.2f, 0.2f, 0.2f, 1.0f);

        private static readonly Color ProSkinSelectionBgColor = new Color(44.0f / 255.0f, 93.0f / 255.0f, 135.0f / 255.0f, 1.0f);
        private static readonly Color PersonalSkinSelectionBgColor = new Color(58.0f / 255.0f, 114.0f / 255.0f, 176.0f / 255.0f, 1.0f);

        private const float AdditionalSpaceMultiplier = 1.0f;

        private const float HeightHeader = 20.0f;
        private const float MarginReorderIcon = 20.0f;
        private const float ShrinkHeaderWidth = 15.0f;
        private const float XShiftHeaders = 15.0f;

        private GUIStyle headersStyle;

        private ReorderableList reordList;
        SerializedProperty _colors;


        SerializedProperty _name;
        Type[] _types;
        string[] _typesStr;
        private static Dictionary<string, GameFeedbackAttribute> typesAttributes;

        GameEvent gameEvent;
         static List<System.Type> gameEventTypes;
        #endregion






        #region Editor Methods


        [InitializeOnLoadMethod]
        private static void OnLoadGetTypes()
        {
            gameEventTypes = (from domainAssembly in System.AppDomain.CurrentDomain.GetAssemblies()
                                                from assemblyType in domainAssembly.GetTypes()
                                                where assemblyType.IsSubclassOf(typeof(GameFeedback))
                                                select assemblyType).ToList();


            typesAttributes = new Dictionary<string, GameFeedbackAttribute>();

            foreach (Type type in gameEventTypes)
            {
                if (Attribute.GetCustomAttribute(type, typeof(GameFeedbackAttribute)) is GameFeedbackAttribute attribute)
                    typesAttributes.Add(type.Name, attribute);
                else typesAttributes.Add(type.Name, new GameFeedbackAttribute(type.Name));
            }
        }

        private void OnEnable()
        {
            headersStyle = new GUIStyle();
            headersStyle.alignment = TextAnchor.MiddleLeft;
            headersStyle.normal.textColor = EditorGUIUtility.isProSkin ? ProSkinTextColor : PersonalSkinTextColor;
            headersStyle.fontStyle = FontStyle.Bold;

            reordList = new ReorderableList(serializedObject, serializedObject.FindProperty("Feedbacks"), true, true, true, true);
            reordList.drawHeaderCallback += OnDrawReorderListHeader;
            reordList.drawElementCallback += OnDrawReorderListElement;
            reordList.drawElementBackgroundCallback += OnDrawReorderListBg;
            reordList.elementHeightCallback += OnReorderListElementHeight;
            reordList.onAddDropdownCallback += OnReorderListAddDropdown;

            _name = serializedObject.FindProperty("Name");

            gameEvent = target as GameEvent;


        }

        private void OnDisable()
        {
            reordList.drawElementCallback -= OnDrawReorderListElement;
            reordList.elementHeightCallback -= OnReorderListElementHeight;
            reordList.drawElementBackgroundCallback -= OnDrawReorderListBg;
            reordList.drawHeaderCallback -= OnDrawReorderListHeader;
            reordList.onAddDropdownCallback -= OnReorderListAddDropdown;
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();

            //EditorGUILayout.PropertyField(_name);

            reordList.DoLayoutList();

            serializedObject.ApplyModifiedProperties();
        }

        #endregion

        #region ReorderableList Callbacks

        private void OnDrawReorderListHeader(Rect rect)
        {
            EditorGUI.LabelField(rect, "Actions settings");
        }

        private void OnDrawReorderListElement(Rect rect, int index, bool isActive, bool isFocused)
        {
            int length = reordList.serializedProperty.arraySize;

            if (length <= 0)
                return;

            SerializedProperty iteratorProp = reordList.serializedProperty.GetArrayElementAtIndex(index);

            //SerializedProperty actionTypeParentProp = iteratorProp.FindPropertyRelative("Feedbacks");
            //string actionName = actionTypeParentProp.enumDisplayNames[actionTypeParentProp.enumValueIndex];
            string actionName = gameEvent.Feedbacks[index].ToString();
            Rect labelfoldRect = rect;
            labelfoldRect.height = HeightHeader;
            labelfoldRect.x += XShiftHeaders;
            labelfoldRect.width -= ShrinkHeaderWidth;

            iteratorProp.isExpanded = EditorGUI.BeginFoldoutHeaderGroup(labelfoldRect, iteratorProp.isExpanded, actionName);

            Rect leftColor = rect;
            leftColor.xMin = rect.xMax - 12;
            leftColor.height = HeightHeader-2;
            EditorGUI.DrawRect(leftColor, gameEvent.Feedbacks[index].Coloration());

            if (iteratorProp.isExpanded)
            {
                ++EditorGUI.indentLevel;

                SerializedProperty endProp = iteratorProp.GetEndProperty();

                int i = 0;
                while (iteratorProp.NextVisible(true) && !EqualContents(endProp, iteratorProp))
                {
                    float multiplier = i == 0 ? AdditionalSpaceMultiplier : 1.0f;
                    rect.y += GetDefaultSpaceBetweenElements() * multiplier;
                    rect.height = EditorGUIUtility.singleLineHeight;

                    EditorGUI.PropertyField(rect, iteratorProp, true);


                    ++i;
                }

                --EditorGUI.indentLevel;
            }

            EditorGUI.EndFoldoutHeaderGroup();
        }

        private void OnDrawReorderListBg(Rect rect, int index, bool isActive, bool isFocused)
        {
            if (!isFocused || !isActive)
                return;

            float height = OnReorderListElementHeight(index);

            SerializedProperty prop = reordList.serializedProperty.GetArrayElementAtIndex(index);

            // remove a bit of the line that goes beyond the header label
            if (!prop.isExpanded)
                height -= EditorGUIUtility.standardVerticalSpacing;

            Rect copyRect = rect;
            copyRect.width = MarginReorderIcon;
            copyRect.height = height;

            // draw two rects indepently to avoid overlapping the header label
            Color color = EditorGUIUtility.isProSkin ? ProSkinSelectionBgColor : PersonalSkinSelectionBgColor;
            
            EditorGUI.DrawRect(copyRect, color);

            float offset = 2.0f;
            rect.x += MarginReorderIcon;
            rect.width -= (MarginReorderIcon + offset);

            rect.height = height - HeightHeader + offset;
            rect.y += HeightHeader - offset;

            EditorGUI.DrawRect(rect, color);
        }

        private float OnReorderListElementHeight(int index)
        {
            int length = reordList.serializedProperty.arraySize;

            if (length <= 0)
                return 0.0f;

            SerializedProperty iteratorProp = reordList.serializedProperty.GetArrayElementAtIndex(index);
            SerializedProperty endProp = iteratorProp.GetEndProperty();

            float height = GetDefaultSpaceBetweenElements();

            if (!iteratorProp.isExpanded)
                return height;

            int i = 0;
            while (iteratorProp.NextVisible(true) && !EqualContents(endProp, iteratorProp))
            {
                float multiplier = i == 0 ? AdditionalSpaceMultiplier : 1.0f;
                height += GetDefaultSpaceBetweenElements() * multiplier;
                ++i;
            }

            return height;
        }

        private void OnReorderListAddDropdown(Rect buttonRect, ReorderableList list)
        {
            GenericMenu menu = new GenericMenu();


            for (int i = 0; i < gameEventTypes.Count; ++i)
            {
                Type type = gameEventTypes[i];
                string actionName = gameEventTypes[i].Name;

                // UX improvement: If no elements are available the add button should be faded out or
                // just not visible.
                //bool alreadyHasIt = DoesReordListHaveElementOfType(actionName);
                //if (alreadyHasIt)
                //    continue;
                int o = i;
                InsertSpaceBeforeCaps(ref actionName);
                //menu.AddItem(new GUIContent(typesAttributes.ElementAt(i).Value.MenuName), false, () => AddItem(gameEventTypes[o]));
                menu.AddItem(new GUIContent(typesAttributes.ElementAt(i).Value.MenuName), false, OnAddItemFromDropdown, (object)type);
            }

            menu.ShowAsContext();
        }

        private void AddItem(Type type)
        {
            serializedObject.Update();
            Undo.RecordObject(gameEvent, "Add feedback");
            gameEvent.Feedbacks.Add(System.Activator.CreateInstance(type) as GameFeedback);
            serializedObject.ApplyModifiedProperties();
        }

        private void OnAddItemFromDropdown(object obj)
        {
            Type settingsType = (Type)obj;
            int last = reordList.serializedProperty.arraySize;
            reordList.serializedProperty.InsertArrayElementAtIndex(last);

            SerializedProperty lastProp = reordList.serializedProperty.GetArrayElementAtIndex(last);
            lastProp.managedReferenceValue = Activator.CreateInstance(settingsType);
            serializedObject.ApplyModifiedProperties();
        }

        #endregion

        #region Helper Methods

        private float GetDefaultSpaceBetweenElements()
        {
            return EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;
        }

        private void InsertSpaceBeforeCaps(ref string theString)
        {
            for (int i = 0; i < theString.Length; ++i)
            {
                char currChar = theString[i];

                if (char.IsUpper(currChar))
                {
                    theString = theString.Insert(i, " ");
                    ++i;
                }
            }
        }

        private bool EqualContents(SerializedProperty a, SerializedProperty b)
        {
            return SerializedProperty.EqualContents(a, b);
        }

        private List<Type> GetNonAbstractTypesSubclassOf<T>(bool sorted = true) where T : class
        {
            Type parentType = typeof(T);
            Assembly assembly = Assembly.GetAssembly(parentType);

            List<Type> types = assembly.GetTypes().Where(type => type.IsClass && !type.IsAbstract && type.IsSubclassOf(parentType)).ToList();

            if (sorted)
                types.Sort(CompareTypesNames);

            return types;
        }

        private int CompareTypesNames(Type a, Type b)
        {
            return a.Name.CompareTo(b.Name);
        }

        private bool DoesReordListHaveElementOfType(string type)
        {
            for (int i = 0; i < reordList.serializedProperty.arraySize; ++i)
            {
                // this works but feels ugly. Type in the array element looks like "managedReference<actualStringType>"
                if (reordList.serializedProperty.GetArrayElementAtIndex(i).type.Contains(type))
                    return true;
            }

            return false;
        }
    }

    #endregion

}