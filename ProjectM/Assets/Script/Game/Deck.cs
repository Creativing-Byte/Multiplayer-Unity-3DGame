using System.Collections.Generic;
public class Deck
{
    public string id;
    public List<string> Cartas;
    public Deck()
    {

    }
    public Deck(string id, List<string> Cartas)
    {
        this.id = id;
        this.Cartas = Cartas;
    }
}
