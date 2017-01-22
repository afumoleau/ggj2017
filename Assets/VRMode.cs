using UnityEngine;
using UnityEngine.UI;

public class VRMode : MonoBehaviour {

	public Camera classicCamera;
	public Camera vrCamera;
	public RawImage leftEye;
	public RawImage rightEye;
	public GameObject vrCanvas;

	void OnEnable() {
		var rt = new RenderTexture(1024, 1024, 32, RenderTextureFormat.ARGB32);

		classicCamera.targetTexture = rt;
		
		leftEye.texture = rt;
		rightEye.texture = rt;
		vrCanvas.SetActive(true);
	}
	void OnDisable() {
		//var rt = new RenderTexture(1024, 1024, 32, RenderTextureFormat.ARGB32);
		classicCamera.targetTexture = null;
		if(vrCanvas != null) {
			vrCanvas.SetActive(false);
		}
	}
}
