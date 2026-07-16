document.addEventListener("DOMContentLoaded", () => {
    const uploadForm = document.getElementById("uploadForm");
    const csvFileInput = document.getElementById("csvFile");
    const uploadMessage = document.getElementById("uploadMessage");

    if (uploadForm) {
        uploadForm.addEventListener("submit", async (e) => {
            e.preventDefault();

            const file = csvFileInput.files[0];
            if (!file) {
                showMessage("Please select a file first.", "red");
                return;
            }

            const formData = new FormData();
            formData.append("file", file);

            try {
                showMessage("Uploading...", "orange");

                const response = await fetch("/api/contacts/upload", {
                    method: "POST",
                    body: formData
                });

                if (response.ok) {
                    showMessage("File uploaded and parsed successfully!", "green");
                    setTimeout(() => {
                        window.location.reload();
                    }, 1500);
                } else {
                    const errorText = await response.text();
                    showMessage(`Upload failed: ${errorText || response.statusText}`, "red");
                }
            } catch (error) {
                console.error("Error during upload:", error);
                showMessage("Connection error. Is backend running?", "red");
            }
        });
    }

    const contactsTable = document.getElementById("contactsTable");
    if (contactsTable) {
        contactsTable.addEventListener("click", async (e) => {

            if (e.target.classList.contains("delete-btn")) {
                const row = e.target.closest("tr");
                const id = row.getAttribute("data-id");

                if (confirm("Are you sure you want to delete this contact?")) {
                    try {
                        const response = await fetch(`/api/contacts/${id}`, {
                            method: "DELETE"
                        });

                        if (response.ok) {
                            row.remove();
                        } else {
                            alert("Failed to delete contact.");
                        }
                    } catch (err) {
                        console.error(err);
                    }
                }
            }

            if (e.target.classList.contains("edit-btn")) {
                const button = e.target;
                const row = button.closest("tr");
                const id = row.getAttribute("data-id");
                const isEditing = row.classList.contains("editing");

                if (!isEditing) {
                    row.classList.add("editing");
                    button.innerText = "Save";
                    button.style.backgroundColor = "#28a745";
                    button.style.color = "white";

                    row.querySelectorAll(".editable").forEach(td => {
                        const field = td.getAttribute("data-field");
                        const currentValue = td.innerText.trim();

                        if (field === "Married") {
                            const checkbox = td.querySelector("input[type='checkbox']");
                            checkbox.disabled = false;
                        } else if (field === "DateOfBirth") {
                            td.innerHTML = `<input type="date" value="${currentValue}" style="width: 90%;" />`;
                        } else {
                            td.innerHTML = `<input type="text" value="${currentValue}" style="width: 90%;" />`;
                        }
                    });
                } else {
                    const updatedData = {};
                    let isValid = true;

                    row.querySelectorAll(".editable").forEach(td => {
                        const field = td.getAttribute("data-field");

                        if (field === "Married") {
                            const checkbox = td.querySelector("input[type='checkbox']");
                            updatedData[field] = checkbox.checked;
                            checkbox.disabled = true;
                        } else {
                            const input = td.querySelector("input");
                            const value = input.value.trim();

                            if (!value && field === "Name") {
                                alert("Name cannot be empty");
                                isValid = false;
                                return;
                            }

                            updatedData[field] = value;
                        }
                    });

                    if (!isValid) return;

                    let formattedDate = null;
                    const rawDate = updatedData["DateOfBirth"];
                    if (rawDate) {
                        const parsedDate = new Date(rawDate);
                        if (!isNaN(parsedDate.getTime())) {
                            formattedDate = parsedDate.toISOString();
                        }
                    }

                    const payload = {
                        name: updatedData["Name"],
                        phone: updatedData["Phone"],
                        dateOfBirth: formattedDate,
                        married: !!updatedData["Married"],
                        salary: parseFloat(updatedData["Salary"]) || 0
                    };

                    try {
                        const response = await fetch(`/api/contacts/${id}`, {
                            method: "PUT",
                            headers: {
                                "Content-Type": "application/json"
                            },
                            body: JSON.stringify(payload)
                        });

                        if (response.ok) {
                            row.classList.remove("editing");
                            button.innerText = "Edit";
                            button.removeAttribute("style");

                            row.querySelectorAll(".editable").forEach(td => {
                                const field = td.getAttribute("data-field");
                                if (field !== "Married") {
                                    td.innerText = updatedData[field];
                                }
                            });
                        } else {
                            const errText = await response.text();
                            alert(`Update failed: ${errText}`);
                            window.location.reload();
                        }
                    } catch (err) {
                        console.error(err);
                        alert("Connection error during update.");
                    }
                }
            }
        });
    }

    function showMessage(text, color) {
        if (uploadMessage) {
            uploadMessage.innerText = text;
            uploadMessage.style.color = color;
        }
    }
});