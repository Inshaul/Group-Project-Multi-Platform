using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CellCraft : CellRenderer {
    protected override void LeftClick()
    {
        m_controller.CraftCellLeftClick(m_cell);
    }
    protected override void RightClick()
    {
        m_controller.CraftCellRightClick(m_cell);
    }
}
