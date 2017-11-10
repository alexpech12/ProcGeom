using UnityEngine;
using System.Collections;

public static class Extensions {

	public static T EnsureComponent<T>(this GameObject obj) where T : Component
	{
		T component = obj.GetComponent<T>();
		if ( component == null ) {
			component = obj.AddComponent<T>();
		}
		return component;
	}

	// Ensures there is a gameobject attached to this object as a child, with the given name
	public static GameObject EnsureChildGameObject(this GameObject obj, string name)
	{
		Transform child = obj.transform.Find(name);
		if ( child == null ) {
			child = (new GameObject(name)).transform;
			child.SetParent(obj.transform);
		}
		return child.gameObject;
	}

}
