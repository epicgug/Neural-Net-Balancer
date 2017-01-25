using UnityEngine;
using System.Collections;

public class BallController : MonoBehaviour {
	public float xSpeed, startY;
	public Collider2D circle, rectangle;
	public Nodes nodes;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		controlBall ();
		detectTouching ();
	}

	void controlBall () {
		Vector2 updatePositon = this.transform.position;
		if (Input.GetKey (KeyCode.LeftArrow) || Input.GetKey (KeyCode.A)) {
			updatePositon.x -= xSpeed;
		}
		if (Input.GetKey (KeyCode.RightArrow) || Input.GetKey (KeyCode.D)) {
			updatePositon.x += xSpeed;
		}
		updatePositon.y = startY;
		this.transform.position = updatePositon;
	}

	void detectTouching() {
		if (circle.IsTouching (rectangle)) {
			float distFromDesiredX = Mathf.Abs(rectangle.transform.position.x - nodes.desiredDistance);
			nodes.score += nodes.amplitudePosition-nodes.amplitudePosition*nodes.TanH(Mathf.Abs(distFromDesiredX)/nodes.amplitudePosition);
//			float desiredX = 2;
//			float distFromDesiredX = Mathf.Abs (rectangle.transform.position.x - desiredX);
//			float scoreMultiplier = 1-nodes.TanH(Mathf.Abs(distFromDesiredX)/5);
//			nodes.score += nodes.touchScoreAdd;
			
		}
	}

	void OnTriggerExit2D(Collider2D col) {
		if (col.gameObject.name == "Boundary") {
			GameObject.Find ("Game Controller").GetComponent<Nodes> ().exitGame = true;
		}
	}

}
