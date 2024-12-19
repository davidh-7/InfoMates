using UnityEngine;
using TMPro;

public class ConPapelera : MonoBehaviour
{
    public int maxUsos = 5; // Número máximo de veces que se puede usar la papelera
    private int usosRestantes;
    public TextMeshProUGUI textoUsosRestantes; // Campo de texto que muestra los usos restantes

    void Start()
    {
        // Inicializar los usos restantes con el máximo de usos
        usosRestantes = maxUsos;
        ActualizarTextoUsos();
    }

    // Método para reducir los usos restantes cuando se usa la papelera
    public bool UsarPapelera()
    {
        if (usosRestantes > 0)
        {
            usosRestantes--;
            ActualizarTextoUsos();
            return true; // Indica que la papelera se pudo usar
        }
        else
        {
            Debug.Log("No hay más usos disponibles para la papelera.");
            return false; // Indica que no hay más usos disponibles
        }
    }

    // Método para actualizar el campo de texto con los usos restantes
    private void ActualizarTextoUsos()
    {
        if (textoUsosRestantes != null)
        {
            textoUsosRestantes.text = "" + usosRestantes;
        }
    }
}
