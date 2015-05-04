using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System;
using System.Text;
using System.Collections.Generic;

public class GameManager : OgreToast.Utility.Singleton<GameManager>
{
	public RoomData[] Rooms;
	public InventoryItem[] InventoryItems;
	private Text _console;
	private int _curRoom = -1;
	private InputField _inputLine;
	private List<string> _verbs;
	private Dictionary<Verbs, Action> _defaultActions;

	#region monobehaviour
	private void Start()
	{
		_defaultActions = createDefaultActions();
		_console = GameObject.FindGameObjectWithTag("console").
														GetComponent<Text>();
		_inputLine = GameObject.FindGameObjectWithTag("inputLine").transform.
								parent.gameObject.GetComponent<InputField>();
		_inputLine.Select();
		displayHeader();
		enterRoom(0);
		_verbs = new List<string>();
		var verbs = Enum.GetValues(typeof(Verbs));
		foreach(var v in verbs)
		{
			_verbs.Add(v.ToString());
		}
	}
	#endregion

	#region io
	public void DisplayText(string txt)
	{
		_console.text += "\n" + txt;
	}

	public void EnterText(string txt)
	{
		DisplayText("> " + txt + "\n");
		parseText(txt);
		_inputLine.ActivateInputField();
	}

	private void parseText(string txt)
	{
		char[] chars = txt.ToUpper().ToCharArray();
		StringBuilder verbSB = new StringBuilder(chars.Length);
		StringBuilder subjectSB = new StringBuilder(chars.Length);
		bool haveVerb = false;
		//character-by-character parse to get the verb
		for(int i=0; i<chars.Length; ++i)
		{
			if(!haveVerb)
			{
				verbSB.Append(chars[i]);
				for(int j=0; j<_verbs.Count && !haveVerb; ++j)
				{
					string v = _verbs[j];
					haveVerb = (v == verbSB.ToString());
				}
			}
			else
			{
				subjectSB.Append(chars[i]);
			}
		}

		if(haveVerb)
		{
			Verbs verb = (Verbs)Enum.Parse(typeof(Verbs), verbSB.ToString());

			//treat the rest of the command as the subject.  This doesn't allow
			//for multiple subjects, but is more foregiving of typos.  Oh, and
			//it's a bit easier.
			string subject = subjectSB.ToString();

			//movement is the only bit that is different from the rest in
			//how the verb is executed, so I made a special condition for it.
			if(verb == Verbs.GO)
			{
				if(subject.Contains("NORTH"))
				{
					enterRoom(Rooms[_curRoom].NorthConnectionIndex);
				}
				else if(subject.Contains("EAST"))
				{
					enterRoom(Rooms[_curRoom].EastConnectionIndex);
				}
				else if(subject.Contains("SOUTH"))
				{
					enterRoom(Rooms[_curRoom].SouthConnectionIndex);
				}
				else if(subject.Contains("WEST"))
				{
					enterRoom(Rooms[_curRoom].WestConnectionIndex);
				}
			}
			else
			{
				bool haveActed = false;

				//go over each item in the room to see if it is the subject
				//of the action
				foreach(var itm in Rooms[_curRoom].Items)
				{
					if(subject.Contains(itm.Name))
					{
						foreach(var action in itm.Actions)
						{
							if(action.Verb == verb && action.Callback != null)
							{
								action.Callback.Invoke(null);
								haveActed = true;
							}
						}
					}
				}

				//Since you can LOOK at things not in the room
				//(inventory items), there needs to be a check to see if that's
				//what we want.
				if(!haveActed && verb == Verbs.LOOK)
				{
					for(int i=0; i<InventoryItems.Length && !haveActed; ++i)
					{
						if(subject.Contains(InventoryItems[i].Name) &&
							InventoryItems[i].IsInInventory)
						{
							DisplayText(InventoryItems[i].Description + "\n");
							haveActed = true;
						}
					}
				}

				//and defaults for all other things
				if(!haveActed && _defaultActions.ContainsKey(verb) &&
					_defaultActions[verb] != null)
				{
					_defaultActions[verb]();
				}
			}
		}
	}
	#endregion

	#region accessors for items and inventory
	public bool IsItemVisible(string name)
	{
		for(int i=0; i<Rooms[_curRoom].Items.Length; ++i)
		{
			ItemData itm = Rooms[_curRoom].Items[i];
			if(itm.Name == name)
			{
				return itm.IsVisible;
			}
		}
		return false;
	}

	public void SetItemVisible(string name, bool isVisible)
	{
		for(int i=0; i<Rooms[_curRoom].Items.Length; ++i)
		{
			ItemData itm = Rooms[_curRoom].Items[i];
			if(itm.Name == name)
			{
				itm.IsVisible = isVisible;
				break;
			}
		}
	}

	public bool IsItemTakeable(string name)
	{
		for(int i=0; i<Rooms[_curRoom].Items.Length; ++i)
		{
			ItemData itm = Rooms[_curRoom].Items[i];
			if(itm.Name == name)
			{
				return itm.IsTakeable;
			}
		}
		return false;
	}

