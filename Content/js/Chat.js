const chatHub = $.connection.chatHub;

 
chatHub.client.receiveMessage = function (sender, message) {
    console.log(`Tin nhắn từ ${sender}: ${message}`);
    const chatBox = document.getElementById(sender === "user" ? "adminMessages" : "userMessages");
    if (chatBox) {
        chatBox.innerHTML += `<div style="text-align:left"><b>${sender}:</b> ${message}</div>`;
        chatBox.scrollTop = chatBox.scrollHeight;
    }
};

$.connection.hub.start().done(function () {
    console.log("Đã kết nối tới ChatHub");

    // Xác định vai trò dựa trên trang hiện tại
    const role = window.location.href.includes("ManagerMovie") || window.location.href.includes("Admin") ? "Admin" : "User";
    chatHub.server.joinGroup(role).fail(function (err) {
        console.error("Lỗi khi tham gia nhóm:", err);
    });
}).fail(function (err) {
    console.error("Lỗi kết nối SignalR:", err);
});

function sendMessage(sender) {
    const input = document.getElementById(sender + "Input");
    const msg = input.value.trim();
    if (msg !== "" && $.connection.hub.state === $.signalR.connectionState.connected) {
        const chatBox = document.getElementById(sender + "Messages");

        if (chatBox) {
            // Hiển thị tin nhắn trên giao diện người gửi
            chatBox.innerHTML += `<div style="text-align:right"><b>Bạn:</b> ${msg}</div>`;
            chatBox.scrollTop = chatBox.scrollHeight;

            // Gửi tin nhắn đến server SignalR
            if (sender === "user") {
                chatHub.server.sendToAdmin(sender, msg).fail(function (err) {
                    console.error("Lỗi khi gửi tin nhắn đến Admin:", err);
                });
            } else if (sender === "admin") {
                chatHub.server.sendToUser(sender, msg).fail(function (err) {
                    console.error("Lỗi khi gửi tin nhắn đến User:", err);
                });
            }

            input.value = "";
        }
    } else {
        console.warn("Kết nối chưa sẵn sàng hoặc tin nhắn trống.");
    }
}


function toggleChat() {
    const chatBox = document.getElementById("chat-box");
    if (chatBox) {
        if (chatBox.style.display === "none") {
            chatBox.style.display = "flex";
        } else {
            chatBox.style.display = "none";
        }
    }
}

function toggleUserChatBox() {
    const userChatBox = document.getElementById("userChatBox");
    userChatBox.style.display = userChatBox.style.display === "none" ? "flex" : "none";
}

// Đăng ký sự kiện click cho các nút chat
document.addEventListener("DOMContentLoaded", function () {
    const userChatBtn = document.getElementById("userChatBtn");
    if (userChatBtn) {
        userChatBtn.addEventListener("click", function () {
            document.getElementById("userChatBox").style.display = "flex";
        });
    }

    const adminChatBtn = document.getElementById("adminChatBtn");
    if (adminChatBtn) {
        adminChatBtn.addEventListener("click", function () {
            document.getElementById("adminChatBox").style.display = "flex";
        });
    }
});