using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using DG.Tweening;

public class ScoreManager : MonoBehaviour {

	[SerializeField] Jump surfer;
	[SerializeField] float airTimeThreshold = 1f;
	[SerializeField] Text airTimeText;
	
	bool countingAirTime;
	float airTimeClock;
	float amountRotated;
	bool landedOnce = false;
	bool aliveLastFrame = true;
	
	[SerializeField] UnityEvent OnFail;
	[SerializeField] UnityEvent OnJump;
	[SerializeField] UnityEvent On360;
	[SerializeField] UnityEvent On720;

	void Update () {
		if(surfer.isInWater && !landedOnce) {
			landedOnce = true;
		}

		if(!surfer.isInWater && !countingAirTime && landedOnce) {
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
			if(surfer.isStanding) {
				if(amountRotated > 680f) {
					showText(string.Format("720° {0:.00}s!!", airTimeClock));
					On720.Invoke();
				} else if(amountRotated > 340f) {
					showText(string.Format("360° {0:.00}s!", airTimeClock));
					On360.Invoke();
				} else if(airTimeClock > airTimeThreshold) {
					showText(string.Format("AIR TIME {0:.00}s", airTimeClock));
					OnJump.Invoke();
				}
			}
			airTimeClock = 0f;
		}

		if(aliveLastFrame && !surfer.alive) {
			showText(string.Format("FAIL", airTimeClock));
			OnFail.Invoke();
		}
		aliveLastFrame = surfer.alive;
	}

	void showText(string text) {
		airTimeText.text = text;
		airTimeText.transform.localScale = Vector3.zero;
		airTimeText.transform.eulerAngles = Vector3.zero;
		var sequence = DOTween.Sequence();
		sequence.Append(airTimeText.transform.DOScale(Vector3.one, 0.5f).SetEase(Ease.OutBack));
		sequence.Insert(0f, airTimeText.transform.DORotate(Vector3.forward * -10f, 0.5f).SetEase(Ease.OutBack));
		sequence.AppendInterval(1.5f);
		sequence.Append(airTimeText.transform.DOScale(Vector3.zero, 0.2f).SetEase(Ease.InBack));
	}
}
