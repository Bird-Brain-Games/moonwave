using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class PlayerScoreboardSection : MonoBehaviour {

	public UnityEvent doneAnimating;
	public Image lightPrefab;
	private Image[] lights;
	public Text scoreText;
	public Image image;
	public Color color;
	[SerializeField] int score;

	public int Score {get{return score;} set{score = value;}}

	public void Populate()
	{

		lights = new Image[MatchSettings.pointsToWin+1];

		for (int i = 0; i <= MatchSettings.pointsToWin; i++)
		{
			lights[i] = Instantiate(lightPrefab, transform);
			
			if (Score > i)	
				lights[i].color = MatchSettings.playerColors[i];
			else
				lights[i].color = MatchSettings.playerColors[i];
		}
	}

	public void AnimateLastLight()
	{
		//lights[Score-1].GetComponent<Animator>().SetTrigger("Turn On");
		IEnumerator animate = AnimateLight(lights[Score-1]);
		StartCoroutine(animate);
	}

	IEnumerator AnimateLight(Image light)
	{
		yield return new WaitForSeconds(1f);
		Animator animator = light.GetComponent<Animator>();
		animator.SetTrigger("Turn On");

		float elapsedTime = 0f;
		float timeToMove = 0.2f;

		while (elapsedTime < timeToMove)
		{
			light.color = Color.Lerp(Color.black, Color.white, elapsedTime / timeToMove);
			yield return new WaitForFixedUpdate();
			elapsedTime += Time.fixedDeltaTime;
		}

		light.color = Color.white;

		elapsedTime = 0f;
		timeToMove = 0.3f;

		while (elapsedTime < timeToMove)
		{
			light.color = Color.Lerp(Color.white, color, elapsedTime / timeToMove);
			yield return new WaitForFixedUpdate();
			elapsedTime += Time.fixedDeltaTime;
		}

		light.color = color;
		doneAnimating.Invoke();
	}
}
