using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class Hearts : MonoBehaviour
{
    [SerializeField] private int maxHearts = 3;
    [SerializeField] private GameObject heartContainer;
    [SerializeField] private GameObject heartPrefab;
    [SerializeField] private GameObject damageEffect;
    [SerializeField] private Image effectRenderer;
    [SerializeField] private float effectFadeDuration = 0.2f;
    [SerializeField] private float effectOpacity = 0.54f;
    [SerializeField] private GameObject deathScreen;
    [SerializeField] private Button respawnBtn;
    [SerializeField] private Sprite fullHeart;
    [SerializeField] private Sprite halfHeart;
    [SerializeField] private Sprite emptyHeart;
    [SerializeField] private AudioSource hurtSFX;

    private GameObject[] hearts;
    private float currentHearts;
    private AudioSource[] allAudioSources;
    private Vector3 startPos;
    public bool isDead = false;
    private GameObject[] checkpoint;
    private CapsuleCollider playerCollider;

    void Start()
    {
        playerCollider = gameObject.GetComponent<CapsuleCollider>();
        currentHearts = maxHearts;
        hearts = new GameObject[maxHearts];
        for (int i = 0; i < maxHearts; i++){
            GameObject heart = heartContainer.transform.GetChild(i).gameObject;
            hearts[i] = heart;
        }
        startPos = gameObject.transform.position;

        respawnBtn.onClick.AddListener(() => {
            deathScreen.SetActive(false);
            isDead = false;
            Time.timeScale = 1;
            AudioListener.pause = false;
            for (int i = 0; i < maxHearts; i++) {
                hearts[i].GetComponent<Image>().sprite = fullHeart;
            }
            currentHearts = maxHearts;
            gameObject.transform.position = startPos;
        });
        //takeDamage(1.5f);
    }

    void Update() {
        //get all game objects with tag checkpoint
        checkpoint = GameObject.FindGameObjectsWithTag("checkpoint");
        //loop through objects, check if player is in bounds
        for (int i = 0; i < checkpoint.GetLength(0); i++) {
            Collider[] hitColliders = Physics.OverlapSphere(checkpoint[i].transform.position, 4.3f);
            foreach (var hitCollider in hitColliders) {
                if (hitCollider == playerCollider) {
                    startPos = transform.position;
                }
            }
        }
        //if player in bounds, set start position to player position
    }

    public void takeDamage(float dmg)
    {
        hurtSFX.Play();
        if (currentHearts <= 0) currentHearts = 0;
        currentHearts -= dmg;

        if (currentHearts <= 0) {
            for (int i = 0; i < maxHearts; i++)
            {
                hearts[i].GetComponent<Image>().sprite = emptyHeart;
            }
            print("You died");
            deathScreen.SetActive(true);
            AudioListener.pause = true;
            isDead = true;
            Time.timeScale = 0;
            GameObject[] doors = GameObject.FindGameObjectsWithTag("BossDoor");
            foreach (var door in doors)
            {
                door.GetComponent<BossDoor>().EnableMovement();
            }
        }
        else {
            for (int i = 0; i < maxHearts; i++) {
                if ((currentHearts - i) >= 1) {
                    hearts[i].GetComponent<Image>().sprite = fullHeart;
                }
                else if ((currentHearts - i) <= 0) {
                    hearts[i].GetComponent<Image>().sprite = emptyHeart;
                }
                else { hearts[i].GetComponent<Image>().sprite = halfHeart; }
            }
        }
        StartCoroutine(hitEffect());
    }

    IEnumerator hitEffect()
    {
        effectRenderer.color = new Color(effectRenderer.color.r, effectRenderer.color.g, effectRenderer.color.b, effectOpacity);
        yield return new WaitForSeconds(0.2f);
        StartCoroutine(fadeOut(effectRenderer, effectFadeDuration));
    }

    IEnumerator fadeOut(Image effectRenderer, float duration) {
        float counter = 0;
        Color spriteColor = effectRenderer.color;

        while (counter < duration)
        {
            counter += Time.deltaTime;
            float alpha = Mathf.Lerp(effectOpacity, 0, counter / duration);

            effectRenderer.color = new Color(spriteColor.r, spriteColor.g, spriteColor.b, alpha);
            yield return null;
        }
    }

}
