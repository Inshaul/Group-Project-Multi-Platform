using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CellInfo // holds info about cell
{
    public int m_count { get; set; }
    public ItemDescription m_item { get; set; }
}

public class CellRenderer : MonoBehaviour {
    public Text m_text; // text that displays count of items in stack
    public RawImage m_image; // item icon

    public CraftCharacterController m_controller;

    public Color32 m_notHighlightedColor; //color of cell if cursor is not over it

    Color32 m_highlightedColor = new Color32(255, 255, 255, 255);// if cursor over it

    public Cell m_cell { get; set; } // holds all info about cell

    void Awake()
    {
        m_notHighlightedColor = GetComponent<Image>().color;
    }
    protected virtual void LeftClick()
    {
        m_controller.CellLeftClick(m_cell);
    }
    protected virtual void RightClick()
    {
        m_controller.CellRightClick(m_cell);
    }
    void Update()
    {
        RectTransform rtransform = GetComponent<RectTransform>();
        float x = Input.mousePosition.x, y = Input.mousePosition.y;
        float rx = rtransform.position.x + rtransform.rect.x, ry = rtransform.position.y - rtransform.rect.y;
        //detect whether user clicked on it or not
        if (x >= rx && x <= rx + rtransform.rect.width && y <= ry && y >= ry - rtransform.rect.height)
        {
            if (Input.GetMouseButtonDown(0))
                LeftClick();
            else if (Input.GetMouseButtonDown(1))
                RightClick();
            else
                Hightlight();
        }
        else DisableHightlight();
    }
    void Hightlight()
    {
        GetComponent<Image>().color = m_highlightedColor;
    }
    void DisableHightlight()
    {
        GetComponent<Image>().color = m_notHighlightedColor;
    }
}