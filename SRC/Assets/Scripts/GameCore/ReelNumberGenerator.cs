using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D;

public class ReelData
{
    public int reelId;
    public string reelName;
    public int reelPayout3;
    public int reelPayout4;
    public int reelPayout5;
}

public class ReelNumberGenerator : MonoBehaviour
{
    public List<ReelData> reelDataList;

    public int numberofReels = 5;
    public int[] symbolsPerReel;
    public float spinDuration = 3f;
    public float delayBetweenReels = 0.5f;
    public int[] paylines;

    public bool isSpinning;
    private int[] currentSymbols;
    private int[] targetSymbols;
    public GameObject[] reel1;
    public GameObject[] reel2;
    public GameObject[] reel3;

    public GameManager GameManager;
    public Sprite[] symbolSprites;

    private void Start()
    {
        // Initialize current and target symbols
        currentSymbols = new int[numberofReels];
        targetSymbols = new int[numberofReels];

        // Create symbol data
        CreateSymbolData(symbolSprites);
    }

    private void CreateSymbolData(Sprite[] symbolSprites)
    {
        // Example symbol data creation
        reelDataList = new List<ReelData>();

        // Define the symbols and their corresponding payouts
        string[] symbols = { "symbols_0", "symbols_1", "symbols_2", "symbols_3", "symbols_4"
                , "symbols_5", "symbols_6", "symbols_7", "symbols_8" };
        int[] payouts3 = { 0, 0, 10, 30, 20, 60, 120, 40, 50 };
        int[] payouts4 = { 0, 0, 10, 30, 20, 60, 120, 40, 50 };
        int[] payouts5 = { 0, 0, 10, 30, 20, 60, 120, 40, 50 };

        // Generate reel data objects and add them to the list
        for (int i = 0; i < numberofReels; i++)
        {
            ReelData reelData = new ReelData();
            reelData.reelId = i;
            reelData.reelName = symbols[Random.Range(0, symbols.Length)];
            reelData.reelPayout3 = payouts3[Random.Range(0, payouts3.Length)];
            reelData.reelPayout4 = payouts4[Random.Range(0, payouts4.Length)];
            reelData.reelPayout5 = payouts5[Random.Range(0, payouts5.Length)];

            reelDataList.Add(reelData);

            GameObject[] reel = GetReelObjects(i);
            for (int j = 0; j < reel.Length; j++)
            {
                GameObject symbolObject = reel[j];
                SpriteRenderer symbolRenderer = symbolObject.GetComponent<SpriteRenderer>();

                // Assign the sprite based on the reel data
                symbolRenderer.sprite = GetSymbolSprite(reelData.reelName, symbolSprites);
            }
        }
    }

    private Sprite GetSymbolSprite(string symbolName, Sprite[] symbolSprites)
    {
        // Find the sprite with the specified name
        Sprite symbolSprite = System.Array.Find(symbolSprites, sprite => sprite.name == symbolName);

        return symbolSprite;
    }

    private IEnumerator SpinCoroutine()
    {
        // Spin each reel individually with a delay between them
        for (int i = 0; i < numberofReels; i++)
        {
            StartCoroutine(SpinReel(i));
            yield return new WaitForSeconds(delayBetweenReels);
        }
    }

