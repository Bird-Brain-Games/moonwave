using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadSceneOnClick : MonoBehaviour {

	public void LoadByIndex(int sceneIndex)
    {
        SceneManager.LoadScene(sceneIndex);
    }

    public void LoadRandom()
    {
        SceneManager.LoadScene(Random.Range(2, SceneManager.sceneCountInBuildSettings - 2));
    }

    public void LoadByName(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }
}
