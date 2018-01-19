using UnityEngine;
using UnityEditor;


public class levelmaker : EditorWindow
{

    [MenuItem("Window/Level Maker")]
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

        assetPath = "Assets/_Prefabs/player/alien rig.prefab";
        GameObject prefab4 = AssetDatabase.LoadAssetAtPath(assetPath, typeof(GameObject)) as GameObject;

        assetPath = "Assets/_Prefabs/player/shield.prefab";
        GameObject prefab5 = AssetDatabase.LoadAssetAtPath(assetPath, typeof(GameObject)) as GameObject;


        //if this object is a user created prefab
        GameObject root = GameObject.Find("Players and spawns");

        for (int i = 0; i < 4; i++)
        {
            GameObject hierarchy = GameObject.Find(PrefabUtility.InstantiatePrefab(prefab1).name = ("player hierarchy " + (i + 1)));
            GameObject player = GameObject.Find(PrefabUtility.InstantiatePrefab(prefab2).name = ("player " + i));
            GameObject boost = GameObject.Find(PrefabUtility.InstantiatePrefab(prefab3).name = ("boost collider " + i));
            GameObject alien = GameObject.Find(PrefabUtility.InstantiatePrefab(prefab4).name = ("alien rig " + i));
            GameObject shield = GameObject.Find(PrefabUtility.InstantiatePrefab(prefab5).name = ("shield " + i));

            hierarchy.transform.parent = root.transform;
            player.transform.parent = hierarchy.transform;
            boost.transform.parent = hierarchy.transform;
            shield.transform.parent = player.transform;
            alien.transform.parent = player.transform;

            hierarchy.name = ("player hierarchy " + (i + 1));
            player.name = ("player");
            boost.name = ("boost collider");
            shield.name = ("shield");
            alien.name = ("alien rig");
        }
    }

    void OnGUI()
    {
        if (GUI.Button(new Rect(10, 10, 150, 100), "Setup players"))
            ImportExample();

    }
}