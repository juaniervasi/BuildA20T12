using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CoinsCounter : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _coinsText;
    [SerializeField] private float _coinsHudTimer = 0.2f;

    private float t = 0f;

    private void Start()
    {
        t = GameManager.instance.CurrentCoins;
        _coinsText.text = Mathf.Round(t).ToString();
    }

    public void ChangeCoins()
    {
        StartCoroutine(LerpTextCounterEffect());        
    }

    IEnumerator LerpTextCounterEffect()
    {
        while (t != GameManager.instance.CurrentCoins)
        {
            t = Mathf.MoveTowards(t, GameManager.instance.CurrentCoins, Time.deltaTime / _coinsHudTimer);
            _coinsText.text = Mathf.Round(t).ToString();

            yield return null;
        }
    }
}
