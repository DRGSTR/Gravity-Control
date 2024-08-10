using UnityEngine;
using UnityEngine.UI;

public class CountdownTimer : MonoBehaviour
{
    public float startTime = 120f; // Starting time in seconds
    private float currentTime;
    public Text timerText; // Reference to the UI Text component
    public bool timerIsRunning = false;

    void Start()
    {
        // Initialize the timer
        currentTime = startTime;
        timerIsRunning = true;
    }

    void Update()
    {
        if (timerIsRunning)
        {
            if (currentTime > 0)
            {
                // Update the timer
                currentTime -= Time.deltaTime;
                UpdateTimerDisplay(currentTime);
            }
            else
            {
                // Stop the timer when it reaches 0
                currentTime = 0;
                timerIsRunning = false;
                UpdateTimerDisplay(currentTime);
                TimerEnded();
            }
        }
    }

    void UpdateTimerDisplay(float timeToDisplay)
    {
        // Convert time to minutes and seconds
        float minutes = Mathf.FloorToInt(timeToDisplay / 60);
        float seconds = Mathf.FloorToInt(timeToDisplay % 60);

        // Update the timer text UI
        timerText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
    }

    void TimerEnded()
    {
        Debug.Log("Timer has ended!");
        // Implement any additional logic when the timer ends, such as triggering events or ending the game.
    }

    public void StartTimer()
    {
        // Method to start or restart the timer
        currentTime = startTime;
        timerIsRunning = true;
    }

    public void StopTimer()
    {
        // Method to manually stop the timer
        timerIsRunning = false;
    }
} //class
