using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CardEffects : MonoBehaviour
{
    private List<CardDummy> cardsInAnimation = new List<CardDummy>();

    private void Awake()
    {
        DragNDrop.onCardUsed += UseAnimation;
    }

    private void UseAnimation(CardPlayer usedCard)
    {
        cardsInAnimation.Add(new CardDummy(usedCard, transform.parent));
    }

    private void Update()
    {
        foreach (var card in cardsInAnimation)
        {
            card.Update();

            if (card.animationEnds)
            {
                cardsInAnimation.Remove(card);
                card.Destroy();
                return;
            }
        }
    }

    public class CardDummy
    {
        public CardPlayer card;

        private Image image;

        private bool zoomOutComplete;
        private float zoomOutSpeed = 5;
        private Vector3 maxZoomOut;

        private float zoomInSpeed = 12;
        public bool animationEnds;

        public CardDummy(CardPlayer card, Transform content)
        {
            this.card = card;

            image = new GameObject("CardDummy", typeof(RectTransform)).AddComponent<Image>();
            image.transform.SetParent(content);

            image.GetComponent<RectTransform>().sizeDelta = new Vector2(90, 120);

            image.transform.position = Input.mousePosition;

            image.color = Color.white;
            image.sprite = Resources.Load<Sprite>("Cards/" + card.Carta.photo);

            maxZoomOut = image.transform.localScale * 4f;
            image.raycastTarget = false;
        }

        public void Destroy()
        {
            Object.Destroy(image.gameObject);
        }

        public void Update()
        {
            if (!zoomOutComplete)
            {
                image.transform.localScale += (Vector3.one * Time.deltaTime * zoomOutSpeed);

                if (image.transform.localScale.magnitude >= (maxZoomOut.magnitude))
                {
                    zoomOutComplete = true;
                }
            }
            else
            {
                image.transform.localScale -= (Vector3.one * Time.deltaTime * zoomInSpeed);

                var alpha = image.color;

                alpha.a -= Time.deltaTime * zoomInSpeed / 3;

                image.color = alpha;

                if (image.transform.localScale.x <= 0)
                {
                    animationEnds = true;
                }
            }

            if (animationEnds)
            {
                image.color = Color.clear;
            }
        }
    }
}
