using System;

namespace _0_Core.Entity.Unit {
    /// <summary>
    /// The data fed to the Unit object
    /// This stores the information such as id and positions on the map grid
    /// </summary>
    public class UnitData : EntityData {
        public int Damage;
        public int Speed;
        
        public UnitData(Guid id, string name, int health, int damage, int speed) : base(id, name, health) {
            Damage = damage;
            Speed = speed;
        }
    }
}