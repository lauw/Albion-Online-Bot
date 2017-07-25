using System;
using UnityEngine;

namespace UnityGameObject
{
	public class Loader
	{
		public static GameObject load_object;

		public static void Load()
		{
			Loader.load_object = new GameObject();
			Loader.load_object.AddComponent<Menu>();
			UnityEngine.Object.DontDestroyOnLoad(Loader.load_object);
		}
	}
}
