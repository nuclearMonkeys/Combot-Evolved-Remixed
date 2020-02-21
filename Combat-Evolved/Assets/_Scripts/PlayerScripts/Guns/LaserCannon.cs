using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserCannon : GunBase
{
    [Header("Line Renderers")]
    public LineRenderer lr1;
    public LineRenderer lr2;
    public LineRenderer lightningRenderer;
    public ParticleSystem suckParticle;

    [Header("Damage Variables")]
    // damage per second
    public float laserDamage = 1;
    public float endDamageMultiplier = 4;
    public float zappingTime = 3;
    float updateSpeedSecs = 0.05f;

    bool zapping = false;
    public int lightningPoints = 5;

    public override void ExtendedStart()
    {
        transform.localPosition = new Vector3(-0.6f, 0f, 0f);

        lr1.enabled = false;
        lr2.enabled = false;
        lightningRenderer.enabled = false;
        lightningRenderer.useWorldSpace = false;
    }

    public override void ExtendedFireBullet(BulletBase bulletPrefab)
    {
        if(!zapping)
        {
            StartCoroutine(Zap());
        }
    }

    IEnumerator Zap()
    {
        zapping = true;
        yield return new WaitForSeconds(zappingTime);
        zapping = false;
    }

    IEnumerator FireEndLaser() 
    {
        lr1.startWidth = 0.07f;
        lr2.startWidth = 0.2f;

        // float offset = (lr1.transform.position - lr1.GetPosition(0)).magnitude;
        // Vector3 pos = new Vector3(transform.position.x, transform.position.y + offset, 0);
        GameObject clone = Instantiate(suckParticle, firePoint.transform.position, firePoint.transform.rotation, this.transform).gameObject;
        yield return new WaitForSeconds(zappingTime/3);

        //Lerp the lr.startWidth
        float elapsed = 0;

        CameraController.instance.ShakeCamera();

        Destroy(clone);
        while (elapsed < updateSpeedSecs) 
        {
            elapsed += Time.deltaTime;
            lr1.startWidth = Mathf.Lerp(lr1.startWidth, 1f, elapsed / updateSpeedSecs);
            lr2.startWidth = Mathf.Lerp(lr2.startWidth, 1.5f, elapsed / updateSpeedSecs);
            yield return new WaitForEndOfFrame();
        }

        StartCoroutine(Lightning());

        float refDamage = laserDamage;
        laserDamage = laserDamage * endDamageMultiplier;
        lr1.startWidth = 1f;

        yield return new WaitForSeconds(zappingTime * 1.7f/3);
        laserDamage = 0;

        elapsed = 0;
        while (elapsed < updateSpeedSecs) 
        {
            elapsed += Time.deltaTime;
            lr1.startWidth = Mathf.Lerp(1f, 0.01f, elapsed / updateSpeedSecs);
            lr2.startWidth = Mathf.Lerp(1.5f, 0.02f, elapsed / updateSpeedSecs);
            yield return new WaitForEndOfFrame();
        }

        laserDamage = refDamage;
    }

    IEnumerator Lightning() 
    {
        lightningRenderer.enabled = true;
        
        while(zapping) {
            float distance = (lr1.GetPosition(0) - lr1.GetPosition(1)).magnitude;
            float offset = (lr1.GetPosition(0) - lr1.transform.position).magnitude;
            lightningRenderer.positionCount = (int)(distance * 2);
            lightningPoints = lightningRenderer.positionCount;

            for (int i = 0; i < lightningPoints; i++) 
            {
                Vector3 pos = new Vector3(i * distance / (lightningPoints - 1) + offset, Random.Range(-2.1f, 2.1f), 0);
                lightningRenderer.SetPosition(i, pos);
            }
            yield return null;
        }
    }

    private void Update()
    {
        if(zapping)
        {
            if(!lr1.enabled)
                StartCoroutine(FireEndLaser());

            lr1.enabled = true;
            lr2.enabled = true;
            
            lr1.SetPosition(0, firePoint.transform.position);
            lr2.SetPosition(0, firePoint.transform.position);
            lightningRenderer.SetPosition(0, firePoint.transform.position);
            RaycastHit2D playerhit = Physics2D.Raycast(firePoint.transform.position, transform.right, 100000, 1 << LayerManager.TANKBODY);
            if (playerhit)
            {
                lr1.SetPosition(1, playerhit.point);
                lr2.SetPosition(1, playerhit.point);
                playerhit.collider.GetComponent<PlayerHealth>().TakeDamage(laserDamage * Time.deltaTime, owner);
            }
            else
            {
                RaycastHit2D blockhit = Physics2D.Raycast(firePoint.transform.position, transform.right, 100000, 1 << LayerManager.BLOCK | 1 << LayerManager.STAGEHAZARD);
                if (blockhit)
                {
                    lr1.SetPosition(1, blockhit.point);
                    lr2.SetPosition(1, blockhit.point);
                }
            }
        }
        else
        {
            lr1.enabled = false;
            lr2.enabled = false;
            lightningRenderer.enabled = false;
        }
    }
}