using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PlayerController : MonoBehaviour
{
    public float forwardSpeed = 5f;
    public float jumpForce = 5f;
    private Rigidbody rb;
    private bool isGround = true;

    private Vector3 originalScale;
    private Vector3 currentVelocity;
    public float squashAmount = 0.2f;
    public float stiffness = 8f;
    public float damping = 2f;

    public float groundJiggleAmount = 0.02f;
    public float groundJiggleSpeed = 4f;

    public float spinDuration = 0.5f;
    private float spinTimer = 0f;
    private bool isSpinning = false;
    private float startRotationY;

    private float lastZ;
    private float accumulatedZ = 0f;
    public float scoreDistanceStep = 1f;

    public float speedIncreaseRate = 0.1f;
    public float maxSpeed = 20f;


    public bool isGameStarted = true;

    public GhostManager ghostManager;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        originalScale = transform.localScale;
        lastZ = transform.position.z;
    }

    void FixedUpdate()
    {
        if (!isGameStarted) return;

        forwardSpeed = Mathf.Min(forwardSpeed + speedIncreaseRate * Time.fixedDeltaTime, maxSpeed);

        rb.MovePosition(rb.position + Vector3.forward * forwardSpeed * Time.fixedDeltaTime);
       

        if (ghostManager != null)
        {
            MovementSnapshot snapshot = new MovementSnapshot(
                transform.position,
                transform.rotation,
                transform.localScale
            );

            ghostManager.ReceiveSnapshot(snapshot);
        }
    }

    void Update()
    {
        float deltaZ = transform.position.z - lastZ;
        accumulatedZ += deltaZ;
        lastZ = transform.position.z;

        if (accumulatedZ >= scoreDistanceStep)
        {
            int steps = Mathf.FloorToInt(accumulatedZ / scoreDistanceStep);
            UIManager.Instance?.AddScore(steps);
            accumulatedZ -= steps * scoreDistanceStep;
        }

        if ((Input.GetKeyDown(KeyCode.Space) || IsMobileJumpPressed()) && isGround)
        {
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            isGround = false;

            currentVelocity = new Vector3(-.2f, .4f, -.2f);
            isSpinning = true;
            spinTimer = 0f;
            startRotationY = .0f;
        }

        if (!isGround)
        {
            ApplyJellyDeform();
            ApplyOneTimeSpin();
        }
        else
        {
            ApplyGroundJelly();
        }
    }

    bool IsMobileJumpPressed()
    {
        return Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began;
    }

    void ApplyJellyDeform()
    {
        Vector3 targetScale = originalScale + currentVelocity;
        transform.localScale = Vector3.Lerp(transform.localScale, targetScale, Time.deltaTime * stiffness);
        currentVelocity = Vector3.Lerp(currentVelocity, Vector3.zero, Time.deltaTime * damping);
    }

    void ApplyGroundJelly()
    {
        float jiggle = Mathf.Sin(Time.time * groundJiggleSpeed) * groundJiggleAmount;
        Vector3 jiggleScale = originalScale + new Vector3(jiggle, -jiggle, jiggle);
        transform.localScale = Vector3.Lerp(transform.localScale, jiggleScale, Time.deltaTime * 10f);
    }

    void ApplyOneTimeSpin()
    {
        if (!isSpinning) return;

        spinTimer += Time.deltaTime;
        float t = Mathf.Clamp01(spinTimer / spinDuration);
        float angle = Mathf.Lerp(0f, 360f, t);
        transform.rotation = Quaternion.Euler(0f, startRotationY + angle, 0f);

        if (t >= 1f)
        {
            isSpinning = false;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGround = true;
            isSpinning = false;
            spinTimer = 0f;
            transform.rotation = Quaternion.Euler(0f, 0f, 0f);
        }
    }
}
