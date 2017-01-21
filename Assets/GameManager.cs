using UnityEngine;

public class GameManager : MonoBehaviour {

	public static GameManager instance;
	
	public Jump surfer; 

	void Awake() {
		if(instance != null) {
			DestroyImmediate(gameObject);
			return;
		}
		instance = this;
	}
	
	public void Restart() {
		surfer.Restart();
	}
}
