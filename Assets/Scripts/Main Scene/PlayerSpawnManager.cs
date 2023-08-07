using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSpawnManager : MonoBehaviour
{
    [SerializeField] private GameObject controllerPlayer;
    [SerializeField] private Vector3 spawnPosition;

    private Transform player;
    void Awake()
    {
        player = controllerPlayer.transform.GetChild(1);
        player.transform.position = spawnPosition;
        Instantiate(controllerPlayer);
    }
}
