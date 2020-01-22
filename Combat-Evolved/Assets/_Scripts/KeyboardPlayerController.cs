using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * This script is used for development purposes only. Players
 * should only use controllers when playing the final game.
 * This is also using the OLD Unity input system.
 */

public class KeyboardPlayerController : MonoBehaviour
{
    [Header("Tank GameObjects")]

    [SerializeField] private GameObject body;
    [SerializeField] private GameObject head;
    [SerializeField] private GameObject firePoint;
    [SerializeField] private DefaultBullet bulletPrefab;

    [Header("Player Variables")]
    [SerializeField] private float m_movementSpeed = 5.0f;
    [SerializeField] private Vector2 direction;

    [Header("Dash Variables")]
    [SerializeField] private float dashSpeed;
    [SerializeField] private float startDashTime;
    
    // private variables. no touchy touchy.
    private bool isDashing = false;
    private float dashTime;
    
    public int maxCooldown = 60;
    private int cooldown = 0;

    private Rigidbody2D m_rigidbody;
    private Vector2 rotation;


    void Start() 
    {
        // Ugly ass code
        body =        this.GetComponentsInChildren<Transform>()[1].gameObject;
        head =        this.GetComponentsInChildren<Transform>()[2].gameObject;
        firePoint =   this.GetComponentsInChildren<Transform>()[4].gameObject;
        m_rigidbody = this.GetComponent<Rigidbody2D>();
        // dottedLine = new DottedLine();
        // trailRenderer.enabled = !trailRenderer.enabled;
        dashTime = startDashTime;
    }

    void Update() 
    {
        if(cooldown > 0)
            cooldown--;

        if(dashTime > 0)
            dashTime -= Time.deltaTime;

        Move();

        if (Input.GetAxis("Fire1") == 1) 
            Fire();

        if(Input.GetAxis("Fire3") == 1)
            Dash();

        Rotate();
    }

    public void Fire()
    {
        if(cooldown > 0)
            return;

        DefaultBullet clone = (DefaultBullet)Instantiate(bulletPrefab, firePoint.transform.position, firePoint.transform.rotation);
        clone.speed = 5f;
        Vector2 rotation = new Vector2(head.transform.rotation.x, head.transform.rotation.z);
        Destroy(clone.gameObject, 2f);
        cooldown = maxCooldown;
    }

    public void Rotate() 
    {
        // When you release stick on resting place
        Vector2 previousRotation = rotation;
        rotation = Input.mousePosition;
        rotation = Camera.main.ScreenToWorldPoint(rotation);

        float h = 0.0f;
        float v = 0.0f;

        if(rotation != Vector2.zero) {
            h = rotation.x;
            v = rotation.y;
        }

        float aim_angle = Mathf.Atan2(v, h) * Mathf.Rad2Deg;
        head.transform.rotation = Quaternion.AngleAxis(aim_angle, Vector3.forward);
    }

    public void Dash() 
    {
        if(dashTime <= 0 && direction != Vector2.zero)
            isDashing = true;
    }

    public void Move() 
    {
        StartCoroutine(boostEnumerator());
    }

    IEnumerator boostEnumerator() 
    {
        if (body == null)
            yield return null;
        // This allows the player to control the direction of the 'dash'
        Vector2 previousDirection = direction;
        direction = direction = new Vector2(Input.GetAxis("Horizontal"), 
            Input.GetAxis("Vertical"));

        // print(direction);

        float h = 0.0f;
        float v = 0.0f;

        if(direction != Vector2.zero) {
            h = direction.x * m_movementSpeed;
            v = direction.y * m_movementSpeed;
        } else {
            h = previousDirection.x * m_movementSpeed;
            v = previousDirection.y * m_movementSpeed;
        }
        
        // direction = previousDirection;
            
        float aim_angle = Mathf.Atan2(v, h) * Mathf.Rad2Deg;
        body.transform.rotation = Quaternion.AngleAxis(aim_angle, Vector3.forward);

        if(dashTime <= 0 && isDashing) {
            // trailRenderer.enabled = !trailRenderer.enabled;
            m_rigidbody.velocity = direction * dashSpeed;
            yield return new WaitForSecondsRealtime(0.5f);
            dashTime = startDashTime;
            isDashing = false;
        }
        else {
            m_rigidbody.velocity = direction * m_movementSpeed;
        }
            
        yield return null;
    }

    IEnumerator dashEnumerator() 
    {
        // This is for a regular dash. Planned for testing.
        // Not for final use.
        direction = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
        float h = direction.x * m_movementSpeed;
        float v = direction.y * m_movementSpeed;

        print(direction);

        if(dashTime <= 0 && isDashing) {
            m_rigidbody.velocity = direction * dashSpeed;
            dashTime = startDashTime;
            //yield return new WaitForSecondsRealtime(0.03f);
            isDashing = false;
        }
        else if(dashTime > 0) {
            yield return null;
        }
        else {
            m_rigidbody.velocity = direction * m_movementSpeed;
            float aim_angle = Mathf.Atan2(v, h) * Mathf.Rad2Deg;
            body.transform.rotation = Quaternion.AngleAxis(aim_angle, Vector3.forward);
        }

        yield return null;
    }

    /*
     * Determines if the absolute value of x and y coordinates
     * of the left stick position (direction) is
     * greater than the value of 
     * some threshold in the function
    */
    private bool withinThreshold(Vector2 vec) 
    {
        float threshold = 0.5f;
        return Mathf.Abs(vec.x) > threshold &&
             Mathf.Abs(vec.y) > threshold;
    }
}
