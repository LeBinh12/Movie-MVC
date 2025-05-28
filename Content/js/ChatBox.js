function toggleChat() {
    const chatBox = document.getElementById("chat-box");
    chatBox.style.display = chatBox.style.display === "none" ? "flex" : "none";
}

function appendMessage(sender, message) {
    const chat = document.getElementById("chat-content");
    const div = document.createElement("div");
    div.innerHTML = `<strong>${sender}:</strong> ${message}`;
    chat.appendChild(div);
    chat.scrollTop = chat.scrollHeight;
}

function sendMessageChatBox() {
    const input = document.getElementById("chat-input");
    const message = input.value.trim();
    if (message === '') return;

    appendMessage("Bạn", message);
    input.value = "";
    console.log("/ChatBox/GetMovieSuggestionFromGemini");
    $.ajax({
        url: '/ChatBox/GetMovieSuggestionFromGPT',
        method: 'POST',
        data: { userMessage: message },
        success: function (response) {
            appendMessage("Bot", response);
        },
        error: function () {
            appendMessage("Bot", "Xin lỗi, không thể phản hồi lúc này.");
        }
    });
}










