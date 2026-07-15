// CollisionProbe.cs
// Attach this to the Player to log every collider that touches the player.
// Useful to see which GameObjects are colliding with the player when damage occurs.

using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class CollisionProbe : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log($"CollisionProbe: OnTriggerEnter2D with '{other.gameObject.name}' (tag='{other.gameObject.tag}') at frame {Time.frameCount}");
        LogComponents(other.gameObject);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log($"CollisionProbe: OnCollisionEnter2D with '{collision.gameObject.name}' (tag='{collision.gameObject.tag}') at frame {Time.frameCount}");
        LogComponents(collision.gameObject);
    }

    private void LogComponents(GameObject go)
    {
        var comps = go.GetComponents<Component>();
        string compList = "";
        foreach (var c in comps)
        {
            if (c == null) continue;
            compList += c.GetType().Name + ", ";
        }
        Debug.Log($"CollisionProbe: Components on '{go.name}': {compList}");
    }
}
