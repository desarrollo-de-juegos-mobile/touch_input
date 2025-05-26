using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Services.Core;
using Unity.Services.Authentication;
using Unity.Services.CloudSave;
using System.Threading.Tasks;
using Unity.Collections.LowLevel.Unsafe;

public class CloudSaveSystem : MonoBehaviour
{
    // Start is called before the first frame update
    async void Start()
    {
        await UnityServices.InitializeAsync();

        await AuthenticationService.Instance.SignInAnonymouslyAsync();

        Debug.Log("Signed in anonymously using Unity Cloud");

        SaveGamneData();
    }

    public async void SaveGamneData()
    {

        string message = "Hello World"; // <- Sumarle el timestamp o la hora o lo fecha o lo que sea potencialmente unico

        // Aquí puedes guardar los datos del juego en la nube
        // Por ejemplo, usando CloudSaveService.Instance.Data
        // Puedes usar un diccionario para almacenar los datos que deseas guardar
        var dataToSave = new Dictionary<string, object> {{ "playerName", message } };
        
        Debug.Log("Saving game data[Message]: " + message);

        // Guardar los datos en la nube
        await CloudSaveService.Instance.Data.Player.SaveAsync(dataToSave);

        // Recuerden ponerlo dentro de un try catch
    }

    public async void LoadGameData()
    {
        // Aquí puedes cargar los datos del juego desde la nube
        // Por ejemplo, usando CloudSaveService.Instance.Data
        // Puedes usar un diccionario para almacenar los datos que deseas cargar
        var keys = new HashSet<string> { "playerName" };
        Debug.Log("Loading game data...");
        // Cargar los datos desde la nube
        var dataToLoad = await CloudSaveService.Instance.Data.Player.LoadAsync(keys);
        // Recuerden ponerlo dentro de un try catch

        Debug.Log("Loaded game data[Message]: " + dataToLoad["playerName"]);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
