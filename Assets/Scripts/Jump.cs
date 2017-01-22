using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class Jump : MonoBehaviour {

	[System.Serializable]
	public class FloatEvent : UnityEvent<float> {};

	float crouchTime;
	float crouchOvertime;
	bool crouching;
	[HideInInspector] public bool rotating;
	new Rigidbody2D rigidbody;
	bool screenTouchedLastFrame;
	
	public float deepness;
	public float rotationSpeed;

	public bool alive = true;
	[SerializeField] Transform floatingPoint;
	[SerializeField] float jumpPower;
	[SerializeField] float maxCrouchTime;
	[SerializeField] float maxCrouchOvertime;
	[SerializeField] float landingAngleTolerance;

	[SerializeField] SpriteRenderer graphics;
	[SerializeField] Sprite standSprite;
	[SerializeField] Sprite crouchSprite;
	[SerializeField] Sprite flySprite;

	[SerializeField] FloatEvent OnDeepnessChange;
	[SerializeField] FloatEvent OnCrouchTimeChange;
	[SerializeField] FloatEvent OnCrouchOvertimeChange;
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
				crouchTime = crouchTime + Time.deltaTime;

				if(crouchTime > maxCrouchTime) {
					crouchTime = maxCrouchTime;
					crouchOvertime += Time.deltaTime;
					if(crouchOvertime > maxCrouchOvertime) {
						alive = false;
						StartCoroutine(Drift());
						crouchOvertime = 0f;
						OnCrouchOvertimeChange.Invoke(crouchOvertime / maxCrouchOvertime);
					} else {
						OnCrouchOvertimeChange.Invoke(crouchOvertime / maxCrouchOvertime);
					}
				}
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

		if(!isInWater) {
			graphics.sprite = flySprite;
		} else if (crouching) {
			graphics.sprite = crouchSprite;
		} else {
			graphics.sprite = standSprite;
		}
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
		crouchTime = 0f;
		crouchOvertime = 0f;
		OnCrouchTimeChange.Invoke(0f);
		OnCrouchOvertimeChange.Invoke(0f);
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
		crouchOvertime = 0f;
		OnCrouchOvertimeChange.Invoke(0f);
	}
}
