using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadSceneBySet : MonoBehaviour {

	public void LoadNext()
	{
		GetComponent<LoadSceneOnClick>().LoadBySet(MatchSettings.setNum);
	}
}
