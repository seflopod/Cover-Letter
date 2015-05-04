using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Events;

[System.Serializable]
public class CLAction
{
	public Verbs Verb;
	public EventTrigger.TriggerEvent Callback;
}