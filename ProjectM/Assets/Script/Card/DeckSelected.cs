using System;
using Firebase.Database;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using System.Threading.Tasks;

public class DeckSelected : MonoBehaviour
{
    [SerializeField] private Transform _unusedCardsParent;
    [SerializeField] private GameObject[] _deckParents;
    [SerializeField] private Button[] _deckButtons;
    [SerializeField] private GameObject _deckContent;

    //Santiago
    private Dictionary<string, List<Card>> decks = new Dictionary<string, List<Card>>();
    private int indexSelectedDeckCard = -1;
    private int indexSelectedUnusedCard = -1;
    private List<Vector3> originalCardsPos = new List<Vector3>();
    private List<Vector3> originalUnusedCardsPos = new List<Vector3>();
    private bool swapingCards = false;
    //

    private GameObject _launcher;
    private UserDbInit _userData;
    private CardDbInit _cardData;
    private List<Card> _allCards = new List<Card>();
    private List<Card> _deck1 = new List<Card>();
    private List<Card> _deck2 = new List<Card>();
    private List<Card> _deck3 = new List<Card>();
    private List<Card> _unusedCards = new List<Card>();
    private List<string> _deck1Paths = new List<string>();
    private List<string> _deck2Paths = new List<string>();
    private List<string> _deck3Paths = new List<string>();
    private string _currentDeckId;
    private string _deck1ID;
    private string _deck2ID;
    private string _deck3ID;
    private int _selectedCardIndex = -1;

    private void Awake()
    {
        for (int i = 0; i < 8; i++)
        {
            _deckContent.transform.GetChild(i).GetComponent<Image>().material =
                new Material(_deckContent.transform.GetChild(i).GetComponent<Image>().material);
        }

        foreach (Transform child in _deckContent.transform)
        {
            originalCardsPos.Add(child.transform.localPosition);
        }
    }

    private void Start()
    {
        _launcher = GameObject.FindGameObjectWithTag("Launcher");

        if (_launcher != null)
        {
            _userData = _launcher.GetComponent<UserDbInit>();
            _cardData = _launcher.GetComponent<CardDbInit>();

            _currentDeckId = _userData.DatosUser.Child("Date").Child("deckid").Value.ToString();

            _deck1ID = _userData.DatosUser.Child("deck1").Child("id").Value.ToString();
            _deck2ID = _userData.DatosUser.Child("deck2").Child("id").Value.ToString();
            _deck3ID = _userData.DatosUser.Child("deck3").Child("id").Value.ToString();

            _allCards = _cardData.LoadAllCard();

            InitDecks();

            LoadDeck(_currentDeckId);
            
            UpdateShaderCard();

            for (int i = 0; i < decks.Count; i++)
            {
                if (decks.Keys.ToArray()[i] == _currentDeckId)
                {
                    ChangeIdSelected(i);
                    break;
                }
            }
        }
        else Debug.LogError("Launcher not found!");
    }

    private void InitDecks()
    {
        _deck1.Clear();
        _deck2.Clear();
        _deck3.Clear();

        decks.Clear();

        foreach (var card in _userData.DatosUser.Child("deck1").Child("Cartas").Children)
        {
            _deck1.Add(_cardData.LoadCard(card.Value.ToString()));
        }

        decks.Add(_deck1ID, _deck1);

        foreach (var card in _userData.DatosUser.Child("deck2").Child("Cartas").Children)
        {
            _deck2.Add(_cardData.LoadCard(card.Value.ToString()));
        }

        decks.Add(_deck2ID, _deck2);

        foreach (var card in _userData.DatosUser.Child("deck3").Child("Cartas").Children)
        {
            _deck3.Add(_cardData.LoadCard(card.Value.ToString()));
        }

        decks.Add(_deck3ID, _deck3);
    }

