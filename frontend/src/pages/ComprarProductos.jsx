import { useEffect, useState } from "react";
import api from "../api/axios";

export default function ComprarProductos() {
  const [products, setProducts] = useState([]);
  const [quantities, setQuantities] = useState({});
  const [loading, setLoading] = useState(true);
  const [msg, setMsg] = useState("");
  const [err, setErr] = useState("");
  const [buyingId, setBuyingId] = useState(null);

  function loadProducts() {
    setLoading(true);
    api.get("/Products").then(r => {
      setProducts(r.data);
      setLoading(false);
    });
  }

  useEffect(() => {
    loadProducts();
  }, []);

  async function handleBuy(product) {
    setMsg(""); setErr("");
    setBuyingId(product.id);
    try {
      await api.post("/Sales", {
        items: [{ productId: product.id, quantity: Number(quantities[product.id] || 1) }]
      });
      setMsg(`Compra exitosa: ${product.name}`);
      loadProducts();
    } catch(e) {
      setErr(e?.response?.data?.message || "Error al comprar");
    }
    setBuyingId(null);
  }

  function handleQty(id, value) {
    setQuantities(q => ({...q, [id]: value}));
  }

  return (
    <div>
      <h2 className="text-2xl font-bold text-mafe mb-4">Comprar productos</h2>
      {msg && <div className="mb-2 text-green-600">{msg}</div>}
      {err && <div className="mb-2 text-red-600">{err}</div>}
      {loading ? <div>Cargando...</div> :
      <table className="min-w-full bg-white rounded shadow">
        <thead>
          <tr>
            <th className="px-3 py-2">Imagen</th>
            <th className="px-3 py-2">Nombre</th>
            <th className="px-3 py-2">Precio</th>
            <th className="px-3 py-2">Stock</th>
            <th className="px-3 py-2">Cantidad</th>
            <th className="px-3 py-2">Acción</th>
          </tr>
        </thead>
        <tbody>
          {products.map(p => (
            <tr key={p.id} className="border-t">
              <td className="px-2 py-2 text-center">
                {p.imageUrl && <img src={p.imageUrl} alt={p.name} className="w-16 h-16 object-contain rounded mx-auto" />}
              </td>
              <td className="px-2 py-2">{p.name}</td>
              <td className="px-2 py-2">${p.price}</td>
              <td className="px-2 py-2">{p.stock}</td>
              <td className="px-2 py-2">
                <input type="number" min={1} max={p.stock} value={quantities[p.id] || 1} onChange={e=>handleQty(p.id, e.target.value)} disabled={p.stock<1} className="border px-2 py-1 rounded w-16"/>
              </td>
              <td className="px-2 py-2">
                <button disabled={buyingId===p.id||p.stock<1} className="bg-mafe text-white px-3 py-1 rounded disabled:opacity-50"
                  onClick={()=>handleBuy(p)}>
                  {buyingId===p.id?"Comprando...":"Comprar"}
                </button>
              </td>
            </tr>
          ))}
        </tbody>
      </table>}
    </div>
  );
}
