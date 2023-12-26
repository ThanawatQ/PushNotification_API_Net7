const PushNotificationsController = (function () {

    let url = "https://localhost:44373/"; // URL Push API

    function urlB64ToUint8Array(base64String) {
        const padding = '='.repeat((4 - base64String.length % 4) % 4);
        const base64 = (base64String + padding).replace(/\-/g, '+').replace(/_/g, '/');

        const rawData = window.atob(base64);
        const outputArray = new Uint8Array(rawData.length);

        for (let i = 0; i < rawData.length; ++i) {
            outputArray[i] = rawData.charCodeAt(i);
        }

        return outputArray;
    }

    return {
        retrievePublicKey: function () {
            return fetch(url+'api/Subscriptions/public-key').then(function (response) {
                if (response.ok) {
                    return response.text().then(function (applicationServerPublicKeyBase64) {
                        return urlB64ToUint8Array(applicationServerPublicKeyBase64);
                    });
                } else {
                    return Promise.reject(response.status + ' ' + response.statusText);
                }
            });
        },
        storePushSubscription: function (pushSubscription) {
            return fetch(url+'api/Subscriptions/subscription', {
                method: 'POST',
                headers: { 'Content-Type': 'application/json' },
                body: JSON.stringify(pushSubscription)
            });
        },
        discardPushSubscription: function (pushSubscription) {
            return fetch(url+'api/Subscriptions/subscription?endpoint=' + encodeURIComponent(pushSubscription.endpoint), {
                method: 'DELETE'
            });
        },
        updateAction: function (object){
            return fetch(url+'api/Subscriptions/push-action', {
                method: 'POST',
                headers: { 'Content-Type': 'application/json' },
                body: JSON.stringify(object)
            });
        }
    };
})();