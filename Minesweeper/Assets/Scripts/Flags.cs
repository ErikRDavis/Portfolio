

using System.Collections.Generic;

public class Flags
{
    public int FlagCount => flags.Count;
    public int MaxFlagCount => maxFlagCount;

    private Dictionary<string, GridSpaceType> flags;
    private int maxFlagCount;

    public Flags(int maxFlagCount) 
    {
        this.maxFlagCount = maxFlagCount;
        flags = new Dictionary<string, GridSpaceType>();
    }

    public bool CanPlantFlag()
    {
        return flags.Count < maxFlagCount;
    }

    public void PlantFlag(string spaceKey, GridSpaceType spaceType)
    {
        flags.Add(spaceKey, spaceType);
    }

    public void RemoveFlag(string spaceKey)
    {
        flags.Remove(spaceKey);
    }

    public bool HasFlag(string spaceKey)
    {
        return flags.ContainsKey(spaceKey);
    }

    public void Reset()
    {
        flags.Clear();
    }
}