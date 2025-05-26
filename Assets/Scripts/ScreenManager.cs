using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreenManager : MonoBehaviour
{

    private ScreenOrientation previousOrientation;

    // Su principal responsabiliadd es mostrarnos infromacion de la pantalla...
    // Es mas una herramienta de debug que otra cosa.

    // Vamos a mostrar por pantalla la resolucion actual y el aspect ratio.
    // Y si cambia la orientacion deberia cambiar el aspect ratio y verse por pantalla.


    // Start is called before the first frame update
    void Start()
    {
        previousOrientation = Screen.orientation;
        ShowDebugScreenInfo();
    }

    private void ShowDebugScreenInfo()
    {
        Debug.Log("Screen Width: " + Screen.width);
        Debug.Log("Screen Height: " + Screen.height);
        Debug.Log("Screen Aspect Ratio: " + ((float)Screen.width / (float)Screen.height));
    }

    // Update is called once per frame
    void Update()
    {
        if (previousOrientation != Screen.orientation)
        {
            ShowDebugScreenInfo();
            previousOrientation = Screen.orientation;
        }
    }
}
