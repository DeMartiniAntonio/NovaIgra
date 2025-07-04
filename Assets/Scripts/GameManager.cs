using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UIElements;
using Random = UnityEngine.Random;

public class GameManager : MonoBehaviour
{
    [SerializeField] private GameObject pausePanel;

    //population, wood, gold, food, stone, iron, tools,
    [Header("RES")]
    [SerializeField] private int population;
    [SerializeField] private int workers;
    [SerializeField] private int unemployed;
    [SerializeField] private int wood;
    [SerializeField] private int gold;
    [SerializeField] private int food;
    [SerializeField] private int stone;
    [SerializeField] private int iron;
    [SerializeField] private int tools;
    [SerializeField] private int days;
    [Header("Buildings")]
    //farm, house, iron mines, gold mines, woodcutter, blacksmith, quarry
    [SerializeField] private int farm;
    [SerializeField] private int house;         //1house = 4ppl
    [SerializeField] private int ironMines;
    [SerializeField] private int goldMines;
    [SerializeField] private int woodcutter;
    [SerializeField] private int blacksmith;
    [SerializeField] private int quarry;


    private float timer;

    private void Update()
    {
        //one min is one day
        TimeOfDay();

        if (Input.GetKeyDown(KeyCode.Escape)) {
            Pause();

        }
    }

    private void TimeOfDay()
    {
        timer += Time.deltaTime;

        if (timer >= 60)
        {
            days++;
            GoodConsuprion(Population());
            IncreasePopulation();
            timer = 0;
        }
    }

    private void IncreasePopulation() {
        if (days % 2 == 0) 
        {
            if (GetMaxPopulation() > Population()) 
            {
                unemployed += house;
            }
        }
    }
    private int Population() 
    {
        return workers + unemployed;
    }

    private void GoodConsuprion(int foodConsumed) {

        food -= foodConsumed * Population();
    }

    //number of max house * 4
    private int GetMaxPopulation() 
    {
        int maxPopulation = house * 4;
        return maxPopulation;
    }

    private void FoodProduction() 
    {
        food += unemployed / 2;    
    }

    public void BuilldFarm() {
        farm++;
        unemployed--;
        workers++;
    }










    private void Pause() {

        if (Time.timeScale == 1)
        {
            Time.timeScale = 0f;
            pausePanel.gameObject.SetActive(true);

        }
        else if (Time.timeScale == 0) {
            Time.timeScale = 1f;
            pausePanel.gameObject.SetActive(false);

        }
    }
}