Ext.Direct.on('exception', function (e) {

    //Check for Authenticate failure during an Ext.Direct call, send 'em to the login page to fix the session
    try {
        //HTTP Status
        var status = e.xhr.status;

        if (status == 403) {
            //Delay just to let them read the message
            window.setTimeout(function () {
                var msgErrorExpirationSession = "Votre session a expirée !! <br/><br/> L'application va être redirigée...";
                Mjc.Admin.util.noty.displayResultMessage(false, msgErrorExpirationSession);
                window.location.href = "/Admin";
            }, 1500);
            return;
        }
    } catch (e) { }
});