    private void Update()
    {
        if (!swapingCards)
        {
            for (int i = 0; i < _deckContent.transform.childCount; i++)
            {
                if (i == indexSelectedDeckCard)
                {
                    var targetPos = originalCardsPos[i];
                    targetPos.y += 20;

                    _deckContent.transform.GetChild(i).localPosition = Vector3.Lerp(_deckContent.transform.GetChild(i).localPosition, targetPos, Time.deltaTime * 3.5f);
                }
                else
                {
                    _deckContent.transform.GetChild(i).localPosition = Vector3.Lerp(_deckContent.transform.GetChild(i).localPosition, originalCardsPos[i], Time.deltaTime * 3.5f);
                }
            }

            //Card selection in unused cards
            for (int i = 0; i < _unusedCardsParent.childCount; i++)
            {
                if (i == indexSelectedUnusedCard)
                {
                    var targetPos = originalUnusedCardsPos[i];
                    targetPos.y += 20;

                    _unusedCardsParent.GetChild(i).localPosition = Vector3.Lerp(_unusedCardsParent.GetChild(i).localPosition, targetPos, Time.deltaTime * 3.5f);
                }
                else
                {
                    _unusedCardsParent.GetChild(i).localPosition = Vector3.Lerp(_unusedCardsParent.GetChild(i).localPosition, originalUnusedCardsPos[i], Time.deltaTime * 3.5f);
                }
            }
        }

        //Swap cards
        if (indexSelectedDeckCard != -1 && indexSelectedUnusedCard != -1 && !swapingCards)
        {
            SwapCard(_currentDeckId, _deckContent.transform.GetChild(indexSelectedDeckCard).GetComponent<CardUI>(), _unusedCardsParent.GetChild(indexSelectedUnusedCard).GetComponent<CardUI>());
        }
    }

    private async void SwapCard(string deckId, CardUI oldCard, CardUI newCard)
    {
        swapingCards = true;

        Vector2 oldCardPos = oldCard.transform.position;
        oldCardPos.y -= 20;
        Vector2 newCardPos = newCard.transform.position;

        Vector3 oldCardScale = oldCard.transform.localScale;
        Vector3 newCardScale = newCard.transform.localScale;

        var auxCanvas = newCard.gameObject.AddComponent<Canvas>();
        auxCanvas.overrideSorting = true;
        auxCanvas.sortingOrder = 10;

        float distance = Vector2.Distance(oldCardPos, newCardPos);

        while (distance > 0.1f)
        {
            oldCard.transform.position = Vector2.Lerp(oldCard.transform.position, newCardPos, Time.deltaTime * 4.75f);
            newCard.transform.position = Vector2.Lerp(newCard.transform.position, oldCardPos, Time.deltaTime * 4.75f);

            oldCard.transform.localScale = Vector2.Lerp(oldCard.transform.localScale, newCardScale, Time.deltaTime * 4.75f);
            newCard.transform.localScale = Vector2.Lerp(newCard.transform.localScale, oldCardScale, Time.deltaTime * 4.75f);

            distance = Vector2.Distance(newCard.transform.position, oldCardPos);

            await Task.Yield();
        }

        oldCard.transform.position = oldCardPos;
        oldCard.transform.localScale = oldCardScale;

        newCard.transform.position = newCardPos;
        newCard.transform.localScale = newCardScale;

        Destroy(auxCanvas);

        var auxOldCard = oldCard.card;
        var auxNewCard = newCard.card;

        decks[deckId][indexSelectedDeckCard] = newCard.card;

        oldCard.Load(newCard.card);
        newCard.Load(auxOldCard);
        UpdateShaderCard();

        await FirebaseDatabase.DefaultInstance.RootReference
                    .Child("users")
                    .Child(_userData.DatosUser.Key)
                    .Child(GetFirebaseDeckName(_currentDeckId))
                    .Child("Cartas")
                    .Child(indexSelectedDeckCard.ToString()).SetValueAsync(auxNewCard.id.ToString());

        swapingCards = false;
        indexSelectedDeckCard = -1;
        indexSelectedUnusedCard = -1;
    }

    private string GetFirebaseDeckName(string deckId)
    {
        var result = "deck";

        for (int i = 0; i < decks.Count; i++)
        {
            if (deckId == decks.Keys.ToArray()[i])
            {
                result += (i + 1).ToString();
                break;
            }
        }

        return result;
    }

