import { useEffect, useState } from "react";
import api from "../api/axios";

export default function AdminProducts({ onEdit }) {
  const [products, setProducts] = useState([]);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState("");

  useEffect(() => {
    fetchProducts();
  }, []);

  function fetchProducts() {
    setLoading(true);
    api.get("/Products")
      .then(res => setProducts(res.data))
      .catch(() => setError("Error al cargar productos"))
      .finally(() => setLoading(false));
  }

  async function handleDelete(id) {
    if (!window.confirm("¿Seguro que deseas eliminar este producto?")) return;
    try {
      await api.delete(`/Products/${id}`);
      fetchProducts();
    } catch (e) {
      alert("No se pudo eliminar");
    }
  }

  return (
    <div className="">
      <h2 className="text-xl font-bold text-mafe mb-6">Gestionar productos</h2>
      {loading ? (
        <div>Cargando...</div>
      ) : error ? (
        <div className="text-red-500">{error}</div>
      ) : (
        <table className="min-w-full bg-white border rounded shadow">
          <thead>
            <tr>
              <th className="px-3 py-2">Imagen</th>
              <th className="px-3 py-2">Nombre</th>
              <th className="px-3 py-2">Precio</th>
              <th className="px-3 py-2">Stock</th>
              <th className="px-3 py-2">Acciones</th>
            </tr>
          </thead>
          <tbody>
            {products.map(product => (
              <tr key={product.id} className="border-t">
                <td className="px-2 py-2">
                  {product.imageUrl && <img src={product.imageUrl} alt={product.name} className="w-16 h-16 object-contain rounded" />}
                </td>
                <td className="px-2 py-2">{product.name}</td>
                <td className="px-2 py-2">${product.price}</td>
                <td className="px-2 py-2">{product.stock}</td>
                <td className="px-2 py-2">
                  <button className="bg-yellow-400 px-3 py-1 rounded mr-2" onClick={() => onEdit(product)}>Editar</button>
                  <button className="bg-red-500 text-white px-3 py-1 rounded" onClick={() => handleDelete(product.id)}>Eliminar</button>
                </td>
              </tr>
            ))}
          </tbody>
        </table>
      )}
    </div>
  );
}
