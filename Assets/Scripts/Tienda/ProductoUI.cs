using UnityEngine;
using UnityEngine.UI;
using TMPro; // Asegúrate de incluir esta línea

public class ProductoUI : MonoBehaviour
{
    public TMP_Text nombreTexto;      // Cambiado a TMP_Text
    public TMP_Text precioTexto;      // Cambiado a TMP_Text
    public Image imagenProducto;      // Imagen del producto
    public Button botonCompra;        // Botón de compra

    // Este método configura los elementos de la UI con los datos del producto
    public void ConfigurarProducto(Producto producto)
    {
        nombreTexto.text = producto.nombre;                 // Asigna el nombre
        precioTexto.text = producto.precio + " monedas";    // Asigna el precio
        imagenProducto.sprite = producto.imagen;            // Asigna la imagen del producto

        //Debug.Log("Compraste el producto: " + producto.nombre);



        // Configura el botón de compra para que ejecute la función de comprar
        botonCompra.onClick.RemoveAllListeners();
        botonCompra.onClick.AddListener(() => ComprarProducto(producto));
        //botonCompra.onClick.AddListener(() => Debug.Log("Hola"));
    }

    // Método que se llamará al hacer clic en el botón de compra
    public void ComprarProducto(Producto producto)
    {
        Debug.Log("Compraste el producto: " + producto.nombre);
    }
}
