// NPC.cs
public class NPC
{
    public string Name { get; }
    public string Dialogue { get; }

    public NPC(string name, string dialogue)
    {
        Name = name;
        Dialogue = dialogue;
    }
}