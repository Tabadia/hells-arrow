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
        _dataDestination = Application.persistentDataPath + "/Leaderboard.txt";
        ClearData();
        if (!File.Exists(_dataDestination))
        {
            File.Create(_dataDestination).Dispose();
        }
    }

    private static void ClearData()
    {
        _dataDestination = Application.persistentDataPath + "/Leaderboard.txt";
        File.Delete(_dataDestination);
        File.Create(_dataDestination).Dispose();
    }


    public static void SaveNewData(string score, string playerName)
    {
        _dataDestination = Application.persistentDataPath + "/Leaderboard.txt";
        var data = FormatData(score, playerName);
        
        if (File.Exists(_dataDestination))
        {
            var fileStream = File.Open(_dataDestination, FileMode.Append);
            
            var bytes = Encoding.UTF8.GetBytes(data);
            fileStream.Write(bytes, 0, bytes.Length);
            fileStream.Write(Encoding.UTF8.GetBytes(Environment.NewLine));
            fileStream.Dispose();
        }
        else
        {
            var fileStream = File.Create(_dataDestination);
            
            var bytes = Encoding.UTF8.GetBytes(data);
            fileStream.Write(bytes, 0, bytes.Length);
            fileStream.Write(Encoding.UTF8.GetBytes(Environment.NewLine));
            fileStream.Dispose();
        }
        
    }

    public static string[] RetrieveData()
    {
        _dataDestination = Application.persistentDataPath + "/Leaderboard.txt";
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

        if (lines[^1].Equals("Empty Leaderboard") || string.IsNullOrEmpty(lines[^1]))
        {
            lines = lines.ToList().GetRange(0, lines.Length - 1).ToArray();
        }

        return lines;
    }

    private static string FormatData(string score, string playerName)
    {
        var strBuilder = new StringBuilder();
        var now = DateTime.Now;

        var nName = new StringBuilder();
        switch (playerName.Length)
        {
            case 8:
                nName.Append(playerName);
                break;
            case > 8:
                nName.Append(playerName.ToCharArray().ToList().GetRange(0, 8).ToArray());
                break;
            case < 8:
                nName.Append(playerName);
                nName.Append(new string(' ', 8-playerName.Length));
                break;
        }
        
        strBuilder.Append(nName + " - ");
        strBuilder.Append(score + " - ");
        strBuilder.Append(now.ToString("HH:mm"));
        return strBuilder.ToString();
    }
}
