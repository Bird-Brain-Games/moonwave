using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerScoreboardSection : MonoBehaviour {

	public Text scoreText;
	public Image image;
	[SerializeField] int score;

	public int Score {get{return score;} set{score = value; scoreText.text = score.ToString();}}
}
