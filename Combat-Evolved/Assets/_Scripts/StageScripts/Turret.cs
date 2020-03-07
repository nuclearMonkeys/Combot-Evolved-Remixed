using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Turret : MonoBehaviour
{
    public GunBase basicGun;
    public GunBase[] guns;
    public float rotateSpeed = 90;
    GunBase gun;
    float behaviorDuration = 3;
    public BulletBase bulletPrefab;
    int circularFireRounds = 3;
    int circularFireBullets = 15;

    // target to rotate
    GameObject target;

    delegate IEnumerator behavior();
    List<behavior> behaviors;
    behavior lastBehavior;

    private void Start()
    {
        gun = GetComponentInChildren<GunBase>();
        behaviors = new List<behavior>();
        behaviors.Add(RepeatedPhase);
        behaviors.Add(CircularPhase);
        behaviors.Add(TargetPhase);
        StartCoroutine(Daemon());
    }

    private void Update()
    {
        if(target == null)
        {
            transform.Rotate(new Vector3(0, 0, rotateSpeed * Time.deltaTime));
        }
        else
        {
            Vector3 vectorToTarget = target.transform.position - transform.position;
            float angle = Mathf.Atan2(vectorToTarget.y, vectorToTarget.x) * Mathf.Rad2Deg;
            Quaternion q = Quaternion.AngleAxis(angle, Vector3.forward);
            transform.rotation = Quaternion.Slerp(transform.rotation, q, Time.deltaTime * 1);
        }
    }

    IEnumerator Daemon()
    {
        while(true)
        {
            behavior nextBehavior = null;
            do
            {
                nextBehavior = behaviors[(int)(Random.value * behaviors.Count)];
            } while (nextBehavior == lastBehavior);
            lastBehavior = nextBehavior;
            // loops through behaviors
            yield return nextBehavior();
        }
    }

    IEnumerator RepeatedPhase()
    {
        // switch to a random gun
        SwitchWeapons();
        float time = 0;
        // keep firing
        while(time < behaviorDuration)
        {
            gun.FireBullet(bulletPrefab);
            yield return new WaitForSeconds(gun.fireRate);
            time += gun.fireRate;
        }        
    }

    void SwitchWeapons(bool reset = false)
    {
        // get a random gun
        GunBase newGunPrefab = guns[(int)(Random.value * guns.Length)];
        if (reset)
            newGunPrefab = basicGun;
        // Instantiate new Gun to replace old Gun
        Transform headTransform = transform;
        Destroy(gun.gameObject);
        gun = Instantiate(newGunPrefab, headTransform);
        gun.transform.localPosition = Vector2.zero;
    }

    IEnumerator CircularPhase()
    {
        // switch back to basic gun
        SwitchWeapons(true);
        // circular fire for rounds
        for(int i = 0; i < circularFireRounds; i++)
        {
            CircularFire();
            yield return new WaitForSeconds(behaviorDuration / circularFireRounds);
        }
    }

    void CircularFire()
    {
        CameraController.instance.ShakeCamera();
        for(int i = 0; i < circularFireBullets; i++)
        {
            transform.Rotate(new Vector3(0, 0, 360 / circularFireBullets));
            gun.FireBullet(bulletPrefab);
        }
    }

    IEnumerator TargetPhase()
    {
        List<GameObject> players = TankSelectionManager.instance.players;
        foreach(GameObject player in players)
        {
            if(player.activeSelf)
            {
                SwitchWeapons();
                StartCoroutine(RepeatedPhase());
                target = player;
                yield return new WaitForSeconds(behaviorDuration);
            }
        }
    }
}
