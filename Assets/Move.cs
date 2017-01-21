using UnityEngine;

public class Move : MonoBehaviour {

	public float speed = 5f;
	
	void Update () {
		transform.Translate(Vector3.right * Time.deltaTime * speed);
	}
}
