using UnityEngine;
using System.Collections;
using UnityEngine.UI;
//using UnityStandardAssets.Characters.FirstPerson;

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
//	float posx = Screen.width / 6;
//	float posy = Screen.height / 6;

	public static int currentSprite = 0;
	public string resourceName = "";
	private Sprite[] additionalDocs = null;
//	public FirstPersonController fpsController;
	Canvas canvas;

	void Awake()
	{

//		fpsController = GetComponent<FirstPersonController>();
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
//			DisableController();
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

//			EnableController ();
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

//	private void DisableController()
//	{
//		fpsController.enabled = false;
//	}
//
//	private void EnableController()
//	{
//		fpsController.enabled = true;
//	}

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