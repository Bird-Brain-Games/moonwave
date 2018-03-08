using UnityEngine;
using UnityEditor;


public class levelmaker : EditorWindow
{

    [MenuItem("Campbell's/Level Maker")]
    public static void ShowWindow()
    {
        EditorWindow.GetWindow<levelmaker>("Level Maker");
    }

    static void ImportExample()
    {
        string assetPath = "Assets/_Prefabs/player/player hierarchy.prefab";
        GameObject prefab1 = AssetDatabase.LoadAssetAtPath(assetPath, typeof(GameObject)) as GameObject;

        assetPath = "Assets/_Prefabs/player/player.prefab";
        GameObject prefab2 = AssetDatabase.LoadAssetAtPath(assetPath, typeof(GameObject)) as GameObject;

        assetPath = "Assets/_Prefabs/player/Boost Collider.prefab";
        GameObject prefab3 = AssetDatabase.LoadAssetAtPath(assetPath, typeof(GameObject)) as GameObject;

        //if this object is a user created prefab
        GameObject root = GameObject.Find("Players and spawns");

        for (int i = 0; i < 4; i++)
        {
            GameObject hierarchy = GameObject.Find(PrefabUtility.InstantiatePrefab(prefab1).name = ("player hierarchy " + (i + 1)));
            GameObject player = GameObject.Find(PrefabUtility.InstantiatePrefab(prefab2).name = ("player " + i));
            GameObject boost = GameObject.Find(PrefabUtility.InstantiatePrefab(prefab3).name = ("boost collider " + i));

            hierarchy.transform.parent = root.transform;
            player.transform.parent = hierarchy.transform;
            boost.transform.parent = hierarchy.transform;

            hierarchy.name = ("player hierarchy " + (i + 1));
            player.name = ("player");
            boost.name = ("boost collider");
        }
    }

    void OnGUI()
    {
        if (GUI.Button(new Rect(10, 10, 150, 100), "Setup players"))
            ImportExample();

    }
}