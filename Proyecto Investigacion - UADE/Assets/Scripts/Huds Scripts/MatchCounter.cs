using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class MatchCounter : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _matchesLeftText;

    private void Start()
    {
        SetText();
    }

    public void OnEndMatchPressed()
    {
        GameManager.instance.IncreaseCurrentPlayed();
        SetText();
    }

    private void SetText()
    {
        string amountOfPlays = GameManager.instance.AmountOfPlays.ToString();
        string currentPlayed = GameManager.instance.CurrentPlayed.ToString();
        _matchesLeftText.text = currentPlayed + "/" + amountOfPlays;
    }
}
