using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CrafterButton : MonoBehaviour {
    public CraftCharacterController m_controller;
    List<Cell> selectedItems;
    public List<Cell> m_selectedItemsForCraft
    {
        get { return selectedItems; }
        set
        {
            value.Sort((a, b) => a.m_count.CompareTo(b.m_count));
            selectedItems = value;
        }
    }
    public ItemDescription m_item { get; set; }
    void Update()
    {
        // if user clicked -> call needed method (CrafterButtonClick)

        RectTransform rtransform = GetComponent<RectTransform>();
        float x = Input.mousePosition.x, y = Input.mousePosition.y;
        float rx = rtransform.position.x + rtransform.rect.x, ry = rtransform.position.y - rtransform.rect.y;
        if (x >= rx && x <= rx + rtransform.rect.width && y <= ry && y >= ry - rtransform.rect.height)
        {
            if (Input.GetMouseButtonDown(0))
                m_controller.CrafterButtonClick(gameObject.GetComponent<CrafterButton>());
        }
    }
}
