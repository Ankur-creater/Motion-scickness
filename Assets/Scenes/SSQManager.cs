using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;

public class SSQManager : MonoBehaviour
{
    [System.Serializable]
    public struct SSQQuestion
    {
        public string question;
    }

    public List<SSQQuestion> questions = new List<SSQQuestion>();
    public TMP_Text questionText;
    public TMP_Dropdown answerDropdown;
    public Button submitButton;

    private int currentQuestionIndex = 0;
    private List<int> responses = new List<int>();

    void Start()
    {
        LoadQuestions();
        ShowNextQuestion();
        submitButton.onClick.AddListener(SubmitAnswer);
    }

    void LoadQuestions()
    {
        questions.Add(new SSQQuestion { question = "Do you feel overall discomfort?" });
        questions.Add(new SSQQuestion { question = "Are you experiencing unusual tiredness?" });
        questions.Add(new SSQQuestion { question = "Do you feel a headache developing?" });
        questions.Add(new SSQQuestion { question = "Are your eyes feeling strained or tired?" });
        questions.Add(new SSQQuestion { question = "Are you having trouble focusing your vision?" });
        questions.Add(new SSQQuestion { question = "Do you notice excessive saliva production?" });
        questions.Add(new SSQQuestion { question = "Are you sweating more than usual?" });
        questions.Add(new SSQQuestion { question = "Do you feel like vomiting or queasy?" });
        questions.Add(new SSQQuestion { question = "Are you struggling to stay mentally focused?" });
        questions.Add(new SSQQuestion { question = "Does your head feel heavy or pressurized?" });
        questions.Add(new SSQQuestion { question = "Do you experience temporary blurred vision?" });
        questions.Add(new SSQQuestion { question = "Do you feel dizzy while keeping your eyes open?" });
        questions.Add(new SSQQuestion { question = "Do you feel dizzy when closing your eyes?" });
        questions.Add(new SSQQuestion { question = "Do you feel like the environment is spinning?" });
        questions.Add(new SSQQuestion { question = "Do you feel a sense of unease in your stomach?" });
        questions.Add(new SSQQuestion { question = "Have you noticed increased burping or belching?" });
    }

    void ShowNextQuestion()
    {
        if (currentQuestionIndex < questions.Count)
        {
            questionText.text = questions[currentQuestionIndex].question;
            answerDropdown.value = 0; // Reset dropdown to default option
        }
        else
        {
            FinishSurvey();
        }
    }

    void SubmitAnswer()
    {
        int selectedValue = answerDropdown.value;
        responses.Add(selectedValue);

        currentQuestionIndex++;
        ShowNextQuestion();
    }

    void FinishSurvey()
    {
        Debug.Log("SSQ Responses: " + string.Join(", ", responses));
        questionText.text = "Survey Completed!";
        submitButton.interactable = false;
        answerDropdown.gameObject.SetActive(false);
    }
}
