using UnityEngine;

public class TiendaManager : MonoBehaviour
{
    public Producto[] productos;              // Array con los productos
    public GameObject prefabProductoUI;       // Prefab de la UI de producto
    public Transform contenedorProductos;     // Contenedor donde se instanciarán los productos

    void Start()
    {
        MostrarProductos();  // Mostrar los productos al inicio
    }

    // Este método muestra todos los productos en la tienda
    void MostrarProductos()
    {
        foreach (Producto producto in productos)
        {
            // Instanciar un nuevo producto en la UI
            GameObject productoUI = Instantiate(prefabProductoUI, contenedorProductos);

            // Configurar el producto con los datos del array de productos
            productoUI.GetComponent<ProductoUI>().ConfigurarProducto(producto);
        }
    }
}
