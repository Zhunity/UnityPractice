using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Unity.Entities;

[Serializable]
public struct UnityChanSpawner : ISharedComponentData
{
	public GameObject prefab;
	public float lastSpawnTime;
	public int count;
	public int index;
}

public class UnityChanSpawnerProxy : SharedComponentDataProxy<UnityChanSpawner> { }


