using UnityEngine;

public class Scroll : MonoBehaviour {

	[SerializeField] Vector2 speed;
	Material material;
	float clock = 0f;

	void Start () {
		if(GetComponent<SpriteRenderer>() != null) {
			material = GetComponent<SpriteRenderer>().material;
		}
		if(GetComponent<MeshRenderer>() != null) {
			material = GetComponent<MeshRenderer>().material;
		}
	}
	
	void Update () {
		clock += Time.deltaTime;
		material.mainTextureOffset = speed * clock;
	}
}
