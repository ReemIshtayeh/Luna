using UnityEngine;

public class TriggerToChangeOtherObject2D : MonoBehaviour
{
    [Header("Target Object (drag your Plane here)")]
    [SerializeField] private Transform targetObject;

    [Header("Target Transform Settings")]
    [SerializeField] private Vector3 targetPosition = new Vector3(0f, 2f, 0f);
    [SerializeField] private Vector3 targetScale    = new Vector3(1.5f, 1.5f, 1.5f);
    [SerializeField] private float duration = 1.5f;

    private void OnTriggerEnter2D(Collider2D other)          // ⚠️ 2‑D callback
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("Player touched square (2D) — starting transformation.");
            StartCoroutine(ChangeTargetTransform());
            AudioManager.Instance.PlayPickedSFX(); // Play sound when triggered
                                                   //delay before destroying the trigger
            Invoke(nameof(DestroyTrigger), 2f); // Delay to allow sound to play
        }
    }

    private void DestroyTrigger()
    {
        Destroy(gameObject); // Destroy this trigger after use
    }

    private System.Collections.IEnumerator ChangeTargetTransform()
    {
        Vector3 startPos   = targetObject.position;
        Vector3 startScale = targetObject.localScale;
        float t = 0f;

        while (t < duration)
        {
            t += Time.deltaTime;
            float k = t / duration;
            targetObject.position   = Vector3.Lerp(startPos,   targetPosition, k);
            targetObject.localScale = Vector3.Lerp(startScale, targetScale,    k);
            yield return null;
        }
    }
}
