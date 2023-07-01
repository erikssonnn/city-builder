using System.Collections.Generic;
using UnityEngine;

public class PopulationController : MonoBehaviour {
	[SerializeField] private int startPopulation = 0;
	[SerializeField] private GameObject unitPrefab = null;
	[SerializeField] private Transform parentObj = null;

	private List<GameObject> allUnits = new List<GameObject>();
	
	public int Population { get; private set; } = 0;

	private void Start() {
		if (unitPrefab == null) {
			throw new System.Exception("unitPrefab is null on " + name);
		}
		
		if (parentObj == null) {
			throw new System.Exception("parentObj is null on " + name);
		}

		PlaceStartUnits();
	}

	private void PlaceStartUnits() {
		for (int i = 0; i < startPopulation; i++) {
			Vector3Int randomPos = new Vector3Int(Random.Range(-10, 10), 0, Random.Range(-10, 10));
			CreateUnit(randomPos);
		}
	}

	public void CreateUnit(Vector3 pos) {
		GameObject newUnit = Instantiate(unitPrefab, pos, Quaternion.identity, parentObj);
		allUnits.Add(newUnit);
	}
}
