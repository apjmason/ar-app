using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class MediaTrigger : MonoBehaviour {

	public bool guiShow;
	public bool played;
	public Sprite HistoricImage;
	public AudioClip HisoricAudio;
	public Material blue;
	MeshRenderer my_rend;
	public Image image;

	AudioClip clip;
//	float posx = Screen.width / 6;
//	float posy = Screen.height / 6;

	public static int currentSprite = 0;
	string resourceName = "Additional Docs";
	public Sprite[] additionalDocs;

	void Awake()
	{
		if (resourceName != "")
			additionalDocs = Resources.LoadAll<Sprite> (resourceName);
	}
		
	void Start() {
		my_rend = GetComponent<MeshRenderer> ();
	}

	public void OnTriggerEnter(Collider other) {		
		if (other.gameObject.CompareTag ("Player")) {
			my_rend.enabled = false;
			guiShow = true;
			if (played == false) {
				AudioSource audio = GetComponent<AudioSource>();
				clip = audio.clip;
				AudioSource.PlayClipAtPoint (clip, transform.position);
			}
			played = true;
			showImage ();
		}
	}

	public void OnTriggerExit(Collider other) {
		if (other.gameObject.CompareTag ("Player")) {
			my_rend.enabled = true;
			guiShow = false;
			if (played == true) {
				my_rend.material = blue;
			}
		}
	}

	public void showImage()
	{
		if (guiShow == true) {
			image.sprite = HistoricImage;
			image.enabled = true;
		} else {
			image.enabled = false;
		}
	}

	public void OnClickChangeBackground()
	{
		foreach (Sprite doc in additionalDocs)
		{
			image.GetComponent<Image> ().sprite = doc;
		} 
	}
}

//	void OnGUI() {
//		Rect position = new Rect (posx, posy, 1024, 512);
//
//		if (!HistoricImage) {
//			Debug.LogError ("Assign a Texture in the inspector.");
//			return;
//		}
//		if (guiShow == true) {
////			GUI.color = new Color() { a = 0.5f };
//			GUI.DrawTexture (position, HistoricImage, ScaleMode.ScaleToFit, true, 0);
//		}
//
//		if (GUI.Button (new Rect (posx + 924, posy + 612, 100, 50), "Next Image")) { 
//			foreach (Texture doc in AdditionalDocs) {
//				GUI.DrawTexture (position, doc, ScaleMode.ScaleToFit, true, 0);
//			}
//
//		}

//	}
//
//}