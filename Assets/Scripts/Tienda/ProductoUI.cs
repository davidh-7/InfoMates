using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ProductoUI : MonoBehaviour
{
    public TMP_Text nombreTexto;      // Nombre del producto
    public TMP_Text precioTexto;      // Precio del producto
    public Image imagenProducto;      // Imagen del producto
    public Button botonCompra;        // Botón de compra
    public Button botonAgregarQuitar; // Botón para agregar o quitar del inventario

    private Producto producto;        // Referencia al producto
    private TiendaManager tiendaManager; // Referencia al manager de la tienda

    // Este método configura los elementos de la UI con los datos del producto
    public void ConfigurarProducto(Producto producto, TiendaManager tienda)
    {
        this.producto = producto;
        this.tiendaManager = tienda;

        // Asignamos los valores del producto a los elementos de la UI
        nombreTexto.text = producto.nombre;
        precioTexto.text = producto.precio + " monedas";
        imagenProducto.sprite = producto.imagen;

        // Configuración del botón de compra
        botonCompra.onClick.RemoveAllListeners();
        botonCompra.onClick.AddListener(() => ComprarProducto());

        // Configuración del botón de agregar/quitar
        botonAgregarQuitar.onClick.RemoveAllListeners();
        botonAgregarQuitar.onClick.AddListener(() => AgregarOQuitarDelInventario());
    }

    // Método que se llama cuando el jugador hace clic en el botón de compra
    void ComprarProducto()
    {
        tiendaManager.ComprarProducto(producto);
    }

    // Método que agrega o quita el producto del inventario según su estado
    void AgregarOQuitarDelInventario()
    {
        if (tiendaManager.InventarioContieneProducto(producto))
        {
            tiendaManager.QuitarProductoDelInventario(producto);
        }
        else
        {
            tiendaManager.AgregarProductoAlInventario(producto);
        }
    }
}
