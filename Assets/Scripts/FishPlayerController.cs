using UnityEngine;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(Rigidbody2D))]
public class FloppyFishController : MonoBehaviour
{
    [Header("Rotation on Ground")]
    public float landTorque = 30f;        // Torque per second when holding left/right on the ground
    public float airTorque = 10f;         // Torque per second when holding left/right in air
    public float maxAngularSpeed = 300f;  // Limit how fast fish spins

    [Header("Flop Jump")]
    public float flopForce = 12f;         // Base upward flop impulse
    public float diagonalMultiplier = 1.2f; // Extra multiplier if left/right is held
    public float flopCooldown = 0.5f;     // Time between flops
    private float lastFlopTime = -999f;

    [Header("Ground Check")]
    public Transform groundCheck;
    public float groundCheckRadius = 0.2f;
    public LayerMask groundLayer;

    [Header("Movement Limits")]
    public float maxHorizontalSpeed = 5f; // Limit horizontal velocity

    private Rigidbody2D rb;
    private bool isGrounded = false;
    [Header("Bounce Stretch Settings")]
public Transform fishModelPrefab;  // Drag your visual model here in Inspector
public Vector2 xScaleRange = new Vector2(0.8f, 1.2f);
public Vector2 yScaleRange = new Vector2(0.8f, 1.2f);
public float bounceLerpSpeed = 5f;
public Vector2 initalScale;
public GameObject sandPartclePrefab;

private Vector3 targetScale = Vector3.one;
public bool isTitleFish = false;



    void OnCollisionEnter2D(Collision2D collision)
{
    if (collision.gameObject.tag == "Enemy")
    {
        Debug.Log("Player hit!");
    }
    //--------------------------------
    float impact = collision.relativeVelocity.magnitude;
    Instantiate(sandPartclePrefab, collision.contacts[0].point, Quaternion.identity);
    camTracker.ShakeCamera(0.06f, impact * 0.04f);

    if (impact > 1f) // Only bounce on noticeable hits 
    {
        float randX = Random.Range(xScaleRange.x, xScaleRange.y);
        float randY = Random.Range(yScaleRange.x, yScaleRange.y);

        // Apply bounce relative to initialScale
        targetScale = new Vector3(
            initalScale.x * randX,
            initalScale.y * randY,
            1f
        );
    }
}
[Header("Camera")]
public CameraLocationTracker camTracker;

[Header("Water Movement")]
public float maxSwimSpeed = 3f;


    void Start()
    {
        targetScale = initalScale;

        rb = GetComponent<Rigidbody2D>();

        // A bit of drag can help keep the fish from sliding around too much
        // but if you want friction for spinning, you might use a low-friction Physics Material instead.
        rb.angularDamping = 0.2f;
        rb.linearDamping = 0.2f;
        initalScale = fishModelPrefab.localScale;
    }
    private float randomTimer = 0f;
private float randomCooldown = 1.5f;
private bool nextRandomMovementIsLeft = false;


void RandomTitleMotion()
{
    randomTimer += Time.deltaTime;

    if (randomTimer >= randomCooldown)
    {
        randomTimer = 0f;
        randomCooldown = Random.Range(1f, 3f);

        // Pick a random direction
        Vector2 force = new Vector2(Random.Range(-1f, 1f), Random.Range(0.5f, 1f)).normalized;
        float impulse = Random.Range(5f, 15f);
    if (nextRandomMovementIsLeft){
        rb.AddForce(-force * impulse, ForceMode2D.Impulse);
        nextRandomMovementIsLeft = false;
    }else{
        rb.AddForce(force * impulse, ForceMode2D.Impulse);
        nextRandomMovementIsLeft = true;
    }
        

        // Add a bit of torque for floppiness
        rb.AddTorque(Random.Range(-20f, 20f), ForceMode2D.Impulse);
    }

    // Smooth idle spin or wiggle (optional)
    if (PlayerStateManager.currentPlayerState == PlayerStateManager.PlayerState.Water)
    {
        Vector2 velocity = rb.linearVelocity;
        if (velocity.sqrMagnitude > 0.1f)
        {
            float angle = Mathf.Atan2(velocity.y, velocity.x) * Mathf.Rad2Deg;
            angle -= 90;
            Quaternion targetRotation = Quaternion.Euler(0, 0, angle);
            transform.rotation = Quaternion.Lerp(
                transform.rotation,
                targetRotation,
                Time.deltaTime * 5f
            );
        }
    }
}


    void Update()
    {
        if (isTitleFish)
        {
            RandomTitleMotion();
            return;
        }
        if (Input.GetKeyDown(KeyCode.P))
        {
            //Emergency reset
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex +1 );
        }
        if (PlayerStateManager.currentPlayerState == PlayerStateManager.PlayerState.Water)
{
    Vector2 velocity = rb.linearVelocity;

    // Only rotate if moving fast enough
    if (velocity.sqrMagnitude > 0.1f)
    {
        float angle = Mathf.Atan2(velocity.y, velocity.x) * Mathf.Rad2Deg;
        angle -= 90;
        Quaternion targetRotation = Quaternion.Euler(0, 0, angle);
        
        transform.rotation = Quaternion.Lerp(
            transform.rotation,
            targetRotation,
            Time.deltaTime * 5f
        );
    }
}