	public void SetItemTakeable(string name, bool isTakeable)
	{
		for(int i=0; i<Rooms[_curRoom].Items.Length; ++i)
		{
			ItemData itm = Rooms[_curRoom].Items[i];
			if(itm.Name == name)
			{
				itm.IsTakeable = isTakeable;
				break;
			}
		}
	}

	public bool IsItemInInventory(int idx)
	{
		if(idx < InventoryItems.Length)
		{
			return InventoryItems[idx].IsInInventory;
		}
		return false;
	}

	public void PutInInventory(int idx)
	{
		if(idx < InventoryItems.Length)
		{
			InventoryItems[idx].IsInInventory = true;
		}
	}

	public void TakeFromInventory(int idx)
	{
		if(idx < InventoryItems.Length)
		{
			InventoryItems[idx].IsInInventory = false;
		}
	}
	#endregion

	#region other helpers
	private void enterRoom(int room)
	{
		if(room != -1)
		{
			_curRoom = room;
			DisplayText(Rooms[room].Description + "\n");
		}
		else
		{
			DisplayText("Can't get there from here.\n");
		}
	}

	private void displayHeader()
	{
		DisplayText("********************************************");
		DisplayText("* Peter Bartosch Cover Letter Adventure!   *");
		DisplayText("* bartoschp@gmail.com                      *");
		DisplayText("* 608.616.0192                             *");
		DisplayText("* github.com/seflopod                      *");
		DisplayText("* (c) 2015                                 *");
		DisplayText("********************************************");
		DisplayText("\nType HELP for a list of commands\n");
	}

	/// <summary>
	/// Creates the default actions.
	/// </summary>
	/// <returns>A Dictionary of the default actions.</returns>
	/// <description>
	/// This seemed like the best way to allow for default actions to be done.
	/// I should be able to easily add other defaults as I create more verbs.
	/// </description>
	private Dictionary<Verbs, Action> createDefaultActions()
	{
		return new Dictionary<Verbs, Action>() {
			{ Verbs.ATTACK, () => {
					float roll = UnityEngine.Random.value;
					if(roll < 0.1f)
					{
						DisplayText("You attack the darkness!\n");
					}
					else if(roll < 0.325f)
					{
						DisplayText("Yeah, that's it.  Punch the air.\n");
					}
					else if(roll < 0.55f)
					{
						DisplayText("So aggressive.\n");
					}
					else if(roll < 0.775f)
					{
						DisplayText("There's nothing to attack.\n");
					}
					else
					{
						DisplayText("I conscientiously object to your " +
						            "belligerent actions.\n");
					}
				}},
			{ Verbs.LOOK, () => {
					DisplayText("You see:");
					foreach(var itm in Rooms[_curRoom].Items)
					{
						if(itm.Name != "" && itm.IsVisible)
						{
							DisplayText(itm.Name);
						}
					}
					string exits = "\nThere are exits to the:";
					if(Rooms[_curRoom].NorthConnectionIndex != -1)
					{
						exits += "\nNORTH";
					}
					if(Rooms[_curRoom].EastConnectionIndex != -1)
					{
						exits += "\nEAST";
					}
					if(Rooms[_curRoom].SouthConnectionIndex != -1)
					{
						exits += "\nSOUTH";
					}
					if(Rooms[_curRoom].WestConnectionIndex != -1)
					{
						exits += "\nWEST";
					}
					DisplayText(exits + "\n");
				}},
			{ Verbs.TALK, () => {
					float roll = UnityEngine.Random.value;
					if(roll < 0.25f)
					{
						DisplayText("You keep on talking but it makes no " +
						            "sense at all.\n");
					}
					else if(roll < 0.5f)
					{
						DisplayText("There's no one here to talk to.\n");
					}
					else if(roll < 0.75f)
					{
						DisplayText("A far-away voice wafts through the " +
							"room, leaving behind the scent of clove " +
							"cigarettes and these words, \"A voice speaks " +
							"into the nothingness expecting nothing in " +
							"return.  And in that moment, perhaps that " +
							"voice receives both confirmation of it's own " +
							"existence and the denial of others.  But if " +
							"there are no others, what need is there for the " +
							"voice to exist?\"\n");
					}
					else
					{
						DisplayText("You try to speak, but the words just " +
						            "don't come out.\n");
					}
				}},
			{ Verbs.USE, () => {
					DisplayText("I have nothing more clever to say about " +
					            "this than, \"No.  That cannot be done.\"\n");
				}},
			{ Verbs.INVENTORY, () => {
					DisplayText("In your inventory you have:");
					for(int i=0;i<InventoryItems.Length;++i)
					{
						if(InventoryItems[i].IsInInventory)
						{
							DisplayText(InventoryItems[i].Name);
						}
					}
					DisplayText("");
				}},
			{ Verbs.HELP, () => {
					DisplayText("These are the commands I can execute: " +
						"ATTACK, GO, HELP, INVENTORY, LOOK, TAKE, TALK, and " +
						"USE.\n");
				}}
		};
	}
	#endregion
}