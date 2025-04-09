// SwipeController.cs - Este script maneja los swipes del jugador
using UnityEngine;
using System.Collections.Generic;

public class SwipeController : MonoBehaviour
{
    [Header("Swipe Settings")]
    public float minSwipeDistance = 0.2f;
    public float maxSwipeTime = 1f;

    [Header("Visual Feedback")]
    public GameObject swipeTrailPrefab;
    public float trailDuration = 0.5f;

    // Tracking para el swipe
    private Vector2 startPos;
    private float startTime;
    private bool isTrackingSwipe = false;

    // Efectos visuales
    private List<GameObject> activeTrails = new List<GameObject>();

    // Layer mask para colisión con frutas
    public LayerMask fruitLayer;

    void Update()
    {
        // Para dispositivos táctiles
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);

            switch (touch.phase)
            {
                case TouchPhase.Began:
                    StartSwipe(touch.position);
                    break;
                case TouchPhase.Moved:
                    ContinueSwipe(touch.position);
                    break;
                case TouchPhase.Ended:
                    EndSwipe(touch.position);
                    break;
                case TouchPhase.Canceled:
                    isTrackingSwipe = false;
                    break;
            }
        }

        // Para pruebas en PC
        if (Input.GetMouseButtonDown(0))
        {
            StartSwipe(Input.mousePosition);
        }
        else if (Input.GetMouseButton(0) && isTrackingSwipe)
        {
            ContinueSwipe(Input.mousePosition);
        }
        else if (Input.GetMouseButtonUp(0) && isTrackingSwipe)
        {
            EndSwipe(Input.mousePosition);
        }

        // Limpiar trails antiguos
        CleanupTrails();
    }

    private void StartSwipe(Vector2 screenPosition)
    {
        startPos = GetWorldPosition(screenPosition);
        startTime = Time.time;
        isTrackingSwipe = true;

        // Crear efecto visual para el inicio del swipe
        if (swipeTrailPrefab != null)
        {
            GameObject trail = Instantiate(swipeTrailPrefab, new Vector3(startPos.x, startPos.y, 0), Quaternion.identity);
            activeTrails.Add(trail);
            Destroy(trail, trailDuration);
        }
    }

    private void ContinueSwipe(Vector2 screenPosition)
    {
        if (!isTrackingSwipe) return;

        Vector2 currentPos = GetWorldPosition(screenPosition);

        // Detectar colisiones con frutas durante el swipe
        DetectFruitCollisions(startPos, currentPos);

        // Actualizar posición de inicio para la siguiente detección
        startPos = currentPos;

        // Añadir efecto visual para continuar el swipe
        if (swipeTrailPrefab != null)
        {
            GameObject trail = Instantiate(swipeTrailPrefab, new Vector3(currentPos.x, currentPos.y, 0), Quaternion.identity);
            activeTrails.Add(trail);
            Destroy(trail, trailDuration);
        }
    }

    private void EndSwipe(Vector2 screenPosition)
    {
        if (!isTrackingSwipe) return;

        Vector2 endPos = GetWorldPosition(screenPosition);
        float swipeTime = Time.time - startTime;

        // Comprobamos si es un swipe válido
        Vector2 swipeVector = endPos - startPos;
        float distance = swipeVector.magnitude;

        if (distance >= minSwipeDistance && swipeTime <= maxSwipeTime)
        {
            // Detectar la última colisión
            DetectFruitCollisions(startPos, endPos);
        }

        isTrackingSwipe = false;
    }

    private Vector2 GetWorldPosition(Vector2 screenPosition)
    {
        // Convertir de coordenadas de pantalla a coordenadas del mundo
        Vector3 worldPos = Camera.main.ScreenToWorldPoint(new Vector3(screenPosition.x, screenPosition.y, 10f));
        return new Vector2(worldPos.x, worldPos.y);
    }

    private void DetectFruitCollisions(Vector2 lineStart, Vector2 lineEnd)
    {
        // Usamos Physics2D.LinecastAll para detectar todas las colisiones a lo largo de la línea de swipe
        RaycastHit2D[] hits = Physics2D.LinecastAll(lineStart, lineEnd, fruitLayer);

        foreach (RaycastHit2D hit in hits)
        {
            // Si colisiona con una fruta
            GameObject fruitObject = hit.collider.gameObject;
            Fruit fruit = fruitObject.GetComponent<Fruit>();

            if (fruit != null && fruit.spawner != null && fruit.spawner.gameActive)
            {
                // Calcular dirección del swipe para el efecto visual
                Vector2 sliceDirection = (lineEnd - lineStart).normalized;

                // Cortar la fruta
                fruit.spawner.SliceFruit(fruitObject, sliceDirection);
            }
        }
    }

    private void CleanupTrails()
    {
        // Eliminar elementos nulos de la lista
        activeTrails.RemoveAll(item => item == null);
    }
}