        if (PlayerStateManager.currentPlayerState == PlayerStateManager.PlayerState.Water)
        {
            if (Input.GetKey(KeyCode.Space))
            {
                rb.AddForce(Vector2.up * 0.3f, ForceMode2D.Impulse);
            }else if (Input.GetKey(KeyCode.A))
            {
                rb.AddForce(Vector2.left * 0.1f, ForceMode2D.Impulse);
            }else if (Input.GetKey(KeyCode.D))
            {
                rb.AddForce(Vector2.right * 0.1f, ForceMode2D.Impulse);
            }
        }else{
              if (Input.GetKeyDown(KeyCode.Space) && isGrounded && (Time.time - lastFlopTime >= flopCooldown))
        {
            DoFlop();
              rb.AddForce(Vector2.up * 5, ForceMode2D.Impulse);
        }
        bool leftHeld = Input.GetKey(KeyCode.A);
        bool rightHeld = Input.GetKey(KeyCode.D);
        bool anyHeld = leftHeld || rightHeld;
        if (Input.GetKeyDown(KeyCode.Space)   && anyHeld)
        {
            Debug.Log("Jumping with left/right held");
          
        }
        }
        // **Jump (Flop) Input**: only allowed if on ground & cooldown passed
      
        // Smoothly return fish model to normal scale
if (fishModelPrefab != null)
{
    fishModelPrefab.localScale = Vector3.Lerp(
        fishModelPrefab.localScale,
        targetScale,
        Time.deltaTime * bounceLerpSpeed
    );
}


    }

   void FixedUpdate()
{
    CheckGrounded();

    // Clamp angular velocity
    rb.angularVelocity = Mathf.Clamp(rb.angularVelocity, -maxAngularSpeed, maxAngularSpeed);

    if (PlayerStateManager.currentPlayerState == PlayerStateManager.PlayerState.Water)
    {
        if (rb.linearVelocity.magnitude > maxSwimSpeed)
        {
            rb.linearVelocity = rb.linearVelocity.normalized * maxSwimSpeed;
        }
    }
    else
    {
        // ROTATION FLOPPY CONTROLS — land mode only
        bool leftHeld = Input.GetKey(KeyCode.A);
        bool rightHeld = Input.GetKey(KeyCode.D);

        if (leftHeld)
        {
            float torquePerFrame = (isGrounded ? landTorque : airTorque) * Time.fixedDeltaTime;
            rb.AddTorque(torquePerFrame, ForceMode2D.Force);
        }
        if (rightHeld)
        {
            float torquePerFrame = (isGrounded ? landTorque : airTorque) * Time.fixedDeltaTime;
            rb.AddTorque(-torquePerFrame, ForceMode2D.Force);
        }

        // Clamp horizontal velocity (land only)
        if (Mathf.Abs(rb.linearVelocity.x) > maxHorizontalSpeed)
        {
            rb.linearVelocity = new Vector2(
                Mathf.Sign(rb.linearVelocity.x) * maxHorizontalSpeed,
                rb.linearVelocity.y
            );
        }
    }
}



    private void DoFlop()
    {
        bool squish = Random.value > 0.5f;
targetScale = squish
    ? new Vector3(initalScale.x * 1.2f, initalScale.y * 0.85f, 1f)
    : new Vector3(initalScale.x * 0.85f, initalScale.y * 1.2f, 1f);

        // Base jump is straight up
        Vector2 jumpDir = Vector2.up;
        bool leftHeld = Input.GetKey(KeyCode.A);
        bool rightHeld = Input.GetKey(KeyCode.D);

        // If left or right is held, we add sideways to the jump
        if (leftHeld)  jumpDir += Vector2.left * 0.5f;
        if (rightHeld) jumpDir += Vector2.right * 0.5f;

        jumpDir.Normalize();

        // Calculate flop strength
        float baseFlop = flopForce;

        // If moving diagonally, multiply for a bigger push
        if (leftHeld || rightHeld)
            baseFlop *= diagonalMultiplier;

        // Apply the impulse
        rb.AddForce(jumpDir * baseFlop, ForceMode2D.Impulse);

        // (Optional) A little random torque to keep it feeling “fishy”
        float randomTorque = Random.Range(-20f, 20f);
        rb.AddTorque(randomTorque, ForceMode2D.Impulse);

        lastFlopTime = Time.time;
    }

    [Header("Ground Check (Capsule)")]
public float capsuleWidth = 0.3f;
public float capsuleHeight = 0.5f;

private void CheckGrounded()
{
    // CapsuleDirection2D.Vertical means it's taller than wide (like a vertical "pill")
    Collider2D hit = Physics2D.OverlapCapsule(
        groundCheck.position,
        new Vector2(capsuleWidth, capsuleHeight),
        CapsuleDirection2D.Vertical,
        0f,               // rotation
        groundLayer
    );

    isGrounded = (hit != null);
}


    void OnDrawGizmosSelected()
{
    if (groundCheck == null) return;

    Gizmos.color = isGrounded ? Color.green : Color.red;

    // Because there's no built-in Gizmos.DrawCapsule in 2D, let's just draw an approximate box & circles:
    // We'll treat "capsuleWidth" as the narrow dimension, "capsuleHeight" as the total "capsule" height.

    Vector2 center = groundCheck.position;
    float halfHeight = capsuleHeight * 0.5f;
    float radius = capsuleWidth * 0.5f;

    // Draw center rectangle
    Vector2 rectTop = center + Vector2.up * (halfHeight - radius);
    Vector2 rectBottom = center + Vector2.down * (halfHeight - radius);
    Vector2 rectSize = new Vector2(capsuleWidth, capsuleHeight - (2f * radius));
    Gizmos.DrawWireCube(center, rectSize);

    // Draw top circle
    Gizmos.DrawWireSphere(rectTop, radius);
    // Draw bottom circle
    Gizmos.DrawWireSphere(rectBottom, radius);
}

}
