import { useState } from "react";
import Products from "./Products";
import Sidebar from "../components/Sidebar";
import CreateProduct from "./CreateProduct";
import AdminProducts from "./AdminProducts";
import EditProduct from "./EditProduct";
import Reportes from "./Reportes";
import ComprarProductos from "./ComprarProductos";
import api from "../api/axios";

export default function Dashboard({ onLogout }) {
  const [view, setView] = useState("productos");
  const [editingProduct, setEditingProduct] = useState(null);
  const [cart, setCart] = useState([]);
  const [cartSuccess, setCartSuccess] = useState("");
  const [cartError, setCartError] = useState("");
  const [buying, setBuying] = useState(false);

  function handleEdit(product) {
    setEditingProduct(product);
    setView("editar");
  }

  function handleAddToCart(product) {
    setCart(prev => {
      const idx = prev.findIndex(p => p.id === product.id);
      if (idx >= 0) {
        const next = [...prev];
        next[idx].quantity += product.quantity;
        return next;
      }
      return [...prev, product];
    });
  }

  async function handleBuy() {
    setBuying(true);
    setCartError("");
    setCartSuccess("");
    try {
      await api.post("/Sales", {
        items: cart.map(item => ({ productId: item.id, quantity: item.quantity }))
      });
      setCart([]);
      setCartSuccess("¡Compra realizada!");
    } catch(e) {
      setCartError("Error al registrar compra: " + (e?.response?.data?.message || ""));
    }
    setBuying(false);
  }

  function handleRemoveItem(id) {
    setCart(arr => arr.filter(p => p.id !== id));
  }

  return (
    <div className="min-h-screen bg-gray-50">
      <header className="bg-mafe text-white py-4 px-8 flex justify-between items-center shadow">
        <h1 className="text-2xl font-semibold">Store by Mafe 💜</h1>
        <button
          onClick={onLogout}
          className="bg-white text-mafe font-semibold px-4 py-2 rounded-lg hover:bg-gray-100 transition"
        >
          Cerrar sesión
        </button>
      </header>
      <main className="flex">
        <Sidebar onSelect={setView} onLogout={onLogout} />
        <div className="flex-1 p-8">
          {view === "productos" && <>
            <Products onAddToCart={handleAddToCart} />
            {cart.length > 0 && (
              <div className="fixed bottom-8 right-8 bg-white border p-6 rounded-xl shadow-xl max-w-md z-40">
                <h3 className="text-lg font-semibold mb-3">Tu carrito</h3>
                <ul className="divide-y max-h-48 overflow-y-auto">
                  {cart.map(item => (
                    <li key={item.id} className="py-2 flex items-center gap-2">
                      <span className="flex-1">{item.name} <span className="text-gray-500">x{item.quantity}</span></span>
                      <span className="text-mafe font-bold">${item.price * item.quantity}</span>
                      <button className="ml-3 text-red-500" onClick={() => handleRemoveItem(item.id)}>
                        Quitar
                      </button>
                    </li>
                  ))}
                </ul>
                <div className="font-bold mt-2">Total: ${cart.reduce((t, p) => t + p.price * p.quantity, 0)}</div>
                <button
                  className="mt-4 w-full bg-mafe text-white py-2 rounded disabled:opacity-60"
                  onClick={handleBuy}
                  disabled={buying}
                >
                  {buying ? "Procesando..." : "Comprar"}
                </button>
                {cartError && <div className="text-red-600 mt-2">{cartError}</div>}
                {cartSuccess && <div className="text-green-600 mt-2">{cartSuccess}</div>}
              </div>
            )}
          </>}
          {view === "crear" && <CreateProduct onCreated={() => setView("adminproductos")} />}
          {view === "adminproductos" && <AdminProducts onEdit={handleEdit} />}
          {view === "editar" && <EditProduct product={editingProduct} onUpdated={() => setView("adminproductos")} />}
          {view === "reportes" && <Reportes />}
          {view === "comprar" && <ComprarProductos />}
          {/* Aquí irá la pantalla de reportes en siguiente paso */}
        </div>
      </main>
    </div>
  );
}
