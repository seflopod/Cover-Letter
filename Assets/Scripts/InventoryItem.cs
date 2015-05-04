using UnityEngine;
using System.Collections;

[System.Serializable]
public class InventoryItem
{
	public string Name;
	[Multiline]
	public string Description;

	[HideInInspector]
	public bool IsInInventory = false;
}
