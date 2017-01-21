using UnityEngine;

public class SinMove : MonoBehaviour {

	Vector3 initialPosition;

	public float timeScale;
	public float xOffset;
	public float xAmplitude;
	public float yOffset;
	public float yAmplitude;

	void Start() {
		initialPosition = transform.localPosition;
	}

	void Update () {
		transform.localPosition = initialPosition + new Vector3(xAmplitude * Mathf.Sin(xOffset + timeScale * Time.time),
																yAmplitude * Mathf.Sin(yOffset + timeScale * Time.time),
																0);
	}
}
