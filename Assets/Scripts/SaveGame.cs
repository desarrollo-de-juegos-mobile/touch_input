using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO; // Para guardar datos en un archivo
using UnityEngine.UI; // Para usar UI Text

public class SaveGame : MonoBehaviour
{
    // A reemplazar con un formato mas optimo..
    public Text scoreText;
    public Text timeText;
    public Text levelText;

    // Datos del jugador...
    private string playername = "Player1";
    private int playerScore = 0;
    private int playerLevel = 1;
    private float playerTime = 0.0f;


    public void AddScore(int score)
    {
        playerScore += score;
        UpdateUI();
    }

    public void AddTime(float time)
    {
        playerTime += time;
        UpdateUI();
    }

    public void AddLevel(int level)
    {
        playerLevel += level;
        UpdateUI();
    }

    public void UpdateUI()
    {
        // Actualizar la UI con los datos del jugador
        scoreText.text = "Puntos: " + playerScore.ToString();
        timeText.text = "Tiempo: " + playerTime.ToString("F2") + "s";
        levelText.text = "Nivel: " + playerLevel.ToString();
    }

    // Solo vamos a guarda player Saved Data in PlayerPrefs...
    // Al guardar en player prefs, no es necesario usar el path pero se tendra acceso a los datos desde otras secciones dentro
    // del dispositivo. 
    void SavePlayerPrefData()
    {
        // Guardar datos del jugador en PlayerPrefs
        PlayerPrefs.SetInt("PlayerScore", playerScore);
        PlayerPrefs.SetInt("PlayerLevel", playerLevel);
        PlayerPrefs.SetFloat("PlayerTime", playerTime);
        PlayerPrefs.SetString("PlayerName", playername);
        PlayerPrefs.Save();
        Debug.Log("Datos del jugador guardados.");
    }

    void LoadPlayerPrefData()
    {
        // Cargar datos del jugador desde PlayerPrefs
        if (PlayerPrefs.HasKey("PlayerScore"))
        {
            playerScore = PlayerPrefs.GetInt("PlayerScore");
            playerLevel = PlayerPrefs.GetInt("PlayerLevel");
            playerTime = PlayerPrefs.GetFloat("PlayerTime");
            playername = PlayerPrefs.GetString("PlayerName");
            Debug.Log("Datos del jugador cargados.");
        }
        else
        {
            Debug.LogWarning("No hay datos guardados en PlayerPrefs.");
        }
    }

    void ResetData()
    {
        // Resetear datos del jugador
        playerScore = 0;
        playerLevel = 1;
        playerTime = 0.0f;
        PlayerPrefs.DeleteAll();
        UpdateUI();
        Debug.Log("Datos del jugador reseteados.");
    }


    // Start is called before the first frame update
    void Start()
    {
        UpdateUI();
        // Cargar datos del jugador al iniciar el juego
        LoadPlayerPrefData();
    }

    // Si se llega a pasar a el background, se guardan los datos del jugador
    private void OnApplicationPause(bool pause)
    {
        if (pause)
        {
            // Guardar datos del jugador al pausar el juego
            SavePlayerPrefData();
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // Vamos guardar los datos en un archivo de text...
    void SaveData(string fileName, string data)
    {
        // Guardar datos en un archivo
        string full_path = Path.Combine(Application.persistentDataPath, fileName);// combinamos el path con el filename
        File.WriteAllText(full_path, data);
        Debug.Log("Datos guardados en: " + full_path);
    }

    string LoadData(string fileName)
    {
        // Cargar datos desde un archivo
        string full_path = Path.Combine(Application.persistentDataPath, fileName);
        if (File.Exists(full_path))
        {
            string data = File.ReadAllText(full_path);
            Debug.Log("Datos cargados desde: " + full_path);
            return data;
        }
        else
        {
            Debug.LogWarning("Archivo no encontrado: " + full_path);
            return null;
        }
    }

}
