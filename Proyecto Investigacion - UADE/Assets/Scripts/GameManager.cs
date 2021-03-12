﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private string[] _cardsIDsToActivate;
    [SerializeField] private int _amountOfPlays = 5;

    private int _currentPlayed = 0;
    private float _currentCoins = 0;
    private string _gameplayCaseID = "";

    public event Action OnCoinsChange;
    public event Action OnFinishPlays;

    public float CurrentCoins { get => _currentCoins; }
    public string[] CardsIDsToActivate { get => _cardsIDsToActivate; set => _cardsIDsToActivate = value; }
    public int AmountOfPlays { get => _amountOfPlays; }
    public int CurrentPlayed { get => _currentPlayed; }

    private void Awake()
    {
        InitGameManagerEntity();
    }

    public void ChangeCoins(float coinsAmount)
    {
        if(_currentCoins + coinsAmount >= 0)
        {
            _currentCoins += coinsAmount;
        }
        else { _currentCoins = 0; }

        OnCoinsChange?.Invoke();
    }

    public void IncreaseCurrentPlayed() 
    { 
        if(_currentPlayed + 1 < _amountOfPlays)
        {
            _currentPlayed += 1;
            print("_currentPlayed  = " + _currentPlayed);
        }
        else
        {
            _currentPlayed += 1;
            OnFinishPlays?.Invoke();
        }
    }

    private void InitGameManagerEntity()
    {
        GameObject[] objs = GameObject.FindGameObjectsWithTag("GameManager");

        if (objs.Length > 1)
        {
            Destroy(this.gameObject);
        }

        DontDestroyOnLoad(this.gameObject);
    }

    public void InitGameManagerProperties(ProvinceProperties provinceValues)
    {
        _amountOfPlays = provinceValues.AmountOfPlays;
        _cardsIDsToActivate = provinceValues.CardsIDs;
        _currentCoins = provinceValues.InitialGameCoins;
        _gameplayCaseID = provinceValues.GameplayCaseID;
    }

}