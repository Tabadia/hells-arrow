using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class Detection : MonoBehaviour
{
    [SerializeField] private GameObject playerObject;
    // [SerializeField] private GameObject playerMesh;
    [SerializeField] private GameObject playerHeadObject;
    [SerializeField] private GameObject unDetectedObject;
    [SerializeField] private GameObject damageObject;
    [SerializeField] private Canvas canvas;
    [SerializeField] private Camera cameraMain;

    [SerializeField] private float secondsBeforeDamage = 15f;
    [SerializeField] private float damagePerTick = 0.15f;
    [SerializeField] public float damageInterval = 2.5f;
    [SerializeField] private float detectionDistance = 120f;
    [SerializeField] private AudioSource fightSFX;
    [SerializeField] private AudioSource ambienceSFX;

    private bool detected;
    private int callsSinceDetection;
    private bool damagePhase;
    private RectTransform canvasRect;
    private CanvasScaler canvasScaler;
    private RectTransform undetRect;
    private RectTransform damRect;
    private CapsuleCollider playerObjectCollider;
    private Hearts hearts;
    private MovementController movementController;

    void Start() 
    {
        canvasRect = canvas.GetComponent<RectTransform>();
        canvasScaler = canvas.GetComponent<CanvasScaler>();
        undetRect = unDetectedObject.GetComponent<RectTransform>();
        damRect = damageObject.GetComponent<RectTransform>();
        playerObjectCollider = playerObject.GetComponent<CapsuleCollider>();
        hearts = playerObject.GetComponent<Hearts>();
        transform.localScale = new Vector3(detectionDistance, detectionDistance, detectionDistance);
        movementController = playerObject.GetComponent<MovementController>();
    }

    void FixedUpdate()
    {
        // convert the World Position of the player head to UI position on screen
        var position = playerHeadObject.transform.position;
        var yDif = playerObjectCollider.height * .75f;
        var viewportPosition = cameraMain.WorldToViewportPoint(new Vector3(
            position.x,
            position.y + yDif,
            position.z));
        var sizeDelta = canvasRect.sizeDelta;

        var scaleFactor = canvasScaler.scaleFactor;

        var spritePosition = new Vector2(
            (viewportPosition.x * sizeDelta.x) * scaleFactor,
            (viewportPosition.y * sizeDelta.y) * scaleFactor);

        undetRect.anchoredPosition = spritePosition;
        damRect.anchoredPosition = spritePosition;

        // Figure out "player is detected" by casting a sphere around the player
        Collider[] overlappingColliders = new Collider[100];
        var hitNum = Physics.OverlapSphereNonAlloc(playerObject.transform.position, detectionDistance, overlappingColliders);
        var enemyInTrigger = false;
        for (var i = 0; i < hitNum; i++)
        {
            if (overlappingColliders[i].gameObject.CompareTag("Enemy"))
            {
                enemyInTrigger = true;
            }
        }
        detected = enemyInTrigger;

        var shrineInTrigger = false;
        for (var i = 0; i < hitNum; i++)
        {
            if (Vector3.Distance(overlappingColliders[i].gameObject.transform.position,
                    playerObject.transform.position) < detectionDistance * .65f && 
                (overlappingColliders[i].gameObject.CompareTag("Enemy") ||
                 (!overlappingColliders[i].transform.parent.IsUnityNull() && overlappingColliders[i].transform.parent.CompareTag("Shrine"))))
            {
                shrineInTrigger = true;
            }
        }
        detected = detected || shrineInTrigger;


        if (detected || !movementController.initMove) {
            callsSinceDetection = 0;
            damagePhase = false;
            unDetectedObject.SetActive(false);
            damageObject.SetActive(false);
            
            if (!fightSFX.isPlaying && !shrineInTrigger) {
                ambienceSFX.Stop();
                fightSFX.Play();
            }
            if(!fightSFX.isPlaying && shrineInTrigger && !ambienceSFX.isPlaying) {
                ambienceSFX.Play();
            }
        }
        else {
            if (!fightSFX.isPlaying && !ambienceSFX.isPlaying) {
                ambienceSFX.Play(); }
            callsSinceDetection += 1;
            /* FixedUpdate is called 50 times a second, thus 50f * timeInSeconds
               After a set time of not being near enemies, reset the timer and start counting for damage */
            if (callsSinceDetection > 50f * secondsBeforeDamage) {
                damagePhase = true;
                callsSinceDetection = 0;
            }

            // Enable no detection warning
            if (!damagePhase) {
                unDetectedObject.SetActive(true);
            }

            if (damagePhase && callsSinceDetection > 50f * damageInterval) {
                // 
                callsSinceDetection = 0;
                unDetectedObject.SetActive(false);
                damageObject.SetActive(true);

                hearts.TakeDamage(damagePerTick);
            }
        }
    }

    IEnumerator FadeOut (AudioSource audioSource, float FadeTime) {
        float startVolume = audioSource.volume;
        while (audioSource.volume > 0) {
            audioSource.volume -= startVolume * Time.deltaTime / FadeTime;
            yield return null;
        }
        audioSource.Stop();
        audioSource.volume = startVolume;
    }
}
