using UnityEngine;
using System.Collections;

public class RectangleController : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {

	}

	void OnTriggerExit2D(Collider2D col) {
		if (col.gameObject.name == "Boundary") {
			GameObject.Find ("Game Controller").GetComponent<Nodes> ().exitGame = true;
		}
	}

}
