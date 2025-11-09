import { useEffect, useState } from "react";
import api from "../api/axios";

export default function Products({ onAddToCart }) {
  const [products, setProducts] = useState([]);
  const [quantities, setQuantities] = useState({});

  useEffect(() => {
    api.get("/Products").then((res) => setProducts(res.data));
  }, []);

  function handleQuantityChange(id, qty) {
    setQuantities(q => ({ ...q, [id]: qty }));
  }

  return (
    <div>
      <h2 className="text-xl font-bold text-mafe mb-4">Productos</h2>
      <div className="grid grid-cols-1 md:grid-cols-3 gap-6">
        {products.map((p) => (
          <div
            key={p.id}
            className="bg-white shadow rounded-xl p-4 border border-gray-200 hover:shadow-lg transition flex flex-col items-center"
          >
            {p.imageUrl && (
              <img
                src={p.imageUrl}
                alt={p.name}
                className="w-32 h-32 object-contain rounded mb-2 border"
                onError={e => {e.target.style.display='none'}}
              />
            )}
            <h3 className="text-lg font-semibold text-center">{p.name}</h3>
            <p className="text-gray-600">${p.price}</p>
            <p className="text-sm text-gray-500">{p.stock} disponibles</p>
            <input type="number" min={1} max={p.stock} value={quantities[p.id] || 1} onChange={e => handleQuantityChange(p.id, e.target.value)} className="border px-2 py-1 rounded w-16 mt-2" />
            <button
              className="mt-2 bg-mafe text-white px-4 py-1 rounded w-full disabled:opacity-60"
              disabled={p.stock < 1}
              onClick={() => onAddToCart({ ...p, quantity: Number(quantities[p.id] || 1) })}
            >
              Agregar al carrito
            </button>
          </div>
        ))}
      </div>
    </div>
  );
}
