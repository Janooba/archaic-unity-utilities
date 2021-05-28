using UnityEngine;
using Archaic.Core.Extensions;
using Sirenix.OdinInspector;

namespace Archaic.Core.Utilities
{
    /// <summary>
    /// A simple script used to spawn prefabs from a list. This will not manage children spawned in any way, so be careful.
    /// </summary>
    public class PrefabSpawner : MonoBehaviour
    {
        public enum SpawnMode { SpawnRandom, SpawnAll, SpawnSequential };

        public SpawnMode spawnMode;
        [AssetsOnly]
        public GameObject[] prefabs;
        public bool spawnAsChild = true;
        public Vector3 spawnOffset;

        public bool randomOffset;
        [ShowIf("randomOffset")]
        public float randomOffsetRadius = 0f;

        public bool randomRotation;

        public bool autoKill;
        [ShowIf("autoKill")]
        public float killDelay;

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
            Vector3 position = transform.position + spawnOffset;
            Quaternion rotation = Quaternion.identity;

            if (randomOffset)
            {
                position += Random.insideUnitSphere * randomOffsetRadius;
            }

            if (randomRotation)
            {
                rotation *= Random.rotationUniform;
            }

            GameObject spawnedObject;
            if (spawnAsChild)
            {
                spawnedObject = Instantiate(objectToSpawn, position, rotation, transform);
            }
            else // Spawn as sibling
            {
                spawnedObject = Instantiate(objectToSpawn, position, rotation, transform.parent);
            }

            if (autoKill)
            {
                spawnedObject.AddComponent<Suicide>().delay = killDelay;
            }
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.cyan;
            DebugExtension.DrawPoint(transform.position + spawnOffset, 0.5f);
        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.cyan;
            Gizmos.DrawWireSphere(transform.position + spawnOffset, randomOffsetRadius);
        }
    }
}