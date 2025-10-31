using UnityEngine;

public class PersistenceManager : MonoBehaviour
{
    void Start()
    {
        // ตรวจสอบว่ามีการบันทึกตำแหน่งออกมาก่อนหน้าหรือไม่
        if (PlayerPrefs.HasKey("ExitX") && PlayerPrefs.HasKey("ExitY"))
        {
            GameObject player = GameObject.FindGameObjectWithTag("Player");
            if (player != null)
            {
                float x = PlayerPrefs.GetFloat("ExitX");
                float y = PlayerPrefs.GetFloat("ExitY");
                // ย้ายผู้เล่นไปยังตำแหน่งที่กำหนด
                player.transform.position = new Vector3(x, y, player.transform.position.z);
                
                // ลบค่าที่บันทึกไว้ เพื่อไม่ให้โหลดตำแหน่งที่ไม่ต้องการในครั้งต่อไป
                PlayerPrefs.DeleteKey("ExitX");
                PlayerPrefs.DeleteKey("ExitY");
            }
        }
    }
}