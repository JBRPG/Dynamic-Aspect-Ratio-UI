using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuManager : MonoBehaviour
{
    public MenuSelector mainMenu;
    public MenuSelector optionsSetting;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void EnterMainMenu()
    {
        optionsSetting.gameObject.SetActive(false);
        mainMenu.gameObject.SetActive(true);
    }



    public void EnterOptionsMenu()
    {
        mainMenu.gameObject.SetActive(false);
        optionsSetting.gameObject.SetActive(true);
    }
}
