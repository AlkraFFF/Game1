using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class EnemyDrop : NetworkBehaviour
{
    [System.Serializable]
    public class Drops
    {
        public GameObject ItemObject;
        public int DropCount;
    }

    [SerializeField] private Drops[] drops;

    public void DropItems()
    {
        CreateDropServerRpc();
    }

    [ServerRpc]
    private void CreateDropServerRpc()
    {
        CreateDropClientRpc();
    }

    [ClientRpc]
    private void CreateDropClientRpc()
    {
        for (int i = 0; i < drops.Length; i++)
        {
            for (int j = 0; j < drops[i].DropCount; j++)
            {
                
                Instantiate(drops[i].ItemObject,
                    transform.position + new Vector3(Random.Range(-1.1f,1.1f), Random.Range(-1.1f, 1.1f), 0f),
                    Quaternion.identity);
            }
        }
        
    }
}
