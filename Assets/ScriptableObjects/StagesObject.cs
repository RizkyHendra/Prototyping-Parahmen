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
    public Vector3 playerDialoguePosition;
    public Quaternion playerDialogueRotatation;
    public GameObject prefabStage;
    [TextArea] public string questName;
    public string PopUpQuestName;

    [Header("Dialogue")]
    public string[] nameDialogue;
    [TextArea] public string[] textDialogue;
    public Sprite[] character;
    public Vector3[] cameraPosition;
    public Quaternion[] cameraRotation;
    public int[] dialogueType;
    public string[] sfxAudioName;
}
