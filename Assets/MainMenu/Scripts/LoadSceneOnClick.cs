using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadSceneOnClick : MonoBehaviour {

    //string[] set1 {;
    //string[] set2;

	public void LoadByIndex(int sceneIndex)
    {
        SceneManager.LoadScene(sceneIndex);
    }

    public void LoadRandom()
    {
        // Don't choose the same scene twice in a row [Graham]
        int newSceneNum = -1;
        int currentBuildNum = SceneManager.GetActiveScene().buildIndex;
        do
        {
            newSceneNum = Random.Range(2, SceneManager.sceneCountInBuildSettings);
        } while (currentBuildNum == newSceneNum);

        SceneManager.LoadScene(newSceneNum);
    }

    public void LoadByName(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }

    public void LoadBySet(int setNum)
    {
        SceneManager.LoadScene(Random.Range(MatchSettings.minRange, MatchSettings.maxRange));
    }
}
