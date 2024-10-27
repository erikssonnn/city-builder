using System;
using _0_Core.Entity;
using UnityEngine;

namespace _0_Core.Building {
	/// <summary>
	/// The data fed to the building object
	/// This stores the information such as id and positions on the map grid
	/// </summary>
	public class BuildingData : EntityData {
		public int Size; // size of the building on the grid

		public BuildingData(Guid id, string name, int health, int size) : base(id, name, health) {
			Size = size;
		}
	}
}
