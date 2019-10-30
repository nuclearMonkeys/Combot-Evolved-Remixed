using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [Header("Tank Variables")]
    public PlayerInput playerInput;
    [SerializeField] private float m_movementSpeed = 5.0f;
    [SerializeField] private GameObject body;
    [SerializeField] private GameObject head;
    [SerializeField] private GameObject firePoint;
    [SerializeField] private DefaultBullet bulletPrefab;
    [SerializeField] private float deadzone = 0.01f;
    [SerializeField] private Vector2 direction;

    [Header("Dash Variables")]
    [SerializeField] private float dashSpeed;
    [SerializeField] private float startDashTime;
    public bool isDashing = false;
    public float dashTime;
    

    public int maxCooldown = 60;
    public int cooldown = 0;

    public Rigidbody2D m_rigidbody;
    private Vector2 rotation;

    void Awake()
    {
        playerInput = new PlayerInput();
    }

    void Start() 
    {
        // Ugly ass code
        body =        GetComponentsInChildren<Transform>()[1].gameObject;
        head =        GetComponentsInChildren<Transform>()[2].gameObject;
        firePoint =   GetComponentsInChildren<Transform>()[4].gameObject;
        m_rigidbody = GetComponent<Rigidbody2D>();
        dashTime = startDashTime;
    }

    void Update() 
    {
        if(cooldown > 0)
            cooldown--;

        if(dashTime > 0)
            dashTime -= Time.deltaTime;
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

    public void Move(InputAction.CallbackContext context) 
    {
        StartCoroutine(dashEnumerator(context));
    }

    public void Rotate(InputAction.CallbackContext context) 
    {
        rotation = context.ReadValue<Vector2>();

        float h = rotation.x;
        float v = rotation.y;

        float aim_angle = Mathf.Atan2(v, h) * Mathf.Rad2Deg;
        head.transform.rotation = Quaternion.AngleAxis(aim_angle, Vector3.forward);
    }

    public void Dash(InputAction.CallbackContext context) 
    {
        if(dashTime <= 0)
            isDashing = true;
    }

    IEnumerator dashEnumerator(InputAction.CallbackContext context) 
    {
        direction = context.ReadValue<Vector2>();
        float h = direction.x * m_movementSpeed;
        float v = direction.y * m_movementSpeed;

        // If you want a regular dash
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

        // If you want a "curved" dash
        /*
            if(dashTime <= 0 && isDashing) {
                m_rigidbody.velocity = direction * dashSpeed;
                dashTime = startDashTime;
                yield return new WaitForSecondsRealtime(0.03f);
                isDashing = false;
            }
            else {
                m_rigidbody.velocity = direction * m_movementSpeed;
            }

            float aim_angle = Mathf.Atan2(v, h) * Mathf.Rad2Deg;
            body.transform.rotation = Quaternion.AngleAxis(aim_angle, Vector3.forward);
         */

        yield return null;
    }
}