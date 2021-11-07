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

    public void CancelClick()
    {
        cancelButton.interactable = false;
        Croupier.CancelActions();
    }

    public void LoadClick()
    {
        int index = dropdown.value;
        LockUI();
        Croupier.StartShuffle(dropdown.options[index].text, UnlockUI);
    }

    void LockUI()
    {
        dropdown.interactable = false;
        loadButton.interactable = false;
        cancelButton.interactable = true;
    }

    void UnlockUI()
    {
        dropdown.interactable = true;
        loadButton.interactable = true;
        cancelButton.interactable = false;
    }
}
