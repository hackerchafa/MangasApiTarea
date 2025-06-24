// ======================= IMPORTACIONES =======================

// Importa React y hooks para manejar estado y efectos
import React, { useEffect, useState } from 'react';

// Importa el logo de React (imagen SVG)
import logo from './logo.svg';

// Importa estilos desde App.css
import './App.css';


// ======================== COMPONENTE =========================

// Función principal del componente App
function App() {
  // Estado para guardar los mangas recibidos desde la API
  const [mangas, setMangas] = useState([]);

  // Estado para mostrar errores (si los hay)
  const [error, setError] = useState('');

  // Hook useEffect: se ejecuta una sola vez al cargar el componente
  useEffect(() => {
    // Hace una petición GET al backend (.NET API)
    fetch('https://localhost:7040/api/manga')
      .then(res => {
        // Si la respuesta no es exitosa (ej. 404, 500), lanza error
        if (!res.ok) throw new Error('Error en la petición');
        return res.json(); // Convierte la respuesta en JSON
      })
      .then(data => {
        console.log('Respuesta del backend:', data); // Muestra datos en consola
        setMangas(data); // Guarda los mangas en el estado
      })
      .catch(err => {
        // Si ocurre error (conexión, CORS, etc), guarda mensaje
        setError(err.message);
        console.error('Error en fetch:', err);
      });
  }, []); // [] indica que solo se ejecuta una vez (al montar el componente)


  // ===================== RENDER DEL COMPONENTE ================
  return (
    <div className="App">
      {/* Cabecera con logo y enlaces de React */}
      <header className="App-header">
        <img src={logo} className="App-logo" alt="logo" />
        <p>
          Edit <code>src/App.js</code> and save to reload.
        </p>
        <a
          className="App-link"
          href="https://reactjs.org"
          target="_blank"
          rel="noopener noreferrer"
        >
          Learn React
        </a>
      </header>

      {/* Sección principal donde se muestran los mangas */}
      <main style={{
  backgroundColor: 'white',
  padding: '2rem',
  textAlign: 'left',
  color: 'black'
}}>
  <h1>Lista de Mangas</h1>
  {error && <p style={{ color: 'red' }}>Error: {error}</p>}
  <ul>
    {mangas.map((m) => (
      <li key={m.id}>
        <strong>{m.titulo}</strong> — {m.autor} ({m.genero})
      </li>
    ))}
  </ul>
</main>

    </div>
  );
}

// Exporta el componente App para que pueda ser usado en index.js
export default App;
