using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class QuestionsManager : MonoBehaviour
{
    public static int PlayTime = 30;

    [Header("Questions Database")]
    public List<UserQuestion> questions = new List<UserQuestion>();

    [Header("UI Containers")]
    public Text Question;
    public Text greenAnswer;
    public Text orangeAnswer;
    public Text redAnswer;
    public Text blueAnswer;
    public Text timeKeeper;

    [Header("Highliners")]
    public Image AnswerA;
    public Image AnswerB;
    public Image AnswerC;
    public Image AnswerD;

    [Header("Sound Effects")]
    public AudioClip sfxBombBeep;
    public AudioClip sfxBombBoom;
    public AudioClip sfxCongratulations;


    [SerializeField]
    private UserQuestion currentQuestion;

    void Start()
    {
        LoadQuestion();

        AnswerA.gameObject.SetActive(false);
        AnswerB.gameObject.SetActive(false);
        AnswerC.gameObject.SetActive(false);
        AnswerD.gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            validateAnswer("A");
            AnswerA.gameObject.SetActive(true);
        }
        if (Input.GetKeyDown(KeyCode.B))
        {
            validateAnswer("B");
            AnswerB.gameObject.SetActive(true);
        }
        if (Input.GetKeyDown(KeyCode.C))
        {
            validateAnswer("C");
            AnswerC.gameObject.SetActive(true);

        }
        if (Input.GetKeyDown(KeyCode.D))
        {
            validateAnswer("D");
            AnswerD.gameObject.SetActive(true);

        }
    }

    private void LoadQuestion()
    {
        if (questions.Count > 0)
        {
            int indiceRandom = UnityEngine.Random.Range(0,questions.Count);
            currentQuestion = questions[indiceRandom];
            Question.text = currentQuestion.question;
            greenAnswer.text = currentQuestion.answerA;
            orangeAnswer.text = currentQuestion.answerB;
            redAnswer.text = currentQuestion.answerC;
            blueAnswer.text = currentQuestion.answerD;

            StartCoroutine(startTimeKeeper());
        }
        else
        {
            Debug.Log("No hay preguntas cargadas");
        }
    }


    private void validateAnswer(string answer)
    {
        StopAllCoroutines();


        switch (currentQuestion.correctAnswer)
        {
            case "A":
                AnswerA.gameObject.SetActive(true);
                break;
            case "B":
                AnswerB.gameObject.SetActive(true);
                break;
            case "C":
                AnswerC.gameObject.SetActive(true);
                break;
            case "D":
                AnswerD.gameObject.SetActive(true);
                break;
        }

        StartCoroutine(NavigateNextScreen(answer));
        
        this.enabled = false;
    }

    private void NavigatetoNextScreen(string answer)
    {
        if (currentQuestion.correctAnswer == answer)
        {
            GameManager.Instance.Congratulations();
        }
        else
        {
            GameManager.Instance.YourLose();
        }
    }

    IEnumerator startTimeKeeper ()
    {
        int currentTime = PlayTime;
        while(currentTime >= 0)
        {
            timeKeeper.text = "00:" + currentTime.ToString("00");
            Vector3 cameraPosition = Camera.main.transform.position;
            AudioSource.PlayClipAtPoint(sfxBombBeep, cameraPosition, 0.3f);
            yield return new WaitForSeconds(1);
            currentTime--;
        }

        validateAnswer("W");
    }

    IEnumerator NavigateNextScreen (string answer)
    {
        if (currentQuestion.correctAnswer != answer)
        {
            GameManager.Instance.cameraVFX.SetActive(true);
            Vector3 cameraPosition = Camera.main.transform.position;
            AudioSource.PlayClipAtPoint(sfxBombBoom, cameraPosition);
        }else
        {
            Vector3 cameraPosition = Camera.main.transform.position;
            AudioSource.PlayClipAtPoint(sfxCongratulations, cameraPosition);
        }
        yield return new WaitForSeconds(3);
        NavigatetoNextScreen(answer);
    }

}

[System.Serializable]
public struct UserQuestion
{
    public string question;
    public string answerA;
    public string answerB;
    public string answerC;
    public string answerD;
    public string correctAnswer;
}