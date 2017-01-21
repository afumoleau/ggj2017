using DG.Tweening;
using UnityEngine;

public class GameManager : MonoBehaviour {

	public static GameManager instance;
	
	[SerializeField] Jump surfer; 

	void Awake() {
		if(instance != null) {
			DestroyImmediate(gameObject);
			return;
		}
		instance = this;
	}

	void Start() {
		surfer.gameObject.SetActive(false);
	}
	
	public void Restart() {
		surfer.Restart();
	}

	[SerializeField] RectTransform startUI;
	[SerializeField] RectTransform gameSelectionUI;
	[SerializeField] RectTransform tutorialUI;
	[SerializeField] RectTransform highscoresUI;
	[SerializeField] RectTransform gameUI;

	public void ShowGameModeSelection() {
		var sequence = DOTween.Sequence();
		sequence.AppendInterval(1f);
		sequence.AppendCallback(() => {
			startUI.gameObject.SetActive(false);
			gameSelectionUI.gameObject.SetActive(true);
		});
	}
	public void ShowHighscore() {
		var sequence = DOTween.Sequence();
		sequence.AppendInterval(1f);
		sequence.AppendCallback(() => {
			startUI.gameObject.SetActive(false);
			gameSelectionUI.gameObject.SetActive(false);
			tutorialUI.gameObject.SetActive(false);
			highscoresUI.gameObject.SetActive(true);
			gameUI.gameObject.SetActive(false);
		});
	}
	public void ShowTutorial() {
		var sequence = DOTween.Sequence();
		sequence.AppendInterval(1f);
		sequence.AppendCallback(() => {
			startUI.gameObject.SetActive(false);
			gameSelectionUI.gameObject.SetActive(false);
			tutorialUI.gameObject.SetActive(true);
			highscoresUI.gameObject.SetActive(false);
			gameUI.gameObject.SetActive(false);
		});
	}
	public void StartGame() {
		var sequence = DOTween.Sequence();
		sequence.AppendInterval(1f);
		sequence.AppendCallback(() => {
			startUI.gameObject.SetActive(false);
			gameSelectionUI.gameObject.SetActive(false);
			tutorialUI.gameObject.SetActive(false);
			highscoresUI.gameObject.SetActive(false);
			gameUI.gameObject.SetActive(true);
			surfer.gameObject.SetActive(true);
		});

	}
	public void Quit() {
		Application.Quit();
	}
}
