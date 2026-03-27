using UnityEngine;
using System.Collections;

public class BombProjectile : MonoBehaviour
{
    public float speed = 3f;
    public GameObject explosionEffect;

    private Vector3 targetPosition;
    private System.Action onHitCallback;

    public void Init(Vector3 target, System.Action onHit)
    {
        targetPosition = target;
        onHitCallback = onHit;

        StartCoroutine(MoveToTarget());
    }

    IEnumerator MoveToTarget()
    {
        float height = 2f;
        float t = 0;
        Vector3 start = transform.position;

        while (t < 1)
        {
            t += Time.deltaTime * speed;
            t = Mathf.Clamp01(t); //
            Vector3 pos = Vector3.Lerp(start, targetPosition, t);
            // Lanzamiento de tipo parábola
            pos.y += Mathf.Sin(t * Mathf.PI) * height;

            transform.position = pos;

            yield return null;
        }

        // Explosión
        if (explosionEffect != null)
        {
            Instantiate(explosionEffect, transform.position, Quaternion.identity);
        }

        onHitCallback?.Invoke();
        Destroy(gameObject);
    }
}