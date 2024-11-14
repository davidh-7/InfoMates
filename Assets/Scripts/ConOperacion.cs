using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ConOperacion : MonoBehaviour
{
    // Variable para guardar la referencia del objeto de texto
    private TextMeshProUGUI NumJug;

    // Start is called before the first frame update
    void Start()
    {
        // Busca el objeto de texto y lo asigna a la variable textSuma
        NumJug = GameObject.Find("TxtResultado").GetComponent<TextMeshProUGUI>();

        //Debug.Log("Texto inicial en TxtResultado: " + NumJug.text);
    }

    // Update is called once per frame
    void Update()
    {
     
        //Debug.Log("Texto actual en TxtResultado: " + NumJug.text);
    }
}
