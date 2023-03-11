using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class Hearts : MonoBehaviour
{
    [SerializeField] private int maxHearts = 3;
    [SerializeField] private GameObject heartContainer;
    [SerializeField] private GameObject heartPrefab;
    [SerializeField] private Sprite fullHeart;
    [SerializeField] private Sprite halfHeart;
    [SerializeField] private Sprite emptyHeart;

    private GameObject[] hearts;

    private float currentHearts;
    void Start()
    {
        currentHearts = maxHearts;
        print(currentHearts);
        hearts = new GameObject[maxHearts];
        for (int i = 0; i < maxHearts; i++){
            GameObject heart = Instantiate(heartPrefab, heartContainer.transform);
            heart.transform.position = new Vector3(heart.transform.position.x + (i * 46), heart.transform.position.y, heart.transform.position.z);
            hearts[i] = heart;
        }
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
    }

}
