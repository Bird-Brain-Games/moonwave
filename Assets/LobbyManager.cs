using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LobbyManager : MonoBehaviour {

	public Text readyUpText;
	public PortraitManager portraits;
	public LevelSelectManager levelSelectors;
	public GameObject pressToStarts;

	public void ShowCharacterSelect()
	{
		readyUpText.text = "Press START to Choose Levels";
		portraits.gameObject.SetActive(true);
		pressToStarts.SetActive(true);
		levelSelectors.gameObject.SetActive(false);
		portraits.Reset();
	}

	public void ShowLevelSelect()
	{
		readyUpText.text = "Choose a Level Set";
		portraits.gameObject.SetActive(false);
		pressToStarts.SetActive(false);
		levelSelectors.gameObject.SetActive(true);
		levelSelectors.Reset();
	}
}
