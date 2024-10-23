using System;

namespace _0_Core.Entity {
    /// <summary>
    /// EntityData is the base class for any type of building, unit etc
    /// </summary>
    public class EntityData {
        public Guid Id { get; }
        public string Name { get; }
        public int Health { get; }
        public EntityData(Guid id, string name, int health) {
            Id = id;
            Name = name;
            Health = health;
        }
    }
}