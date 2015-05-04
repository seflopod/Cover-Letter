using UnityEngine;

[System.Serializable]
public class ItemData
{
	public string Name;
	public bool IsVisible = true;
	public bool IsTakeable = false;
	public CLAction[] Actions;
}
