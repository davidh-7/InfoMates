using UnityEngine;

[System.Serializable]
public class Producto
{
    public string nombre;
    public int precio;
    public Sprite imagen;
    [TextArea] // Esto permitirá escribir descripciones largas en el Inspector
    public string descripcion;
}
