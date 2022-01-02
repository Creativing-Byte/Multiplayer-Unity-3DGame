using System.Collections.Generic;
public class User
{
    public string username;
    public string email;
    public int destreza;
    public int coins;
    public int diamond;
    public string ultcon;
    public string scene;
    public string deckid;
    public int level;
    public int exp;
    public int dice;
    public int gift;
    public string giftUnlocked;
    public List<string> friends;
    public User()
    {
    }

    public User(string username, string email, int destreza, int coins, int diamond, string ultcon, string scene, string deckid, int level, int exp, int dice, int gift, string giftUnlocked, List<string> friends)
    {
        this.username = username;
        this.email = email;
        this.destreza = destreza;
        this.coins = coins;
        this.diamond = diamond;
        this.ultcon = ultcon;
        this.scene = scene;
        this.deckid = deckid;
        this.level = level;
        this.exp = exp;
        this.dice = dice;
        this.gift = gift;
        this.giftUnlocked = giftUnlocked;
        this.friends = friends;
    }
}

