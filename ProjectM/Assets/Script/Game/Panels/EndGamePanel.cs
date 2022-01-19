using UnityEngine.UI;
using UnityEngine;
using TMPro;

public class EndGamePanel : MonoBehaviour
{
    [Header("UI Config")]
    [SerializeField] private GameObject Battle;

    [Header("Mine")]
    [SerializeField] private TMP_Text nameMine;
    [SerializeField] private TMP_Text teamMine;
    [SerializeField] private Transform starsMine;
    [SerializeField] private Image teamColorMine;

    [Header("Opponent")]
    [SerializeField] private TMP_Text nameOpponent;
    [SerializeField] private TMP_Text teamOpponent;
    [SerializeField] private Transform starsOpponent;
    [SerializeField] private Image teamColorOpponent;

    [Header("Rewards")]
    [SerializeField] private TMP_Text coinsReward;
    [SerializeField] private TMP_Text xpReward;
    [SerializeField] private TMP_Text gemsReward;
    [SerializeField] private TMP_Text trophyReward;

    [Space(32)]
    [SerializeField] private TMP_Text txtWinnerOrLoser;
    [SerializeField] private Button btnOK;

    [Space(32)]
    [SerializeField] private Sprite sprBlue;
    [SerializeField] private Sprite[] sprStarBlue;
    [SerializeField] private Color colBlue;
    [SerializeField] private Sprite sprRed;
    [SerializeField] private Sprite[] sprStarRed;
    [SerializeField] private Color colRed;

    private void OnEnable()
    {
        Battle = GameObject.FindGameObjectWithTag("GameInterface");
        Battle.SetActive(false);

        btnOK.onClick.AddListener(() =>
        {
            BattleManager.instance.BackToLobby();
            btnOK.interactable = false;
        });
    }

    public void SetData(bool isWinner, string teamColor, 
        string name, string opponentName, 
        string team, string teamEnemy, 
        int myStars, int enemyStars,
        int coinsReward, int xpReward, int gemsReward, int trophyReward)
    {
        teamColorMine.sprite = teamColor == "red" ? sprRed : sprBlue;
        nameMine.text = name;
        teamMine.text = string.IsNullOrEmpty(team) ? "NO TEAM" : team;
        nameMine.color = teamColor == "red" ? colRed : colBlue;

        teamColorOpponent.sprite = teamColor == "red" ? sprBlue: sprRed;
        nameOpponent.text = opponentName;
        teamOpponent.text = string.IsNullOrEmpty(teamEnemy) ? "NO TEAM" : teamEnemy;
        nameOpponent.color = teamColor == "red" ? colBlue : colRed;

        for (int i = 0; i < 3; ++i)
        {
            starsMine.GetChild(i).gameObject.GetComponent<Image>().sprite = teamColor == "red" ? sprStarRed[i] : sprStarBlue[i];
            starsMine.GetChild(i).gameObject.SetActive(myStars - i > 0);

            starsOpponent.GetChild(i).gameObject.GetComponent<Image>().sprite = teamColor == "red" ? sprStarBlue[i] : sprStarRed[i];
            starsOpponent.GetChild(i).gameObject.SetActive(enemyStars - i > 0);
        }

        this.coinsReward.text = $"+{coinsReward}";
        this.xpReward.text = $"+{xpReward}";
        this.gemsReward.text = $"+{gemsReward}";
        this.trophyReward.text = $"+{trophyReward}";

        if (BattleManager.instance.isDraw)
            txtWinnerOrLoser.text = "DRAW!";
        else
            txtWinnerOrLoser.text = isWinner ? "WINNER!" : "LOSER!";
    }
}
