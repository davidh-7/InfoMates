using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;

public class TiendaManager : MonoBehaviour
{
    public Producto[] productos; // Array con los productos
    public GameObject prefabProductoUI; // Prefab de la UI de producto
    public GameObject prefabProductoInventario; // Prefab para mostrar los productos en el inventario
    public Transform contenedorProductos; // Contenedor donde se instanciarán los productos de la tienda
    public Transform contenedorInventario; // Contenedor donde se instanciarán los productos del inventario
    public List<Producto> inventario = new List<Producto>(); // Inventario del jugador
    private Dictionary<Producto, GameObject> visualInventario = new Dictionary<Producto, GameObject>();
    private Dictionary<Producto, int> contadorInventario = new Dictionary<Producto, int>();


    void Start()
    {
        MostrarProductos(); // Mostrar los productos al inicio
    }

    // Este método muestra todos los productos en la tienda
    void MostrarProductos()
    {
        foreach (Producto producto in productos)
        {
            // Instanciar un nuevo producto en la UI de la tienda
            GameObject productoUI = Instantiate(prefabProductoUI, contenedorProductos);

            // Configurar el producto con los datos del array de productos y la referencia a TiendaManager
            productoUI.GetComponent<ProductoUI>().ConfigurarProducto(producto, this);
        }
    }

    // Método para agregar un producto al inventario
    // Método que agrega el producto al inventario
    public void AgregarProductoAlInventario(Producto producto)
    {
        if (inventario.Count < 3) // Verificamos que el inventario no esté lleno
        {
            // Instanciamos el prefab en el contenedor del inventario
            GameObject productoInventario = Instantiate(prefabProductoInventario, contenedorInventario);
            Debug.Log("Prefab instanciado: " + productoInventario.name);

            // Buscar el componente Image en el prefab o sus hijos
            Image img = productoInventario.GetComponentInChildren<Image>();
            if (img != null)
            {
                Debug.Log("Componente Image encontrado en el prefab o un hijo.");
                img.sprite = producto.imagen; // Asignamos la imagen del producto
            }
            else
            {
                Debug.LogError("El prefab ProductoInventario no tiene un componente Image.");
            }

            // Añadimos el producto al inventario y guardamos la referencia visual
            inventario.Add(producto);
            visualInventario[producto] = productoInventario; // Asociamos el producto con su GameObject
        }
        else
        {
            Debug.Log("El inventario está lleno.");
        }
    }





    // Método para quitar un producto del inventario
    public void QuitarProductoDelInventario(Producto producto)
    {
        if (inventario.Contains(producto))
        {
            inventario.Remove(producto);

            // Buscar y destruir la instancia visual asociada al producto
            if (visualInventario.ContainsKey(producto))
            {
                Destroy(visualInventario[producto]); // Destruir el GameObject
                visualInventario.Remove(producto); // Eliminar la referencia del diccionario
            }

            Debug.Log("Producto quitado del inventario: " + producto.nombre);
        }
        else
        {
            Debug.LogWarning("El producto no está en el inventario.");
        }
    }



    // Método para comprobar si un producto ya está en el inventario
    public bool InventarioContieneProducto(Producto producto)
    {
        return inventario.Contains(producto);
    }

    // Método para manejar la compra del producto
    public void ComprarProducto(Producto producto)
    {
        Debug.Log("Compraste el producto: " + producto.nombre);
    }
}
