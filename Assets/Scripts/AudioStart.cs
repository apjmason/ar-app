using UnityEngine;
using System.Collections;

public class AudioStart : MonoBehaviour {

	public AudioClip clip;

	// Use this for initialization
	void Start () {
		AudioSource.PlayClipAtPoint (clip, transform.position);
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
