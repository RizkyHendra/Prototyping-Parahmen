using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class WaypointQuestIndicator : MonoBehaviour
{
    public Image questIconImg;
    public Transform target;
    public TextMeshProUGUI meter, indicatorGround;
    public Vector3 offset;
    public GameObject player;

    private bool isLimitation;
    private bool isPlayerGround, isTargetGround;

    private void Start()
    {
        questIconImg = GameObject.FindGameObjectWithTag("QuestIndicator").GetComponent<Image>();
        meter = questIconImg.gameObject.transform.GetChild(0).GetComponent<TextMeshProUGUI>();
        indicatorGround = questIconImg.gameObject.transform.GetChild(1).GetComponent<TextMeshProUGUI>();

        target = GameObject.FindGameObjectWithTag("QuestTrigger").GetComponent<Transform>();
    }

    private void Update()
    {
        float minX = questIconImg.GetPixelAdjustedRect().width / 2;
        float maxX = Screen.width - minX;

        float minY = questIconImg.GetPixelAdjustedRect().height / 2;
        float maxY = Screen.height - minY;

        Vector2 pos = Camera.main.WorldToScreenPoint(target.position + offset);

        if (Vector3.Dot((target.position - transform.position), transform.forward) < 0)
        {
            if (pos.x < Screen.width / 2)
            {
                pos.x = maxX;
            }
            else
            {
                pos.x = minX;
            }
        }

        pos.x = Mathf.Clamp(pos.x, minX, maxX);
        pos.y = Mathf.Clamp(pos.y, minY, maxY);

        questIconImg.transform.position = pos;

        if (questIconImg.transform.position.x <= minX+1 || questIconImg.transform.position.x == maxX || questIconImg.transform.position.y <= minY+1 || questIconImg.transform.position.y == maxY)
        {
            isLimitation = true;
        }
        else
        {
            isLimitation = false;
        }

        meter.text = ((int)Vector3.Distance(target.position, player.transform.position)).ToString() + "m";

        betterIndicatorUI();
    }

    private void betterIndicatorUI()
    {
        if (isLimitation)
        {
            meter.gameObject.SetActive(false);
            indicatorGround.gameObject.SetActive(false);
        }
        else
        {
            meter.gameObject.SetActive(true);
            indicatorGround.gameObject.SetActive(true);

            if (isPlayerGround && isTargetGround || !isPlayerGround && !isTargetGround)
            {
                indicatorGround.gameObject.SetActive(false);
            }
            else
            {
                indicatorGround.gameObject.SetActive(true);
            }
        }

        if (player.transform.position.y < 1.8f)
        {
            isPlayerGround = true;
        }
        else
        {
            isPlayerGround = false;
        }

        if (target.position.y < 1.8f)
        {
            indicatorGround.text = "[Bawah]";
            isTargetGround = true;
        }
        else
        {
            indicatorGround.text = "[Atas]";
            isTargetGround = false;
        }
    }
}
