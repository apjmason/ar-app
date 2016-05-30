using UnityEngine;
using System.Collections;

public class MediaTrigger : MonoBehaviour {

	public bool guiShow;
	public bool played;
	public Texture HistoricImage;
	public Material blue;
	AudioClip clip;
	float posx = Screen.width / 6;
	float posy = Screen.height / 6;
	MeshRenderer my_rend;
//	public float alpha;

	void Start() {
		my_rend = GetComponent<MeshRenderer> ();
	}

	void OnTriggerEnter(Collider other) {		
		if (other.gameObject.CompareTag ("Player")) {
			my_rend.enabled = false;
			guiShow = true;
			if (played == false) {
				AudioSource audio = GetComponent<AudioSource>();
				clip = audio.clip;
				AudioSource.PlayClipAtPoint (clip, transform.position);
			}
			played = true;
		}
	}

	void OnTriggerExit(Collider other) {
		if (other.gameObject.CompareTag ("Player")) {
			my_rend.enabled = true;
			guiShow = false;
			if (played == true) {
				my_rend.material = blue;
			}
		}
	}

	void OnGUI() {
		if (!HistoricImage) {
			Debug.LogError ("Assign a Texture in the inspector.");
			return;
		}
		if (guiShow == true) {
//			GUI.color = new Color() { a = 0.5f };
			GUI.DrawTexture (new Rect (posx, posy, 1024, 512), HistoricImage, ScaleMode.ScaleToFit, true, 0);
		}
	}

}