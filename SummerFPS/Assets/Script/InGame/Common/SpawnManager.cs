using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
	public static SpawnManager Instance;

	Spawnpoint[] spawnPoints;

	void Awake()
	{
		Instance = this;
		spawnPoints = GetComponentsInChildren<Spawnpoint>();
	}

	public Transform GetSpawnpoint()
	{
		return spawnPoints[Random.Range(0, spawnPoints.Length)].transform;
	}
}
