using UnityEngine;
using UnityEditor;
using UnityEngine.SceneManagement;

public class GameOptions : EditorWindow
{

    //Code seperated by regions
    #region Wrapping
    //The toggle state
    bool wrapping;
    //The GameObject we are changing in this case it is a prefab
    GameObject wrappingPrefab;
    void WrapAwake()
    {
        //Setup
        wrapping = false;
        wrappingPrefab = AssetDatabase.LoadAssetAtPath("Assets/_Prefabs/map building/wrapping bounding box.prefab", typeof(GameObject)) as GameObject;

    }
    void WrapOnGUI()
    {
        //If the variable isnt loaded recall the awake function =
        if (null == wrappingPrefab)
        {
            WrapAwake();
        }
        //Display button
        wrapping = EditorGUILayout.Toggle("Player wrap", wrapping);
        //Set prefab active/deactive
        wrappingPrefab.SetActive(wrapping);

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

        bulletGravOptions.m_fallGravMultiplier = EditorGUILayout.DelayedFloatField("BulletGravity ", bulletGravOptions.m_fallGravMultiplier);

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

        //This should always be the last function call
        ResetOnGUI();
    }
}
