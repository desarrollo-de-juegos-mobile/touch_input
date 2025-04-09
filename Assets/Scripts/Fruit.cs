// Fruit.cs - Este script se adjuntará a cada fruta
using UnityEngine;

public class Fruit : MonoBehaviour
{
    public FruitSpawner spawner;
    public int pointValue = 10;
    private bool isSliced = false;

    private void OnBecameInvisible()
    {
        // Si la fruta sale de la pantalla y no ha sido cortada, se pierde una vida
        if (!isSliced && spawner != null && spawner.gameActive)
        {
            spawner.MissFruit(gameObject);
        }
    }
}