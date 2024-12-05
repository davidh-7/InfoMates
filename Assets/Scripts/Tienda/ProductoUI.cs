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
    public TMP_Text cantidadTexto; // Cantidad del producto
    public Producto producto;        // Referencia al producto
    private TiendaManager tiendaManager; // Referencia al manager de la tienda
    public Button botonInfo; // Botón para mostrar la información del producto
    public GameObject miniPanel; // Panel que muestra la descripción del producto
    public TMP_Text descripcionTexto; // Texto dentro del mini panel

    // Este método configura los elementos de la UI con los datos del producto
    public void ConfigurarProducto(Producto producto, TiendaManager tienda)
    {
        this.producto = producto;
        this.tiendaManager = tienda;

        nombreTexto.text = producto.nombre;
        precioTexto.text = producto.precio + " monedas";
        imagenProducto.sprite = producto.imagen;

        // Configuración de la descripción
        descripcionTexto.text = producto.descripcion;

        // Configuración del botón de información
        botonInfo.onClick.RemoveAllListeners();
        botonInfo.onClick.AddListener(() => ToggleMiniPanel());

        // Asegúrate de que el panel esté oculto al inicio
        miniPanel.SetActive(false);

        // Otros botones
        botonCompra.onClick.RemoveAllListeners();
        botonCompra.onClick.AddListener(() => ComprarProducto());

        botonAgregarQuitar.onClick.RemoveAllListeners();
        botonAgregarQuitar.onClick.AddListener(() => AgregarOQuitarDelInventario());

        ActualizarCantidad(tiendaManager.ObtenerCantidadProducto(producto));
        ActualizarTextoBoton();
    }

    // Método para mostrar/ocultar el panel
   void ToggleMiniPanel()
    {
        // Activa o desactiva el mini panel
        miniPanel.SetActive(!miniPanel.activeSelf);
    }






// Método que se llama cuando el jugador hace clic en el botón de compra
void ComprarProducto()
    {
        tiendaManager.ComprarProducto(producto);

        // Actualiza la cantidad en la interfaz después de la compra
        ActualizarCantidad(tiendaManager.ObtenerCantidadProducto(producto));
    
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

        // Actualizamos el texto del botón después de realizar la acción
        ActualizarTextoBoton();
    }
    void ActualizarTextoBoton()
    {
        // Si el producto está en el inventario, cambiamos el texto a "Quitar del Inventario"
        if (tiendaManager.InventarioContieneProducto(producto))
        {
            botonAgregarQuitar.GetComponentInChildren<TMP_Text>().text = "Quitar";
        }
        else
        {
            botonAgregarQuitar.GetComponentInChildren<TMP_Text>().text = "Agregar";
        }
    }

    public void ActualizarCantidad(int cantidad)
    {
        cantidadTexto.text = "x" + cantidad;
    }



}