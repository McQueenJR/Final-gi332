using UnityEngine;

public class PlayerRunner : MonoBehaviour
{
    [Header("Movement")]
    public float forwardSpeed = 5f;      // ความเร็ววิ่งไปข้างหน้า
    public float laneDistance = 2.5f;    // ระยะห่างแต่ละเลน
    public float laneChangeSpeed = 10f;  // ความเร็วสลับเลน

    [Header("Jump")]
    public float jumpForce = 8f;
    public LayerMask groundLayer;
    public Transform groundCheck;
    public float groundCheckRadius = 0.2f;

    private Rigidbody2D rb;
    private int targetLane = 0;   // -1 ซ้าย, 0 กลาง, 1 ขวา
    private bool isGrounded;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        // เช็คพื้น
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer);

        // กระโดด
        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, 0);
            rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
        }

        // สลับเลน
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            targetLane = Mathf.Clamp(targetLane - 1, -1, 1);
        }
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            targetLane = Mathf.Clamp(targetLane + 1, -1, 1);
        }
    }

    void FixedUpdate()
    {
        // วิ่งไปข้างหน้า
        rb.linearVelocity = new Vector2(forwardSpeed, rb.linearVelocity.y);

        // ตำแหน่งเลนเป้าหมาย
        float targetX = targetLane * laneDistance;
        float newX = Mathf.Lerp(transform.position.x, targetX, laneChangeSpeed * Time.fixedDeltaTime);

        transform.position = new Vector2(newX, transform.position.y);
    }

    void OnDrawGizmosSelected()
    {
        if (groundCheck != null)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(groundCheck.position, groundCheckRadius);
        }
    }
}