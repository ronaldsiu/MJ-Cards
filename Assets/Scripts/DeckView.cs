using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeckView : MonoBehaviour
{
   Deck deck;
   public GameObject cardPrefab;

    public Vector3 start;
    public float cardOffSet;

    void Start()
    {
        deck = GetComponent<Deck>();
    }

    /*public void ShowCards()
    {
        int cardCount = 0;

        foreach(int i in deck.GetCards())
        {
            float co = cardOffSet * cardCount;

            GameObject cardCopy = (GameObject)Instantiate(cardPrefab);
            Vector3 temp = start + new Vector3(co, 0f);
            cardCopy.transform.position = temp;

            CardModel cardModel = cardCopy.GetComponent<CardModel>();
            SpriteRenderer spriteRenderer = cardCopy.GetComponent<SpriteRenderer>();
            spriteRenderer.sortingOrder = cardCount;
            cardModel.cardIndex = i;
            cardModel.showFace = true;
            cardModel.ToggleFace();

            cardCount++;
        }*/
    }
