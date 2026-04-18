using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class EnemyAttackAbility : NetworkBehaviour
{
    [SerializeField] private float _attackTimer = 15f;

    [Space]
    [SerializeField] private int _damage = 3;
    [SerializeField] private Collider2D _attackCollider;

    [Space]
    [SerializeField] private float _attackAnimTimer = 0.3f;

    private bool _attackRadius = false;


    private Animator animator;

    void Start()
    {
        if (!IsServer)
        {
            return;
        }

        animator = GetComponent<Animator>();
        StartCoroutine(Attack());
        _attackCollider.enabled = false;
    }

    private IEnumerator Attack()
    {
        while (true)
        {
            yield return new WaitForSeconds(_attackTimer);

            while (_attackRadius)
            {
                animator.SetBool("Attack", true);
                _attackCollider.enabled = true;

                yield return new WaitForSeconds(_attackAnimTimer);

                // Take damage.

                animator.SetBool("Attack", false);
                _attackCollider.enabled = false;
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            _attackRadius = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            _attackRadius = false;
        }
    }
}
