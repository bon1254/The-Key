using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ProloguePottery : MonoBehaviour
{
    public string[] sentence = {"看起來像是文字的符號…但是無法辨識上面寫了什麼。"};
    private int index = 0;
    public Text textDisplay;
    public GameObject continueButton;
    public GameObject DialogueManager;
    public Image textImage;
    public float typingspeed;
    public float FadeTime;
    public PrologueSceneChange prologueSceneChange;

    public GameObject Player;
    public bool IsPlayer = false;

    public void StartDialogue()
    {
        DialogueManager.SetActive(true);
        textImage.CrossFadeAlpha(1, FadeTime, true);//FadeOut
        StartCoroutine(Type2());
    }

    public IEnumerator Type2()
    {
        yield return new WaitForSeconds(.5f);
        foreach (char letter in sentence[index].ToCharArray())
        {
            textDisplay.text += letter;
            yield return new WaitForSeconds(typingspeed);
        }
        continueButton.SetActive(true);
    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log(index);
        if (Input.GetKeyDown(KeyCode.E) && IsPlayer == true)
        {
            Player.GetComponent<PlayerMovement>().enabled = false;
            StartDialogue();
        }
    }

    public void NextSentences()
    {
        continueButton.SetActive(false);
        textDisplay.text = "";
        if (index < sentence.Length)
        {
            index++;
            if (index == 1)    //先對話1句話，超過段落數量時結束
            {
                Player.GetComponent<PlayerMovement>().enabled = true;    //玩家可以動了                             
                StopCoroutine(Type2());
            }
        }
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Player")
        {
            IsPlayer = true;           
        }
    }

    void OnTriggerExir2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            IsPlayer = false;
        }
    }
}
