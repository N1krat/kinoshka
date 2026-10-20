async function loadFilms() {
    const container = document.getElementById("films-list");
    if (!container) return;

    try {
        const response = await fetch("/api/films", { method: "GET" });
        if (!response.ok) throw new Error(`HTTP error: ${response.status}`);

        const films = await response.json();

        container.innerHTML = films.map(film => `
  <article class="film-card">
    <div class="film-poster">
      <img src="/${film.poster}" alt="${film.title}" loading="lazy"
           onerror="this.style.display='none'">
      <span class="film-badge">${film.genre}</span>
    </div>
    <div class="film-info">
      <h2>${film.title}</h2>
      <p class="film-meta">${film.year} · ${film.duration} min</p>
      <a href="/Home/Movie/${film.id}" class="film-btn">Detalii</a>
    </div>
  </article>
`).join("");
    } catch (error) {
        console.error("Could not load films:", error);
        container.textContent = "Failed to load films. Please try again later.";
    }
}

document.addEventListener("DOMContentLoaded", loadFilms);