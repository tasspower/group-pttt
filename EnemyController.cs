using UnityEngine;

public class EnemyController : MonoBehaviour
{
    public int maxHealth = 2;
    private int currentHealth;
    public int attackDamage = 1;

    public float patrolSpeed = 2f;
    public float patrolWaitTime = 2f; // เวลารอเมื่อถึงจุดสิ้นสุด
    private float waitTimer;
    private bool movingRight = true;

    private Animator anim;
    private Rigidbody2D rb;
    private SpriteRenderer spriteRenderer;

    void Start()
    {
        currentHealth = maxHealth;
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        waitTimer = patrolWaitTime;
    }

    void Update()
    {
        PatrolBehavior();
    }

    // === อนิเมชั่นการเดินของ Enemy / ระบบ Patrol (Core Requirement 2) ===
    void PatrolBehavior()
    {
        if (waitTimer > 0)
        {
            // หยุดรอ
            rb.velocity = Vector2.zero;
            anim.SetBool("IsWalking", false);
            waitTimer -= Time.deltaTime;
            
            if(waitTimer <= 0)
            {
                // เปลี่ยนทิศทางการเดิน
                movingRight = !movingRight;
                spriteRenderer.flipX = movingRight; 
            }
        }
        else
        {
            // เดิน
            Vector2 direction = movingRight ? Vector2.right : Vector2.left;
            rb.velocity = direction * patrolSpeed;
            anim.SetBool("IsWalking", true);
        }
    }
    
    // === การทำดาเมจผู้เล่น ===
    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            PlayerController player = collision.gameObject.GetComponent<PlayerController>();
            if (player != null)
            {
                player.TakeDamage(attackDamage);
            }
        }
    }

    // === การรับดาเมจจากผู้เล่น (Player Attack Logic) ===
    public void TakeHit(int damage)
    {
        currentHealth -= damage;
        
        if (currentHealth <= 0)
        {
            Die();
        } else {
            // เล่น Hurt animation ของศัตรู (ถ้ามี)
        }
    }
    
    private void Die()
    {
        // ปิด Collider และหยุดการทำงาน
        GetComponent<Collider2D>().enabled = false;
        GetComponent<Collider2D>().isTrigger = true; // เพื่อให้ตกผ่านพื้นได้ (ถ้าใช้ Box Collider ปกติ)
        
        rb.velocity = Vector2.zero;
        anim.SetTrigger("Die"); 
        
        // รอ 1.5 วินาทีแล้วลบศัตรูออกจากฉาก
        Destroy(gameObject, 1.5f);
    }
}