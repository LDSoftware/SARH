const connection = new signalR.HubConnectionBuilder()
    .withUrl("/notificationHub")
    .configureLogging(signalR.LogLevel.Information)
    .build();

connection.serverTimeoutInMilliseconds = 100000;

connection.on('ReceiveMessage', (user, message, row) => {

    toastr.options = {
        "closeButton": false,
        "debug": false,
        "newestOnTop": true,
        "progressBar": true,
        "positionClass": "toast-top-right",
        "preventDuplicates": true,
        "onclick": null,
        "showDuration": 400,
        "hideDuration": 100,
        "timeOut": 5000,
        "extendedTimeOut": 1000,
        "showEasing": "swing",
        "hideEasing": "linear",
        "showMethod": "fadeIn",
        "hideMethod": "fadeOut"
    }

    toastr.success("Ha recibido un formato pendiente para aprobación, Actualize la pantalla para visualizar", "Sistema de notificaciones");
    $('#totalpendingsapprobedhead').text(row)


});

Object.defineProperty(WebSocket, 'OPEN', { value: 1, });

connection.start().catch(err => console.error(err.toString()));

// re-establish the connection if connection dropped
connection.onclose(() => setTimeout(startSignalRConnection(connection), 5000));


document.getElementById("sendbuttonNS").addEventListener("click", event => {
    var idformat = document.getElementById('IdFormatRequest').value;
    connection.invoke("SendMessage", "usuario", "oa", idformat).catch(err => console.error(err.toString()));
    event.preventDefault();
});