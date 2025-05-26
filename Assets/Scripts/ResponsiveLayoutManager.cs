using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResponsiveLayoutManager : MonoBehaviour
{
    [SerializeField] private GameObject smallScreenLayout;
    [SerializeField] private GameObject mediumScreenLayout;
    [SerializeField] private GameObject largeScreenLayout;

    // Start is called before the first frame update
    void Start()
    {
        UpdateLayout();
    }
    
    private void UpdateLayout()
    {
        float screenWidth = Screen.width;

        smallScreenLayout.SetActive(screenWidth < 1280);
        mediumScreenLayout.SetActive(screenWidth >= 1280 && screenWidth < 1920);
        largeScreenLayout.SetActive(screenWidth >= 1920);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
