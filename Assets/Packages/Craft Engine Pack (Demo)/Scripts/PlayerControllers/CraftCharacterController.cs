using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CraftCharacterController : MonoBehaviour
{
    public Inventory m_inventory; //model
    public CraftCharacterView m_view; //view system
    public Transform m_toolHandler;// where we hold tool
    public ToolActionController m_toolActionController;
    public AccessViaRayCast m_eyes;
    public PlacementManager m_placementManager;

    int m_currentToolNumber = -1;

    void Start()
    {
        SetTool(1);
    }
    #region ClickHandlers
    public void CellLeftClick(Cell cell)
    {
        if (m_inventory.m_cellOnCursor == null && cell.m_item == null) //empty cursor click on empty cell
            return;

        if (m_inventory.m_cellOnCursor == null) //empty cursor -> move all items in cell to cursor
        {
            m_inventory.MoveItemsFromCellToCursor(cell);
        }
        else //if some items are on cursor
        {
            if (cell.m_item == null) // emty cell -> move all items on cursor to cell
            {
                m_inventory.MoveItemsFromCursorToCell(cell);
            }
            else if (cell.m_item.Equals(m_inventory.m_cellOnCursor.m_item)) // equal items
            {
                m_inventory.RecountToCell(cell, Mathf.Min(m_inventory.m_cellOnCursor.m_count, cell.m_item.m_maxCount - cell.m_count));
            }
            else // different items
            {
                m_inventory.SwapCursorAndCellItems(cell);
            }
        }
    }
    public void CellRightClick(Cell cell)
    {
        if (cell.m_item == null && m_inventory.m_cellOnCursor == null) // empty cursor click on empty cell -> nothing to do
            return;

        if (cell.m_item == null) // empty cell under cursor -> put 1 item to cell
            m_inventory.RecountToCell(cell, 1);
        else if (m_inventory.m_cellOnCursor == null) // empty cursor -> get half of items to cursor
        {
            int count = cell.m_count / 2;
            if (count == 0)
            {
                m_inventory.MoveItemsFromCellToCursor(cell, 1);// if only 1 item in cell -> we get this
            }
            else
                m_inventory.MoveItemsFromCellToCursor(cell, count);// get a half
        }
        // cursor not empty && cell not empty -> put one item from cursor to cell if compatible types
        else if (m_inventory.m_cellOnCursor.m_item == cell.m_item)
            m_inventory.RecountToCell(cell, Mathf.Min(1, cell.m_item.m_maxCount - cell.m_count));
    }
    public void CraftCellLeftClick(Cell cell)
    {
        CellLeftClick(cell);
        CrafterAction();
    }
    public void CraftCellRightClick(Cell cell)
    {
        CellRightClick(cell);
        CrafterAction();
    }
    public void ArmorCellLeftClick(CellArmor cellRenderer)
    {
        if (m_inventory.AreCompatibleArmorAndCellOnCursor(cellRenderer.m_type))
            CellLeftClick(cellRenderer.m_cell);
    }
    public void ArmorCellRightClick(CellArmor cellRenderer)
    {
        if (m_inventory.AreCompatibleArmorAndCellOnCursor(cellRenderer.m_type))
            CellRightClick(cellRenderer.m_cell);
    }
    public void CrafterButtonClick(CrafterButton button)
    {
        //if cursor items incompatible with button items -> exit
        if (m_inventory.m_cellOnCursor != null && m_inventory.m_cellOnCursor.m_item != button.m_item)
            return;

        int countUserWantsToCraft = 1;
        if (Input.GetKey(KeyCode.LeftShift))//in this case user wants to craft as maximum items as possible
            countUserWantsToCraft = button.m_selectedItemsForCraft[0].m_count; // it depends on minimum count of ingridients

        //but this number can not be bigger than maximum stack size
        int countUserCanCraft = (int)Mathf.Min(countUserWantsToCraft, button.m_item.m_maxCount - (m_inventory.m_cellOnCursor == null ? 0 : m_inventory.m_cellOnCursor.m_count));
        if (countUserCanCraft == 0)
            return;
        foreach (Cell cell in button.m_selectedItemsForCraft)
        {
            cell.m_count -= countUserCanCraft;
            m_view.CellCountChangedHandler(cell);
        }
        CrafterAction();

        if (m_inventory.m_cellOnCursor == null)
            m_inventory.m_cellOnCursor = new CellInfo { m_item = button.m_item, m_count = 0 };
        m_inventory.m_cellOnCursor.m_count += countUserCanCraft;
        m_view.CursorStateChangedHandler(m_inventory.m_cellOnCursor);
    }
    #endregion

    #region DroppingItems
    public void DropItemsOnCrafter()
    {
        if (m_inventory.m_craftCells == null)
            return;

        foreach (Cell cell in m_inventory.m_craftCells)
        {
            if(cell.m_item != null)
            {
                GameObject prefab = Resources.Load<GameObject>("Prefabs/" + cell.m_item.m_prefab);
                do
                    DropItemInRandomPosition(prefab);
                while (--cell.m_count != 0);
                cell.m_item = null;
            }
        }
        CrafterAction();
    }
    public void DropAllItems()
    {
        DropItemsOnCursor();
        DropItemsOnCrafter();
    }
    void DropItemsOnCursor()
    {
        if (m_inventory.m_cellOnCursor != null)
        {
            GameObject prefab = Resources.Load<GameObject>("Prefabs/" + m_inventory.m_cellOnCursor.m_item.m_prefab);
            while (m_inventory.m_cellOnCursor.m_count-- != 0)
                DropItemInRandomPosition(prefab);
            m_inventory.m_cellOnCursor = null;
            m_view.CursorStateChangedHandler(m_inventory.m_cellOnCursor);
        }
    }
    void DropItemInRandomPosition(GameObject prefab)
    {
        Vector2 randomInCircle = UnityEngine.Random.insideUnitCircle * 2.0f + new Vector2(transform.position.x, transform.position.z);
        Vector3 newTransform = new Vector3(randomInCircle.x, transform.up.y, randomInCircle.y);
        Instantiate(prefab, newTransform, Quaternion.Euler(UnityEngine.Random.insideUnitSphere * 100));
    }
    void DropItemForward(GameObject prefab)
    {
        Vector3 newTransform = transform.position + transform.forward + Vector3.up;
        Instantiate(prefab, newTransform, Quaternion.Euler(UnityEngine.Random.insideUnitSphere * 100));
    }
    public void DropSelectedItem()
    {
        if (m_inventory.m_selectedToolCell.m_item == null)
            return;
        if (!m_inventory.m_selectedToolCell.m_item.m_isLiftable)
            return;
        DropItemForward(Resources.Load<GameObject>("Prefabs/" + m_inventory.m_selectedToolCell.m_item.m_prefab));
        --m_inventory.m_selectedToolCell.m_count;
        SelectedToolChangedHandler();
        m_view.CellCountChangedHandler(m_inventory.m_selectedToolCell);
    }
    #endregion

    #region Object Placement
    public void ObjectPlacement()
    {
        if(m_placementManager.m_objectToPlace) // if some object in placement manager and it is possible to place it - place it
        {
            if (!m_placementManager.PlaceObject())
                return;
            --m_inventory.m_selectedToolCell.m_count;
            SelectedToolChangedHandler();
            m_view.CellCountChangedHandler(m_inventory.m_selectedToolCell);
        }
        // if there is nothing to place -> set object for placement
        else if (m_inventory.m_selectedToolCell.m_item != null && Resources.Load<GameObject>("Prefabs/" + m_inventory.m_selectedToolCell.m_item.m_prefab).GetComponent<PlacableItem>()) 
        {
            m_placementManager.m_objectToPlace = Resources.Load<GameObject>("Prefabs/" + m_inventory.m_selectedToolCell.m_item.m_prefab);
        }
    }
    public void ObjectRotating()
    {
        m_placementManager.RotateObject();
    }
    public void DisablePlacementMode()
    {
        m_placementManager.m_objectToPlace = null;
    }
    #endregion
    void CrafterAction()
    {
        m_view.DeleteAllCrafterButtons();//smth has been changed -> refresh craft buttons
        foreach (ItemDescription item in m_inventory.m_prefabsWithReceipt) //loop all craftable items
        {
            List<Cell> selected = new List<Cell>();//link to cells with ingredients (at a moment of craft count if this cells will decrement)
            bool isItemCraftable = true;//flag (shame on me). It indicates whether you have resources for craft this item
            foreach (ItemDescription element in item.m_receiptItems)//loop all ingredients for craft current item
            {
                Cell cell = m_inventory.SearchCellInCrafterByItemDescription(element, selected);
                if (cell == null)
                {
                    isItemCraftable = false;
                    break;
                }
                selected.Add(cell);
            }
            if (isItemCraftable)
            {
                m_view.InstantiateCrafterButton(item, selected);
            }
        }
    }
    public void SelectNextTool()
    {
        SetTool(m_currentToolNumber == m_inventory.m_toolCount ? 1 : m_currentToolNumber + 1);
    }
    public void SelectPreviousTool()
    {
        SetTool(m_currentToolNumber - 1 == 0 ? m_inventory.m_toolCount : m_currentToolNumber - 1);
    }
    public void SetTool(int number)//set tool by number
    {
        if (number == m_currentToolNumber)
            return;

        m_currentToolNumber = number;

        Cell curTool;
        curTool = m_inventory.GetCellOnIndex(number - 1);
        m_inventory.m_selectedToolCell = curTool;

        SelectedToolChangedHandler();
    }
    public void SelectedToolChangedHandler()
    {
        m_placementManager.m_objectToPlace = null; // disable placement mode

        if (m_toolHandler.childCount > 0) // clear hands for new tool
            Destroy(m_toolHandler.GetChild(0).GetComponent<Transform>().gameObject);


        // if in selected cell are some items and if it is possible to hold item in hands
        if (m_inventory.m_selectedToolCell.m_item && m_inventory.m_selectedToolCell.m_item.m_isVisibleInHands)
        {
            // pick up tool
            GameObject tool = Instantiate(Resources.Load<GameObject>("Prefabs/" + m_inventory.m_selectedToolCell.m_item.m_prefab), m_toolHandler);

            //disable all that can disturb us
            if(tool.GetComponent<Rigidbody>())
                tool.GetComponent<Rigidbody>().useGravity = false;
            Destroy(tool.GetComponent<Collider>());
            foreach (Collider c in tool.GetComponentsInChildren<Collider>())
                Destroy(c);

            //set position and rotation
            tool.transform.localPosition = new Vector3();
            tool.transform.localRotation = Quaternion.Euler(new Vector3());

            //if item is tool -> set reference to eyes for it
            if (m_toolActionController.m_tool = tool.GetComponent<Tool>())
                m_toolActionController.m_tool.m_eyes = m_eyes;
        }
    }
}