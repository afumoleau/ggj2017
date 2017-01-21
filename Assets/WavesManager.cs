using UnityEngine;

public class WavesManager : MonoBehaviour {

	[Range(0f, 0.5f)] public float waveAmplitude;
	[SerializeField] MeshRenderer seaRendering;
	[SerializeField] SurfaceLevel seaPhysics;

	void Start () {
		
	}
	
	void Update () {
		seaPhysics.amplitude = waveAmplitude;
		seaRendering.material.SetFloat("_WaveAmplitude", waveAmplitude);
	}
}
