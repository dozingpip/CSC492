using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunController : MonoBehaviour {

    // bullet object to instantiate
    public GameObject _bulletPrefab;

    // where to put the bullet when it is first fired
    public Transform _spawnPoint;

    private void Update()
    {
        //Shoot a bullet based on key inputb
        if (Input.GetKeyDown("f"))
        {
            fireGun();
        }
    }



    void fireGun()
    {
        //Create a bullet
        GameObject bullet;
        // clone the bullet prefab, at the spawnpoint position and rotation (not a child)
        bullet = Instantiate(_bulletPrefab, _spawnPoint.position, _spawnPoint.rotation);
        // Launch it in the left direction
        Rigidbody bulletRB = bullet.GetComponent<Rigidbody>();
        bulletRB.velocity = transform.TransformDirection(Vector3.left * 10);

        // Destroy it 3 later
        Destroy(bullet, 3);
    }
}
