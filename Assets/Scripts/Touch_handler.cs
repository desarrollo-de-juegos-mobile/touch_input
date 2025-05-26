using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.EnhancedTouch;
//using UnityEngine.InputSystem.iOS;
using UnityEngine.InputSystem.Layouts;
using Touch = UnityEngine.InputSystem.EnhancedTouch.Touch;

public class Touch_handler : MonoBehaviour
{

    [SerializeField] private Camera cam;

    private Vector3 calibrateOffset;
    public bool calibrate = false;
    private ScreenOrientation previousOrientation;
    public RectTransform safeAreaRect;


    // Start is called before the first frame update
    void Start()
    {
        if (cam == null)
        {
            cam = Camera.main;
        }
        previousOrientation = Screen.orientation;
    }

    private void Awake()
    {
        EnhancedTouchSupport.Enable();
        ApplySafeArea();
    }

    private void OnDisable()
    {
        EnhancedTouchSupport.Disable();
    }

    private void ApplySafeArea()
    {
        Rect safeArea = Screen.safeArea;
        Vector2 anchorMin = safeArea.position;
        Vector2 anchorMax = safeArea.position + safeArea.size;
        anchorMin.x /= Screen.width;
        anchorMin.y /= Screen.height;
        anchorMax.x /= Screen.width;
        anchorMax.y /= Screen.height;
        Debug.Log("Safe Area: " + safeArea);
        safeAreaRect.anchorMin = anchorMin;
        safeAreaRect.anchorMax = anchorMax;
        // Resetear posición local
        safeAreaRect.offsetMin = Vector2.zero;
        safeAreaRect.offsetMax = Vector2.zero;
        Debug.LogFormat("Safe Area aplicada: x={0}, y={1}, w={2}, h={3}",
        safeArea.x, safeArea.y, safeArea.width, safeArea.height);
    }

    // Update is called once per frame
    void Update()
    {
        //if ( (Touchscreen.current != null) && Touchscreen.current.primaryTouch.press.isPressed)
        if ( Touch.activeTouches.Count > 0)
        {
            //Vector2 touchPos = Touchscreen.current.primaryTouch.position.ReadValue();
            Touch activeTouch = Touch.activeTouches[0]; //Equivalent to Touchscreen.current.primaryTouch
            Vector2 touchPos = activeTouch.screenPosition;
            Vector3 worldPos = cam.ScreenToWorldPoint(new Vector3(touchPos.x, touchPos.y, cam.nearClipPlane));
            Debug.Log("Touch Position: " + worldPos);

            // Se aplica si y solo si estoy en un area segura...
            // IF el worldPos esta dentro de la safeArea... completar el chequeo...
            transform.position = worldPos;

        }

        if( previousOrientation != Screen.orientation)
        {
            OrientationChanged();
            previousOrientation = Screen.orientation;
        }

        // Codigo compatible con un for each de touches entrantes en la pantalla.
        /* foreach(Touch touch in Touch.activeTouches)
         {
             if ( touch.phase == 0) // podemos poner las distintos phase... para evaluar
             {

             }
             else
             {

             }

             // decidimos que hacemos con los touches... cuando los queremos gestionar...
         }*/

        /* /// Habilitamos en la proxima clase el acelerometro...
         * if (calibrate)
         {
             calibrateOffset = -Accelerometer.current.acceleration.ReadValue();
             calibrate = false;
         }

         Vector3 acceleration = Accelerometer.current.acceleration.ReadValue() + calibrateOffset;
        */
    }

    private void OrientationChanged()
    {
        Debug.Log("Orientation Changed");
        ApplySafeArea();
    }
}
