using System.Collections.Generic;
using System.Collections;
using Unity.Netcode;
using UnityEngine;

public class EnemySummonAbility : NetworkBehaviour
{
    [SerializeField] private float _summonTimer = 15f;
    [SerializeField] private float _summonRadius = 1.5f;

    [Space]
    [SerializeField] private int _summonCount = 3;
    [SerializeField] private GameObject _summonedPrefab;

    [Space]
    [SerializeField] private float _summonAnimTimer = 0.3f;

    private Animator animator;

    private void Start()
    {
        if (!IsServer)
        {
            return;
        }

        animator = GetComponent<Animator>();
        StartCoroutine(SummonCycle());
    }

    private IEnumerator SummonCycle()
    {
        while (true)
        {
            yield return new WaitForSeconds(_summonTimer);

            animator.SetBool("Summon", true);

            yield return new WaitForSeconds(_summonAnimTimer);

            for (int i = 0; i < _summonCount; i++)
            {
                Vector2 spawnPosition = GenerateRandomPositionAround();
                NetworkObject enemyObject = Instantiate(_summonedPrefab, spawnPosition, Quaternion.identity)
                    .GetComponent<NetworkObject>();

                enemyObject.Spawn();
            }

            animator.SetBool("Summon", false);
        }
    }

    private Vector2 GenerateRandomPositionAround()
    {
        Vector2 randomOffset = Random.insideUnitCircle * _summonRadius;
        return (Vector2)transform.position + randomOffset;
    }

    void Update()
    {
        
    }
}
