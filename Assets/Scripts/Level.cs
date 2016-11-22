using System;
using UnityEngine;

namespace AssemblyCSharp
{
	public interface Level
	{
		string LevelName();
		void Spawn (GameObject player);
		void Initialize();
	}
}

