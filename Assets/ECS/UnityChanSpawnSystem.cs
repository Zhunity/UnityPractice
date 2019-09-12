using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using Unity.Transforms;
using UnityEngine;

// ComponentSystems run on the main thread. Use these when you have to do work that cannot be called from a job.
public class UnityChanSpawnSystem : ComponentSystem
{
	EntityQuery _spawner;

	protected override void OnCreateManager()
	{
		_spawner = GetEntityQuery(typeof(UnityChanSpawner), typeof(Translation));
	}

	protected override void OnUpdate()
	{
		// Get all the spawners in the scene.
		using (var spawners = _spawner.ToEntityArray(Unity.Collections.Allocator.TempJob))
		{
			foreach(var spawner in spawners)
			{
				var spawnerData = EntityManager.GetSharedComponentData<UnityChanSpawner>(spawner);
				int count = spawnerData.count;
				if (count <= 0)
				{
					EntityManager.DestroyEntity(spawner);
					continue;
				}
				var lastSpawnTime = spawnerData.lastSpawnTime;
				if (lastSpawnTime < 1)
				{
					lastSpawnTime += UnityEngine.Time.deltaTime;
					spawnerData.lastSpawnTime = lastSpawnTime;
				}
				else
				{
					var entity = EntityManager.Instantiate(spawnerData.prefab);
					var position = EntityManager.GetComponentData<Translation>(spawner);
					int index = spawnerData.index;
					// 为什么y值一直在加？
					position.Value += new Unity.Mathematics.float3(1.1f * index, -1, 0);
					EntityManager.SetComponentData(entity, position);
					spawnerData.index++;
					spawnerData.count--;
					spawnerData.lastSpawnTime = 0;
				}
				EntityManager.SetSharedComponentData(spawner, spawnerData);
			}
		}

	}
}
