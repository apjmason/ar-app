using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class MediaTrigger : MonoBehaviour {

	public bool guiShow;
	public bool played;
	public Sprite HistoricImage;
	public AudioClip HistoricAudio;
	public Material blue;
	MeshRenderer my_rend;
	public Image image;
	public Button RightB;
	public Button LeftB;

	AudioClip clip;

	public static int currentSprite = 0;
	public string resourceName = "";
	private Sprite[] additionalDocs = null;
	Canvas canvas;

	void Awake() {
		canvas = GameObject.Find ("Canvas").GetComponent<Canvas> ();
	}
		
	void Start() {
		my_rend = GetComponent<MeshRenderer> ();
	}

	public void OnTriggerEnter(Collider other) {		
		if (resourceName != "") {
			additionalDocs = Resources.LoadAll<Sprite> (resourceName);
		}

		if (other.gameObject.CompareTag ("Player")) {
			my_rend.enabled = false;
			guiShow = true;
			if (played == false) {
				AudioSource audio = GetComponent<AudioSource>();
				clip = audio.clip;
				AudioSource.PlayClipAtPoint (clip, transform.position);
			}
			played = true;
			showCanvas ();
		}
	}

	public void OnTriggerExit(Collider other) {
		if (other.gameObject.CompareTag ("Player")) {
			my_rend.enabled = true;
			guiShow = false;
			if (played == true) {
				my_rend.material = blue;
			}
			showCanvas ();
			additionalDocs = null;
		}
	}

	private void showCanvas()
	{
		if (guiShow == true) {
			image.sprite = HistoricImage;
			image.enabled = true;
			//enable buttons
			canvas.enabled = true;
		} else {
			image.enabled = false;
			canvas.enabled = false;
		}
	}


//	public void OnClickChangeBackground()
//	{
//		foreach (Sprite doc in additionalDocs)
//		{
//			Debug.Log (doc.name);
//			image.sprite = doc;
//		} 
//	}

	void Update() {
		// check for if in sphere?
		if (Input.GetKeyDown (KeyCode.R)) {
			if (currentSprite < additionalDocs.Length) {
				image.sprite = additionalDocs [currentSprite];
				currentSprite++;
			} else {
				currentSprite = 0;
			}
		} 		

		// fix to make not play twice
		if (Input.GetKeyDown (KeyCode.L)) {
			AudioSource audio = GetComponent<AudioSource>();
			audio.Stop ();
			AudioSource.PlayClipAtPoint (HistoricAudio, transform.position);

		}

	}
}
	