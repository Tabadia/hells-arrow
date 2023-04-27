using System;
using System.Collections;
using System.Collections.Generic;
using Microsoft.Unity.VisualStudio.Editor;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class Detection : MonoBehaviour
{
    [SerializeField] private GameObject playerObject;
    [SerializeField] private GameObject playerMesh;
    [SerializeField] private GameObject playerHeadObject;
    [SerializeField] private GameObject unDetectedObject;
    [SerializeField] private GameObject damageObject;
    [SerializeField] private Canvas canvas;
    [SerializeField] private Camera cameraMain;

    [SerializeField] private float secondsBeforeDamage = 15f;
    [SerializeField] private float damagePerTick = 0.15f;
    [SerializeField] public float damageInterval = 2.5f;
    [SerializeField] private float detectionDistance = 120f;

    private bool detected;
    private int callsSinceDetection;
    private bool damagePhase;
    private RectTransform canvasRect;
    private CanvasScaler canvasScaler;
    private RectTransform undetRect;
    private RectTransform damRect;
    private CapsuleCollider playerObjectCollider;
    private Hearts hearts;

    void Start()
    {
        canvasRect = canvas.GetComponent<RectTransform>();
        canvasScaler = canvas.GetComponent<CanvasScaler>();
        undetRect = unDetectedObject.GetComponent<RectTransform>();
        damRect = damageObject.GetComponent<RectTransform>();
        playerObjectCollider = playerObject.GetComponent<CapsuleCollider>();
        hearts = playerObject.GetComponent<Hearts>();
        transform.localScale = new Vector3(detectionDistance, detectionDistance, detectionDistance);
    }
    
    void FixedUpdate()
    {
        var position = playerHeadObject.transform.position;
        var yDif = playerObjectCollider.height * .75f;
        var viewportPosition = cameraMain.WorldToViewportPoint(new Vector3(
            position.x, 
            position.y + yDif, 
            position.z));
        var sizeDelta = canvasRect.sizeDelta;

        var scaleFactor = canvasScaler.scaleFactor;
        
        var spritePosition = new Vector2(
            (viewportPosition.x * sizeDelta.x)*scaleFactor,
            (viewportPosition.y * sizeDelta.y)*scaleFactor);

        undetRect.anchoredPosition = spritePosition;
        damRect.anchoredPosition = spritePosition;

        Collider[] overlappingColliders = Physics.OverlapSphere(playerObject.transform.position, detectionDistance);
        // Debug.Log(sc.radius);
        bool enemyInTrigger = false;
        foreach (var overlappingCollider in overlappingColliders)
        {
            // Debug.Log(overlappingCollider.gameObject.name);
            if (overlappingCollider.gameObject.CompareTag("Enemy"))
            {
                enemyInTrigger = true;
            }
        }
        detected = enemyInTrigger;

        
        if (detected)
        {
            callsSinceDetection = 0;
            damagePhase = false;
            unDetectedObject.SetActive(false);
            damageObject.SetActive(false);
        }
        else
        {
            callsSinceDetection += 1;
            if (callsSinceDetection > 50f * secondsBeforeDamage)
            {
                damagePhase = true;
                callsSinceDetection = 0;
            }

            if (!damagePhase)
            {
                unDetectedObject.SetActive(true);
            }

            if (damagePhase && callsSinceDetection > 50f * damageInterval)
            {
                callsSinceDetection = 0;
                unDetectedObject.SetActive(false);
                damageObject.SetActive(true);
                
                hearts.takeDamage(damagePerTick);
            }
        }
    }
    
    // private void OnTriggerStay(Collider other)
    // {
    //     if (other.gameObject.CompareTag("Enemy"))
    //     {
    //         detected = true;
    //     }
    // }
    //
    // private void OnTriggerExit(Collider other)
    // {
    //     if (other.gameObject.CompareTag("Enemy"))
    //     {
    //         detected = false;
    //     }
    // }
    
}
