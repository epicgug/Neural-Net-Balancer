	using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Nodes : MonoBehaviour {
	private static int NUM_NODES_LEVEL_1 = 5;
	private static int INPUT_SIZE = 7;
	private int NUM_WEIGHTS;
	private static int START_WEIGHT_MIN = -10;
	private static int START_WEIGHT_MAX = 10;
	private static float MAX_ADJUSTMENT_AMOUNT = 0.3f;
	public float outputScaleFactor, score, oldScore, lastLayer, touchScoreAdd, desiredDistance, amplitudePosition;
	public GameObject ball, rectangle;
	private Rigidbody2D rectangleRB;
	public float[] inputs = {0, 0, 0, 0, 0, 0, 0};
	public float[] weights, oldWeights, bestWeights;
	public bool exitGame = false;
	public int numIterations, numTimeSteps;
	public float[] layer1Output;
	public bool useOldWeights = false;
	public Toggle toggle;

	// Use this for initialization
	void Start () {
		NUM_WEIGHTS = NUM_NODES_LEVEL_1 * INPUT_SIZE + NUM_NODES_LEVEL_1;
		weights = new float[NUM_WEIGHTS];
		oldWeights = new float[NUM_WEIGHTS];
		InitializeWeights ();
		rectangleRB = rectangle.GetComponent<Rigidbody2D> ();
		numIterations = 0;
		numTimeSteps = 0;
		toggle.isOn = false;
	}
	
	// Update is called once per frame
	void Update () {
		GetData ();
		controlBall ();
		ControlGame ();
		//InvokeRepeating ("InitializeWeights", 1f, 0.01f);
		if (Input.GetKeyDown(KeyCode.Space)) {
			InitializeWeights ();
		}
	}

	public float GetMoveSpeed() {
		return NodeCompute ();
	}
		
	void ControlGame() {
		numTimeSteps++;
		if (numTimeSteps > 500) {
			exitGame = true;
		}
		if (exitGame) {
			if (numIterations > 0) {
				if (!toggle.isOn) {
					if (score > oldScore) {
						oldScore = score;
						bestWeights = weights;
						//List<GameObject> temporary = new List<GameObject>(master);
						oldWeights = new float[NUM_WEIGHTS];
						for (int i = 0; i < NUM_WEIGHTS; i++) {
							oldWeights [i] = weights [i];
						}
					} else {
						weights = new float[NUM_WEIGHTS];
						for (int i = 0; i < NUM_WEIGHTS; i++) {
							weights [i] = oldWeights [i] + getRandomAmount (MAX_ADJUSTMENT_AMOUNT);
						}
						if (Random.value < .2) {
							InitializeWeights ();
						}
					}
				} else {
					Debug.Log ("First" + weights[1]);
					for (int i = 0; i < NUM_WEIGHTS; i++) {
						weights [i] = bestWeights [i];
					}
					Debug.Log ("Second" + weights[1]);
				}
			}
			numIterations++;
			ResetScene ();
			exitGame = false;
		}
	}

	private float getRandomAmount(float maximum) {
		return (float) (maximum * (Random.value - 0.5) * 2);
	}

	void controlBall() {
		float moveSpeed = NodeCompute ();
		Vector2 newPosition = ball.transform.position;
		if (!System.Single.IsNaN (moveSpeed))
			newPosition.x += moveSpeed * outputScaleFactor;
		ball.transform.position = newPosition;
	}

	void InitializeWeights() {
		for (int i = 0; i < weights.Length; i++) {
			weights [i] = getRandomAmount(START_WEIGHT_MAX);
		}
	}

	void GetData() {
		inputs [0] = ball.transform.position.x;
		inputs [1] = rectangle.transform.position.x;
		inputs [2] = rectangle.transform.position.y;
		inputs [3] = rectangle.transform.rotation.z;
		inputs [4] = rectangleRB.velocity.x;
		inputs [5] = rectangleRB.velocity.y;
		inputs [6] = rectangleRB.angularVelocity;
	}

	public float TanH (float x) {
		double ex = Mathf.Exp (x);
		double eMinusX = Mathf.Exp (-x);
		double denominator = (ex + eMinusX);
		float result = (float)((ex - eMinusX) / (denominator));
		if (System.Single.IsNaN (result)) {
			return 0;
		} else {
			return result;
		}
	}

	private float NodeCompute() {
		layer1Output = new float[NUM_NODES_LEVEL_1];

		for (int i = 0; i < NUM_NODES_LEVEL_1; i++) {
			layer1Output [i] = 0;
			for (int j = 0; j < INPUT_SIZE; j++) {
				layer1Output[i] += weights [j + (i * INPUT_SIZE)] * inputs [j];
			}
			if (System.Single.IsNaN (TanH (layer1Output[i]))) {
				Debug.Log(layer1Output[i]);
			}
			layer1Output [i] = TanH (layer1Output [i]);
		}
		int weightIndexOffset = NUM_WEIGHTS - NUM_NODES_LEVEL_1 - 1;
		lastLayer = 0;
		for (int i = 0; i < NUM_NODES_LEVEL_1; i++) {
			lastLayer += layer1Output[i] * weights[i + weightIndexOffset];
		}
		return TanH (lastLayer);
	}

	void ResetScene() {
		rectangle.transform.position = new Vector3 (0, 1);
		rectangle.transform.eulerAngles = new Vector3 (0, 0, 90);
		rectangleRB.velocity = new Vector2(0, 0);
		rectangleRB.angularVelocity = 0;
		ball.transform.position = new Vector3(0, -0.5f);
		score = 0;
		numTimeSteps = 0;
	}
}
