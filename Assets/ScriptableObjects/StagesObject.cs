using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/StagesObject")]

public class StagesObject : ScriptableObject
{
    [Header("Player")]
    public Vector3 playerSpawnPosition;
    public Quaternion playerSpawnRotation;

    [Header("Quest")]
    public Vector3 questSpawnPosition;
    public Quaternion questSpawnRotation;
    public GameObject prefabStage;
    public string questName;

    [Header("Dialogue")]
    public string[] nameDialogue;
    public string[] textDialogue;
    public Sprite[] character;
    public Vector3[] cameraPosition;
    public Quaternion[] cameraRotation;
}
