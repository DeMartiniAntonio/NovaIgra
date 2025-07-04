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
    [SerializeField] private int workers;       //rjeseno
    [SerializeField] private int unemployed;    //rjeseno
    [SerializeField] private int wood;          //rjeseno
    [SerializeField] private int gold;
    [SerializeField] private int food;          //rjeseno
    [SerializeField] private int stone;
    [SerializeField] private int iron;
    [SerializeField] private int tools;
    [SerializeField] private int days;          //rjeseno
    [Header("Buildings")]
    //farm, house, iron mines, gold mines, woodcutter, blacksmith, quarry
    [SerializeField] private int farm;          //rjeseno
    [SerializeField] private int house;         //1house = 4ppl //rjeseno
    [SerializeField] private int ironMines;
    [SerializeField] private int goldMines;
    [SerializeField] private int woodcutter;    //rjeseno
    [SerializeField] private int blacksmith;
    [SerializeField] private int quarry;
    [Header("Resource text")]
    [SerializeField] private TMP_Text populationText;
    [SerializeField] private TMP_Text daysText;
    [SerializeField] private TMP_Text woodText;
    [SerializeField] private TMP_Text foodText;
    [SerializeField] private TMP_Text ironText;
    [Header("Building text")]
    [SerializeField] private TMP_Text farmText;
    [SerializeField] private TMP_Text woodcutterText;
    [SerializeField] private TMP_Text housesText;

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
            FoodGathering();
            FoodProductions();
            WoodProduction();
            FoodConsuprion(1);
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

    private void FoodConsuprion(int foodConsumed) {

        food -= foodConsumed * Population();
    }

    private void FoodProductions()
    {

        food += farm * 4;
    }

    //number of max house * 4
    private int GetMaxPopulation() 
    {
        int maxPopulation = house * 4;
        return maxPopulation;
    }

    private void FoodGathering() 
    {
        food += unemployed / 2;    
    }

    public void BuilldFarm() {
        if (wood >= 10 && CanAssignWorker(2))
        {
            wood -= 10;
            farm++;
            WorkerAssign(2);
        }
        else if(wood < 10)
        {
            Debug.Log($"Not enough resources to build Farm, you need {10 - wood} wood");
        }
        else if (!CanAssignWorker(2))
        {
            Debug.Log($"Not enough resources to build Farm, you need {2-unemployed} workers");
        }
    }

    public void BuilldWoodcutter()
    {
        if (wood >= 5 && iron > 0 && CanAssignWorker(1)) 
        {
            wood -= 5;
            iron--;
            woodcutter++;
            WorkerAssign(1);
        }
        else if (wood < 5)
        {
            Debug.Log($"Not enough resources to build Farm, you need {10 - wood} wood");
        }
        else if (iron == 0)
        {
            Debug.Log($"Not enough resources to build Farm, you need 1 iron");
        }
        else if (!CanAssignWorker(1))
        {
            Debug.Log($"Not enough resources to build Farm, you need 1 worker");
        }
    }

    public void WoodProduction()
    {
        wood += woodcutter * 2;
    }

    //TODO: make this method a class
    private void BuildCost(int woodCost, int stoneCost, int workerAssign) {

        if (wood >= woodCost && stone >= stoneCost && unemployed >= workerAssign)
        {
            wood -=woodCost;
            stone -= stoneCost;
            workers += workerAssign;
        }
    }

    private void WorkerAssign(int amount) 
    {
        unemployed -= amount;
        workers += amount;
    }

    private bool CanAssignWorker(int amount) {
        return unemployed >= amount;
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