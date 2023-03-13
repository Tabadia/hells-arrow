using System.Collections;
using System.Collections.Generic;
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


    private GameObject[] hearts;
    private float currentHearts;

    public bool isDead = false;

    void Start()
    {
        currentHearts = maxHearts;
        hearts = new GameObject[maxHearts];
        for (int i = 0; i < maxHearts; i++){
            GameObject heart = Instantiate(heartPrefab, heartContainer.transform);
            heart.transform.position = new Vector3(heart.transform.position.x + (i * 46), heart.transform.position.y, heart.transform.position.z);
            hearts[i] = heart;
        }

        respawnBtn.onClick.AddListener(() => {
            deathScreen.SetActive(false);
            isDead = false;
            Time.timeScale = 1;
            for (int i = 0; i < maxHearts; i++)
            {
                hearts[i].GetComponent<Image>().sprite = fullHeart;
            }
            currentHearts = maxHearts;
        });
        //takeDamage(1.5f);
    }

    void Update()
    {
    }

    public void takeDamage(float dmg)
    {
        if (currentHearts <= 0) currentHearts = 0;
        currentHearts -= dmg;

        if (currentHearts <= 0) {
            for (int i = 0; i < maxHearts; i++)
            {
                hearts[i].GetComponent<Image>().sprite = emptyHeart;
            }
            print("You died");
            deathScreen.SetActive(true);
            isDead = true;
            Time.timeScale = 0;
        }
        else {
            for (int i = 0; i < maxHearts; i++) {
                print("i: " + i);
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
        //Get current color
        Color spriteColor = effectRenderer.color;

        while (counter < duration)
        {
            counter += Time.deltaTime;
            //Fade from 1 to 0
            float alpha = Mathf.Lerp(effectOpacity, 0, counter / duration);
            //Debug.Log(alpha);

            //Change alpha only
            effectRenderer.color = new Color(spriteColor.r, spriteColor.g, spriteColor.b, alpha);
            //Wait for a frame
            yield return null;
        }
    }

}
