var connection = new signalR.HubConnectionBuilder().withUrl("/chatHub").build();

connection.start();

$(document).on("click", "#sendButton", function (e) {
    let user = $("#userInput").val();
    let message = $("#messageInput").val();

    connection.invoke("SendMessage", user, message)
})

connection.on("ReceiveMessage", function (user, message) {
    var li = document.createElement("li");
    document.getElementById("messagesList").appendChild(li);
    li.textContent = `${user}: ${message}`;
})
