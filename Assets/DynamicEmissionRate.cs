using UnityEngine;

public class DynamicEmissionRate : MonoBehaviour {

	ParticleSystem ps;
	[Range(0f,1f)] public float emissionRateFactor;
	[SerializeField] float maxEmissionRate;

	void Awake () {
		ps = GetComponent<ParticleSystem>();
	}
	
	void Update () {
		var emission = ps.emission;
		emission.rateOverTime = maxEmissionRate * emissionRateFactor;
	}

	public void setEmissionRateFactor(float factor) {
		emissionRateFactor = factor;

	}
}
