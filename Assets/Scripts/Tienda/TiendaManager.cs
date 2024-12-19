using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;
using TMPro;

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
               
                img.sprite = producto.imagen; // Asignamos la imagen del producto
            }

            // Añadimos el producto al inventario y guardamos la referencia visual
            inventario.Add(producto);
            visualInventario[producto] = productoInventario; // Asociamos el producto con su GameObject

            // Inicializamos la cantidad en 0 si no se ha comprado todavía
            if (!contadorInventario.ContainsKey(producto))
            {
                contadorInventario[producto] = 0; // Inicializamos la cantidad en 0
            }

            // Actualizamos la cantidad visual en el inventario
            ActualizarInventarioVisual(producto);
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
        // Verificar si el jugador tiene suficientes monedas
        if (DatosGlobales.monedero >= producto.precio)
        {
            // Descontar las monedas del jugador
            DatosGlobales.monedero -= producto.precio;

            // Si el producto no está en el inventario, se agrega con la cantidad 1
            if (!contadorInventario.ContainsKey(producto))
            {
                contadorInventario[producto] = 1; // Inicializamos en 1

            }
            else
            {
                contadorInventario[producto]++; // Incrementamos la cantidad
            }

            // Actualizamos la cantidad visual en el inventario
            ActualizarInventarioVisual(producto);

            Debug.Log("Compraste el producto: " + producto.nombre);
        }
        else
        {
            Debug.LogWarning("No tienes suficientes monedas para comprar este producto.");
        }
    }



    public int ObtenerCantidadProducto(Producto producto)
    {
        return contadorInventario.ContainsKey(producto) ? contadorInventario[producto] : 0;
    }

    // Este método actualiza la cantidad visual en el inventario
    public void ActualizarInventarioVisual(Producto producto)
    {
        if (visualInventario.ContainsKey(producto))
        {
            // Aquí deberías encontrar el componente de texto (por ejemplo, TMP_Text)
            // y actualizar la cantidad visualmente en el inventario
            TMP_Text cantidadTexto = visualInventario[producto].GetComponentInChildren<TMP_Text>();
            if (cantidadTexto != null)
            {
                cantidadTexto.text = "x" + contadorInventario[producto].ToString(); // Actualiza la cantidad
            }
            else
            {
                Debug.LogError("El prefab del inventario no tiene un componente TMP_Text para la cantidad.");
            }
        }
     
        
    }

}