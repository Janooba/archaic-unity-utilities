using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;

namespace Archaic.Core.Utilities.Editor
{
    public class HiddenObjectExplorer : EditorWindow
    {
        [MenuItem("Tools/HiddenObjectExplorer")]
        static void Init()
        {
            GetWindow<HiddenObjectExplorer>();
        }
        List<GameObject> m_Objects = new List<GameObject>();
        Vector2 scrollPos = Vector2.zero;

        void OnEnable()
        {
            FindObjects();
        }

        void FindObjects()
        {
            var objs = Resources.FindObjectsOfTypeAll(typeof(GameObject)) as GameObject[];
            m_Objects.Clear();
            foreach (var O in objs)
            {
                var go = O.transform.root.gameObject;
                if (!m_Objects.Contains(go))
                    m_Objects.Add(go);
            }
        }
        void FindObjectsAll()
        {
            var objs = Resources.FindObjectsOfTypeAll(typeof(GameObject)) as GameObject[];
            m_Objects.Clear();
            m_Objects.AddRange(objs);
        }

        void UnhideAllObjects()
        {
            var objs = Resources.FindObjectsOfTypeAll(typeof(GameObject)) as GameObject[];

            for (int i = 0; i < objs.Length; i++)
            {
                if (objs[i] == null)
                    continue;

                objs[i].hideFlags &= ~HideFlags.HideInHierarchy;
            }
        }

        HideFlags HideFlagsButton(string aTitle, HideFlags aFlags, HideFlags aValue)
        {
            if (GUILayout.Toggle((aFlags & aValue) > 0, aTitle, "Button"))
                aFlags |= aValue;
            else
                aFlags &= ~aValue;
            return aFlags;
        }

        void OnGUI()
        {
            GUILayout.BeginHorizontal();
            if (GUILayout.Button("Find top Level"))
            {
                FindObjects();
            }
            if (GUILayout.Button("Find ALL Object"))
            {
                FindObjectsAll();
            }
            if (GUILayout.Button("Unhide ALL Objects in Hierarchy"))
            {
                UnhideAllObjects();
            }
            GUILayout.EndHorizontal();
            scrollPos = GUILayout.BeginScrollView(scrollPos);
            for (int i = 0; i < m_Objects.Count; i++)
            {
                GameObject O = m_Objects[i];
                if (O == null)
                    continue;
                GUILayout.BeginHorizontal();
                EditorGUILayout.ObjectField(O.name, O, typeof(GameObject), true);
                HideFlags flags = O.hideFlags;
                flags = HideFlagsButton("HideInHierarchy", flags, HideFlags.HideInHierarchy);
                flags = HideFlagsButton("HideInInspector", flags, HideFlags.HideInInspector);
                flags = HideFlagsButton("DontSave", flags, HideFlags.DontSave);
                flags = HideFlagsButton("NotEditable", flags, HideFlags.NotEditable);
                O.hideFlags = flags;
                GUILayout.Label("" + ((int)flags), GUILayout.Width(20));
                GUILayout.Space(20);
                if (GUILayout.Button("DELETE"))
                {
                    DestroyImmediate(O);
                    FindObjects();
                    GUIUtility.ExitGUI();
                }
                GUILayout.Space(20);
                GUILayout.EndHorizontal();
            }
            GUILayout.EndScrollView();
        }
    }
}