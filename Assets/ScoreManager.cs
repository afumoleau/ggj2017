using UnityEngine;
using UnityEngine.UI;

public class ScoreManager : MonoBehaviour {

	[SerializeField] Jump surfer;
	[SerializeField] float airTimeThreshold = 1f;
	[SerializeField] Text airTimeText;
	
	bool countingAirTime;
	float airTimeClock;
	float amountRotated;


	void Update () {
		if(!surfer.isInWater && !countingAirTime) {
			countingAirTime = true;
			amountRotated = 0f;
		}

		if(countingAirTime) {
			if(surfer.rotating) {
				amountRotated += Time.deltaTime * surfer.rotationSpeed;
			}
			airTimeClock += Time.deltaTime;
		}

		if(countingAirTime && surfer.isInWater) {
			countingAirTime = false;
			if(amountRotated > 680f) {
				airTimeText.text = string.Format("720° {0:.00}s!!", airTimeClock);
			} else if(amountRotated > 340f) {
				airTimeText.text = string.Format("360° {0:.00}s!", airTimeClock);
			} else if(airTimeClock > airTimeThreshold) {
				airTimeText.text = string.Format("AIR TIME {0:.00}s", airTimeClock);
			}
			airTimeClock = 0f;
		}

	}
}
