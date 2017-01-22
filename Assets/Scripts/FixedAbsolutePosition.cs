using UnityEngine;

public class FixedAbsolutePosition : MonoBehaviour {

	[SerializeField] BuoyancyEffector2D buoyancyEffector;
	[SerializeField] Vector3 absolutePosition;
	[SerializeField] float displacementFactor;
	
	void Update () {
		transform.position = absolutePosition + new Vector3(0, buoyancyEffector.surfaceLevel * displacementFactor, 0);
	}
}
