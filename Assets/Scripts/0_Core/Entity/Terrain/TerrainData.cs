using System;

namespace _0_Core.Entity.Terrain {
    /// <summary>
    /// The data fed to the terrain object
    /// This stores the information such as id and positions on the map grid
    /// </summary>
    public class TerrainData : EntityData {
        public int Size;
        public TerrainType Type;
        public int Weight;

        public TerrainData(Guid id, string name, int health, int size, TerrainType type, int weight) : base(id, name, health) {
            Size = size;
            Type = type;
            Weight = weight;
        }
    }
}