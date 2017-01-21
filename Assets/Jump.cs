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
	[SerializeField] float maxCrouchTime;

	[SerializeField] FloatEvent OnDeepnessChange;
	[SerializeField] FloatEvent OnCrouchTimeChange;

	void Awake() {
		rigidbody = GetComponent<Rigidbody2D>();
	}
	
	void Update() {
		if(Input.GetButtonDown("Jump")) {
			Debug.Log("crouch");
			crouchTime = 0f;
			OnCrouchTimeChange.Invoke(crouchTime / maxCrouchTime);
			if(transform.position.y <= seaAltitude) {
				crouching = true;
			}
		}
		if(crouching && transform.position.y <= seaAltitude) {
			crouchTime = Mathf.Clamp(crouchTime + Time.deltaTime, 0f, maxCrouchTime);
			OnCrouchTimeChange.Invoke(crouchTime / maxCrouchTime);
		}
		if(Input.GetButtonUp("Jump")) {
			crouching = false;
			if(transform.position.y <= seaAltitude) {
				Debug.Log("jump, time = "+crouchTime);
				rigidbody.AddForce(Vector2.up * crouchTime * jumpPower);
			}
		}

		deepness = Mathf.Clamp01(-(seaAltitude + transform.position.y));
		OnDeepnessChange.Invoke(deepness);
	}
}
