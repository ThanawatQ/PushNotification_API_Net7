

export const   PushNotifications  = {
    initialize,
};
    let applicationServerPublicKey;
    let pushServiceWorkerRegistration;
    let subscribeButton, unsubscribeButton;
    let topicInput, urgencySelect, notificationInput;

    function initialize(prop) {
        if (!('serviceWorker' in navigator)) {
            console.log('Service Workers are not supported');
            return;
        }
    
        if (!('PushManager' in window)) {
            console.log('Push API not supported');
            return;
        }
    
        registerPushServiceWorker(prop);
        updatePushServiceWorker();
    }


    function registerPushServiceWorker(prop) {
        navigator.serviceWorker.register('/src/scripts/push-service-worker.js', { scope: '/src/scripts/push-service-worker/' })
            .then(function (serviceWorkerRegistration) {
                pushServiceWorkerRegistration = serviceWorkerRegistration;

                initializeUIState(prop);

                console.log('Push Service Worker has been registered successfully');
            }).catch(function (error) {
                console.log('Push Service Worker registration has failed: ' + error);
            });
    }
    function updatePushServiceWorker() {
        navigator.serviceWorker.register('/src/scripts/push-service-worker.js', { scope: '/src/scripts/push-service-worker/' })
        .then(function (serviceWorkerRegistration) {

            serviceWorkerRegistration.update();


            console.log('Push Service Worker has been Updated successfully');
        }).catch(function (error) {
            console.log('Push Service Worker Updated has failed: ' + error);
        });
    }
    function initializeUIState(prop) {


        pushServiceWorkerRegistration.pushManager.getSubscription()
            .then(function (subscription) {
                changeUIState(Notification.permission === 'denied', subscription !== null);
            });

        subscribeForPushNotifications(prop);

    }

    function changeUIState(notificationsBlocked, isSubscibed) {
        subscribeButton.disabled = notificationsBlocked || isSubscibed;
        unsubscribeButton.disabled = notificationsBlocked || !isSubscibed;

        if (notificationsBlocked) {
            console.log('Permission for Push Notifications has been denied');
        }
    }

    function subscribeForPushNotifications(prop) {
        if (applicationServerPublicKey) {
            subscribeForPushNotificationsInternal(prop);
        } else {
            PushNotificationsController.retrievePublicKey()
                .then(function (retrievedPublicKey) {
                    applicationServerPublicKey = retrievedPublicKey;
                    console.log('Successfully retrieved Public Key');

                    subscribeForPushNotificationsInternal(prop);
                }).catch(function (error) {
                    console.log('Failed to retrieve Public Key: ' + error);
                });
        }
    }

    function subscribeForPushNotificationsInternal(prop) {
        pushServiceWorkerRegistration.pushManager.subscribe({
            userVisibleOnly: true,
            applicationServerKey: applicationServerPublicKey
        })
            .then(function (pushSubscription) {
                var newObject = {
                    subscription : pushSubscription,
                    Group : prop.Group,
                    UserId: prop.UserId
                };
                PushNotificationsController.storePushSubscription(newObject)
                    .then(function (response) {
                        if (response.ok) {
                            console.log('Successfully subscribed for Push Notifications');
                        } else {
                            console.log('Failed to store the Push Notifications subscrition on server');
                        }
                    }).catch(function (error) {
                        console.log('Failed to store the Push Notifications subscrition on server: ' + error);
                    });

                changeUIState(false, true);
            }).catch(function (error) {
                if (Notification.permission === 'denied') {
                    changeUIState(true, false);
                } else {
                    console.log('Failed to subscribe for Push Notifications: ' + error);
                }
            });
    }

    function unsubscribeFromPushNotifications() {
        pushServiceWorkerRegistration.pushManager.getSubscription()
            .then(function (pushSubscription) {
                if (pushSubscription) {
                    pushSubscription.unsubscribe()
                        .then(function () {
                            PushNotificationsController.discardPushSubscription(pushSubscription)
                                .then(function (response) {
                                    if (response.ok) {
                                        console.log('Successfully unsubscribed from Push Notifications');
                                    } else {
                                        console.log('Failed to discard the Push Notifications subscrition from server');
                                    }
                                }).catch(function (error) {
                                    console.log('Failed to discard the Push Notifications subscrition from server: ' + error);
                                });

                            changeUIState(false, false);
                        }).catch(function (error) {
                            console.log('Failed to unsubscribe from Push Notifications: ' + error);
                        });
                }
            });
    }

    function sendPushNotification() {
        let payload = { topic: topicInput.value, notification: notificationInput.value, urgency: urgencySelect.value, Title:"test"};
        console.log(JSON.stringify(payload))
        fetch('push-notifications-api/notifications', {
            method: 'POST',
            headers: { 'Content-Type': 'application/json' },
            body: JSON.stringify(payload)
        })
            .then(function (response) {
                if (response.ok) {
                    console.log('Successfully sent Push Notification');
                } else {
                    console.log('Failed to send Push Notification');
                }
            }).catch(function (error) {
                console.log('Failed to send Push Notification: ' + error);
            });
    }


    
    
