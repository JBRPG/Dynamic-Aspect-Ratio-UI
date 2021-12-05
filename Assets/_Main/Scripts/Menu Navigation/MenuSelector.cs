using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class MenuSelector : MonoBehaviour
{
    public GameObject firstSelectedItem;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnEnable()
    {
        // select the desired menu item as first selected object
        StartCoroutine(selectFirstMenuItem());
    }

    private void OnDisable()
    {
        // clear selected menu item object
        EventSystem.current.SetSelectedGameObject(null);
    }

    IEnumerator selectFirstMenuItem()
    {
        yield return new WaitForEndOfFrame();
        EventSystem.current.SetSelectedGameObject(firstSelectedItem);
    }
}
