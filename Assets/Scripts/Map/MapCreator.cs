using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class MapCreator : NetworkBehaviour
{
    [SerializeField] private GameObject[] terrainChunks;
    private List<Transform> terrainChunksGenerated = new List<Transform>();
    private TerrainChunk curChunk;

    public override void OnNetworkSpawn()
    {
        base.OnNetworkSpawn();

        if (!IsServer)
        {
            return;
        }

        CreateChunk(transform.position);
    }
    public GameObject[] SpawnChunks(Vector3 pos, TerrainChunk chunk)
    {
        if (curChunk != null)
        {
            ClearFarChunks(chunk);
        }

        curChunk = chunk;

        GameObject[] neighbours = new GameObject[8];

        neighbours[0] = CreateChunk(pos + new Vector3(0, 14, 0f));
        neighbours[1] = CreateChunk(pos + new Vector3(0, -14, 0f));

        neighbours[2] = CreateChunk(pos + new Vector3(22, 0, 0f));
        neighbours[3] = CreateChunk(pos + new Vector3(-22, 0, 0f));

        neighbours[4] = CreateChunk(pos + new Vector3(-22, 14, 0f));
        neighbours[5] = CreateChunk(pos + new Vector3(-22, -14, 0f));

        neighbours[6] = CreateChunk(pos + new Vector3(22, 14, 0f));
        neighbours[7] = CreateChunk(pos + new Vector3(22, -14, 0f));

        return neighbours;
    }

    private GameObject CreateChunk(Vector3 pos)
    {
        for (int i = 0; i < terrainChunksGenerated.Count; i++)
        {
            if (terrainChunksGenerated[i].position == pos)
            {
                if (!terrainChunksGenerated[i].gameObject.activeSelf)
                {
                    terrainChunksGenerated[i].gameObject.SetActive(true);
                    terrainChunksGenerated[i].GetComponent<TerrainChunk>().SetPropsActivity(true);
                }

                return terrainChunksGenerated[i].gameObject;
            }
        }

        int rnd = Random.Range(0, terrainChunks.Length);
        GameObject chunk = Instantiate(terrainChunks[rnd], pos, Quaternion.identity);
        chunk.GetComponent<TerrainChunk>().Setup(this);
        chunk.GetComponent<NetworkObject>().Spawn();
        chunk.GetComponent<NetworkObject>().TrySetParent(transform);
        terrainChunksGenerated.Add(chunk.transform);
        return chunk;
    }

    private void ClearFarChunks(TerrainChunk newChunk)
    {
        for (int i = 0; i < curChunk.NeighbourChunks.Length; i++)
        {
            if (curChunk.NeighbourChunks[i].transform.position == newChunk.transform.position)
            {
                continue;
            }

            if (curChunk.NeighbourChunks[i].gameObject.activeSelf)
            {
                curChunk.NeighbourChunks[i].SetActive(false);
                curChunk.NeighbourChunks[i].GetComponent<TerrainChunk>().SetPropsActivity(false);
            }
        }
    }
}
