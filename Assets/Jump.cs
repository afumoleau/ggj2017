using UnityEngine;
using UnityEngine.Events;

public class Jump : MonoBehaviour {

	[System.Serializable]
	public class FloatEvent : UnityEvent<float> {};

	float crouchTime;
	bool crouching;
	new Rigidbody2D rigidbody;
	[SerializeField] float jumpPower;
	[SerializeField] float seaAltitude;
	public float deepness;

	[SerializeField] FloatEvent OnDeepnessChange;

	void Awake() {
		rigidbody = GetComponent<Rigidbody2D>();
	}
	
	void Update() {
		if(Input.GetButtonDown("Jump")) {
			Debug.Log("crouch");
			crouchTime = 0f;
			crouching = true;
			if(transform.position.y <= seaAltitude) {

			}
		}
		if(crouching && transform.position.y <= seaAltitude) {
			crouchTime += Time.deltaTime;
		}
		if(Input.GetButtonUp("Jump")) {
			if(transform.position.y <= seaAltitude) {
				Debug.Log("jump, time = "+crouchTime);
				rigidbody.AddForce(Vector2.up * crouchTime * jumpPower);
			}
		}

		deepness = Mathf.Clamp01(-(seaAltitude + transform.position.y));
		OnDeepnessChange.Invoke(deepness);
	}
}
