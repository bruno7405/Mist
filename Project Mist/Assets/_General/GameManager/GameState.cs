using System;
using System.Collections.Generic;
using UnityEngine;

public class GameState : State
{
    [SerializeField] Transform levelSpawn;
    [SerializeField] List<Collider> borderColliders = new List<Collider>();
    [SerializeField] List<Transform> monsterSpawns = new List<Transform>();
    [SerializeField] List<Transform> monsters = new List<Transform>();
    [SerializeField] GameState nextState;

    // Player Reference
    PlayerManager player;
    CharacterController playerController;

    private void Start()
    {
        player = PlayerManager.instance;
        playerController = player.GetComponent<CharacterController>();
    }

    public override void OnStart()
    {
        if (player == null) player = PlayerManager.instance;
        if (playerController == null) playerController = player.GetComponent<CharacterController>();
    }

    public override void OnUpdate()
    {
        
    }


    public void ResetLevel()
    {
        Debug.Log("Resetting Level");
        playerController.enabled = false;
        player.transform.position = levelSpawn.position;
        player.transform.rotation = levelSpawn.rotation;
        playerController.enabled = true;

        // Fade from black (?)

    }

    public void ProgressToNextLevel()
    {
        Debug.Log("Progressing to Next Level");

        stateMachine.SetNewState(nextState);
    }
}
