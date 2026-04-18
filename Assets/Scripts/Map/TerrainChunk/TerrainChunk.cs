using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class TerrainChunk : NetworkBehaviour
{
    private MapCreator mapCreator;

    public  GameObject[] NeighbourChunks => neighbourChunks;
    private GameObject[] neighbourChunks;
    private PropRandomizer[] propRandomizer;

    public void SetPropsActivity(bool isActiveNow)
    {
        for (int i = 0; i < propRandomizer.Length; i++)
        {
            propRandomizer[i].SetPropsActivity(isActiveNow);
        }
    }

    public void Setup(MapCreator creator)
    {
        propRandomizer = GetComponentsInChildren<PropRandomizer>(true);
        mapCreator = creator;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!IsServer)
            return;
        if (collision.tag == "Player")
        {
            neighbourChunks = mapCreator.SpawnChunks(transform.position, this);
        }
    }
}
