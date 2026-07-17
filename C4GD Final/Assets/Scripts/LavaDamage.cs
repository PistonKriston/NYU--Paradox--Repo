using UnityEngine;

public class LavaDamager : MonoBehaviour
{
    public float ignore_timer = 0f;
    public GameObject Player;

    private void OnTriggerStay2D(Collider2D collision)
    {
        Debug.Log($"[Lava] OnTriggerStay2D with '{collision.gameObject.name}' (tag='{collision.tag}')");

        if (collision.CompareTag("Player"))
        {
            Debug.Log($"[Lava] Player detected. ignore_timer = {ignore_timer}");

            if (Player == null)
            {
                Debug.LogError("[Lava] ERROR: Player reference is NULL!");
                return;
            }

            Health hp = Player.GetComponent<Health>();
            if (hp == null)
            {
                Debug.LogError("[Lava] ERROR: Player has NO Health component!");
                return;
            }

            if (ignore_timer <= 0f)
            {
                Debug.Log("[Lava] Damaging player for 1 HP.");
                hp.TakeDamage(1);
                ignore_timer = 0.5f;
            }
            else
            {
                ignore_timer -= Time.deltaTime;
                Debug.Log($"[Lava] Cooling down... new ignore_timer = {ignore_timer}");
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log($"[Lava] OnTriggerEnter2D with '{collision.gameObject.name}' (tag='{collision.tag}')");
    }
}
