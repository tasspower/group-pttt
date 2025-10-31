using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    public int attackDamage = 1;

    // ตรวจสอบว่าชนกับศัตรูหรือไม่
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Enemy"))
        {
            EnemyController enemy = other.GetComponent<EnemyController>();
            if (enemy != null)
            {
                enemy.TakeHit(attackDamage);
                // อาจจะเพิ่ม Feedback เช่น Knockback, Sound Effect
            }
        }
    }
}