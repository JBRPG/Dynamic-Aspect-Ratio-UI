using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TabGroup : MonoBehaviour
{
    public List<TabButton> tabButtons;
    public Sprite tabIdle;
    public Sprite tabHover;
    public Sprite tabSelected;
    public Color tabIdleColor;
    public Color tabHoverColor;
    public Color tabSelectedColor;

    public TabButton selectedTab;
    public List<GameObject> swapableMenuElements;

    public void Subscribe(TabButton button)
    {
        if (tabButtons == null)
        {
            tabButtons = new List<TabButton>();
        }

        tabButtons.Add(button);
    }

    public void OnTabEnter(TabButton button)
    {

        ResetTabs();
        if (selectedTab == null || button != selectedTab)
        {
            button.backgroundImage.color = tabHoverColor;
        }
        
    }

    public void OnTabExit(TabButton button)
    {
        ResetTabs();
    }

    public void OnTabSelected(TabButton button)
    {
        if (selectedTab != null)
        {
            selectedTab.Deselect();
        }

        selectedTab = button;
        selectedTab.Select();

        ResetTabs();
        button.backgroundImage.color = tabSelectedColor;
        int index = button.transform.GetSiblingIndex();
        for (int i = 0; i < swapableMenuElements.Count; ++i)
        {
            if (i == index)
            {
                swapableMenuElements[i].SetActive(true);
            }
            else
            {
                swapableMenuElements[i].SetActive(false);
            }
        }
    }

    public void ResetTabs()
    {
        foreach (TabButton button in tabButtons)
        {
            if (selectedTab != null && button == selectedTab)
            {
                continue;
            }
            // Placeholder for color only
            button.backgroundImage.color = tabIdleColor;
        }
    }

    private void Start()
    {
        ResetTabs();
        initializeTabs();
    }

    private void initializeTabs()
    {
        tabButtons = new List<TabButton>();
        TabButton[] allTabs = GetComponentsInChildren<TabButton>();
        foreach (TabButton button in allTabs)
        {
            tabButtons.Add(button);
        }
    }
}
