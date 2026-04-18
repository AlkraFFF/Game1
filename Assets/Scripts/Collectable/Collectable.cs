using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Collectable : MonoBehaviour
{
    [SerializeField] protected float followSpeed = 6f;

    public virtual void Collect(GameObject player)
    {
        StartCoroutine(StartMovement(player.transform));
    }

    protected IEnumerator StartMovement(Transform transform)
    {
        while(true)
        {
            transform.position = Vector3.MoveTowards(transform.position, transform.position,
                followSpeed * Time.deltaTime);
            yield return new WaitForEndOfFrame();
        }
    }

    protected void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player" &&
            collision.transform.GetChild(1).TryGetComponent(out PlayerCollector playerCollector)
            && playerCollector.enabled)
        {
            OnCollected();
            Destroy(gameObject);
        }
    }

    protected abstract void OnCollected();
}
