using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneTransition : MonoBehaviour
{
    public string targetSceneName; // ฉากที่ต้องการโหลด
    public Vector2 exitPosition;   // ตำแหน่งของผู้เล่นเมื่อเข้าฉากใหม่

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            // บันทึกตำแหน่งที่จะไปในฉากถัดไป (สำคัญมากในการเปลี่ยนฉาก)
            PlayerPrefs.SetFloat("ExitX", exitPosition.x);
            PlayerPrefs.SetFloat("ExitY", exitPosition.y);
            
            // โหลดฉากใหม่
            SceneManager.LoadScene(targetSceneName);
        }
    }
}