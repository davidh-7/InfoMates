using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ProductoUI : MonoBehaviour
{
    public TMP_Text nombreTexto;      // Nombre del producto
    public TMP_Text precioTexto;      // Precio del producto
    public Image imagenProducto;      // Imagen del producto
    public Button botonCompra;        // Bot�n de compra
    public Button botonAgregarQuitar; // Bot�n para agregar o quitar del inventario

    private Producto producto;        // Referencia al producto
    private TiendaManager tiendaManager; // Referencia al manager de la tienda

    // Este m�todo configura los elementos de la UI con los datos del producto
    public void ConfigurarProducto(Producto producto, TiendaManager tienda)
    {
        this.producto = producto;
        this.tiendaManager = tienda;

        // Asignamos los valores del producto a los elementos de la UI
        nombreTexto.text = producto.nombre;
        precioTexto.text = producto.precio + " monedas";
        imagenProducto.sprite = producto.imagen;

        // Configuraci�n del bot�n de compra
        botonCompra.onClick.RemoveAllListeners();
        botonCompra.onClick.AddListener(() => ComprarProducto());

        // Configuraci�n del bot�n de agregar/quitar
        botonAgregarQuitar.onClick.RemoveAllListeners();
        botonAgregarQuitar.onClick.AddListener(() => AgregarOQuitarDelInventario());
    }

    // M�todo que se llama cuando el jugador hace clic en el bot�n de compra
    void ComprarProducto()
    {
        tiendaManager.ComprarProducto(producto);
    }

    // M�todo que agrega o quita el producto del inventario seg�n su estado
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
