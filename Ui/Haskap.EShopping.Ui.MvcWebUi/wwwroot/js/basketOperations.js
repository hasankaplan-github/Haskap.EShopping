// const basketHubConnection = new signalR.HubConnectionBuilder()
//     .withUrl("/basket-hub")
//     .configureLogging(signalR.LogLevel.Information)
//     .build();

// async function basketHubConnectionStart() {
//     try {
//         await basketHubConnection.start();
//         console.log("SignalR Connected.");

//         updateBasketItemCountNotify();
//     } catch (err) {
//         console.log(err);
//         setTimeout(basketHubConnectionStart, 5000);
//     }
// };

// basketHubConnection.onclose(async () => {
//     await basketHubConnectionStart();
// });

// // Start the connection.
// basketHubConnectionStart();

// basketHubConnection.on("updateBasketItemCountNotify", function () {
//     updateBasketItemCountNotify();
// });

function updateBasketItemCountNotify() {
    $.ajax({
        url: '/Basket/GetItemCount',
        type: 'GET',
        // headers: {
        //     'RequestVerificationToken': '@antiforgeryToken'
        // },
        // data: {
        //     name: name,
        //     isActive: isActive
        // },
    }).done(function (result, status, xhr) {
        if (result > 0) {
            $("#basketNotifyA").addClass("icon-header-noti").attr("data-notify", result);
            $("#basketNotifyMobileA").addClass("icon-header-noti").attr("data-notify", result);
        } else {
            $("#basketNotifyA").removeClass("icon-header-noti").attr("data-notify", 0);
            $("#basketNotifyMobileA").removeClass("icon-header-noti").attr("data-notify", 0);
        }
    });
}