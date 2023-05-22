using System.Linq;
using System.Text;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class Leaderboard : MonoBehaviour
{
    [SerializeField] private GameObject leaderboardCanvas;
    [SerializeField] private TextMeshProUGUI lbLines;
    [SerializeField] private GameObject leftButton;
    [SerializeField] private GameObject rightButton;
    private string[] currentDisplay = new string[10];
    private string[] allDisplay;
    private int pageNum = 0;

    void Update()
    {
        if (!gameObject.activeSelf)
        {
            return;
        }

        var display = new StringBuilder();
        foreach (var c in currentDisplay)
        {
            display.Append(c + "\n");
        }

        if (string.IsNullOrWhiteSpace(display.ToString()) || string.IsNullOrWhiteSpace(display.ToString()))
        {
            display = new StringBuilder("Empty Leaderboard");
        }

        if (!lbLines.IsUnityNull())
        {
            lbLines.text = display.ToString();
        }

        rightButton.SetActive((allDisplay.Length-1) / 10 > pageNum);
        leftButton.SetActive(pageNum > 0);
    }

    public void Exit()
    {
        leaderboardCanvas.SetActive(false);
    }

    public void Open()
    {
        leaderboardCanvas.SetActive(true);
        allDisplay = LeaderboardData.RetrieveData();

        if (allDisplay.Length > 10)
        {
            currentDisplay = allDisplay.ToList().GetRange(0, 10).ToArray();
        }
        else
        {
            currentDisplay = allDisplay;
        }
    }

    public void Left()
    {
        pageNum -= 1;
        currentDisplay = allDisplay.ToList().GetRange(10 * pageNum, 10).ToArray();
    }

    public void Right()
    {
        pageNum += 1;
        currentDisplay = allDisplay.ToList().GetRange(10 * pageNum, (allDisplay.Length - 10*pageNum)<10?(allDisplay.Length - 10*pageNum):10).ToArray();
    }
}
