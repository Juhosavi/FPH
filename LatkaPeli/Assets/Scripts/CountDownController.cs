using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CountDownController : MonoBehaviour
{
    public int countdownTime;
    public TextMeshProUGUI countdownDisplay;
    public ProjectSettings projectSettings;

    private void Start()
    {
        projectSettings = GameObject.Find("ProjectSettings").GetComponent<ProjectSettings>();


    }

    public IEnumerator CountdownToStart()
    {
        if (countdownDisplay.gameObject.activeInHierarchy == false)
        {
            countdownDisplay.gameObject.SetActive(true);
            countdownTime = 5;
        }
        while (countdownTime > 0)
        {
            countdownDisplay.text = countdownTime.ToString();

            yield return new WaitForSeconds(1f);

            countdownTime--;
        }

        projectSettings.ReleasePlayersClientRpc();
        //countdownDisplay.text = "GO!";

        TimerController.instance.StartTimer();

        yield return new WaitForSeconds(1f);

        countdownDisplay.gameObject.SetActive(false);

    }
}
