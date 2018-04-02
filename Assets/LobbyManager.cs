using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LobbyManager : MonoBehaviour {

	public GameObject moon;
	public Text readyUpText;
	public Text titleText;
	public PortraitManager portraits;
	public LevelSelectManager levelSelectors;
	public GameObject ruleContinueText;
	public GameObject pressToStarts;
	public GameObject quickInfo;
	public GameObject border;
	public GameObject infoPanel;

	public void ShowCharacterSelect()
	{
		titleText.text = "Character     Select";
		portraits.gameObject.SetActive(true);
		pressToStarts.SetActive(true);
		levelSelectors.gameObject.SetActive(false);
		portraits.Reset();
		moon.SetActive(true);
		quickInfo.SetActive(true);
		border.SetActive(true);
		infoPanel.SetActive(false);
		ruleContinueText.SetActive(false);
	}

	public void ShowLevelSelect()
	{
		titleText.text = "Elimination   Match";
		readyUpText.gameObject.SetActive(false);
		portraits.SavePortraits();
		portraits.gameObject.SetActive(false);
		pressToStarts.SetActive(false);
		levelSelectors.gameObject.SetActive(true);
		levelSelectors.Reset();
		moon.SetActive(false);	
		quickInfo.SetActive(false);
		border.SetActive(false);
		infoPanel.SetActive(true);
		ruleContinueText.SetActive(true);
	}
}
