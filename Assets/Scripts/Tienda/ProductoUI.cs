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
    public TMP_Text cantidadTexto; // Cantidad del producto
    public Producto producto;        // Referencia al producto
    private TiendaManager tiendaManager; // Referencia al manager de la tienda
    public Button botonInfo; // Bot�n para mostrar la informaci�n del producto
    public GameObject miniPanel; // Panel que muestra la descripci�n del producto
    public TMP_Text descripcionTexto; // Texto dentro del mini panel

    // Este m�todo configura los elementos de la UI con los datos del producto
    public void ConfigurarProducto(Producto producto, TiendaManager tienda)
    {
        this.producto = producto;
        this.tiendaManager = tienda;

        nombreTexto.text = producto.nombre;
        precioTexto.text = producto.precio + " monedas";
        imagenProducto.sprite = producto.imagen;

        // Configuraci�n de la descripci�n
        descripcionTexto.text = producto.descripcion;

        // Configuraci�n del bot�n de informaci�n
        botonInfo.onClick.RemoveAllListeners();
        botonInfo.onClick.AddListener(() => ToggleMiniPanel());

        // Aseg�rate de que el panel est� oculto al inicio
        miniPanel.SetActive(false);

        // Otros botones
        botonCompra.onClick.RemoveAllListeners();
        botonCompra.onClick.AddListener(() => ComprarProducto());

        botonAgregarQuitar.onClick.RemoveAllListeners();
        botonAgregarQuitar.onClick.AddListener(() => AgregarOQuitarDelInventario());

        ActualizarCantidad(tiendaManager.ObtenerCantidadProducto(producto));
        ActualizarTextoBoton();
    }

    // M�todo para mostrar/ocultar el panel
   void ToggleMiniPanel()
    {
        // Activa o desactiva el mini panel
        miniPanel.SetActive(!miniPanel.activeSelf);
    }






// M�todo que se llama cuando el jugador hace clic en el bot�n de compra
void ComprarProducto()
    {
        tiendaManager.ComprarProducto(producto);

        // Actualiza la cantidad en la interfaz despu�s de la compra
        ActualizarCantidad(tiendaManager.ObtenerCantidadProducto(producto));
    
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

        // Actualizamos el texto del bot�n despu�s de realizar la acci�n
        ActualizarTextoBoton();
    }
    void ActualizarTextoBoton()
    {
        // Si el producto est� en el inventario, cambiamos el texto a "Quitar del Inventario"
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