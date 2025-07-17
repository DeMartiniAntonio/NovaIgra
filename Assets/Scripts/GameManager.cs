using System;
using System.Collections;
using System.Net.NetworkInformation;
using System.Transactions;
using TMPro;
using UnityEditor;
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
    [SerializeField] private int days;          //rjeseno
    [Header("Buildings")]
    //farm, house, iron mines, gold mines, woodcutter, blacksmith, quarry
    [SerializeField] private int farm;          //rjeseno
    [SerializeField] private int house;         //1house = 4ppl //rjeseno
    [SerializeField] private int ironMines;
    [SerializeField] private int goldMines;
    [SerializeField] private int woodcutter;    //rjeseno

    [Header("Resource text")]
    [SerializeField] private TMP_Text populationText;
    [SerializeField] private TMP_Text daysText;
    [SerializeField] private TMP_Text woodText;
    [SerializeField] private TMP_Text foodText;
    [SerializeField] private TMP_Text ironText;
    [SerializeField] private TMP_Text goldText;
    [Header("Building text")]
    [SerializeField] private TMP_Text farmText;
    [SerializeField] private TMP_Text woodcutterText;
    [SerializeField] private TMP_Text housesText;
    [SerializeField] private TMP_Text notificationText;
    [SerializeField] private TMP_Text ironMinesText;
    [SerializeField] private TMP_Text goldMinesText;
    [Header("Building Level")]
    [SerializeField] private int farmLvl;          
    [SerializeField] private int ironMinesLvl;
    [SerializeField] private int goldMinesLvl;
    [SerializeField] private int woodcutterLvl;
    [Header("Level up cost")]
    [SerializeField] private TMP_Text farmLvlCost;
    [SerializeField] private TMP_Text ironMinesLvlCost;
    [SerializeField] private TMP_Text goldMinesLvlCost;
    [SerializeField] private TMP_Text woodcutterLvlCost;
    
    [Header("Zadaca 17.7.")]
    private const string HIGHSCORE = "HIGHSCORE";
    private const string PLAYER_NAME = "PLAYERNAME";
    private int daysCounter;
    //nalaze se na main menu
    [SerializeField] private TMP_InputField inputName;
    [SerializeField] private TMP_Text highscoreText;

    private float timer;
    
    bool isGameRunning=false;


    //zadaca 17.7.
    private void Start()
    {
        highscoreText.text = $"Highscore: {SaveSystem.GetIntValue(HIGHSCORE)} days\nPlayer: {SaveSystem.GetStringValue(PLAYER_NAME)}";
    }

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

        if (timer >= 7)
        {
            days++;
            FoodGathering();
            FoodProductions();
            WoodProduction();
            IronProduction();
            GoldProduction();
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
                //zadaca 17.7.
                if (days > daysCounter)
                {
                    daysCounter = days;
                    SaveSystem.SetIntValue(HIGHSCORE, daysCounter);
                    SaveSystem.SetStringValue(PLAYER_NAME, inputName.text);
                }
            }
        }
    }

    public void InitializeGame() {
        UpdateText();
        isGameRunning=true;
    }

    private void IncreasePopulation() {
        if (days % 5 == 0) 
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

        food += farm * 4 * farmLvl;
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

    public void BuilldGoldMine()
    {
        if (wood >= 30 && iron >=30 && CanAssignWorker(10))
        {
            wood -= 30;
            iron -= 30;
            goldMines++;
            WorkerAssign(10);
            UpdateText();
            string text = $"You Have built gold mine";
            StartCoroutine(NotificationText(text));
        }
        else if (wood < 30)
        {
            string text = $"Not enough resources to build Gold mine, you need {30 - wood} wood";
            StartCoroutine(NotificationText(text));
        }
        else if (iron < 30)
        {
            string text = $"Not enough resources to build Gold mine, you need {30 - iron} wood";
            StartCoroutine(NotificationText(text));
        }
        else if (!CanAssignWorker(10))
        {
            string text = $"Not enough resources to build Gold, you need {10 - unemployed} workers";
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
        wood += woodcutter * 2 * woodcutterLvl;
    }

    public void IronProduction()
    {
        iron += ironMines * 2 * ironMinesLvl;
    }
    public void GoldProduction()
    {
        gold += goldMines*goldMinesLvl;
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

    public void FarmLevelUp() {
        if (wood >= farmLvl * farmLvl * 30 && gold >= farmLvl * farmLvl * 10) {
            wood -= farmLvl * farmLvl * 30;
            gold -= farmLvl * farmLvl * 10;
            farmLvl++;
            UpdateText();
            string text = $"Farm Level Up";
            StartCoroutine(NotificationText(text));
        }
        else if (wood < farmLvl * farmLvl * 30)
        {
            string text = $"Not enough resources to level up Farm, you need {farmLvl * farmLvl * 30 - wood} wood";
            StartCoroutine(NotificationText(text));

        }
        else if (gold < farmLvl * farmLvl * 10)
        {
            string text = $"Not enough resources to build Woodcutter, you need {farmLvl * farmLvl * 10 - gold} gold";
            StartCoroutine(NotificationText(text));

        }
    }
    public void WoodcutterLevelUp()
    {
        if (wood >= woodcutterLvl * woodcutterLvl * 30 && gold >= woodcutterLvl * woodcutterLvl * 10)
        {
            wood -= woodcutterLvl * woodcutterLvl * 30;
            gold -= woodcutterLvl * woodcutterLvl * 10;
            woodcutterLvl++;
            UpdateText();
            string text = $"Woodcutter Level Up";
            StartCoroutine(NotificationText(text));
        }
        else if (wood < woodcutterLvl * woodcutterLvl * 30)
        {
            string text = $"Not enough resources to level up Farm, you need {woodcutterLvl * woodcutterLvl * 30 - wood} wood";
            StartCoroutine(NotificationText(text));

        }
        else if (gold < woodcutterLvl * woodcutterLvl * 10)
        {
            string text = $"Not enough resources to build Woodcutter, you need {woodcutterLvl * woodcutterLvl * 10 - gold} gold";
            StartCoroutine(NotificationText(text));

        }
    }

    public void IronLevelUp()
    {
        if (iron >= ironMinesLvl * ironMinesLvl * 30 && gold >= ironMinesLvl * ironMinesLvl * 10)
        {
            iron -= ironMinesLvl * ironMinesLvl * 30;
            gold -= ironMinesLvl * ironMinesLvl * 10;
            ironMinesLvl++;
            UpdateText();
            string text = $"Iron mines Level Up";
            StartCoroutine(NotificationText(text));
        }
        else if (iron < ironMinesLvl * ironMinesLvl * 30)
        {
            string text = $"Not enough resources to level up Farm, you need {ironMinesLvl * ironMinesLvl * 30 - iron} iron";
            StartCoroutine(NotificationText(text));
        }
        else if (gold < ironMinesLvl * ironMinesLvl * 10)
        {
            string text = $"Not enough resources to build Woodcutter, you need {ironMinesLvl * ironMinesLvl * 10 - gold} gold";
            StartCoroutine(NotificationText(text));
        }
    }

    public void GoldMinesLevelUp()
    {
        if (gold >= goldMinesLvl * goldMinesLvl * 10)
        {

            gold -= goldMinesLvl * goldMinesLvl * 10;
            goldMinesLvl++;
            UpdateText();
            string text = $"Gold mines Level Up";
            StartCoroutine(NotificationText(text));

        }

        else if (gold < goldMinesLvl * goldMinesLvl * 10)
        {
            string text = $"Not enough resources to build Woodcutter, you need {goldMinesLvl * goldMinesLvl * 10 - gold} gold";
            StartCoroutine(NotificationText(text));
        }
    }

    public void UpdateText() { 

        //resources
        populationText.text= $" Population: {Population()}/{GetMaxPopulation()}\n     Workers: {workers}\n     Unemployed: {unemployed}";
        woodText.text = $" Wood: {wood}";
        foodText.text = $" Food: {food}";
        ironText.text = $" Iron: {iron}";
        goldText.text = $" Gold: {gold}";

        //buildings
        farmText.text = $" Farms (level: {farmLvl}): {farm}";
        woodcutterText.text = $" Woodcutter (level: {woodcutterLvl}): {woodcutter}";
        housesText.text = $" Houses: {house}";
        daysText.text = $" Days: {days}";
        ironMinesText.text = $" Iron mine (level: {ironMinesLvl}): {ironMines}";
        goldMinesText.text = $" Gold mine (level: {goldMinesLvl}): {goldMines}";

        farmLvlCost.text = $"Wood: {farmLvl * farmLvl * 30}\n Gold: {farmLvl * farmLvl * 10}";
        ironMinesLvlCost.text = $"Iron: {ironMinesLvl * ironMinesLvl * 30}\n Gold: {ironMinesLvl * ironMinesLvl * 10}";
        woodcutterLvlCost.text = $"Wood: {woodcutterLvl * woodcutterLvl * 30}\n Gold: {woodcutterLvl * woodcutterLvl * 10}";
        goldMinesLvlCost.text = $"Gold: {goldMinesLvl * goldMinesLvl * 10}";
    }

    public void BuyFoodSmall()
    {
        string text;
        if (gold < 5)
        {
            text = $"Not enough gold to buy 5 food, you need at least 5 gold";
            StartCoroutine(NotificationText(text));
            return;
        }
        gold -= 5;
        food += 100;

        text = $"You bought 5 food";
        StartCoroutine(NotificationText(text));
        UpdateText();
    }
    public void BuyFoodBig()
    {
        string text;
        if (gold < 50)
        {
            text = $"Not enough gold to buy 50 food, you need at least 50 gold";
            StartCoroutine(NotificationText(text));
            return;
        }
        gold -= 50;
        food += 1100;

        text = $"You bought 50 food";
        StartCoroutine(NotificationText(text));
        UpdateText();
    }

    public void BuyWoodSmall()
    {
        string text;
        if (gold < 5)
        {
            text = $"Not enough gold to buy 5 wood, you need at least 5 gold";
            StartCoroutine(NotificationText(text));
            return;
        }
        gold -= 5;
        wood += 50;

        text = $"You bought 5 wood";
        StartCoroutine(NotificationText(text));
        UpdateText();
    }
    public void BuyWoodBig()
    {
        string text;
        if (gold < 50)
        {
            text = $"Not enough gold to buy 550 wood, you need at least 50 gold";
            StartCoroutine(NotificationText(text));
            return;
        }
        gold -= 50;
        wood += 550;

        text = $"You bought 550 wood";
        StartCoroutine(NotificationText(text));
        UpdateText();
    }

    public void BuyIronSmall()
    {
        string text;
        if (gold < 5)
        {
            text = $"Not enough gold to buy 5 iron, you need at least 5 gold";
            StartCoroutine(NotificationText(text));
            return;
        }
        gold -= 5;
        iron += 30;

        text = $"You bought 5 iron";
        StartCoroutine(NotificationText(text));
        UpdateText();
    }
    public void BuyIronBig()
    {
        string text;
        if (gold < 50)
        {
            text = $"Not enough gold to buy 50 iron, you need at least 50 gold";
            StartCoroutine(NotificationText(text));
            return;
        }
        gold -= 50;
        iron += 330;

        text = $"You bought 50 iron";
        StartCoroutine(NotificationText(text));
        UpdateText();
    }
    public void SellWoodSmall()
    {
        string text;

        if (wood < 20)
        {
            text = $"Not enough wood to sell, you need at least 20 wood";
            StartCoroutine(NotificationText(text));
            return;
        }
        gold += 1;
        wood -= 20;
        text = $"You sold 20 wood";
        StartCoroutine(NotificationText(text));
        UpdateText();
    }

    public void SellWoodBig()
    {
        string text;
        if (wood < 100)
        {
            text = $"Not enough wood to sell, you need at least 100 wood";
            StartCoroutine(NotificationText(text));
            return;
        }
        gold += 7;
        wood -= 100;
        text = $"You sold 100 wood";
        StartCoroutine(NotificationText(text));
        UpdateText();
    }
    public void SellIronSmall()
    {
        string text;
        if (iron < 10)
        {
            text = $"Not enough iron to sell, you need at least 10 iron";
            StartCoroutine(NotificationText(text));
            return;
        }
        
        gold += 2;
        iron -= 10;
        text = $"You sold 10 iron";
        StartCoroutine(NotificationText(text));
        UpdateText();
    }
    public void SellIronBig()
    {
        string text;
        if (iron < 50)
        {
            text = $"Not enough iron to sell, you need at least 50 iron";
            StartCoroutine(NotificationText(text));
            return;
        }
        gold += 6;
        iron -= 50;
        text = $"You sold 50 iron";
        StartCoroutine(NotificationText(text));
        UpdateText();
    }
    public void SellFoodSmall()
    {
        string text;
        if (food < 10)
        {
            text = $"Not enough food to sell, you need at least 10 food";
            StartCoroutine(NotificationText(text));
            return;
        }
        gold += 1;
        food -= 10;
         text = $"You sold 10 food";
        StartCoroutine(NotificationText(text));
        UpdateText();
    }
    public void SellFoodBig()
    {
        string text;
        if (food < 100)
        {
             text = $"Not enough food to sell, you need at least 100 food";
            StartCoroutine(NotificationText(text));
            return;
        }
        gold += 1;
        food -= 100;
        text = $"You sold 100 food";
        StartCoroutine(NotificationText(text));
        UpdateText();
    }

    public void BuyCasle()
    {
        if (wood >= 10000 && iron >= 10000 && food >= 10000 && GetMaxPopulation() >= 10000 && gold >= 5000)
        {
            notificationText.text = "You have built a castle, you win!";
            isGameRunning = false;
        }
        else
        {
            notificationText.text = "You need 10000 wood, 10000 iron, 10000 food, 10000 population and 5000 gold to build a castle";
        }
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