using System;

namespace _0_Core.Entity.Building {
	/// <summary>
	/// The data fed to the building object
	/// This stores the information such as id and positions on the map grid
	/// </summary>
	public class BuildingData : EntityData {
		public int Size;

		public BuildingData(Guid id, string name, int health, int size) : base(id, name, health) {
			Size = size;
		}
	}
}
