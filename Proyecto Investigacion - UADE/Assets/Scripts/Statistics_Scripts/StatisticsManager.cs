using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatisticsManager : MonoBehaviour
{
    [SerializeField] private GameObject _playAgainButtonRef;
    [SerializeField] private GameObject _endGameButtonRef;
    [SerializeField] private float _delayToShowFinishButton = 2.3f;

    private void Start()
    {
        GameManager.instance.OnFinishPlays += OnFinishPlaysHandler;
    }

    private void OnFinishPlaysHandler()
    {
        _playAgainButtonRef.SetActive(false);
        _endGameButtonRef.SetActive(true);
    }

    IEnumerator DelayToUnhide()
    {
        yield return new WaitForSeconds(_delayToShowFinishButton);
    }
}
