using System.IO;
 using UnityEditor;
 using UnityEngine;
 
 //place this script in a folder name "Editor" somewhere in the project Assets
 public class ShowExtentionsInProjectBrowser
 {
    private static GUIStyle rStyle;
    [InitializeOnLoadMethod]
    public static void Init()
    {
       EditorApplication.projectWindowItemOnGUI += OnProjectWindowItemOnGui;
    }

    private static void OnProjectWindowItemOnGui(string guid, Rect rect)
    {
       if (rStyle == null)
       {
          rStyle = new GUIStyle(GUI.skin.GetStyle("Label"))
          {
             alignment = TextAnchor.MiddleRight
          };
       }

       if (rect.height > EditorGUIUtility.singleLineHeight + 2f) return;
       var width = 70;
       rect.x += rect.width - width;
       rect.width = width;
       rect.height = EditorGUIUtility.singleLineHeight;

       var path = AssetDatabase.GUIDToAssetPath(guid);
       GUI.Label(rect, Path.GetExtension(path),rStyle);
       

    }
 }