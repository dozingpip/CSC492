using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class toggleMaterial : MonoBehaviour {
	// the material that will be applied to this object.
	public Material mat;
	// (used raw images in canvases) the texture to be applied to an object
	public Texture texture;
	// default material shown before the correct material is applied
	public Material defaultMat = null;
	// (used raw images in canvases) default texture shown before the correct material is applied
	public Texture defaultTexture = null;
	// is the material/ texture applied or not?
	public bool toggled;

	void Start(){
		if(transform.childCount>0){
			foreach(Transform child in transform){
				child.gameObject.AddComponent(typeof(toggleMaterial));
			}
		}else if(GetComponent<Renderer>()!=null){
			defaultMat = Resources.Load("Default") as Material;
			mat = GetComponent<Renderer>().material;
			GetComponent<Renderer>().material = defaultMat;
			toggled = false;
		}else if(GetComponent<RawImage>()!=null){
			defaultTexture = Resources.Load("blank") as Texture;
			texture = GetComponent<RawImage>().texture;
			GetComponent<RawImage>().texture = defaultTexture;
			toggled = false;
		}else{
			Debug.Log("no child object, no material, whyd you put this script on here?");
		}
	}

	// toggle the material/ texture of this object (and or all its children)
	public void toggleMat(){
		if(transform.childCount>0){
			foreach(Transform child in transform){
				child.GetComponent<toggleMaterial>().toggleMat();
			}
		}else{
			if(!toggled){
				if(GetComponent<RawImage>()==null)
					GetComponent<Renderer>().material = mat;
				else
					GetComponent<RawImage>().texture = texture;
				toggled = true;
			}else{
				if(GetComponent<RawImage>()==null)
					GetComponent<Renderer>().material = defaultMat;
				else
					GetComponent<RawImage>().texture = defaultTexture;
				toggled = false;
			}
		}
	}
}
