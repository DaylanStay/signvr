using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Controller2 : MonoBehaviour {
    public GameObject  game;
    public TextMeshProUGUI gameHitCountOut;
    public TextMeshProUGUI letterOut;
    public Image imageOut;

    public GameObject results;
    public TextMeshProUGUI resultsHitCountOut;

    private int hitCount = 0; 
    private bool showingResults = false;

    private Sign targetSign;

    // --- AÑADIDO ---
    // Referencia para comunicarse con el script LetterReceiver
    private LetterReceiver receiver;

    // La lógica de este método no cambia
    public void ChooseSign() {
        targetSign = State.data.signs[hitCount];
        imageOut.sprite = targetSign.sprite;
        letterOut.text  = targetSign.text.ToString();
    }

    void Start() {
        // --- AÑADIDO ---
        // Al iniciar, busca el componente LetterReceiver en la escena
        receiver = FindObjectOfType<LetterReceiver>();
        ChooseSign(); //
    }

    // --- MÉTODO MODIFICADO ---
    // Se reemplazó la entrada de teclado por la lectura desde LetterReceiver
    void Update() {
        if ( showingResults ) { return; } //

        // Obtener la letra desde el receiver
        string receivedLetter = (receiver != null && !string.IsNullOrEmpty(receiver.Letter)) ? receiver.Letter : " ";

        // Validar si se recibió una letra (diferente al valor por defecto " ")
        if ( receivedLetter != " " ) {
            // Compara la letra recibida con la letra objetivo
            if ( targetSign.text.ToString().ToUpper() == receivedLetter ) {
                hitCount++; //
                
                // Comprueba si aún quedan señas por mostrar
                if ( hitCount < State.data.signs.Length ) {
                    ChooseSign(); //
                } else {
                    ShowResults(); //
                }
            }
            
            // Importante: Limpia la letra del receiver para procesarla solo una vez
            receiver.ClearLetter();
        }

        gameHitCountOut.text = hitCount.ToString(); //
    }

    // La lógica de este método no cambia
    public void Restart() {
        showingResults = false;

        game.SetActive(true);
        results.SetActive(false);

        hitCount = 0;
        ChooseSign();
    }

    // La lógica de este método no cambia
    public void ShowResults() {
        showingResults = true;

        game.SetActive(false);
        results.SetActive(true);

        resultsHitCountOut.text = hitCount.ToString();
    }
}