using UnityEngine;
using UnityEngine.UI;

public class WindowsText : MonoBehaviour {

	public string desktopText;

	void Start () {
		if (!Application.isMobilePlatform) {
			GetComponent<Text>().text = desktopText;
		}
	}
}
