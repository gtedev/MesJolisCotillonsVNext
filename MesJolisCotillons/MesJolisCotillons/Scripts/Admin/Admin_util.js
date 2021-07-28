Ext.ns('Mjc.Admin.util');
Ext.ns('Mjc.Admin.util.noty');


Mjc.Admin.util.Icon = function (icon, iconsBase) {
    return Mjc.Icon(icon, iconsBase);
}

Mjc.Admin.util.decodeHTML = function (html) {
    return $('<div>').html(html).text();
}

Mjc.Admin.util.noty.bubbleRightMessage = function (text, type) {

    noty({
        text: text,
        type: type,
        //dismissQueue: true,
        layout: 'topRight',
        //closeWith: ['click'],
        timeout: 2000, // delay for closing event. Set false for sticky notifications
        killer: true, // for close all notifications before show
        theme: 'relax',
        maxVisible: 3,
        animation: {
            open: 'animated bounceInRight',
            close: 'animated bounceOutUp',
            easing: 'swing',
            speed: 500
        }
    });
};

Mjc.Admin.util.noty.errorCenterMessage = function (text) {

    noty({
        text: text,
        type: 'error',
        //dismissQueue: true,
        layout: 'center',
        closeWith: ['click'],
        timeout: 10000, // delay for closing event. Set false for sticky notifications
        killer: true, // for close all notifications before show
        theme: 'relax',
        maxVisible: 3,
        animation: {
            open: 'animated flipInX',
            close: 'animated flipOutX',
            easing: 'swing',
            speed: 500
        }
    });
};

Mjc.Admin.util.info = function (title, msg, icon) {
    Ext.create('widget.uxNotification', {
        position: 'tr',
        useXAxis: false,
        cls: 'ux-notification-light',
        iconCls: (icon ? TP.util.Icon(icon) : null) || Mjc.Admin.util.Icon('information-balloon'),
        width: 300,
        manager: 'fullscreen',
        useXAxis: false,
        closable: true,
        title: title || '&nbsp',
        html: msg || 'No information available',
        slideInDuration: 800,
        slideBackDuration: 1500,
        autoCloseDelay: 1000,
        slideInAnimation: 'elasticIn',
        slideBackAnimation: 'elasticIn',
        stickOnClick: true,
        stickWhileHover: true
    }).show();
}

Mjc.Admin.util.MessageBox = Ext.create('Ext.window.MessageBox', {
    buttonText: {
        yes: 'Oui',
        no: 'Non'
    }
});

Mjc.Admin.util.noty.displayResultMessage = function (isSuccess, msg) {

    if (isSuccess) {
        Mjc.Admin.util.noty.bubbleRightMessage(msg, 'success');
    } else {
        Mjc.Admin.util.noty.errorCenterMessage(msg);
    }
}

Mjc.Admin.util.downloadFile = function (url, params) {
    method = 'POST',
    params = params || {};

    // Create form panel. It contains a basic form that we need for the file download.
    var form = Ext.create('Ext.form.Panel', {
        standardSubmit: true,
        url: url,
        method: method
    });

    // Call the submit to begin the file download.
    form.submit({
        target: 'download', // point to an hidden iFrame, avoids leaving the page.
        params: params
    });

    // Clean-up the form after 100 milliseconds.
    // Once the submit is called, the browser does not care anymore with the form object.
    Ext.defer(function () {
        form.close();
    }, 100);
};