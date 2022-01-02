using System;

public class BattleResult
{
    public class BattleResultInfo
    {
        public string Date;
        public int Destreza;
        public int Win;
        public BattleResultInfo()
        {
        }
        public BattleResultInfo(string Date, int Destreza, int Win)
        {
            this.Date = Date;
            this.Destreza = Destreza;
            this.Win = Win;
        }
    }
    public class BattleResultDateUser
    {
        public string Destreza;
        public string ID;
        public string Team;
        public BattleResultDateUser()
        {
        }
        public BattleResultDateUser(string Destreza, string ID, string Team)
        {
            this.Destreza = Destreza;
            this.ID = ID;
            this.Team = Team;
        }
    }
}

