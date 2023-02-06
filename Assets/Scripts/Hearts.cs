using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
    // Start is called before the first frame update
    void Start()
    {
        currentHearts = maxHearts;
        hearts = new GameObject[maxHearts];
        for (int i = 0; i < maxHearts; i++){
            GameObject heart = Instantiate(heartPrefab, heartContainer.transform);
            heart.transform.position = new Vector3(heart.transform.position.x + (i * 20), heart.transform.position.y, heart.transform.position.z);
            hearts[i] = heart;
            print(i + " " + hearts[i]);
            print(maxHearts);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void takeDamage(float hearts){
        currentHearts -= hearts;

        if (currentHearts <= 0){
            print("You died");
        }
        else {
            // for (int i = 0; i < maxHearts; i++){
            //     if (i < currentHearts){
            //         hearts[i].GetComponent<SpriteRenderer>().sprite = fullHeart;
            //     }
            //     else if (i < currentHearts + 0.5f){
            //         hearts[i].GetComponent<SpriteRenderer>().sprite = halfHeart;
            //     }
            //     else {
            //         hearts[i].GetComponent<SpriteRenderer>().sprite = emptyHeart;
            //     }
            // }
        }
    }
}
