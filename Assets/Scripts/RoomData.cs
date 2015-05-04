using UnityEngine;

[System.Serializable]
public class RoomData
{
	public string Name;
	public int NorthConnectionIndex = -1;
	public int EastConnectionIndex = -1;
	public int SouthConnectionIndex = -1;
	public int WestConnectionIndex = -1;
	[Multiline]
	public string Description;
	public ItemData[] Items;
}
