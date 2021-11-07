using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;

public static class Croupier
{
    private static readonly List<CardDisplay> cards = new List<CardDisplay>();
    private static Action _callback = null;
    private static string shuffleMode = null;
    private static int cardsTurnedOn = 0;
    private static int cardsTurnedOff = 0;
    private static int texturesLoaded = 0;

    public static void AddCard(CardDisplay card)
    {
        cards.Add(card);
    }

    public static void StartShuffle(string mode, Action callback)
    {
        shuffleMode = mode;
        _callback = callback;
        cardsTurnedOff = 0;
        if (shuffleMode == "One By One")
        {
            cards.Sort((x, y) => x.transform.position.x.CompareTo(y.transform.position.x));
        }
        TurnAllCardsOff();
    }

    public static void CardIsTurnedOff()
    {
        cardsTurnedOff++;
        if (cardsTurnedOff == cards.Count - 1)
        {
            texturesLoaded = 0;
            switch (shuffleMode)
            {
                case "All At Once":
                    foreach (CardDisplay cd in cards)
                        StartTextureLoad(cd);
                    break;
                case "One By One":
                    cardsTurnedOn = 0;
                    StartTextureLoad(cards[0]);
                    break;
                case "When Image Ready":
                    cardsTurnedOn = 0;
                    foreach (CardDisplay cd in cards)
                        StartTextureLoad(cd);
                    break;
                case "Cancel":
                    _callback?.Invoke();
                    break;
                default:
                    Debug.Log("Dropdown error!");
                    break;
            }
        }
    }

    public static void TextureLoaded(CardDisplay card)
    {
        texturesLoaded++;
        switch (shuffleMode)
        {
            case "All At Once":
                if (texturesLoaded == cards.Count - 1)
                {
                    cardsTurnedOn = 0;
                    foreach (CardDisplay cd in cards)
                        TurnCardOn(cd);
                }
                break;
            case "One By One":
                TurnCardOn(card);
                break;
            case "When Image Ready":
                TurnCardOn(card);
                break;
            default:
                Debug.Log("Dropdown error!");
                break;
        }
    }

    public static void CardIsTurnedOn()
    {
        cardsTurnedOn++;
        switch (shuffleMode)
        {
            case "All At Once":
                if (cardsTurnedOn == cards.Count - 1)
                    _callback?.Invoke();
                break;
            case "One By One":
                if (cardsTurnedOn == cards.Count)
                    _callback?.Invoke();
                else
                    StartTextureLoad(cards[cardsTurnedOn]);
                break;
            case "When Image Ready":
                if (cardsTurnedOn == cards.Count - 1)
                    _callback?.Invoke();
                break;
            default:
                Debug.Log("Dropdown error!");
                break;
        }
    }

    public static void CancelActions()
    {
        foreach (CardDisplay cd in cards)
        {
            cd.StopAllCoroutines();
        }
        shuffleMode = "Cancel";
        cardsTurnedOn = 0;
        cardsTurnedOff = 0;
        texturesLoaded = 0;
        TurnAllCardsOff();
    }

    public static void StartTextureLoad(CardDisplay card)
    {
        card.StartCoroutine("SetTexture");
    }

    public static void TurnCardOn(CardDisplay card)
    {
        card.StartCoroutine("TurnOn");
    }

    public static void TurnAllCardsOff()
    {
        foreach (CardDisplay cd in cards)
        {
            cd.StartCoroutine("TurnOff");
        }
    }

    public static void RemoveCard(CardDisplay cd)
    {
        cards.Remove(cd);
    }
}
