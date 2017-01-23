using UnityEngine;

public class SinMove : MonoBehaviour {

	Vector3 initialPosition;
	RectTransform rt;

	public float timeScale = 1f;
	public float xOffset = 0f;
	public float xAmplitude = 1f;
	public float yOffset = 0f;
	public float yAmplitude = 1f;
	public float amplitude = 1f;

	public void setTimeScale(float value) { timeScale = value; }
	public void setXOffset(float value) { xOffset = value; }
	public void setXAmplitude(float value) { xAmplitude = value; }
	public void setYOffset(float value) { yOffset = value; }
	public void setYAmplitude(float value) { yAmplitude = value; }
	public void setAmplitude(float value) { amplitude = value; }

	void Start() {
		rt = GetComponent<RectTransform>();
		if(rt != null) {
			initialPosition = rt.anchoredPosition;
		} else {
			initialPosition = transform.localPosition;
		}
	}

	void Update () {
		var offset = new Vector3(xAmplitude * amplitude * Mathf.Sin(xOffset + timeScale * Time.time),
								 yAmplitude * amplitude * Mathf.Sin(yOffset + timeScale * Time.time),
								 0);
		if(rt != null) {
			rt.anchoredPosition = initialPosition + offset;
		} else {
			transform.localPosition = initialPosition + offset;
		}
	}
}
