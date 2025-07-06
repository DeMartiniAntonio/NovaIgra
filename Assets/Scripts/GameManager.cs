using System;
using System.Collections;
using System.Net.NetworkInformation;
using TMPro;
using UnityEngine;
using UnityEngine.UIElements;
using Random = UnityEngine.Random;

public class GameManager : MonoBehaviour
{
    [SerializeField] private GameObject pausePanel;
   

    //population, wood, gold, food, stone, iron, tools,
    [Header("RES")]

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
    [SerializeField] private TMP_Text notificationText;
    [SerializeField] private TMP_Text ironMinesText;

    private float timer;
    bool isGameRunning=false;

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
        if (!isGameRunning) {
            return;
        }


        timer += Time.deltaTime;

        if (timer >= 3)
        {
            days++;
            FoodGathering();
            FoodProductions();
            WoodProduction();
            IronProduction();
            FoodConsuprion(1);
            IncreasePopulation();
            UpdateText();
            GetMaxPopulation();
            timer = 0;

            if(food <= 0 && food >= -100)
            {
                string text = $"You have run out of food, your population will start to die of starvation!";
                StartCoroutine(NotificationText(text));
            }
            else if (food <=-100)
            {
                string text = $"Your population have gone to Germani! Game ower";
                StartCoroutine(NotificationText(text));
                isGameRunning = false;
            }
        }
    }

    public void InitializeGame() {
        UpdateText();
        isGameRunning=true;
    }

    private void IncreasePopulation() {
        if (days % 10 == 0) 
        {
            if (GetMaxPopulation() > Population()) 
            {
                unemployed = Math.Min(unemployed + house, GetMaxPopulation());
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
        return house * 4;
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
            UpdateText();
            string text = $"You Have built farm";
            StartCoroutine(NotificationText(text));
        }
        else if(wood < 10)
        {
            string text = $"Not enough resources to build Farm, you need {10 - wood} wood";
            StartCoroutine(NotificationText(text));
        }
        else if (!CanAssignWorker(2))
        {
            string text = $"Not enough resources to build Farm, you need {2-unemployed} workers";
            StartCoroutine(NotificationText(text));
        }
    }

    public void BuilldIronMine()
    {
        if (wood >= 15 && CanAssignWorker(3))
        {
            wood -= 15;
            ironMines++;
            WorkerAssign(3);
            UpdateText();
            string text = $"You Have built iron mine";
            StartCoroutine(NotificationText(text));
        }
        else if (wood < 10)
        {
            string text = $"Not enough resources to build Iron mine, you need {15 - wood} wood";
            StartCoroutine(NotificationText(text));
        }
        else if (!CanAssignWorker(3))
        {
            string text = $"Not enough resources to build Farm, you need {3 - unemployed} workers";
            StartCoroutine(NotificationText(text));
        }
    }

    public void BuillHouse()
    {
        if (wood >= 2)
        {
            wood -= 2;
            house++;
            
            UpdateText();
            string text = $"You Have built house";
            StartCoroutine(NotificationText(text));
        }
        else if (wood < 2)
        {
            string text = $"Not enough resources to build House, you need {2 - wood} wood";
            NotificationText(text);

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
            string text = $"You Have built woodcutter";
            StartCoroutine(NotificationText(text));

            UpdateText();
        }
        else if (wood < 5)
        {
            string text = $"Not enough resources to build Woodcutter, you need {5 - wood} wood";
            StartCoroutine(NotificationText(text));

        }
        else if (iron == 0)
        {
            string text = $"Not enough resources to build Woodcutter, you need 1 iron";
            StartCoroutine(NotificationText(text));

        }
        else if (!CanAssignWorker(1))
        {
            string text = $"Not enough resources to build Woodcutter, you need 1 worker";
            StartCoroutine(NotificationText(text));

        }
    }

    public void WoodProduction()
    {
        wood += woodcutter * 2;
    }

    public void IronProduction()
    {
        iron += ironMines * 2;
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

    public void UpdateText() { 

        //resources
        populationText.text= $"Population: {Population()}/{GetMaxPopulation()}\n     Workers: {workers}\n     Unemployed: {unemployed}";
        woodText.text = $"Wood: {wood}";
        foodText.text = $"Food: {food}";
        ironText.text = $"Iron: {iron}";

        //buildings
        farmText.text = $"Farms: {farm}";
        woodcutterText.text = $"Wood Cutter: {woodcutter}";
        housesText.text = $"Houses: {house}";
        daysText.text = $"Days: {days}";
        ironMinesText.text = $"Iron: {ironMines}";
    }

    IEnumerator NotificationText(string text) 
    {
        notificationText.text= text;
        yield return new WaitForSeconds(2);
        notificationText.text = string.Empty;
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