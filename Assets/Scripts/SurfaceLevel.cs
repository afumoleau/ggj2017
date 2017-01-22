using UnityEngine;

public class SurfaceLevel : MonoBehaviour {

	float clock = 0f;
	BuoyancyEffector2D buoyancy;
	public float amplitudeOffset;
	public float amplitude;
	public float timeOffset;
	public float timeFactor;

	void Awake() {
		buoyancy = GetComponent<BuoyancyEffector2D>();
	}

	void Update () {
		clock += Time.deltaTime;
		buoyancy.surfaceLevel = amplitudeOffset + amplitude * Mathf.Sin(timeOffset + timeFactor * clock);
	}
}
