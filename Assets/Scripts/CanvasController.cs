using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class CanvasController : MonoBehaviour {
	public Text lastLayerText, scoreText, bestScore;
	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void Update () {
		SetText ();
	}

	void SetText() {
		lastLayerText.text = "Last Layer TanH (Movespeed): " + GameObject.Find ("Game Controller").GetComponent<Nodes> ().GetMoveSpeed ();
		scoreText.text = "Score: " + GameObject.Find ("Game Controller").GetComponent<Nodes> ().score;
		bestScore.text = "Best Score: " + GameObject.Find ("Game Controller").GetComponent<Nodes> ().oldScore;
	}
}
