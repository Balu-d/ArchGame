using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class UnlockManifolds : MonoBehaviour
{
    public List<Button> buttons;
    public List<Button> shuffledButtons;
    private int counter = 0;

    void Start()
    {
        RestartTheGame();
    }

    public void RestartTheGame()
    {
        counter = 0;
        shuffledButtons = buttons.OrderBy(a => Random.Range(0, 100)).ToList();

        for (int i = 1; i <= 10; i++)
        {
            shuffledButtons[i - 1].GetComponentInChildren<Text>().text = i.ToString();
            shuffledButtons[i - 1].interactable = true;
            shuffledButtons[i - 1].image.color = new Color32(177, 220, 233, 255);
        }
    }

    public void PressButton(Button button)
    {
        int buttonNumber = int.Parse(button.GetComponentInChildren<Text>().text);
        if (buttonNumber - 1 == counter)
        {
            counter++;
            button.interactable = false;
            button.image.color = Color.green;

            if (counter == 10)
            {
                StartCoroutine(PresentResult(true));
            }
        }
        else
        {
            StartCoroutine(PresentResult(false));
        }
    }

    public IEnumerator PresentResult(bool win)
    {
        if (!win)
        {
            foreach (var button in shuffledButtons)
            {
                button.image.color = Color.red;
                button.interactable = false;
            }
        }

        yield return new WaitForSeconds(2f);
        RestartTheGame();
    }
}