    private void UpdateShaderCard()
    {
        for (int i = 0; i < 8; i++)
        {
            _deckContent.transform.GetChild(i).GetComponent<Image>().material.SetTexture("MainTexture", _deckContent.transform.GetChild(i).GetComponent<CardUI>().image.sprite.texture);
        }
    }

    private void LoadDeck(string deckId)
    {
        indexSelectedDeckCard = -1;
        indexSelectedUnusedCard = -1;

        for (int i = 0; i < decks[deckId].Count; i++)
        {
            int j = i;

            _deckContent.transform.GetChild(i).GetComponent<CardUI>().Load(decks[deckId][i], Resources.Load<Sprite>($"Cards/{decks[deckId][i].photo}"), () => SelectCard(j));
        }
    }

    private void SelectCard(int index)
    {
        indexSelectedDeckCard = index;
    }

    private void SelectUnusedCard(int index)
    {
        indexSelectedUnusedCard = index;
    }

    public void ChangeIdSelected(int id)
    {
        for (int i = 0; i < _deckButtons.Length; i++)
        {
            _deckButtons[i].interactable = true;
        }

        _deckButtons[id].interactable = false;

        ChangeDeckSelected(id);
        ReloadAvailableCards(id);

        LoadDeck(decks.Keys.ToArray()[id]);

        UpdateShaderCard();

        _currentDeckId = _userData.DatosUser.Child("Date").Child("deckid").Value.ToString();
    }

    private void ChangeDeckSelected(int id)
    {
        print("Saving new deck to Firebase");
        switch (id)
        {
            case 0:
                FirebaseDatabase.DefaultInstance.GetReference("users").Child(_launcher.GetComponent<UserDbInit>().DatosUser.Key).Child("Date").Child("deckid").SetValueAsync(_deck1ID);
                break;
            case 1:
                FirebaseDatabase.DefaultInstance.GetReference("users").Child(_launcher.GetComponent<UserDbInit>().DatosUser.Key).Child("Date").Child("deckid").SetValueAsync(_deck2ID);
                break;
            case 2:
                FirebaseDatabase.DefaultInstance.GetReference("users").Child(_launcher.GetComponent<UserDbInit>().DatosUser.Key).Child("Date").Child("deckid").SetValueAsync(_deck3ID);
                break;
        }

        ReloadData();
    }

    private void ReloadData() => _launcher.GetComponent<UserDbInit>().reloadDate();

    private void ReloadAvailableCards(int id)
    {
        print("Reloading available cards");

        for (int i = 0; i < _unusedCardsParent.childCount; i++)
            Destroy(_unusedCardsParent.GetChild(i).gameObject);

        Canvas.ForceUpdateCanvases();

        _unusedCards = new List<Card>();

        for (int i = 0; i < _allCards.Count; i++)
        {
            var currentDeck = decks.Values.ToArray()[id];

            var cardInDeck = false;

            for (int j = 0; j < currentDeck.Count; j++)
            {
                if (_allCards[i].id == currentDeck[j].id)
                {
                    cardInDeck = true;
                    break;
                }
            }

            if (!cardInDeck)
            {
                int j = _unusedCards.Count;
                _unusedCards.Add(_allCards[i]);

                GameObject card = new GameObject("Unused Card: " + _allCards[i].id);
                card.transform.SetParent(_unusedCardsParent);
                card.AddComponent<Image>();
                card.AddComponent<Button>();
                card.AddComponent<CardUI>();
                card.GetComponent<CardUI>().Load(_allCards[i], Resources.Load<Sprite>($"Cards/{_allCards[i].photo}"), () => SelectUnusedCard(j));

                card.transform.localScale = Vector3.one * 0.8f;
            }
        }

        Canvas.ForceUpdateCanvases();

        originalUnusedCardsPos.Clear();

        foreach (Transform unusedCard in _unusedCardsParent)
        {
            originalUnusedCardsPos.Add(unusedCard.localPosition);
        }

        originalUnusedCardsPos.Add(_unusedCardsParent.GetChild(0).localPosition);
    }
}

