// FruitSpawner.cs - Coloca este script en un objeto vacío en la escena
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class FruitSpawner : MonoBehaviour
{
    [Header("Prefabs")]
    public GameObject[] fruitPrefabs;        // Array de frutas para instanciar
    public GameObject slicedFruitPrefab;     // Fruta cortada (para simplicidad, usamos el mismo para todas)

    [Header("Spawn Settings")]
    public float spawnRate = 1f;             // Frutas por segundo
    public float minForce = 10f;             // Fuerza mínima de lanzamiento
    public float maxForce = 15f;             // Fuerza máxima de lanzamiento
    public float minTorque = -10f;           // Rotación mínima
    public float maxTorque = 10f;            // Rotación máxima
    public float spawnWidth = 5f;            // Ancho de la zona de spawn

    [Header("Game")]
    public int score = 0;                    // Puntuación
    public int lives = 3;                    // Vidas del jugador
    public bool gameActive = true;           // Estado del juego

    public TMPro.TextMeshProUGUI scoreText;  // Texto de puntuación
    public TMPro.TextMeshProUGUI livesText;  // Texto de vidas
    public GameObject gameOverPanel;         // Panel de Game Over

    private List<GameObject> spawnedFruits = new List<GameObject>();

    void Start()
    {
        UpdateUI();
        StartCoroutine(SpawnFruits());

        // Desactivar panel de Game Over al inicio
        if (gameOverPanel)
            gameOverPanel.SetActive(false);
    }

    IEnumerator SpawnFruits()
    {
        while (gameActive)
        {
            yield return new WaitForSeconds(1f / spawnRate);
            SpawnFruit();
        }
    }

    void SpawnFruit()
    {
        // Elegir una fruta aleatoria
        int fruitIndex = Random.Range(0, fruitPrefabs.Length);
        GameObject fruitPrefab = fruitPrefabs[fruitIndex];

        // Elegir una posición aleatoria en X para el spawn
        float xPos = Random.Range(-spawnWidth / 2, spawnWidth / 2);
        Vector3 spawnPosition = new Vector3(xPos, -6f, 0); // Spawn por debajo de la pantalla

        // Crear la fruta
        GameObject newFruit = Instantiate(fruitPrefab, spawnPosition, Quaternion.identity);
        spawnedFruits.Add(newFruit);

        // Agregar componentes necesarios
        Rigidbody2D rb = newFruit.GetComponent<Rigidbody2D>();
        Fruit fruitComponent = newFruit.GetComponent<Fruit>();
        if (fruitComponent == null)
            fruitComponent = newFruit.AddComponent<Fruit>();

        fruitComponent.spawner = this;
        fruitComponent.pointValue = 10;

        // Aplicar fuerza hacia arriba en un ángulo aleatorio
        float angle = Random.Range(-30f, 30f); // Ángulo en grados
        Vector2 forceDirection = Quaternion.Euler(0, 0, angle) * Vector2.up;
        float forceMagnitude = Random.Range(minForce, maxForce);

        rb.AddForce(forceDirection * forceMagnitude, ForceMode2D.Impulse);

        // Aplicar rotación aleatoria
        rb.AddTorque(Random.Range(minTorque, maxTorque), ForceMode2D.Impulse);
    }

    public void SliceFruit(GameObject fruitObject, Vector2 sliceDirection)
    {
        Fruit fruitComponent = fruitObject.GetComponent<Fruit>();
        if (fruitComponent == null)
            return;

        // Eliminar la fruta original
        spawnedFruits.Remove(fruitObject);
        Destroy(fruitObject);

        // Crear efectos de corte (dos mitades de fruta)
        Vector3 fruitPosition = fruitObject.transform.position;

        // Primera mitad - va en una dirección
        GameObject slicedFruit1 = Instantiate(slicedFruitPrefab, fruitPosition, Quaternion.identity);
        Rigidbody2D rb1 = slicedFruit1.GetComponent<Rigidbody2D>();
        Vector2 force1 = new Vector2(sliceDirection.y, -sliceDirection.x).normalized * 5f;
        rb1.AddForce(force1, ForceMode2D.Impulse);
        rb1.AddTorque(Random.Range(minTorque, maxTorque), ForceMode2D.Impulse);
        Destroy(slicedFruit1, 2f); // Destruir después de 2 segundos

        // Segunda mitad - va en dirección opuesta
        GameObject slicedFruit2 = Instantiate(slicedFruitPrefab, fruitPosition, Quaternion.identity);
        Rigidbody2D rb2 = slicedFruit2.GetComponent<Rigidbody2D>();
        Vector2 force2 = new Vector2(-sliceDirection.y, sliceDirection.x).normalized * 5f;
        rb2.AddForce(force2, ForceMode2D.Impulse);
        rb2.AddTorque(Random.Range(minTorque, maxTorque), ForceMode2D.Impulse);
        Destroy(slicedFruit2, 2f); // Destruir después de 2 segundos

        // Aumentar la puntuación
        score += fruitComponent.pointValue;
        UpdateUI();
    }

    public void MissFruit(GameObject fruitObject)
    {
        spawnedFruits.Remove(fruitObject);
        Destroy(fruitObject);

        lives--;

        if (lives <= 0)
        {
            GameOver();
        }

        UpdateUI();
    }

    void GameOver()
    {
        gameActive = false;

        // Destruir todas las frutas que queden
        foreach (GameObject fruit in spawnedFruits.ToArray())
        {
            Destroy(fruit);
        }
        spawnedFruits.Clear();

        // Mostrar panel de Game Over
        if (gameOverPanel)
            gameOverPanel.SetActive(true);
    }

    public void RestartGame()
    {
        // Reiniciar variables
        score = 0;
        lives = 3;
        gameActive = true;

        // Actualizar UI
        UpdateUI();

        // Ocultar panel de Game Over
        if (gameOverPanel)
            gameOverPanel.SetActive(false);

        // Reiniciar el spawner
        StartCoroutine(SpawnFruits());
    }

    void UpdateUI()
    {
        if (scoreText)
            scoreText.text = "Puntuación: " + score;

        if (livesText)
            livesText.text = "Vidas: " + lives;
    }
}