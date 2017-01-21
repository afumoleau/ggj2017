using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class Jump : MonoBehaviour {

	[System.Serializable]
	public class FloatEvent : UnityEvent<float> {};

	float crouchTime;
	bool crouching;
	[HideInInspector] public bool rotating;
	new Rigidbody2D rigidbody;
	bool screenTouchedLastFrame;
	
	public float deepness;
	public float rotationSpeed;

	bool alive = true;
	[SerializeField] Transform floatingPoint;
	[SerializeField] float jumpPower;
	[SerializeField] float maxCrouchTime;
	[SerializeField] float landingAngleTolerance;

	[SerializeField] FloatEvent OnDeepnessChange;
	[SerializeField] FloatEvent OnCrouchTimeChange;
	[SerializeField] UnityEvent OnDead;

	public bool isInWater { get { return deepness > 0f; }}
	bool isFalling { get { return rigidbody.velocity.y < 0f; }}
	public bool isStanding { get { return Mathf.Abs(Mathf.DeltaAngle(transform.eulerAngles.z, 0f)) <= landingAngleTolerance; }}

	Vector3 initialPosition;

	void Awake() {
		rigidbody = GetComponent<Rigidbody2D>();
		initialPosition = transform.position;
	}
	
	void Update() {
		var screenTouched = (Input.touchCount > 0);

		deepness = (floatingPoint.position.y - transform.position.y);

		if(alive) {
			if(Input.GetButtonDown("Jump") || (screenTouched && !screenTouchedLastFrame)) {
				if(isInWater) {
					crouching = true;
				} else {
					rotating = true;	
				}
			}
			else if(Input.GetButtonUp("Jump") || (!screenTouched && screenTouchedLastFrame)) {
				crouching = false;
				rotating = false;
				if(isInWater) {
					rigidbody.AddForce(Vector2.up * crouchTime * jumpPower);
					StartCoroutine(ResetCrouchTime());
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
		OnDeepnessChange.Invoke(alive ? deepness : 0f);

		screenTouchedLastFrame = (Input.touchCount > 0);
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

	IEnumerator ResetCrouchTime() {
		var initial = crouchTime;
		for(var clock = 0f; clock < 0.25f; clock += Time.deltaTime) {
			crouchTime = Mathf.Lerp(initial, 0f, clock / 0.25f);
			OnCrouchTimeChange.Invoke(crouchTime / maxCrouchTime);
			yield return null;
		}
		crouchTime = 0f;
		OnCrouchTimeChange.Invoke(crouchTime / maxCrouchTime);
	}
}
