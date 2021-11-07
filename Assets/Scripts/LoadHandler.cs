using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LoadHandler : MonoBehaviour
{
    [SerializeField]
    private Dropdown dropdown;
    [SerializeField]
    private Button loadButton;
    [SerializeField]
    private Button cancelButton;
    [SerializeField]
    private GameObject[] cards;

    void Start()
    {

    }

    public void cancelClick()
    {
        StopAllCoroutines();
        CoroutineExtension.routinesStopped();
        foreach (GameObject card in cards)
        {
            var cd = card.GetComponent<CardDisplay>();
            cd.StopAllCoroutines();
        }
        StartCoroutine(cancelActions());
    }

    public void loadClick()
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
                StartCoroutine(whenImageReady());
                break;
            default:
                Debug.Log("Dropdown error!");
                break;
        }
    }

    IEnumerator allAtOnce()
    {
        lockUI();

        foreach (GameObject card in cards)
        {
            var cd = card.GetComponent<CardDisplay>();
            cd.turnOff().parallelCoroutinesGroup(this, "turnOff");
        }

        while (CoroutineExtension.GroupProcessing("turnOff"))
            yield return null;

        foreach (GameObject card in cards)
        {
            var cd = card.GetComponent<CardDisplay>();
            cd.setTexture().parallelCoroutinesGroup(this, "setTexture");
        }

        while (CoroutineExtension.GroupProcessing("setTexture"))
            yield return null;

        foreach (GameObject card in cards)
        {
            var cd = card.GetComponent<CardDisplay>();
            cd.turnOn().parallelCoroutinesGroup(this, "turnOn");
        }

        while (CoroutineExtension.GroupProcessing("turnOn"))
            yield return null;

        unlockUI();
    }

    IEnumerator oneByOne()
    {
        lockUI();

        foreach (GameObject card in cards)
        {
            var cd = card.GetComponent<CardDisplay>();
            cd.turnOff().parallelCoroutinesGroup(this, "turnOff");
        }

        while (CoroutineExtension.GroupProcessing("turnOff"))
            yield return null;

        foreach (GameObject card in cards)
        {
            var cd = card.GetComponent<CardDisplay>();
            yield return StartCoroutine(cd.setTexture());
            yield return StartCoroutine(cd.turnOn());
        }

        unlockUI();
    }

    IEnumerator whenImageReady()
    {
        lockUI();

        foreach (GameObject card in cards)
        {
            var cd = card.GetComponent<CardDisplay>();
            cd.getCard().parallelCoroutinesGroup(this, "imageReady");
        }

        while (CoroutineExtension.GroupProcessing("imageReady"))
            yield return null;

        unlockUI();
    }

    IEnumerator cancelActions()
    {
        cancelButton.interactable = false;
        foreach (GameObject card in cards)
        {
            var cd = card.GetComponent<CardDisplay>();
            cd.turnOff().parallelCoroutinesGroup(this, "cancel");
        }

        while (CoroutineExtension.GroupProcessing("cancel"))
            yield return null;

        dropdown.interactable = true;
        loadButton.interactable = true;
    }

    void lockUI()
    {
        dropdown.interactable = false;
        loadButton.interactable = false;
        cancelButton.interactable = true;
    }

    void unlockUI()
    {
        dropdown.interactable = true;
        loadButton.interactable = true;
        cancelButton.interactable = false;
    }
}
