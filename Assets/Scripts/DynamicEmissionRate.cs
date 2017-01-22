using UnityEngine;

public class DynamicEmissionRate : MonoBehaviour {

	ParticleSystem ps;
	[Range(0f,1f)] public float emissionRateFactor;
	[SerializeField] float maxEmissionRate;
	[SerializeField] float factor;

	void Awake () {
		ps = GetComponent<ParticleSystem>();
	}
	
	void Update () {
		var emission = ps.emission;
		emission.rateOverTime = maxEmissionRate * Mathf.Clamp01(emissionRateFactor * factor);
	}

	public void setEmissionRateFactor(float factor) {
		emissionRateFactor = factor;
	}
}
