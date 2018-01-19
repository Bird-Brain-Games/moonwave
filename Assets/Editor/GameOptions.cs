using UnityEngine;
using UnityEditor;

public class GameOptions : EditorWindow
{
    //The toggle state
    bool wrapping;
    //The GameObject we are changing in this case it is a prefab
    GameObject wrappingPrefab;

    private void Awake()
    {
        //Setup
        wrapping = false;
        wrappingPrefab = AssetDatabase.LoadAssetAtPath("Assets/_Prefabs/map building/wrapping bounding box.prefab", typeof(GameObject)) as GameObject;
    }

    //Create window
    [MenuItem("GameOptions/Game Options")]
    static void ShowWindow()
    {
        EditorWindow.GetWindow<GameOptions>().Show();
    }

    private void OnGUI()
    {
        //Display button
        wrapping = EditorGUILayout.Toggle("Player wrap", wrapping);
        //Set prefab active/deactive
        wrappingPrefab.SetActive(wrapping);
       
    }
}
