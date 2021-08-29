[System.Serializable]
public class CropData
{
    private string name;
    private int level;
    private CropState state;

    public string Name { get => name; set => name = value; }
    public int Level { get => level; set => level = value; }
    public CropState State { get => state; set => state = value; }
}

public enum CropState
{
    PLANTED,
    HARVESTED,
    DRIED
}