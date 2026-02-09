(function () {
    const el = document.getElementById("system-info");
    if (!el) return;

    fetch("/api/SystemApi/GetSystemInfo")
        .then(async response => {
            if (!response.ok) {
                const errorText = await response.text();
                throw new Error(errorText || "Unable to load system info");
            }
            return response.json();
        })
        .then(data => {
            el.style.color = "white";
            el.textContent =
                `${data.ApplicationName} · v${data.Version} · ${data.BuildDate} · ${data.ServerInfo.FriendlyName}`;
        })
        .catch(err => {
            el.style.color = "red";
            el.textContent = err.message;
        });
})();
