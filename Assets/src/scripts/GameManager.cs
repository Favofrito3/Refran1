using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public Text sayingText;
    public Toggle[] answerToggles;
    public Button nextButton;
    public Text scoreText;
    public Text playerNameText;
    public GameObject feedbackPanel;
    public Text feedbackText;

    private List<Saying> sayings;
    private int currentSayingIndex = 0;
    private int score = 0;

    private string playerName;
    private string selectedDifficulty;

    private bool gameFinished = false;

    private void Start()
    {
        playerName = PlayerPrefs.GetString("PlayerName", "Jugador");
        selectedDifficulty = PlayerPrefs.GetString("SelectedDifficulty", "Facil");

        playerNameText.text = "Jugador: " + playerName;

        sayings = GetSayingsForDifficulty(selectedDifficulty);
        ShuffleSayings(); // Baraja los refranes al inicio.

        ShowSaying();
    }

    private void ShowFeedback(string message)
    {
        feedbackText.text = message;
        feedbackPanel.SetActive(true);
        StartCoroutine(HideFeedback());
    }

    private IEnumerator HideFeedback()
    {
        yield return new WaitForSeconds(1f); // Modificar los segundos
        feedbackPanel.SetActive(false);
    }


    public void NextSaying()
    {
        if (gameFinished)
        {
            return;
        }

        int selectedAnswerIndex = -1;
        for (int i = 0; i < answerToggles.Length; i++)
        {
            if (answerToggles[i].isOn)
            {
                selectedAnswerIndex = i;
                break;
            }
        }

        if (selectedAnswerIndex == -1)
        {
            // El usuario no ha seleccionado ninguna opción, muestra un mensaje de feedback.
            ShowFeedback("¡Por favor, selecciona una opción antes de continuar!");
            return;
        }

        if (selectedAnswerIndex == sayings[currentSayingIndex].CorrectAnswerIndex)
        {
            score++;
            ShowFeedback("Respuesta correcta");
        }
        else
        {
            ShowFeedback("Respuesta incorrecta");
        }

        foreach (Toggle toggle in answerToggles)
        {
            toggle.isOn = false;
        }

        // Importante
        currentSayingIndex++;
        if (currentSayingIndex < sayings.Count)
        {
            ShowSaying();
        }
        else
        {
            scoreText.text = "Puntuación: " + score;
            nextButton.GetComponentInChildren<Text>().text = "Cerrar juego";
            nextButton.onClick.RemoveAllListeners();
            nextButton.onClick.AddListener(CloseGame);
            gameFinished = true;
            sayingText.text = "Juego Terminado !!";

            if (answerToggles != null)
            {
                for (int i = 0; i < answerToggles.Length; i++)
                {
                    Destroy(answerToggles[i].gameObject);
                }
                answerToggles = null;
            }
        }
    }


    private void ShowSaying()
    {
        sayingText.text = sayings[currentSayingIndex].SayingText;

        for (int i = 0; i < answerToggles.Length; i++)
        {
            answerToggles[i].GetComponentInChildren<Text>().text = sayings[currentSayingIndex].Answers[i];
        }
    }

    // Refranes 'Modificable'
    private List<Saying> GetSayingsForDifficulty(string difficulty)
    {
        List<Saying> sayingsList = new List<Saying>();

        switch (difficulty)
        {
            case "Facil":
                // Frase - 4 respuestas - Respuesta con index desde 0 a 3
                sayingsList.Add(new Saying("Al que madruga Dios lo... ", new string[] { "enciende", "abraza", "ayuda", "escucha" }, 2));
                sayingsList.Add(new Saying("No por mucho madrugar amanece... ", new string[] { "más rápido", "mejor el día", "más temprano", "más claro" }, 2));
                sayingsList.Add(new Saying("Cría cuervos y te... ", new string[] { "te llevarán al cielo", "comerán los ojos", "serán tus amigos", "sacarán los ojos" }, 3));
                sayingsList.Add(new Saying("Más vale prevenir que... ", new string[] { "lamentar", "curar", "ignorar", "esperar" }, 0));
                break;
            case "Normal":
                sayingsList.Add(new Saying("No hay mal que por bien no... ", new string[] { "termine", "dure", "pase", "venga" }, 3));
                sayingsList.Add(new Saying("A caballo regalado no le mires el... ", new string[] { "cola", "cabello", "diente", "rabo" }, 2));
                sayingsList.Add(new Saying("El que nace pa'tamar, en el suelo no... ", new string[] { "reza", "cae", "anda", "camina" }, 0));
                sayingsList.Add(new Saying("Más vale tarde que... ", new string[] { "temprano", "siempre", "tarde", "nunca" }, 3));
                break;
            case "Dificil":
                sayingsList.Add(new Saying("El que mucho abarca, poco... ", new string[] { "aprieta", "logra", "consigue", "sufre" }, 0));
                sayingsList.Add(new Saying("Camarón que se duerme se lo... ", new string[] { "queda sin hambre", "duerme para siempre", "despierta temprano", "lleva la corriente" }, 3));
                sayingsList.Add(new Saying("No dejes para mañana lo que puedas... ", new string[] { "dejar para siempre", "hacer hoy", "hacer en otro momento", "olvidar" }, 1));
                sayingsList.Add(new Saying("En boca cerrada no entran... ", new string[] { "ideas", "palabras", "moscas", "sueños" }, 2));
                break;
        }

        return sayingsList;
    }

    private void ShuffleSayings()
    {
        List<int> randomIndexes = new List<int>();
        for (int i = 0; i < sayings.Count; i++)
        {
            randomIndexes.Add(i);
        }

        for (int i = 0; i < randomIndexes.Count; i++)
        {
            int temp = randomIndexes[i];
            int randomIndex = Random.Range(i, randomIndexes.Count);
            randomIndexes[i] = randomIndexes[randomIndex];
            randomIndexes[randomIndex] = temp;
        }

        List<Saying> shuffledSayings = new List<Saying>();
        foreach (int index in randomIndexes)
        {
            shuffledSayings.Add(sayings[index]);
        }

        sayings = shuffledSayings;
    }

    private void CloseGame()
    {
        SceneManager.LoadScene("Main");
    }

    [System.Serializable]
    public class Saying
    {
        public string SayingText;
        public string[] Answers;
        public int CorrectAnswerIndex;

        public Saying(string sayingText, string[] answers, int correctAnswerIndex)
        {
            SayingText = sayingText;
            Answers = answers;
            CorrectAnswerIndex = correctAnswerIndex;
        }
    }
}
