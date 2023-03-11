using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Parry : MonoBehaviour
{
    [SerializeField] private float parryDuration = 1f;
    [SerializeField] private float parryCooldown = 5f;

    private bool canParry = true;
    public bool isParrying = false;

    void Start()
    {
        
    }


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
        // START ANIMATION
        print("Parrying");
        yield return new WaitForSeconds(parryDuration);
        print("Not parrying");
        // END ANIMATION
        isParrying = false;
        yield return new WaitForSeconds(parryCooldown);
        print("Can parry again"); 
        canParry = true;
    }
}
