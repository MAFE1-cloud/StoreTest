import React from "react";

export default function Sidebar({ onSelect, onLogout }) {
  return (
    <aside className="w-60 h-screen bg-mafe/90 text-white flex flex-col items-center py-8 shadow-lg">
      <div className="font-bold text-2xl mb-10">Admin</div>
      <nav className="flex-1 flex flex-col gap-6 items-center w-full">
        <button onClick={() => onSelect("comprar")} className="w-full text-center hover:bg-white/10 rounded-lg py-2 transition">Comprar productos</button>
        <button onClick={() => onSelect("adminproductos")} className="w-full text-center hover:bg-white/10 rounded-lg py-2 transition">Gestionar productos</button>
        <button onClick={() => onSelect("crear")} className="w-full text-center hover:bg-white/10 rounded-lg py-2 transition">Crear producto</button>
        <button onClick={() => onSelect("reportes")} className="w-full text-center hover:bg-white/10 rounded-lg py-2 transition mt-8">Reportes</button>
      </nav>
      <button
        className="bg-white text-mafe font-semibold px-4 py-2 rounded-lg mt-12 hover:bg-gray-100 transition"
        onClick={onLogout}
      >
        Cerrar sesión
      </button>
    </aside>
  );
}
