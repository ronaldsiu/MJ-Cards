using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DiscardPile : MonoBehaviour
{
    public static DiscardPile instance = null;

    public List<int> discardPile;
    public List<CardModel> discardCardModel;
    public CardModel cardModel;

    public GamestateManager gamestateManager;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }

        discardPile = new List<int>();
        discardCardModel = new List<CardModel>();
    }



}
