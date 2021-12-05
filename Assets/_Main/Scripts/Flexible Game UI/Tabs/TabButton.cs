using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.Events;

[RequireComponent(typeof(Image))]
[RequireComponent(typeof(Color))]
public class TabButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{

    public TabGroup tabgroup;

    public Image backgroundImage;

    public Color hilightColor;

    public UnityEvent onTabSelected;
    public UnityEvent onTabDeselected;

    private Button buttonComp;

    public void OnPointerClick(PointerEventData eventData)
    {
        tabgroup.OnTabSelected(this);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        tabgroup.OnTabEnter(this);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        tabgroup.OnTabExit(this);
    }

    public void Select()
    {
        if (onTabSelected != null)
        {
            onTabSelected.Invoke();
        }
    }

    public void Deselect()
    {
        if (onTabDeselected != null)
        {
            onTabDeselected.Invoke();
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        //tabgroup.Subscribe(this);
        backgroundImage = this.GetComponent<Image>();
        buttonComp = GetComponent<Button>();
        buttonComp.onClick.AddListener(TabClicked);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void TabClicked()
    {
        tabgroup.OnTabSelected(this);
    }
}
