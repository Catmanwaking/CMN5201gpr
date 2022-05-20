//Author: Dominik Dohmeier
using System;

[Serializable]
public struct Records
{
    public bool recordSize4Exists;
    public TimeSpan recordSize4;
    public bool recordSize6Exists;
    public TimeSpan recordSize6;
    public int score;

    public Records(TimeSpan timeSize4, TimeSpan timeSize6, int score)
    {
        recordSize4 = timeSize4;
        recordSize4Exists = false;
        recordSize6 = timeSize6;
        recordSize6Exists = false;
        this.score = score;
    }
}