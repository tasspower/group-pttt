using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    // === สถิติ ===
    public int maxHealth = 3;
    private int currentHealth;
    public float moveSpeed = 5f;
    private bool isDead = false;

    // === การรับความเสียหาย ===
    private bool isInvulnerable = false;
    public float invulnerabilityDuration = 1.0f; 

    // === องค์ประกอบ ===
    private Rigidbody2D rb;
    private Animator anim;
    private SpriteRenderer spriteRenderer; 
    public HealthUI healthUI; // สมมติว่ามี UI แสดงเลือด

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        
        currentHealth = maxHealth;
        // healthUI.UpdateHealth(currentHealth);
    }

    void Update()
    {
        if (isDead) return;
        HandleMovement();
        
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Attack();
        }
    }

    void HandleMovement()
    {
        float moveX = Input.GetAxisRaw("Horizontal");
        float moveY = Input.GetAxisRaw("Vertical");
        Vector2 movement = new Vector2(moveX, moveY).normalized;
        
        rb.velocity = movement * moveSpeed;

        // อนิเมชั่นเดิน
        anim.SetBool("IsWalking", movement.magnitude > 0);

        // กลับทิศทาง Sprite
        if (moveX != 0)
        {
            spriteRenderer.flipX = moveX < 0;
        }
    }

    void Attack()
    {
        // เล่นอนิเมชั่นโจมตี
        anim.SetTrigger("Attack");
        // ในอนิเมชั่นจะมีการเรียกเปิด/ปิด Attack Trigger Collider
    }

    // === ระบบ Damage รับความเสียหาย (Core Requirement 1) ===
    public void TakeDamage(int damage)
    {
        if (isInvulnerable || isDead) return;

        currentHealth -= damage;
        // healthUI.UpdateHealth(currentHealth); // อัพเดท UI
        
        // เล่นอนิเมชั่นความเสียหาย (Hurt Animation)
        anim.SetTrigger("Hurt"); // เล่นอนิเมชั่น Hurt

        if (currentHealth <= 0)
        {
            Die();
        }
        else
        {
            StartCoroutine(StartInvulnerability());
        }
    }
    
    // === การตาย ===
    private void Die()
    {
        isDead = true;
        rb.velocity = Vector2.zero; // หยุดการเคลื่อนไหว
        anim.SetTrigger("Die"); 
        
        // โหลดฉาก Game Over หลังจาก Animation จบ
        // *จำเป็นต้องมี delay เพื่อให้เห็นอนิเมชั่นตาย*
        StartCoroutine(LoadGameOver(2.0f)); 
        
        // ปิด Collider
        GetComponent<Collider2D>().enabled = false;
    }

    IEnumerator LoadGameOver(float delay)
    {
        yield return new WaitForSeconds(delay);
        SceneManager.LoadScene("99_GameOverScreen");
    }

    // === ช่วง Invulnerability และการ Flash/กระพริบ ===
    IEnumerator StartInvulnerability()
    {
        isInvulnerable = true;
        float elapsed = 0f;
        
        // Effect การกระพริบ
        while (elapsed < invulnerabilityDuration)
        {
            spriteRenderer.enabled = !spriteRenderer.enabled; // ทำให้กระพริบ
            yield return new WaitForSeconds(0.1f);
            elapsed += 0.1f;
        }

        spriteRenderer.enabled = true; 
        isInvulnerable = false;
    }
}