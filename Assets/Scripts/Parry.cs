using System.Collections;
using UnityEngine;

public class Parry : MonoBehaviour
{
    [SerializeField] private float parryDuration = 1f;
    [SerializeField] private float parryCooldown = 5f;
    [SerializeField] private AudioSource parrySFX;
    //[SerializeField] private AudioSource blockSFX;
    [SerializeField] private GameObject parry;
    [SerializeField] private Animator parryAnimator;

    private bool canParry = true;
    public bool isParrying;

    void Update()
    {
        if(Input.GetMouseButtonDown(1))
        {
            if(canParry && !isParrying){
                StartCoroutine(ParryCooldown());
            }
        }
    }

    IEnumerator ParryCooldown()
    {
        canParry = false;
        isParrying = true;
        parry.SetActive(true);
        parrySFX.Play();
        parryAnimator.Play("parry");
        yield return new WaitForSeconds(parryDuration);
        parry.SetActive(false);
        isParrying = false;
        yield return new WaitForSeconds(parryCooldown); 
        canParry = true;
    }
}
