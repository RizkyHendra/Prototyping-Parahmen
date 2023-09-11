using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class RageEffect : MonoBehaviour
{
    public Animator anim;
    public VisualEffect rageUp;
    private bool ragelingUp;
    public GameObject particleSystemm;
 

    // Update is called once per frame
    void Update()
    {
        
    }
    public void RageEffectt()
    {
        if (anim != null)
        {
           if( !ragelingUp)
            {
                particleSystemm.SetActive(true);
                anim.SetTrigger("Skill1");
                if (rageUp != null)
                {
                    rageUp.Play();
                }

                ragelingUp = true;
                StartCoroutine(ResetBool(ragelingUp, 4f));
            }


        }
    }
    IEnumerator ResetBool (bool boolToReset, float delay = 10)
    {
        
        yield return new WaitForSeconds(delay);
        ragelingUp = !ragelingUp;
        particleSystemm.SetActive(false);
        
    }
}
