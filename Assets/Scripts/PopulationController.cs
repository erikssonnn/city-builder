using System.Collections.Generic;
using System.Linq;
using ScriptableObjects;
using UnityEngine;

public class PopulationController : MonoBehaviour {
	[Header("POPULATION: ")]
	[SerializeField] private int startPopulation = 0;
	[SerializeField] private GameObject unitPrefab = null;
	[SerializeField] private Transform parentObj = null;

	[Header("OCCUPATION: ")]
	[SerializeField] private List<Occupation> allOccupations = new List<Occupation>();
	
	private List<UnitController> allUnits = new List<UnitController>();
	public int Population { get; private set; } = 0;

	private void Start() {
		NullObjectCheck();
		PlaceStartUnits();
	}
	
	private void NullObjectCheck() {
		if (unitPrefab == null) {
			throw new System.Exception("unitPrefab is null on " + name);
		}
		if (parentObj == null) {
			throw new System.Exception("parentObj is null on " + name);
		}
	}

	private void PlaceStartUnits() {
		for (int i = 0; i < startPopulation; i++) {
			Vector3Int randomPos = new Vector3Int(Random.Range(-10, 10), 0, Random.Range(-10, 10));
			CreateUnit(randomPos);
		}
	}

	public void CreateUnit(Vector3 pos) {
		GameObject newUnit = Instantiate(unitPrefab, pos, Quaternion.identity, parentObj);
		UnitController unitObject = newUnit.GetComponent<UnitController>();
		unitObject.Occupation = allOccupations[0];
		allUnits.Add(unitObject);
	}

	public bool HasFreeUnit() {
		return allUnits.Any(unit => unit.Occupation.occupationType == OccupationType.DEFAULT ||
			unit.Occupation.occupationType == OccupationType.DEFAULT);
	}

	public UnitController GetFreeUnit() {
		foreach (UnitController t in allUnits.Where(t => t.Occupation.occupationType == OccupationType.HOMELESS 
				|| t.Occupation.occupationType == OccupationType.DEFAULT)) {
			return t;
		}
		throw new System.Exception("No free unit was detected, please check before you run this function!");
	}
}
