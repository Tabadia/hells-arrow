using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Shrines : MonoBehaviour
{
    [SerializeField] private float maxDistance = 100f;
    [SerializeField] private GameObject shrineText;
    [SerializeField] private GameObject powerUpMenu;

    private GameObject[] shrines;

    public bool inMenu = false;

    // Start is called before the first frame update
    void Start()
    {
        shrines = GameObject.FindGameObjectsWithTag("Shrine");
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            print("test");
        }
        GameObject closest = null;
        float distance = Mathf.Infinity;

        Vector3 position = transform.position;
        foreach (GameObject s in shrines)
        {
            Vector3 diff = s.transform.position - position;
            float curDistance = diff.sqrMagnitude;
            if (curDistance < distance)
            {
                closest = s;
                distance = curDistance;
            }
        }

        // If close to shrine
        if (distance < maxDistance)
        {
            if (Input.GetKeyDown(KeyCode.Alpha1))
            {
                if (inMenu)
                {
                    inMenu = false;
                    powerUpMenu.SetActive(false);
                    print("Chose power-up 1");
                }
            }
            if (Input.GetKeyDown(KeyCode.Alpha2))
            {
                if (inMenu)
                {
                    inMenu = false;
                    powerUpMenu.SetActive(false);
                    print("Chose power-up 2");
                }
            }
            if (Input.GetKeyDown(KeyCode.Alpha3))
            {
                if (inMenu)
                {
                    inMenu = false;
                    powerUpMenu.SetActive(false);
                    print("Chose power-up 3");
                }
            }

            shrineText.SetActive(true);
            if (Input.GetKeyDown(KeyCode.E))
            {
                // show menu
                inMenu = true;
                powerUpMenu.SetActive(true);

            }
            //print("Closest shrine is " + closest.name + " at " + distance + " units away");
        }
        else shrineText.SetActive(false);
    }
}
