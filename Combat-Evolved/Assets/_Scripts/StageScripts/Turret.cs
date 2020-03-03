using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Turret : MonoBehaviour
{
    public float rotateSpeed = 90;
    GunBase gun;
    public BulletBase bulletPrefab;

    private void Start()
    {
        gun = GetComponentInChildren<GunBase>();
        StartCoroutine(RepeatedFire());
    }

    private void Update()
    {
        transform.Rotate(new Vector3(0, 0, rotateSpeed * Time.deltaTime));
    }

    IEnumerator RepeatedFire()
    {
        while(true)
        {
            gun.FireBullet(bulletPrefab);
            yield return new WaitForSeconds(gun.fireRate);
        }        
    }
}
