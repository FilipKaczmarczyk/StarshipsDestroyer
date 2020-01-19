using UnityEngine;

[CreateAssetMenu(fileName = "DungeonGenrationData.asset", menuName = "DungeonGenerationData/Dungeon Data")]
public class DungeonGenerationData : ScriptableObject
{
    public int numberOfCrawlers;

    public int intertionsMin;

    public int interationsMax;

}
