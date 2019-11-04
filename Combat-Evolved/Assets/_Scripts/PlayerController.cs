using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [Header("Tank GameObjects")]
    public PlayerInput playerInput;

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
    private TrailRenderer trailRenderer;

    private LineRenderer line;

    void Awake()
    {
        playerInput = new PlayerInput();
    }

    void Start() 
    {
        // Ugly ass code
        body =        this.GetComponentsInChildren<Transform>()[1].gameObject;
        head =        this.GetComponentsInChildren<Transform>()[2].gameObject;
        firePoint =   this.GetComponentsInChildren<Transform>()[4].gameObject;
        m_rigidbody = this.GetComponent<Rigidbody2D>();
        trailRenderer = this.GetComponent<TrailRenderer>();
        line = this.GetComponent<LineRenderer>();
        // dottedLine = new DottedLine();
        // trailRenderer.enabled = !trailRenderer.enabled;
        dashTime = startDashTime;
    }

    void Update() 
    {
        DrawLine();

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
        StartCoroutine(boostEnumerator(context));
    }

    public void Rotate(InputAction.CallbackContext context) 
    {
        // When you release stick on resting place
        Vector2 previousRotation = rotation;
        rotation = context.ReadValue<Vector2>();

        float h = 0.0f;
        float v = 0.0f;

        //print(rotation);

        if(rotation != Vector2.zero && withinThreshold(rotation)) {
            h = rotation.x;
            v = rotation.y;
        } else {
            h = previousRotation.x;
            v = previousRotation.y;
        }

        
        float aim_angle = Mathf.Atan2(v, h) * Mathf.Rad2Deg;
        head.transform.rotation = Quaternion.AngleAxis(aim_angle, Vector3.forward);
    }

    public void Dash(InputAction.CallbackContext context) 
    {
        if(dashTime <= 0 && direction != Vector2.zero)
            isDashing = true;
    }

    IEnumerator boostEnumerator(InputAction.CallbackContext context) 
    {
    
        // This allows the player to control the direction of the 'dash'
        Vector2 previousDirection = direction;
        direction = context.ReadValue<Vector2>();

        // print(direction);

        float h = 0.0f;
        float v = 0.0f;

        if(direction != Vector2.zero && withinThreshold(direction)) {
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

    IEnumerator dashEnumerator(InputAction.CallbackContext context) 
    {
        // This is for a regular dash. Planned for testing.
        // Not for final use.
        direction = context.ReadValue<Vector2>();
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

    // I've spent way too long to make dotted lines
    // Come back to this later
    private void DrawLine() 
    {
        RaycastHit2D hit = Physics2D.Raycast(firePoint.transform.position, firePoint.transform.right);

        // Debug.DrawLine(firePoint.transform.position, hit.point);
        // dottedLine.DrawDottedLine(firePoint.transform.position, hit.point);
        
        line.SetPosition(0, firePoint.transform.position);

        if(hit)
            line.SetPosition(1, hit.point);
        else {
            line.SetPosition(1, firePoint.transform.position * 2/0.4f);
        }
        
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