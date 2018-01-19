using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEditor.SceneManagement;
using UnityEditor;
using System.IO;

public class AddPrefabToAllMaps : EditorWindow
{
    Object prefab;

    [MenuItem("Window/Add prefab to all maps")]
    static void ShowWindow()
    {
        var window = GetWindowWithRect<AddPrefabToAllMaps>(new Rect(0, 0, 165, 100));
        window.Show();
    }

    private void Awake()
    {
        position = new Rect(10, 10, 20, 20);
    }

    void OnInspectorUpdate()
    {
        Repaint();
    }

    void OnGUI()
    {
        EditorGUILayout.BeginHorizontal();
        prefab = EditorGUILayout.ObjectField(prefab, typeof(Object), false);
        EditorGUILayout.EndHorizontal();

        if (GUILayout.Button("Add!"))
        {
            Debug.Log("Add");
            if (prefab != null)
            {
                Scene scene;
                string path = "";
                string orignal;
                //fetch file names from the folder
                var info = new DirectoryInfo("Assets/_Scenes/Completed Maps");
                FileInfo[] fileInfo = info.GetFiles();
                orignal = info + EditorSceneManager.GetActiveScene().name;
                Debug.Log("Orignal" + orignal);

                //for all non meta files add the object to that scene file
                for (int i = 0; i < fileInfo.Length; i++)
                {
                    if (!fileInfo[i].Name.Contains("meta"))
                    {
                        path = info + "/" + fileInfo[i].Name;
                        Debug.Log(path);


                        EditorSceneManager.OpenScene(path);
                        scene = EditorSceneManager.GetActiveScene();
                        Debug.Log(scene.name);
                        PrefabUtility.InstantiatePrefab(prefab, scene);
                        EditorSceneManager.SaveScene(scene);
                       

                    }
                }
                EditorSceneManager.OpenScene(orignal);

            }
        }
    }
}