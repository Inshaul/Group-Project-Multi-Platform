using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InventoryManager : MonoBehaviour
{
    // Dictionary to track collected items and their counts
    private Dictionary<string, int> inventory = new Dictionary<string, int>();
    [SerializeField] private TextMeshProUGUI inventoryText; // To display item counts on the UI
    
    
    void Start()
    {
        // Initialize the inventory with items set to 0
        inventory["Wood"] = 0;
        inventory["Stone"] = 0;
        UpdateInventoryUI();
    }

    // Function to collect items
    public void CollectItem(string itemName)
    {
        if (inventory.ContainsKey(itemName))
        {
            inventory[itemName]++;
            UpdateInventoryUI();
        }
    }

    // Update the UI to reflect current item counts
    void UpdateInventoryUI()
    {
        inventoryText.text = $"Wood: {inventory["Wood"]} | Stone: {inventory["Stone"]}";
    }

    // Function to craft an item (e.g., sword) if enough resources are available
    public void CraftItem()
    {
        if (inventory["Wood"] >= 1 && inventory["Stone"] >= 1) // Example requirement
        {
            // Deduct required materials
            inventory["Wood"] -= 1;
            inventory["Stone"] -= 1;
            UpdateInventoryUI();
            Debug.Log("Crafted an Item!");
            
        }
        else
        {
            Debug.Log("Not enough resources to craft!");
            
        }
    }
}
