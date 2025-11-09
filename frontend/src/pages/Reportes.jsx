import { useState } from "react";
import api from "../api/axios";

export default function Reportes() {
  const [desde, setDesde] = useState("");
  const [hasta, setHasta] = useState("");
  const [loading, setLoading] = useState(false);
  const [data, setData] = useState(null);
  const [error, setError] = useState("");

  async function handleBuscar(e) {
    e.preventDefault();
    setError(""); setData(null); setLoading(true);
    try {
      const res = await api.get(`/Sales/report?from=${desde}&to=${hasta}`);
      setData(res.data);
    } catch(e) {
      setError(e?.response?.data?.message || "Error al cargar reporte");
    }
    setLoading(false);
  }

  return (
    <div className="max-w-xl mx-auto p-6 bg-white rounded shadow">
      <h2 className="text-2xl font-bold mb-4 text-mafe">Reportes de Ventas</h2>
      <form className="flex gap-2 items-end mb-6" onSubmit={handleBuscar}>
        <div>
          <label className="text-gray-700 text-sm">Desde</label>
          <input type="date" value={desde} onChange={e=>setDesde(e.target.value)} required className="border rounded px-2 py-1 ml-2" />
        </div>
        <div>
          <label className="text-gray-700 text-sm">Hasta</label>
          <input type="date" value={hasta} onChange={e=>setHasta(e.target.value)} required className="border rounded px-2 py-1 ml-2" />
        </div>
        <button className="bg-mafe text-white px-4 py-2 rounded" disabled={loading}>Buscar</button>
      </form>
      {loading && <div>Cargando...</div>}
      {error && <div className="text-red-500">{error}</div>}
      {data && (
        <div>
          <div className="font-bold text-mafe mb-2">Total ventas: ${data.grandTotal}</div>
          <table className="min-w-full text-sm bg-white">
            <thead>
              <tr>
                <th className="px-2 py-1 text-left">Fecha</th>
                <th className="px-2 py-1 text-right">Total del día</th>
              </tr>
            </thead>
            <tbody>
              {data.rows.map(row => (
                <tr key={row.date}>
                  <td className="px-2 py-1">{new Date(row.date).toLocaleDateString()}</td>
                  <td className="px-2 py-1 text-right">${row.total.toFixed(2)}</td>
                </tr>
              ))}
            </tbody>
          </table>
        </div>
      )}
    </div>
  );
}
