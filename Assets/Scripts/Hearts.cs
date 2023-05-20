using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
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
    [SerializeField] private TextMeshProUGUI scoreVar;
    [SerializeField] private TextMeshProUGUI displayScore;
    [SerializeField] private TextMeshProUGUI nameInput;
    [SerializeField] private PauseManager pauseManager;
    
    private GameObject[] hearts;
    private float currentHearts;
    private AudioSource[] allAudioSources;
    private Vector3 startPos;
    public bool isDead;
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
            // for (int i = 0; i < maxHearts; i++) {
            //     hearts[i].GetComponent<Image>().sprite = fullHeart;
            // }
            // currentHearts = maxHearts;
            // gameObject.transform.position = startPos;

            LeaderboardData.SaveNewData(scoreVar.text, string.IsNullOrEmpty(nameInput.text)?"OOO":nameInput.text);
            displayScore.text = "00";
            nameInput.text = "";
            pauseManager.playerScore = 0f;
            
            SceneManager.LoadScene("Ice Map");
        });
        //takeDamage(1.5f);
    }

    void Update() {
        scoreVar.text = pauseManager.playerScore.ToString("0000");
        if (!displayScore.text.Equals(scoreVar.text))
        {
            displayScore.text = scoreVar.text;
        }

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

    public void TakeDamage(float dmg)
    {
        hurtSFX.Play();
        if (currentHearts <= 0) currentHearts = 0;
        currentHearts -= dmg;

        if (currentHearts <= 0) {
            for (int i = 0; i < maxHearts; i++)
            {
                hearts[i].GetComponent<Image>().sprite = emptyHeart;
            }
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
        StartCoroutine(HitEffect());
    }

    IEnumerator HitEffect()
    {
        effectRenderer.color = new Color(effectRenderer.color.r, effectRenderer.color.g, effectRenderer.color.b, effectOpacity);
        yield return new WaitForSeconds(0.2f);
        StartCoroutine(FadeOut(effectRenderer, effectFadeDuration));
    }

    IEnumerator FadeOut(Image effect, float duration) {
        float counter = 0;
        Color spriteColor = effect.color;

        while (counter < duration)
        {
            counter += Time.deltaTime;
            float alpha = Mathf.Lerp(effectOpacity, 0, counter / duration);

            effect.color = new Color(spriteColor.r, spriteColor.g, spriteColor.b, alpha);
            yield return null;
        }
    }

}
