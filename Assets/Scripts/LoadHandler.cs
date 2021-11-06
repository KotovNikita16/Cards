using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LoadHandler : MonoBehaviour
{
    public Dropdown dropdown;
    public Button button;
    private GameObject[] cards;

    void Start()
    {
        cards = GameObject.FindGameObjectsWithTag("Card");
    }


    public void Click()
    {
        int index = dropdown.value;
        switch (dropdown.options[index].text)
        {
            case "All At Once":
                StartCoroutine(allAtOnce());
                break;
            case "One By One":
                StartCoroutine(oneByOne());
                break;
            case "When Image Ready":
                whenImageReady();
                break;
            default:
                Debug.Log("Dropdown error!");
                break;
        }
    }

    IEnumerator allAtOnce()
    {
        dropdown.interactable = false;
        button.interactable = false;

        foreach (GameObject card in cards)
        {
            var cd = card.GetComponent<CardDisplay>();
            StartCoroutine(cd.turnOff());
        }

        foreach (GameObject card in cards)
        {
            var cd = card.GetComponent<CardDisplay>();
            yield return StartCoroutine(cd.GetTexture());
        }

        foreach (GameObject card in cards)
        {
            var cd = card.GetComponent<CardDisplay>();
            StartCoroutine(cd.turnOn());
        }

        dropdown.interactable = true;
        button.interactable = true;
    }

    IEnumerator oneByOne()
    {
        dropdown.interactable = false;
        button.interactable = false;

        foreach (GameObject card in cards)
        {
            var cd = card.GetComponent<CardDisplay>();
            StartCoroutine(cd.turnOff());
        }

        foreach (GameObject card in cards)
        {
            var cd = card.GetComponent<CardDisplay>();
            yield return StartCoroutine(cd.getCard());
        }
        dropdown.interactable = true;
        button.interactable = true;
    }

    void whenImageReady()
    {
        dropdown.interactable = false;
        button.interactable = false;

        foreach (GameObject card in cards)
        {
            var cd = card.GetComponent<CardDisplay>();
            StartCoroutine(cd.getCard());
        }
        
        dropdown.interactable = true;
        button.interactable = true;
    }
}
