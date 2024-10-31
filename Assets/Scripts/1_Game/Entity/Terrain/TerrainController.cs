using _0_Core.Entity.Terrain;
using UnityEngine;

namespace _1_Game.Entity.Terrain {
    /// <summary>
    /// Only used to create terrain objects using unity
    /// For the generation code see TerrainGenerator.cs
    /// </summary>
    public class TerrainController : MonoBehaviour {
        private void OnEnable() {
            TerrainGenerator.Generate();
        }
    }
}