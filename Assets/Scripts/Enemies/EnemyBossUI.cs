using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBossUI : MonoBehaviour
{
    [SerializeField] private GameObject bossText;
    [SerializeField] private int bossTextCount = 3;
    void Start()
    {
        bossText.SetActive(false);
    }

    public void ShowBoss()
    {
        StartCoroutine(ShowBossText());
    }

    IEnumerator ShowBossText()
    {
        for (int i = 0; i < bossTextCount; i++)
        {
            bossText.SetActive(true);
            yield return new WaitForSeconds(0.4f);
            bossText.SetActive(false);
            yield return new WaitForSeconds(0.4f);
        }
    }
}
