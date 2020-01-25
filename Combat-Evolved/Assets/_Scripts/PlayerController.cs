using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [Header("Tank GameObjects")]
    [SerializeField] private GameObject body;
    [SerializeField] private GameObject head;
    [SerializeField] private GameObject firePoint;
    [SerializeField] private DefaultBullet bulletPrefab;

    [Header("Player Variables")]
    [SerializeField] private float m_movementSpeed = 5.0f;
    [SerializeField] private Vector2 direction;
    [SerializeField] private float fireRate = .5f;

    [Header("Dash Variables")]
    [SerializeField] private float dashSpeed;
    [SerializeField] private float dashDuration;
    [SerializeField] private bool dashRotate = false;

    // private variables. no touchy touchy.
    private bool isDashing = false;
    private bool canFire = true;

    // Components
    private Rigidbody2D m_rigidbody;

    void Start() 
    {
        body =        transform.Find("Body").gameObject;
        head =        transform.Find("Head").gameObject;
        firePoint =   head.transform.Find("Barrel").Find("FirePoint").gameObject;
        m_rigidbody = this.GetComponent<Rigidbody2D>();
    }

    void Update() 
    {
        m_rigidbody.velocity = direction * m_movementSpeed;
    }

    public void Move(InputAction.CallbackContext context) 
    {
        // if not dashing, move according to joystick
        if(!isDashing || dashRotate)
        {
            direction = context.ReadValue<Vector2>();
        }

        // prevent snapping
        if(direction.magnitude > .1f)
        {
            body.transform.right = direction.normalized;
        }
    }

    public void Rotate(InputAction.CallbackContext context) 
    {
        // When you release stick on resting place
        Vector2 rotation = context.ReadValue<Vector2>();
        // prevent snapping
        if(rotation.magnitude > .1f)
        {
            head.transform.right = rotation;
        }
    }

    public void Fire()
    {
        if(canFire)
        {
            StartCoroutine(fireEnumerator());
        }
    }

    IEnumerator fireEnumerator()
    {
        canFire = false;
        DefaultBullet clone = (DefaultBullet)Instantiate(bulletPrefab, firePoint.transform.position, firePoint.transform.rotation);
        Destroy(clone.gameObject, 2f);
        yield return new WaitForSeconds(fireRate);
        canFire = true;
    }

    public void Dash(InputAction.CallbackContext context) 
    {
        if(!isDashing)
        {
            StartCoroutine(dashEnumerator(context));
        }
    }

    IEnumerator dashEnumerator(InputAction.CallbackContext context) 
    {
        isDashing = true;
        float oldSpeed = m_movementSpeed;
        m_movementSpeed = dashSpeed;
        yield return new WaitForSeconds(dashDuration);
        m_movementSpeed = oldSpeed;
        isDashing = false;
    }
}