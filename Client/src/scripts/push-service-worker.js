self.importScripts("/src/scripts/push-notifications-controller.js");
self.importScripts("https://cdn.jsdelivr.net/npm/idb@4.0.5/build/iife/with-async-ittr-min.js");

const pushNotificationTitle = "Notification";
 let PustEnable = true;

// self.addEventListener('message', (event) => {
//   if (event.data && event.data.type === 'MESSAGE_IDENTIFIER') {
//     console.log("Message Receive: ",event.data)

//   }
// });


// self.addEventListener('activate', function (event) {

//   const tabs =  self.clients.matchAll({type: 'window'})
//   tabs.forEach((tab) => {
//      console.log(tab)
//     tab.navigate(tab.url)
//   })
  
//     setInterval(() => { 
//     console.log("WorkerRunning: ")

//     }, 5000);
// });



self.addEventListener("push", function (event) {
  let jsonObject = JSON.parse(event.data.text());
  console.log("JSon: ",jsonObject.Title)
    if(PustEnable == true){
        event.waitUntil(
            self.registration.showNotification(jsonObject.Title, {
              requireInteraction: true,
              body:jsonObject.Body,
              icon:jsonObject.Icon,
              data:jsonObject,
              actions:jsonObject.actions
            })
          );
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

self.addEventListener("notificationclick", function (event) {
  // event.stopImmediatePropagation();
  event.notification.close();
  var actionsType = event.notification.data.Type;
  var action = event.notification.data.Url;
  console.log("On notification click: ",action);

  if (actionsType == "openlink") {
    action.map((url) => {
      if (url) {
        //---------- for open link
        event.waitUntil(
          clients
            .matchAll({
              type: "window",
            })
            .then(function (clientList) {
              for (var i = 0; i < clientList.length; i++) {
                var client = clientList[i];
                if (client.url == url && "focus" in client)
                  return client.focus();
              }
              if (clients.openWindow) {
                clients
                  .openWindow(url)
                  .then((windowClient) =>
                    windowClient ? windowClient.focus() : null
                  );
              }
            })
        );
      }
    });
  } else if (actions == "API") {
    //---------- for fatch data  sample ---------------------------------------

  }
});
