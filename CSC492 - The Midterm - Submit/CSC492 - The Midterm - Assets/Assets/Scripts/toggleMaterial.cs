using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class toggleMaterial : MonoBehaviour {
	// the material that will be applied to this object.
	private Material mat = null;
	// (used raw images in canvases) the texture to be applied to an object
	private Texture texture = null;
	// default material shown before the correct material is applied
	private Material defaultMat = null;
	// (used raw images in canvases) default texture shown before the correct material is applied
	private Texture defaultTexture = null;
	// is the material/ texture applied or not?
	private bool toggled;

	// if this object has a renderer component, this is it
	private Renderer renderer;
	// if this object has a rawImage component, this is it
	private RawImage rawImage;

	void Start(){
		renderer = GetComponent<Renderer>();
		rawImage = GetComponent<RawImage>();
		// make sure the toggle material script gets to all of this object's children
		if(transform.childCount>0){
			foreach(Transform child in transform){
				child.gameObject.AddComponent(typeof(toggleMaterial));
			}
		}
		if(renderer){
			setMaterial();
		}
		if(rawImage){
			setTexture();
		}
	}

	// set up the material to be whatever the default is and prepare to toggle
	void setMaterial(){
		defaultMat = Resources.Load("Default") as Material;
		mat = GetComponent<Renderer>().material;
		GetComponent<Renderer>().material = defaultMat;
		toggled = false;
	}

	// set up the texture to be whatever the default is and prepare to toggle
	void setTexture(){
		defaultTexture = Resources.Load("blank") as Texture;
		texture = GetComponent<RawImage>().texture;
		GetComponent<RawImage>().texture = defaultTexture;
		toggled = false;
	}

	// toggle the material/ texture of this object (and or all its children)
	public void toggleMat(){
		if(transform.childCount>0){
			foreach(Transform child in transform){
				child.GetComponent<toggleMaterial>().toggleMat();
			}
		}
		if(!toggled){
			if(mat)
				GetComponent<Renderer>().material = mat;
			else if(texture)
				GetComponent<RawImage>().texture = texture;
			toggled = true;
		}else if(toggled){
			if(mat)
				GetComponent<Renderer>().material = defaultMat;
			else if(texture)
				GetComponent<RawImage>().texture = defaultTexture;
			toggled = false;
		}
	}
}
