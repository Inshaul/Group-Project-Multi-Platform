using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractableObject : MonoBehaviour {
    public ItemDescription m_itemDescription;
    public virtual void Interact() { }
}
