using System;
using System.IO;
using System.Linq;
using System.Text;
using UnityEngine;

public class LeaderboardData : MonoBehaviour
{
    private static string _dataDestination;

    void Start()
    {
        _dataDestination = Application.dataPath + "/Leaderboard/Leaderboard.txt";
    }


    public static void SaveNewData(string score, string playerName)
    {
        _dataDestination = Application.dataPath + "/Leaderboard/Leaderboard.txt";
        var data = FormatData(score, playerName);
        
        if (File.Exists(_dataDestination))
        {
            var fileStream = File.Open(_dataDestination, FileMode.Append);
            fileStream.Write(new UTF8Encoding(true).GetBytes(data), 0, data.Length);
        }
        else
        {
            var fileStream = File.Create(_dataDestination);
            fileStream.Write(new UTF8Encoding(true).GetBytes(data), 0, data.Length);
        }
    }

    public static string[] RetrieveData()
    {
        _dataDestination = Application.dataPath + "/Leaderboard/Leaderboard.txt";
        if (!File.Exists(_dataDestination))
        {
            return new[] { "Empty Leaderboard" };
        }

        // Dictionary<int, int> scoreMap = new Dictionary<int, int>();
        var lines = File.ReadAllLines(_dataDestination);
        for (var i = 0; i < lines.Length; i++)
        {
            var line = lines[i];
            var score = int.Parse(line.Split(" - ")[1]);
            for (var j = i - 1; j >= 0; j--)
            {
                var nextLine = lines[j];
                var nextScore = int.Parse(nextLine.Split(" - ")[1]);
                if (score >= nextScore)
                {
                    lines[j] = line;
                    lines[j + 1] = nextLine;
                }
            }
        }

        return lines;
    }

    private static string FormatData(string score, string playerName)
    {
        // var nScore = int.Parse(score);
        var strBuilder = new StringBuilder();
        var now = DateTime.Now;
        

        var nName = new StringBuilder();
        switch (playerName.Length)
        {
            case > 8:
                nName.Append(playerName.ToCharArray().ToList().GetRange(0, 7).ToArray());
                break;
            case < 8:
                nName.Append(playerName + new String(' ', 8-playerName.Length));
                break;
            default:
                nName.Append(playerName);
                break;
        }
        
        strBuilder.Append(nName + " - ");
        strBuilder.Append(score + " - ");
        strBuilder.Append(now.ToString("HH:mm") + "\r\n");
        return strBuilder.ToString();
    }
}
