using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunController : MonoBehaviour {

    public GameObject _bulletPrefab;
    public Transform _spawnPoint;

    private void Update()
    {
        //Shoot a bullet based on key inpute
        if (Input.GetKeyDown("f"))
        {
            fireGun();
        }
    }


    /// <summary>
    /// Shoots a bullet in the up direction
    /// </summary>
    void fireGun()
    {
        //Create a bullet
        GameObject bullet;
        bullet = Instantiate(_bulletPrefab, _spawnPoint.position, _spawnPoint.rotation);
        //Launch it in the right direction
        Rigidbody bulletRB = bullet.GetComponent<Rigidbody>();
        bulletRB.velocity = transform.TransformDirection(Vector3.left * 10);

        //Destroy it when done
        Destroy(bullet, 3);
    }
}
