using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ArmorType
{
    Helmet,
    Bib,
    Leggings,
    Boots,
    Hand
}

public class CellArmor : CellRenderer {
    public ArmorType m_type;
    protected override void LeftClick()
    {
        m_controller.ArmorCellLeftClick(gameObject.GetComponent<CellArmor>());
    }
    protected override void RightClick()
    {
        m_controller.ArmorCellRightClick(gameObject.GetComponent<CellArmor>());
    }
}
