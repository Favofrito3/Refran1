using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class FormManager : MonoBehaviour
{
    public InputField playerNameInput;
    public Dropdown difficultyDropdown;

    public void StartGame()
    {
        string playerName = playerNameInput.text;
        string selectedDifficulty = difficultyDropdown.options[difficultyDropdown.value].text;

        // Verificar si se ha ingresado un nombre de jugador válido
        if (!string.IsNullOrEmpty(playerName))
        {
            PlayerPrefs.SetString("PlayerName", playerName);
            PlayerPrefs.SetString("SelectedDifficulty", selectedDifficulty);
            SceneManager.LoadScene("GameInit");
        }
        else
        {
            // Aquí puedes mostrar un mensaje de error o realizar alguna otra acción
            Debug.LogError("Debes ingresar un nombre de jugador válido.");
        }
    }
}
