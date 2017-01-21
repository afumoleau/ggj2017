using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class Jump : MonoBehaviour {

	[System.Serializable]
	public class FloatEvent : UnityEvent<float> {};

	float crouchTime;
	bool crouching;
	bool rotating;
	new Rigidbody2D rigidbody;
	public float deepness;

	bool alive = true;
	[SerializeField] Transform floatingPoint;
	[SerializeField] float jumpPower;
	[SerializeField] float maxCrouchTime;
	[SerializeField] float rotationSpeed;
	[SerializeField] float landingAngleTolerance;

	[SerializeField] FloatEvent OnDeepnessChange;
	[SerializeField] FloatEvent OnCrouchTimeChange;
	[SerializeField] UnityEvent OnDead;

	bool isInWater { get { return deepness > 0f; }}
	bool isFalling { get { return rigidbody.velocity.y < 0f; }}
	bool isStanding { get { return Mathf.Abs(Mathf.DeltaAngle(transform.eulerAngles.z, 0f)) <= landingAngleTolerance; }}

	Vector3 initialPosition;

	void Awake() {
		rigidbody = GetComponent<Rigidbody2D>();
		initialPosition = transform.position;
	}
	
	void Update() {

		deepness = (floatingPoint.position.y - transform.position.y);

		if(alive) {
			if(Input.GetButtonDown("Jump")) {
				crouchTime = 0f;
				OnCrouchTimeChange.Invoke(crouchTime / maxCrouchTime);
				if(isInWater) {
					crouching = true;
				} else {
					rotating = true;
				}
			}
			else if(Input.GetButtonUp("Jump")) {
				crouching = false;
				rotating = false;
				if(isInWater) {
					rigidbody.AddForce(Vector2.up * crouchTime * jumpPower);
				}
			}

			if(crouching && isInWater) {
				crouchTime = Mathf.Clamp(crouchTime + Time.deltaTime, 0f, maxCrouchTime);
				OnCrouchTimeChange.Invoke(crouchTime / maxCrouchTime);
			}
			if(rotating && !isInWater) {
				transform.Rotate(Vector3.forward * Time.deltaTime * rotationSpeed);
			}
		}

		if(alive && isFalling && isInWater && !isStanding) {
			alive = false;
			StartCoroutine(Drift());
		}
		OnDeepnessChange.Invoke(deepness);
	}

	IEnumerator Drift() {
		rigidbody.simulated = false;
		float clock = 0f;
		while(clock < 1.5f) {
			clock += Time.deltaTime;
			transform.position = transform.position + Vector3.left * Time.deltaTime * 10f;
			transform.position = transform.position + Vector3.up * Mathf.Sin(10f * clock) * 0.02f;
			transform.position = transform.position + Vector3.down * clock * 0.03f;
			yield return null;
		}

		OnDead.Invoke();		
	}

	public void Restart() {
		rigidbody.simulated = true;
		alive = true;
		crouching = false;
		rotating = false;
		transform.position = initialPosition;
		transform.localEulerAngles = Vector3.zero;
	}
}
