using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class desktopImage : MonoBehaviour {
	public List<Texture> textureOptions = new List<Texture>();
	RawImage image;
	// Use this for initialization
	void Start () {
		image = GetComponent<RawImage>();
		// pick a random number for which image should be displayed on this desktop
		int randomNumber = Random.Range(0, textureOptions.Count);
		image.texture = textureOptions[randomNumber];
	}
}
