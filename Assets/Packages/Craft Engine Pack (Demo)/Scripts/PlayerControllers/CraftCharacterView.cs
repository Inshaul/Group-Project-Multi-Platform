using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class CraftCharacterView : MonoBehaviour
{
    public Color m_selectedToolColor = Color.red; // selected cell on the tool panel

    Cell m_selected = null;
    Color m_selectedOldColor;

    public GameObject m_crafterButtonsContainer;

    public GameObject m_crafterButtonPrefab;

    public GameObject m_cellOnCursorRenderer;
    public void DeleteAllCrafterButtons()
    {
        foreach (Button but in m_crafterButtonsContainer.GetComponentsInChildren<Button>())
            Destroy(but.gameObject);
    }
    public void InstantiateCrafterButton(ItemDescription item, List<Cell> selected)
    {
        GameObject button = Instantiate(m_crafterButtonPrefab, m_crafterButtonsContainer.transform);
        //initialize button fields and render icon and name
        CrafterButton buttonDescription = button.GetComponent<CrafterButton>();
        buttonDescription.m_item = item;
        buttonDescription.m_selectedItemsForCraft = selected;
        buttonDescription.m_controller = GetComponent<CraftCharacterController>();
        button.GetComponentInChildren<RawImage>().texture = Resources.Load<Texture>("Sprites/" + item.m_sprite);
        button.GetComponentInChildren<Text>().text = item.m_name;
    }
    public void CellItemChangedHandler(Cell cell)
    {
        if (cell.m_item == null)
        {
            cell.m_renderer.m_image.gameObject.SetActive(false);
        }
        else
        {
            cell.m_renderer.m_image.gameObject.SetActive(true);
            cell.m_renderer.m_image.texture = Resources.Load<Texture>("Sprites/" + cell.m_item.m_sprite);
        }
    }
    public void CellCountChangedHandler(Cell cell)
    {
        cell.m_renderer.m_text.text = cell.m_count > 1 ? cell.m_count.ToString() : "";
    }
    public void CursorStateChangedHandler(CellInfo cursorCellInfo)
    {
        if (cursorCellInfo == null)//if cursor is without items -> disable icon and enable cursor arrow
        {
            m_cellOnCursorRenderer.gameObject.SetActive(false);
            Cursor.visible = true;
        }
        else
        {
            Cursor.visible = false; //disable cursor
            //set position of icon
            m_cellOnCursorRenderer.GetComponent<RectTransform>().position = new Vector3(Input.mousePosition.x, Input.mousePosition.y);
            //activate
            m_cellOnCursorRenderer.SetActive(true);
            //set image
            m_cellOnCursorRenderer.GetComponent<RawImage>().texture = Resources.Load<Texture>("Sprites/" + cursorCellInfo.m_item.m_sprite);
            //set text
            m_cellOnCursorRenderer.GetComponentInChildren<Text>().text = cursorCellInfo.m_count > 1 ? cursorCellInfo.m_count.ToString() : "";
        }
    }
    public void SelectedToolChangedHandler(Cell cell)
    {
        if(m_selected != null)
        {
            //return old color because it is not selected now
            m_selected.m_renderer.m_notHighlightedColor = m_selectedOldColor;
        }
        //set new selected tool and color
        m_selected = cell;
        m_selectedOldColor = m_selected.m_renderer.m_notHighlightedColor;
        m_selected.m_renderer.m_notHighlightedColor = m_selectedToolColor;
    }
}
