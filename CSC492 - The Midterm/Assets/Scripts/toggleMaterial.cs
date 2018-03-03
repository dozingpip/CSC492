using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class toggleMaterial : MonoBehaviour {
	public Material mat;
	public Material defaultMat = null;
	public bool toggled;

	void Start(){
		if(GetComponent<Renderer>()==null){
			if(transform.childCount>0){
				foreach(Transform child in transform){
					child.gameObject.AddComponent(typeof(toggleMaterial));
				}
			}else{
				Debug.Log("no child object, no material, whyd you put this script on here?");
			}
		}else{
			defaultMat = Resources.Load("Default") as Material;
			mat = GetComponent<Renderer>().material;
			GetComponent<Renderer>().material = defaultMat;
			toggled = false;
		}
	}

	public void toggleMat(){
		if(transform.childCount>0){
			foreach(Transform child in transform){
				child.GetComponent<toggleMaterial>().toggleMat();
			}
		}else{
			if(!toggled){
				GetComponent<Renderer>().material = mat;
				toggled = true;
			}else{
				GetComponent<Renderer>().material = defaultMat;
				toggled = false;
			}
		}
	}
}
