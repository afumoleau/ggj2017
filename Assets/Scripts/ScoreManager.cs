using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using DG.Tweening;
using Firebase.Database;
using System.Collections.Generic;

public class ScoreManager : MonoBehaviour {

	enum Trick {
		None,
		Air,
		SevenTwenty,
		ThreeSixty
	}

	[SerializeField] Jump surfer;
	[SerializeField] float airTimeThreshold = 1f;
	[SerializeField] Text airTimeText;
	[SerializeField] Text comboText;
	[SerializeField] Text scoreText;
	
	bool countingAirTime;
	float airTimeClock;
	float amountRotated;
	bool landedOnce = false;
	bool aliveLastFrame = true;
	Trick lastTrick;
	int combo;
	int score;
	
	[SerializeField] UnityEvent OnFail;
	[SerializeField] UnityEvent OnJump;
	[SerializeField] UnityEvent On360;
	[SerializeField] UnityEvent On720;

	public string playerName { get; set; }

	void Start() {
	}

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
				if(amountRotated > 540f) {
					registerCombo(Trick.SevenTwenty);
					showText(string.Format("720° {0:.00}s!!", airTimeClock));
					On720.Invoke();
					updateScore(Mathf.FloorToInt(score + 720 * combo * airTimeClock));
				} else if(amountRotated > 180f) {
					registerCombo(Trick.ThreeSixty);
					showText(string.Format("360° {0:.00}s!", airTimeClock));
					On360.Invoke();
					updateScore(Mathf.FloorToInt(score + 360 * combo * airTimeClock));
				} else if(airTimeClock > airTimeThreshold) {
					registerCombo(Trick.Air);
					showText(string.Format("AIR TIME {0:.00}s", airTimeClock));
					OnJump.Invoke();
					updateScore(Mathf.FloorToInt(score + 90 * combo * airTimeClock));
				}
			}
			airTimeClock = 0f;
		}

		if(aliveLastFrame && !surfer.alive) {
			lastTrick = Trick.None;
			combo = 0;
			showText(string.Format("FAIL", airTimeClock));
			OnFail.Invoke();
			publishScore(playerName, score);
			updateScore(0);
		}
		aliveLastFrame = surfer.alive;
	}
	
	public class LeaderboardEntry {
		public string name;
		public int score = 0;

		public LeaderboardEntry() {
		}

		public LeaderboardEntry(string name, int score) {
			this.name = name;
			this.score = score;
		}

		public Dictionary<string, System.Object> ToDictionary() {
			Dictionary<string, System.Object> result = new Dictionary<string, System.Object>();
			result["name"] = name;
			result["score"] = score;
			return result;
		}
	}

	public void publishScore() {
		publishScore(playerName, score);
	}

	public void publishScore(string name, int score) {
		if(name == "" || name == null) {
			name = "anonymous";
		}
		DatabaseReference reference = FirebaseDatabase.DefaultInstance.RootReference;
		string key = reference.Child("scores").Push().Key;
		LeaderboardEntry entry = new LeaderboardEntry(name, score);
		Dictionary<string, System.Object> entryValues = entry.ToDictionary();
		Dictionary<string, System.Object> childUpdates = new Dictionary<string, System.Object>();
		childUpdates["/scores/" + key] = entryValues;
		childUpdates["/user-scores/" + name + "/" + key] = entryValues;
		reference.UpdateChildrenAsync(childUpdates);
	}

	public void updateScore(int newScore) {
		float localScore = score;
		score = newScore;
		DOTween.To(() => localScore, (val) => { localScore = val; scoreText.text = "SCORE:"+Mathf.FloorToInt(localScore); }, score, 0.5f);
	}

	void registerCombo(Trick trick) {
		if(lastTrick == trick) {
			combo++;
		} else {
			combo = 1;
			lastTrick = trick;
		}

		if(combo < 2)
			return;

		comboText.text = "COMBO X" + combo;
		comboText.transform.localScale = Vector3.zero;
		comboText.transform.eulerAngles = Vector3.zero;
		var sequence = DOTween.Sequence();
		sequence.AppendInterval(0.5f);
		sequence.Append(comboText.transform.DOScale(Vector3.one, 0.5f).SetEase(Ease.OutBack));
		sequence.Insert(0f, comboText.transform.DORotate(Vector3.forward * -10f, 0.5f).SetEase(Ease.OutBack));
		sequence.AppendInterval(1.5f);
		sequence.Append(comboText.transform.DOScale(Vector3.zero, 0.2f).SetEase(Ease.InBack));
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