    private IEnumerator SpinReel(int reelIndex)
    {
        int symbolIndex = currentSymbols[reelIndex];
        int targetIndex = targetSymbols[reelIndex];

        float elapsedTime = 0f;
        while (elapsedTime < spinDuration)
        {
            // Calculate the current symbol based on time elapsed
            symbolIndex = (symbolIndex + 1) % symbolsPerReel[reelIndex];
            currentSymbols[reelIndex] = symbolIndex;

            // Update the reel's symbol display here (e.g., change sprite)
            UpdateSymbolSprite(reelIndex, symbolIndex);

            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // Set the final symbol to the target symbol
        currentSymbols[reelIndex] = targetIndex;
        UpdateSymbolSprite(reelIndex, targetIndex);
    }

    private void UpdateSymbolSprite(int reelIndex, int symbolIndex)
    {
        GameObject[] reel = GetReelObjects(reelIndex);
        GameObject symbolObject = reel[symbolIndex];
        SpriteRenderer symbolRenderer = symbolObject.GetComponent<SpriteRenderer>();

        // Assign the sprite based on the reel data or any other logic you have
        ReelData reelData = reelDataList[currentSymbols[reelIndex]];
        symbolRenderer.sprite = GetSymbolSprite(reelData.reelName, symbolSprites);
    }

    private GameObject[] GetReelObjects(int reelIndex)
    {
        GameObject[] reelObjects = null;

        switch (reelIndex)
        {
            case 0:
                reelObjects = reel1;
                break;
            case 1:
                reelObjects = reel2;
                break;
            case 2:
                reelObjects = reel3;
                break;
                // Add more cases for additional reels if needed
        }

        return reelObjects;
    }


    private bool HasReachedTargetSymbols()
    {
        for (int i = 0; i < numberofReels; i++)
        {
            if (currentSymbols[i] != targetSymbols[i])
                return false;
        }

        return true;
    }

    private void CheckForWin()
    {
        // Iterate through each payline
        for (int i = 0; i < paylines.Length; i++)
        {
            int[] symbolsOnPayline = new int[numberofReels];

            // Get the symbols on the current payline
            for (int j = 0; j < numberofReels; j++)
            {
                symbolsOnPayline[j] = currentSymbols[j];
            }

            // Check if the symbols on the payline match a winning combination
            if (CheckWinningCombination(symbolsOnPayline, paylines))
            {
                // Perform actions for a winning payline, such as awarding points, triggering animations, or playing sound effects
                Debug.Log("Payline " + (i + 1) + " wins!");

                // Get the symbol IDs on the winning payline
                int[] symbolIds = new int[numberofReels];
                for (int j = 0; j < numberofReels; j++)
                {
                    symbolIds[j] = reelDataList[currentSymbols[j]].reelId;
                }

                // Calculate the payouts for the winning combination
                int streakLength = numberofReels;
                int payout = CalculatePayout(symbolIds, streakLength);
                Debug.Log("Payout for the winning combination: " + payout);

                GameManager.playerWin += payout;
                GameManager.winText.text = GameManager.playerWin.ToString();

                GameManager.gameCredits += GameManager.playerWin;
                GameManager.playerCredits.text = GameManager.gameCredits.ToString();



            }
        }
    }

    private bool CheckWinningCombination(int[] symbolsOnPayline, int[] winningCombination)
    {
        // Compare the symbols on the payline with the winning combination
        for (int i = 0; i < numberofReels; i++)
        {
            if (symbolsOnPayline[i] != winningCombination[i])
                return false;
        }

        return true;
    }

    private int CalculatePayout(int[] symbolIds, int streakLength)
    {
        int totalPayout = 0;

        for (int i = 0; i < symbolIds.Length; i++)
        {
            int symbolId = symbolIds[i];
            ReelData reelData = reelDataList.Find(data => data.reelId == symbolId);

            if (reelData == null)
            {
                Debug.LogError("Reel data not found for symbol ID: " + symbolId);
                continue;
            }

            int payout = 0;
            switch (streakLength)
            {
                case 3:
                    payout = reelData.reelPayout3;
                    break;
                case 4:
                    payout = reelData.reelPayout4;
                    break;
                case 5:
                    payout = reelData.reelPayout5;
                    break;
                default:
                    Debug.LogError("Invalid streak length: " + streakLength);
                    break;
            }

            totalPayout += payout;
        }

        return totalPayout;
    }

    public void SpinReels()
    {
        if (!isSpinning)
        {
            isSpinning = true;

            // Generate random target symbols for each reel
            for (int i = 0; i < numberofReels; i++)
            {
                targetSymbols[i] = Random.Range(0, symbolsPerReel[i]);
            }

            // Start spinning the reels
            StartCoroutine(SpinCoroutine());
        }
    }

    private void Update()
    {
        if (isSpinning)
        {
            // Check if all reels have reached their target symbols
            if (HasReachedTargetSymbols())
            {
                isSpinning = false;
                Debug.Log("Spin finished!");

                // Perform win-checking logic here
                CheckForWin();
            }
        }
    }
}
