// TrailEffect.cs - Script opcional para efectos visuales
using UnityEngine;

public class TrailEffect : MonoBehaviour
{
    public float fadeSpeed = 2f;
    public float shrinkSpeed = 0.5f;

    private SpriteRenderer spriteRenderer;
    private Color originalColor;
    private Vector3 originalScale;

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();

        if (spriteRenderer)
        {
            originalColor = spriteRenderer.color;
            originalScale = transform.localScale;
        }
    }

    void Update()
    {
        if (spriteRenderer)
        {
            // Desvanecer el color
            Color newColor = spriteRenderer.color;
            newColor.a -= fadeSpeed * Time.deltaTime;
            spriteRenderer.color = newColor;

            // Reducir el tamaño
            transform.localScale = Vector3.Lerp(transform.localScale, Vector3.zero, shrinkSpeed * Time.deltaTime);

            // Si es casi invisible, destruirlo
            if (newColor.a <= 0.05f)
            {
                Destroy(gameObject);
            }
        }
    }
}