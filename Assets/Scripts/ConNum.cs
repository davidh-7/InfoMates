using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; // Necesario para usar el componente Image

public class ConNum : MonoBehaviour
{
    public Sprite[] posiblesNumeros = new Sprite[10]; // Array de sprites que representan los n�meros del 1 al 9
    private int NumSprite; // Variable que almacena el n�mero representado por el sprite

    void Start()
    {
        // Obtiene el componente Image en lugar de SpriteRenderer
        Image img = GetComponent<Image>();
        if (img == null)
        {
            Debug.LogError("No se encontr� un componente Image en este objeto.");
            return;
        }

        // Genera un �ndice aleatorio entre 0 y 9 para seleccionar un sprite del array
        System.Random aleatorio = new System.Random();
        NumSprite = aleatorio.Next(0, posiblesNumeros.Length);

        // Asigna el sprite correspondiente al n�mero generado
        img.sprite = posiblesNumeros[NumSprite];

        // Ajusta el valor de NumSprite para que represente el n�mero visual en pantalla (0 a 9)
        NumSprite += 0;
    }

    // M�todo para obtener el valor num�rico del sprite actual
    public int ObtenerNumeroSprite()
    {
        return NumSprite;
    }
}
