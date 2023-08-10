using System.Collections;
using UnityEngine;

public class MiniMapDemo : MonoBehaviour
{
    private MeshRenderer player;
    private MeshRenderer questTrigger;

    public Sprite objSprite;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<MeshRenderer>();
        questTrigger = GameObject.FindGameObjectWithTag("QuestTrigger").GetComponent<MeshRenderer>();

        var minimap = FindObjectOfType<Arikan.MiniMapView>();

        // Green object example
        //var img2 = minimap.Follow(obj2.transform);
        //img2.color = obj2.material.color;

        var img = minimap.FollowCentered(player.transform);
        img.color = Color.red;

        var img3 = minimap.Follow(questTrigger.transform, objSprite);
        img3.color = Color.yellow;
    }
}
