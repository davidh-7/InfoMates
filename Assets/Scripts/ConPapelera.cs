using UnityEngine;
using TMPro;

public class ConPapelera : MonoBehaviour
{
    public int maxUsos = 5; // N�mero m�ximo de veces que se puede usar la papelera
    private int usosRestantes;
    public TextMeshProUGUI textoUsosRestantes; // Campo de texto que muestra los usos restantes

    void Start()
    {
        // Inicializar los usos restantes con el m�ximo de usos
        usosRestantes = maxUsos;
        ActualizarTextoUsos();
    }

    // M�todo para reducir los usos restantes cuando se usa la papelera
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
            Debug.Log("No hay m�s usos disponibles para la papelera.");
            return false; // Indica que no hay m�s usos disponibles
        }
    }

    // M�todo para actualizar el campo de texto con los usos restantes
    private void ActualizarTextoUsos()
    {
        if (textoUsosRestantes != null)
        {
            textoUsosRestantes.text = "" + usosRestantes;
        }
    }
}
