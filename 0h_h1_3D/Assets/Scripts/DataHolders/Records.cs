//Author: Dominik Dohmeier
using System;

[Serializable]
public struct Records
{
    public bool recordSize4Exists;
    public double recordSize4;
    public bool recordSize6Exists;
    public double recordSize6;
    public int score;
    public int oldScore;

    public Records(TimeSpan timeSize4, TimeSpan timeSize6, int score)
    {
        recordSize4 = timeSize4.TotalSeconds;
        recordSize4Exists = false;
        recordSize6 = timeSize6.TotalSeconds;
        recordSize6Exists = false;
        this.score = score;
        oldScore = score;
    }
}