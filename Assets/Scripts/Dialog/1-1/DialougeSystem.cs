using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialougeSystem : MonoBehaviour
{
    public Text textDisplay;
    public Image textImage;
    public string[] sentences;
    private int index;
    public float typingspeed;
    public float FadeTime;

    float speed;
    Animator animator;   

    public GameObject continueButton;
    public GameObject Player;

    public GameObject DialougeManager;
    public GameObject DialougeMushroom;
    public GameObject DialougeVillage;
    public GameObject DialougeEnemyMonkey;

    public GameObject EnemyMonkey;
    
    public Rigidbody2D PlayerRb2d;

    public bool InMushroom = false;
    public bool InVillage = false;
    public bool InEnemyMonkey = false;

    public GameObject JPF1;

    // Start is called before the first frame update
    private void Awake()
    {      
        Player.GetComponent<PlayerMovement>().enabled = false;     //玩家一開始不動      
    }

    void Start()
    {
        StartCoroutine(Type());
    }

    public IEnumerator Type()
    {
        foreach (char letter in sentences[index].ToCharArray())
        {            
            textDisplay.text += letter;
            yield return new WaitForSeconds(typingspeed);
        }
        continueButton.SetActive(true);
    }   
    // Update is called once per frame
    void Update()
    {
       
    }
    
    public void NextSentences()
    {
        continueButton.SetActive(false);
        textDisplay.text = "";
        if (index < sentences.Length)
        {
            index++;
            if (index == 2)    //先對話2句話，超過段落數量時結束
            {
                Player.GetComponent<PlayerMovement>().enabled = true;    //玩家可以動了  
                textImage.CrossFadeAlpha(0, FadeTime, false);            //FadeOut
                StopCoroutine(Type());                                        
            }          
            else if (index == 3)    //先對話1句話，超過段落數量時結束                香菇
            {
               speed = 65f;
               Player.GetComponent<PlayerMovement>().enabled = true;     //玩家可以動了
               textImage.CrossFadeAlpha(0, FadeTime, false);             //FadeOut
               StopCoroutine(Type());
               DialougeMushroom.SetActive(false);                
            }
            else if (index == 4)    //先對話1句話，超過段落數量時結束                村莊
            {
                speed = 65f;
                Player.GetComponent<PlayerMovement>().enabled = true;    //玩家可以動了
                textImage.CrossFadeAlpha(0, FadeTime, false);            //FadeOut
                StopCoroutine(Type());
                DialougeVillage.SetActive(false);      
            }
            else if (index == 9)    //先對話4句話，超過段落數量時結束                捲尾猴村民
            {
                speed = 65f;                
                Player.GetComponent<PlayerMovement>().enabled = true;    //玩家可以動了
                EnemyMonkey.GetComponent<Monkey>().enabled = true;       //村民可以動了    
                textImage.CrossFadeAlpha(0, FadeTime, false);            //FadeOut
                StopCoroutine(Type());
                DialougeEnemyMonkey.SetActive(false);
            }
            else
            {
                StartCoroutine(Type());
            }
        }             
    }
     

    void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "DialogueFieldMushRoom")
        {
            if (InMushroom == false)
            {
                InMushroom = true;
                textImage.CrossFadeAlpha(1, FadeTime, false);   //FadeIn 
                StartCoroutine(Type());               
                PlayerRb2d.velocity = Vector2.zero;
                Player.GetComponent<PlayerMovement>().animator.Rebind();  //玩家不動  
                Player.GetComponent<PlayerMovement>().enabled = false;
                JPF1.SetActive(true);
            }          
        }

        if (collision.tag == "DialogueFieldVillage")
        {
            if (InVillage == false)
            {
                InVillage = true;
                textImage.CrossFadeAlpha(1, FadeTime, false);   //FadeIn 
                StartCoroutine(Type());
                PlayerRb2d.velocity = Vector2.zero;
                Player.GetComponent<PlayerMovement>().animator.Rebind();
                Player.GetComponent<PlayerMovement>().enabled = false;   //玩家不動 
            }
        }

        if (collision.tag == "DialogueFieldEnemyMonkey")
        {
            if (InEnemyMonkey == false)
            {
                InEnemyMonkey = true;
                textImage.CrossFadeAlpha(1, FadeTime, false);   //FadeIn 
                EnemyMonkey.SetActive(true);
                StartCoroutine(Type());
                speed = 0f;
                PlayerRb2d.velocity = Vector2.zero;
                EnemyMonkey.GetComponent<Monkey>().enabled = false; //捲尾猴村民不動
                Player.GetComponent<PlayerMovement>().animator.Rebind();   
                Player.GetComponent<PlayerMovement>().enabled = false;   //玩家不動 
            }
        }
    }

    void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "DialogueFieldMushRoom")
        {
            InMushroom = false;           
        }

        if (collision.tag == "DialogueFieldVillage")
        {
            InVillage = false;
        }

        if (collision.tag == "DialogueFieldEnemyMonkey")
        {
            InEnemyMonkey = false;
        }
    }
}
