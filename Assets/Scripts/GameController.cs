

/**Alexander Amsenius Junior Unity Developer Clay AB**/
//**Kontrollerar spelmekaniken + UI**/

//Iteration 3
using TMPro;
using UnityEngine;

public class GameController : MonoBehaviour
{
    public TextMeshProUGUI moveCounterText; // Text för att visa antal drag
    public GameObject[] disks; // Array för alla diskar
    private Vector3[] initialPositions; // Startpositioner för diskarna
    private int moveCount = 0; // Räknare för antalet drag
    private float diskHeight = 0.5f; // Diskarnas höjd

    // Spel-loopens faser. Istället för switcha scenes
    public GameObject startScreen; // UI för startskärmen
    public GameObject gameDoneScreen; // UI för slutskärmen
    public bool isGameStarted = false; // Om spelet är startat

    void Start()
    {
        // Spara alla diskars initiala positioner vid start
        initialPositions = new Vector3[disks.Length];
        for (int i = 0; i < disks.Length; i++)
        {
            initialPositions[i] = disks[i].transform.position;
        }

        UpdateMoveCounter(); // Visa startvärdet (0)

        // Visa startskärmen och dölja spelslut-skärmen
        startScreen.SetActive(true);
        gameDoneScreen.SetActive(false);
    }

    // Starta spelet när startknappen trycks
    public void StartGame()
    {
        isGameStarted = true;
        startScreen.SetActive(false); // Dölja startskärmen
    }

    // Återställ spelet och visa startskärmen
    public void ResetGame()
    {
        // Återställ diskarnas positioner till deras initiala positioner
        for (int i = 0; i < disks.Length; i++)
        {
            disks[i].transform.position = initialPositions[i];
        }

        // Återställ dragens räknare
        moveCount = 0;
        UpdateMoveCounter();

        // Återgå till startskärmen
        isGameStarted = false;
        startScreen.SetActive(true);
        gameDoneScreen.SetActive(false);
    }

    // Flyttar disken till målstaven och justerar Y-höjden korrekt
    public void MoveDisk(GameObject disk, Transform targetPole)
    {
        if (!isGameStarted) return; // Om spelet inte är startat, gör ingenting

        Vector3 targetPosition = new Vector3(
            targetPole.position.x,
            GetNextDiskHeight(targetPole),
            targetPole.position.z
        );

        disk.transform.position = targetPosition;

        moveCount++;
        UpdateMoveCounter();

        // Kolla om spelet är klart efter varje drag
        CheckGameDone();
    }

    private float GetNextDiskHeight(Transform pole)
    {
        int disksOnPole = 0;

        foreach (GameObject disk in disks)
        {
            if (Mathf.Approximately(disk.transform.position.x, pole.position.x))
            {
                disksOnPole++;
            }
        }

        return pole.position.y + (disksOnPole * diskHeight);
    }

    private void UpdateMoveCounter()
    {
        moveCounterText.text = "Moves: " + moveCount;
    }

    // Kontrollera om alla diskar är på sista staven (spelet klart)
    private void CheckGameDone()
    {
        int disksOnLastPole = 0;

        // Vi antar att sista staven har en X-position någonstans mellan 4 och 6
        float lastPoleMinX = 4.0f;
        float lastPoleMaxX = 6.0f;

        foreach (GameObject disk in disks)
        {
            // Kolla om diskens X-position är inom intervallet för sista staven
            if (disk.transform.position.x >= lastPoleMinX && disk.transform.position.x <= lastPoleMaxX)
            {
                disksOnLastPole++;
            }
        }

        Debug.Log("Disks on last pole: " + disksOnLastPole);

        // Om alla diskar är på sista staven
        if (disksOnLastPole == disks.Length)
        {
            Debug.Log("Game is done!");
            GameDone();
        }
    }


    // Spelet är klart
    //Lägg till Debug felmedellande. Är det fel i UI eller i scriptet? 
    private void GameDone()
    {
        // Kontrollera om gameDoneScreen är satt och sedan aktivera den
        if (gameDoneScreen != null)
        {
            gameDoneScreen.SetActive(true); // Visa slutskärmen
        }
        else
        {
            Debug.LogError("GameDone screen is not assigned in the Inspector.");
        }

        isGameStarted = false; // Stoppa spelet
    }

}
