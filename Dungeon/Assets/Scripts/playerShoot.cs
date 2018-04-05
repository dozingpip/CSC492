using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerShoot : MonoBehaviour {
	public GameObject bulletPrefab;
	public Transform bulletSpawn;
	
	// Update is called once per frame
	void Update () {
		// when the player left clicks, spawn a bullet at the given spawn location
		if(Input.GetMouseButtonUp(0)){
			GameObject bullet = GameObject.Instantiate(bulletPrefab, bulletSpawn.position, bulletSpawn.transform.rotation);
			// give the bullet some forward velocity
			bullet.GetComponent<Rigidbody>().velocity = bullet.transform.forward * 6;
			// destroy the bullet after 2 seconds
			Destroy(bullet, 2.0f);
		}
	}
}
