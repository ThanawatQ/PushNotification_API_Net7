self.importScripts("/src/scripts/push-notifications-controller.js");
self.importScripts(
  "https://cdn.jsdelivr.net/npm/idb@4.0.5/build/iife/with-async-ittr-min.js"
);

let PustEnable = true;
let preSerintercal = 0;
let timemer;
self.addEventListener("push", function (event) {
  let jsonObject = JSON.parse(event.data.text());

  if (PustEnable == true) {
    event.waitUntil(
      self.registration.showNotification(jsonObject.Title, {
        requireInteraction: true,
        body: jsonObject.Body,
        icon: jsonObject.Icon,
        data: jsonObject,
        actions: jsonObject.actions,
      })
    );
        //---------- logger
        let data = {
          Url: "",
          UserId: jsonObject.UserId,
          Response: "",
          ActionType: "",
          Status: "Received",
        };
        Logger(data);

        //--------- Check timeOut 
        CheckAction(jsonObject.UserId)
  }
});

self.addEventListener("pushsubscriptionchange", function (event) {
  const handlePushSubscriptionChangePromise = Promise.resolve();

  if (event.oldSubscription) {
    handlePushSubscriptionChangePromise =
      handlePushSubscriptionChangePromise.then(function () {
        return PushNotificationsController.discardPushSubscription(
          event.oldSubscription
        );
      });
  }

  if (event.newSubscription) {
    handlePushSubscriptionChangePromise =
      handlePushSubscriptionChangePromise.then(function () {
        return PushNotificationsController.storePushSubscription(
          event.newSubscription
        );
      });
  }

  if (!event.newSubscription) {
    handlePushSubscriptionChangePromise =
      handlePushSubscriptionChangePromise.then(function () {
        return PushNotificationsController.retrievePublicKey().then(function (
          applicationServerPublicKey
        ) {
          return pushServiceWorkerRegistration.pushManager
            .subscribe({
              userVisibleOnly: true,
              applicationServerKey: applicationServerPublicKey,
            })
            .then(function (pushSubscription) {
              return PushNotificationsController.storePushSubscription(
                pushSubscription
              );
            });
        });
      });
  }

  event.waitUntil(handlePushSubscriptionChangePromise);
});


self.addEventListener("notificationclose", (event) => {
  console.log("this clear Notification Event ");

});


self.addEventListener("notificationclick", function (event) {
  event.notification.close();
  var actionsType = event.notification.data.Type;
  var actionDetail = event.notification.data.actions;
  var userId = event.notification.data.UserId;
  var actionBtn = event.action;


  if (actionsType == "openlink") {
    actionDetail.map((item) => {
      if (actionBtn == "Link" && item.action == actionBtn) {
        event.waitUntil(
          clients
            .matchAll({
              type: "window",
            })
            .then(function (clientList) {
              for (var i = 0; i < clientList.length; i++) {
                var client = clientList[i];
                if (client.url == item.url && "focus" in client)
                  return client.focus();
              }
              if (clients.openWindow) {
                clients
                  .openWindow(item.url)
                  .then((windowClient) =>
                    windowClient ? windowClient.focus() : null
                  );
              }
            })
        );
      } else if (actionBtn == "API" && item.action == actionBtn) {
        //---------- post API someting

       //---------- logger
       clearInterval(timemer);
       
       let data = {
        Url: "",
        UserId: userId,
        Response: "",
        ActionType: actionBtn,
        Status: "Active-API",
      };
      console.log("btnName API: ", data);
      Logger(data);
      }
    });
  }
});

function Logger(obj) {
  const handlePushSubscriptionChangePromise = Promise.resolve();

  handlePushSubscriptionChangePromise.then(function () {
    return PushNotificationsController.updateAction(obj)
      .then((data) => {
        console.log(data);
      })
      .catch((err) => {
        console.log(err);
      });
  });
}

function CheckAction(UserId){
  preSerintercal = 0;

   timemer = setInterval(()=>{
   preSerintercal++;
   if(preSerintercal == 10){
       //---------- logger
       let data = {
        Url: "",
        UserId: UserId,
        Response: "",
        ActionType: "",
        Status: "TimeOut",
      };
      Logger(data);
      clearInterval(timemer);
   }
   console.log("Time...!")
  },1000)
}
