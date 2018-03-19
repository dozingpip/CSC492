using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class toggleMaterial : MonoBehaviour {
	public Material mat;
	public Texture texture;
	public Material defaultMat = null;
	public Texture defaultTexture = null;
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
