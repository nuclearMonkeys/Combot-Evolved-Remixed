using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    public int tankID;
    public bool enableKeyboard = true;
    [Header("Tank GameObjects")]
    [SerializeField] private GameObject body;
    [SerializeField] private GameObject head;

    [Header("Player Variables")]
    [SerializeField] private float m_movementSpeed = 5.0f;
    [SerializeField] private Vector2 direction;
    [SerializeField] private PlayerStamina playerStamina;
    public Color tankColor;
    private PlayerWeapons playerWeapons;

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
        m_rigidbody = this.GetComponent<Rigidbody2D>();
        playerWeapons = GetComponent<PlayerWeapons>();
    }

    public void AssignTankID(int id)
    {
        tankID = id;

        foreach (SpriteRenderer sr in GetComponentsInChildren<SpriteRenderer>())
        {
            if (tankID == 0)
                tankColor = Color.blue;
            else if (tankID == 1)
                tankColor = Color.red;
            else if (tankID == 2)
                tankColor = Color.yellow;
            else if (tankID == 3)
                tankColor = Color.green;
            sr.color = tankColor;
        }
    }

    void Update() 
    {
        // Keyboard Controls:
        // WASD to move
        // Mouse to aim
        // Left Click to shoot
        // Left Shift to dash
        if(enableKeyboard)
        {
            // keyboard controls
            if (Input.GetKeyDown(KeyCode.Space))
                Fire();
            if (Input.GetKeyDown(KeyCode.LeftShift))
                Dash();
            if (Input.GetKeyDown(KeyCode.P))
                Pause();
            Move(new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical")).normalized);
            Rotate((Vector2)Camera.main.ScreenToWorldPoint(Input.mousePosition) - ((Vector2)transform.position).normalized);
        }

        m_rigidbody.velocity = direction * m_movementSpeed;
    }

    public void ControllerMove(InputAction.CallbackContext context)
    {
        Move(context.ReadValue<Vector2>());
    }

    public void Move(Vector2 inputDirection) 
    {
        // if not dashing, move according to joystick
        if(!isDashing || dashRotate)
        {
            direction = inputDirection;
        }
        // prevent snapping
        if(direction.magnitude > .1f)
        {
            body.transform.right = direction.normalized;
        }
    }

    public void ControllerRotate(InputAction.CallbackContext context)
    {
        Rotate(context.ReadValue<Vector2>());
    }

    public void Rotate(Vector2 inputDirection) 
    {
        if(!PauseMenu.instance.gameObject.activeSelf)
            return;
        if (PauseMenu.instance.isPaused)
            return;
        // When you release stick on resting place
        Vector2 rotation = inputDirection;
        // prevent snapping
        if(rotation.magnitude > .1f)
        {
            head.transform.right = rotation;
        }
    }

    public void Fire()
    {
        if(canFire && !PauseMenu.instance.isPaused)
        {
            StartCoroutine(fireEnumerator());
        }
    }

    IEnumerator fireEnumerator()
    {
        GunBase gun = playerWeapons.gunReference;
        BulletBase bullet = playerWeapons.bulletPrefab;
        if(playerStamina.HasEnoughStamina(gun.fireStaminaUsage))
        {
            canFire = false;
            gun.FireBullet(bullet, this);
            playerStamina.DecrementStamina(gun.fireStaminaUsage);
            yield return new WaitForSeconds(gun.fireRate);
            canFire = true;
        }
    }

    public void Dash() 
    {
        if(!isDashing && !PauseMenu.instance.isPaused)
        {
            StartCoroutine(dashEnumerator());
        }
    }

    IEnumerator dashEnumerator() 
    {
        isDashing = true;
        float oldSpeed = m_movementSpeed;
        m_movementSpeed = dashSpeed;
        yield return new WaitForSeconds(dashDuration);
        m_movementSpeed = oldSpeed;
        isDashing = false;
    }

    public void Pause()
    {
        if (PauseMenu.instance == null) {
            TankSelectionManager.instance.PlayerLeft(this.gameObject.GetComponent<PlayerInput>());
            return;
        }
        if (!PauseMenu.instance.isPaused)
            PauseMenu.instance.PauseGame();
        else
            PauseMenu.instance.ResumeGame();
    }
}