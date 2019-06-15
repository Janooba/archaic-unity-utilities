using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Archaic.Core.Extensions;

using Sirenix.OdinInspector;

public class PrefabSpawner : MonoBehaviour
{
    public enum SpawnMode { SpawnRandom, SpawnAll, SpawnSequential };

    public SpawnMode spawnMode;
    [AssetsOnly]
    public GameObject[] prefabs;
    public bool spawnAsChild = true;
    public Vector3 spawnOffset;

    private int spawnIndex = 0;

    private bool IsPrefabsEmpty { get { return prefabs == null || prefabs.Length <= 0; } }

    [Button(ButtonSizes.Large), DisableIf("IsPrefabsEmpty")]
    public void Spawn()
    {
        if (prefabs == null || prefabs.Length <= 0)
            return;

        switch (spawnMode)
        {
            case SpawnMode.SpawnRandom:
                SpawnPrefab(prefabs.GetRandom());
                break;

            case SpawnMode.SpawnAll:
                for (int i = 0; i < prefabs.Length; i++)
                {
                    SpawnPrefab(prefabs[i]);
                }
                break;

            case SpawnMode.SpawnSequential:
                SpawnPrefab(prefabs[spawnIndex++]);
                if (spawnIndex >= prefabs.Length)
                    spawnIndex = 0;
                break;
        }
    }

    private void SpawnPrefab(GameObject objectToSpawn)
    {
        if (spawnAsChild)
        {
            Instantiate(objectToSpawn, transform.position + spawnOffset, Quaternion.identity, transform);
        }
        else // Spawn as sibling
        {
            Instantiate(objectToSpawn, transform.position + spawnOffset, Quaternion.identity, transform.parent);
        }
    }
}
