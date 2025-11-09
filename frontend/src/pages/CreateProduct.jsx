import { useState } from "react";
import api from "../api/axios";

export default function CreateProduct({ onCreated }) {
  const [name, setName] = useState("");
  const [price, setPrice] = useState("");
  const [stock, setStock] = useState("");
  const [image, setImage] = useState(null);
  const [loading, setLoading] = useState(false);
  const [error, setError] = useState("");

  async function handleSubmit(e) {
    e.preventDefault();
    setLoading(true);
    setError("");
    try {
      const formData = new FormData();
      formData.append("Name", name);
      formData.append("Price", price);
      formData.append("Stock", stock);
      if (image) formData.append("Image", image);

      await api.post("/Products", formData, {
        headers: { "Content-Type": "multipart/form-data" },
      });
      onCreated();
    } catch (err) {
      setError(err?.response?.data?.message || "Error al crear producto");
    } finally {
      setLoading(false);
    }
  }

  return (
    <div className="max-w-lg mx-auto bg-white p-8 rounded shadow">
      <h2 className="text-2xl font-bold text-mafe mb-6">Crear producto</h2>
      <form className="space-y-4" onSubmit={handleSubmit}>
        <input
          className="w-full border rounded px-3 py-2"
          placeholder="Nombre"
          value={name}
          onChange={e => setName(e.target.value)}
          required
        />
        <input
          className="w-full border rounded px-3 py-2"
          placeholder="Precio"
          type="number"
          step="any"
          value={price}
          onChange={e => setPrice(e.target.value)}
          required
        />
        <input
          className="w-full border rounded px-3 py-2"
          placeholder="Stock"
          type="number"
          value={stock}
          onChange={e => setStock(e.target.value)}
          required
        />
        <input
          className="w-full border rounded px-3 py-2"
          type="file"
          accept="image/*"
          onChange={e => setImage(e.target.files[0])}
        />
        {error && <p className="text-red-500">{error}</p>}
        <button
          type="submit"
          className="bg-mafe text-white px-6 py-2 rounded w-full disabled:opacity-50"
          disabled={loading}
        >
          {loading ? "Creando..." : "Crear producto"}
        </button>
      </form>
    </div>
  );
}
