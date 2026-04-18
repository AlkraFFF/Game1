using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class PropRandomizer : NetworkBehaviour
{
    [SerializeField] private GameObject[] propSpawnPoints;
    [SerializeField] private GameObject[] propPrefabs;
    private List<GameObject> props = new List<GameObject>();
    void Start()
    {
        if (!IsServer)
        {
            return;
        }

        foreach (GameObject spawn in propSpawnPoints)
        {   
            int rnd = Random.Range(0, propPrefabs.Length);
            GameObject prop = Instantiate(propPrefabs[rnd], spawn.transform.position,
                Quaternion.identity);
            prop.GetComponent<NetworkObject>().Spawn();
            props.Add(prop);
        }
    }

    public void SetPropsActivity(bool isActiveNow)
    {
        foreach (GameObject prop in props)
        {
            if (prop.activeSelf != isActiveNow)
            {
                prop.SetActive(isActiveNow);
            }
        }
    }
}
