[System.Serializable]
public class ItemData
{
    private string name;
    private int currentStack;

    public string Name { get => name; set => name = value; }
    public int CurrentStack { get => currentStack; set => currentStack = value; }
}