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
    private float init_movementSpeed;
    [SerializeField] private Vector2 direction;
    [SerializeField] private Vector2 gunDirection;
    public Color tankColor;

    // components
    private PlayerWeapons playerWeapons;
    private PlayerHealth playerHealth;
    private PlayerStamina playerStamina;

    // private variables. no touchy touchy.
    private bool canMove = true;
    private bool canFire = true;
    private bool canActivatePassive = true;

    // Components
    private Rigidbody2D m_rigidbody;

    void Start() 
    {
        body =        transform.Find("Body").gameObject;
        head =        transform.Find("Head").gameObject;
        m_rigidbody = this.GetComponent<Rigidbody2D>();
        playerWeapons = GetComponent<PlayerWeapons>();

        init_movementSpeed = m_movementSpeed;

        playerWeapons = GetComponent<PlayerWeapons>();
        playerHealth = GetComponentInChildren<PlayerHealth>();
        playerStamina = GetComponentInChildren<PlayerStamina>();

        AssignTankID(tankID);
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
                ActivatePassive();
            if (Input.GetKeyDown(KeyCode.P))
                Pause();
            Move(new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical")).normalized);
            Rotate(((Vector2)Camera.main.ScreenToWorldPoint(Input.mousePosition) - (Vector2)transform.position).normalized);
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
        if(canMove)
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
            gunDirection = rotation;
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
            gun.FireBullet(bullet);
            playerStamina.DecrementStamina(gun.fireStaminaUsage);
            yield return new WaitForSeconds(gun.fireRate);
            canFire = true;
        }
    }

    public void ActivatePassive()
    {
        if(canActivatePassive && !PauseMenu.instance.isPaused)
        {
            StartCoroutine(ActivatePassiveEnumerator());
        }
    }

    IEnumerator ActivatePassiveEnumerator()
    {
        canActivatePassive = false;
        playerWeapons.passiveReference.ActivatePassive(this);
        yield return new WaitForSeconds(playerWeapons.passiveReference.cooldown);
        canActivatePassive = true;
    }

    public void Pause()
    {
        if (PauseMenu.instance == null)
        {
            TankSelectionManager.instance.PlayerLeft(this.gameObject.GetComponent<PlayerInput>());
            return;
        }
        if (!PauseMenu.instance.isPaused)
            PauseMenu.instance.PauseGame();
        else
            PauseMenu.instance.ResumeGame();
    }

    // Called when loading new level
    public void Reset()
    {
        gameObject.SetActive(true);

        // Reset components
        playerHealth.ResetHealth();
        playerStamina.ResetStamina();
        playerWeapons.ResetWeapons();

        // Reset flags
        StopAllCoroutines();
        m_movementSpeed = init_movementSpeed;
        canMove = true;
        canFire = true;
        canActivatePassive = true;
}

    public float GetMovementSpeed() { return m_movementSpeed; }
    public void SetMovementSpeed(float f) { m_movementSpeed = f; }
    public Vector2 GetDirection() { return direction; }
    public void SetDirection(Vector2 dir) { direction = dir; }
    public Vector2 GetGunDirection() { return gunDirection; }
    public void SetGunDirection(Vector2 dir) { gunDirection = dir; }
    public bool GetCanMove() { return canMove; }
    public void SetCanMove(bool b) { canMove = b; }
}