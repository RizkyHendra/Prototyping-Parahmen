using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CursorManager : MonoBehaviour
{
    public static CursorManager Instance;

    [SerializeField] private GameObject popUpQuest;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }
    private void Start()
    {
        DisableCursorMouse();
        StartCoroutine(PopUpQuest());
    }

    public void DisableCursorMouse()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    public void EnableCursorMouse()
    {
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }

    private IEnumerator PopUpQuest()
    {
        popUpQuest.SetActive(false);
        yield return new WaitForSeconds(0.5f);
        SoundManager.Instance.PlaySFX("SFX - Swoosh");
        Time.timeScale = 0.2f;
        popUpQuest.SetActive(true);
        popUpQuest.GetComponent<Animator>().Play("popupquest");
        popUpQuest.GetComponent<Animator>().speed = 1 * 5f;

        yield return new WaitForSeconds(3f / 5f);
        Time.timeScale = 1f;
        popUpQuest.SetActive(false);
    }
}
