document.getElementById("login-form").addEventListener("submit", async (e) => {
    e.preventDefault();
    const error = document.getElementById("login-error");
    error.textContent = "";

    try {
        const response = await fetch("/api/auth/login", {
            method: "POST",
            headers: { "Content-Type": "application/json" },
            body: JSON.stringify({
                email: document.getElementById("Email").value,
                password: document.getElementById("Password").value
            })
        });

        const data = await response.json();

        if (!response.ok) {
            error.textContent = data.message || "Eroare la autentificare.";
            return;
        }

        window.location.href = data.redirect;
    } catch (err) {
        console.error(err);
        error.textContent = "Serverul nu răspunde.";
    }
});