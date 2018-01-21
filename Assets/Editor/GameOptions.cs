using UnityEngine;
using UnityEditor;
using UnityEngine.SceneManagement;
using UnityEditor.SceneManagement;
using System.IO;

public class GameOptions : EditorWindow
{
    //seperates the top and bot
    //The textfield between the two of them
    public void Line(string textfield, int top = 10, int bottom = 3)
        {
        GUILayout.Space(top);
        GUILayout.Box(textfield, AssetUsageDetectorNamespace.AssetUsageDetector.GL_EXPAND_WIDTH);
        GUILayout.Space(bottom);
    }

    //Code seperated by regions
    #region Wrapping
    //The toggle state
    bool wrapping;
    //The GameObject we are changing in this case it is a prefab
    GameObject wrappingPrefab;
    GameObject bulletWrap;
    void WrapAwake()
    {
        //Setup
        wrapping = false;
        wrappingPrefab = AssetDatabase.LoadAssetAtPath("Assets/_Prefabs/map building/wrapping bounding box.prefab", typeof(GameObject)) as GameObject;
        Debug.Log("Pre bullet load: " + bulletWrap);
        bulletWrap = AssetDatabase.LoadAssetAtPath("Assets/_Prefabs/bullet/bullet.prefab", typeof(GameObject)) as GameObject;
        Debug.Log("Post bullet load: " + bulletWrap);
    }
    void WrapOnGUI()
    {
        Debug.Log(bulletWrap);
        //If the variable isnt loaded recall the awake function =
        if (null == wrappingPrefab || null == bulletWrap)
        {
            WrapAwake();
        }
        //Display button
        wrapping = EditorGUILayout.Toggle("Player wrap", wrapping);
        bulletWrap.GetComponent<BulletDuration>().enabled = wrapping;
        wrappingPrefab.SetActive(wrapping);

        //display editable float if the wrap is active
        if (wrapping == true)
            bulletWrap.GetComponent<BulletDuration>().MaxDuration =
                EditorGUILayout.DelayedFloatField("Bullet Duration", bulletWrap.GetComponent<BulletDuration>().MaxDuration);

    }
    #endregion

    #region BulletGravity
    Bullet bulletGravOptions;

    void BulletGravAwake()
    {
        GameObject temp = AssetDatabase.LoadAssetAtPath("Assets/_Prefabs/bullet/bullet.prefab", typeof(GameObject)) as GameObject;
        bulletGravOptions = temp.GetComponent<Bullet>();
    }

    void BulletOnGUI()
    {
        //recall awake function if the variable isnt loaded
        if (null == bulletGravOptions)
        {
            BulletGravAwake();
        }
        bulletGravOptions.m_gravityEnabled = EditorGUILayout.Toggle("Bullet Gravity", bulletGravOptions.m_gravityEnabled);

        if (bulletGravOptions.m_gravityEnabled == true)
            bulletGravOptions.m_fallGravMultiplier = EditorGUILayout.DelayedFloatField("BulletGravity ", bulletGravOptions.m_fallGravMultiplier);

    }
    #endregion

    #region Map Select
    int index;
    string[] scenes;
    bool loaded = false;
    private void MapAwake()
    {

        var info = new DirectoryInfo("Assets/_Scenes/Completed Maps");
        FileInfo[] fileInfo = info.GetFiles();
        scenes = new string[fileInfo.Length / 2];
        int count = 0;

        for (int i = 0; i < fileInfo.Length; i++)
        {
            if (!fileInfo[i].Name.Contains("meta"))
            {
                scenes[count] = fileInfo[i].Name;
                ++count;
            }
        }

        loaded = true;
    }

    private void MapOnGUI()
    {
        if (false == loaded)
            MapAwake();
        index = EditorGUILayout.Popup(index, scenes);
        if (GUILayout.Button("Change level"))
        {
            EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo();
            if (Application.isPlaying)
                EditorSceneManager.LoadScene("Assets/_Scenes/Completed Maps/" + scenes[index]);
            else
                EditorSceneManager.OpenScene("Assets/_Scenes/Completed Maps/" + scenes[index]);

        }

    }
    #endregion

    #region Reset
    private void ResetOnGUI()
    {
        if (GUILayout.Button("Reset active scene") && Application.isPlaying)
        {
            Scene currentScene = SceneManager.GetActiveScene();
            SceneManager.LoadScene(currentScene.buildIndex);
        }
    }
    #endregion

    private void Awake()
    {
        WrapAwake();
        BulletGravAwake();
    }

    //Create window
    [MenuItem("GameOptions/Game Options")]
    static void ShowWindow()
    {
        EditorWindow.GetWindow<GameOptions>().Show();
    }

    private void OnGUI()
    {
        WrapOnGUI();
        BulletOnGUI();

        Line("Level select");

        MapOnGUI();

        Line("Reset", 10, 3);

        //This should always be the last function call
        ResetOnGUI();
    }
}
