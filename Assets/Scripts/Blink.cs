using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class Blink : MonoBehaviour {
	
	public void blink() {
		var delay = 0.1f;
		var image = GetComponent<Image>();
		var sequence = DOTween.Sequence();
		sequence.AppendCallback(() => {SetAlpha(image, 0.25f);});
		sequence.AppendInterval(delay);
		sequence.AppendCallback(() => {SetAlpha(image, 1f);});
		sequence.AppendInterval(delay);
		sequence.AppendCallback(() => {SetAlpha(image, 0.25f);});
		sequence.AppendInterval(delay);
		sequence.AppendCallback(() => {SetAlpha(image, 1f);});
		sequence.AppendInterval(delay);
		sequence.AppendCallback(() => {SetAlpha(image, 0.25f);});
		sequence.AppendInterval(delay);
		sequence.AppendCallback(() => {SetAlpha(image, 1f);});
	}

	void SetAlpha(Image image, float alpha) {
		var color = image.color;
		color.a = alpha;
		image.color = color;
	}
}
