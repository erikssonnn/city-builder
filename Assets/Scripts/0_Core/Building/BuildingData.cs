using System;
using _0_Core.Entity;

namespace _0_Core.Building {
	public class BuildingData : EntityData{
		public BuildingData(Guid id, string name, int health) : base(id, name, health) { }
	}
}
