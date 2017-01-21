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


	[SerializeField] AudioSource intro;
	[SerializeField] AudioSource music;

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

		var volume = music.volume;
		var audioSequence = DOTween.Sequence();
		audioSequence.Append(intro.DOFade(0, 1f));
		audioSequence.AppendCallback(() => { music.volume = 0f; music.Play(); });
		audioSequence.Append(music.DOFade(volume, 1f));
	}

	public void Quit() {
		Application.Quit();
	}
}
