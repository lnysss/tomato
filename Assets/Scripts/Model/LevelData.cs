using System.Collections.Generic;

public class LevelData
{
    public int id;
    public int waveTimer;
    public List<WaveData> enemys;
}

public class WaveData
{
    public string enemyName;
    public int timeAxis;
    public int count;
    public int elite;
}

