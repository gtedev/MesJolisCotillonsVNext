Ext.ns('Mjc.Admin.app');

Ext.application({
    name: 'MesJolisCotillons',
    launch: function () {

        var delayedTask = new Ext.util.DelayedTask(function () {

            Mjc.Admin.app.viewPort = Ext.create('Mjc.Admin.ux.MainViewPort', {
                listeners: {
                    afterrender: function () {
                        $("html").removeClass("maskLoading");
                    }
                }
            });
        }, this, {}, false);

        delayedTask.delay(1500); // In order to allow loadingMask to animate a bit :D
    }
});

//Mjc.Admin.app.addTabPanelConfig = function (panelUx) {
//    Mjc.Admin.app.viewPort.addPanelToCenterPanel(panelUx);
//};

Mjc.Admin.app.addTabPanelConfig = function (panelUx, config) {

    if (!config.itemId) Ext.Error.raise('missing parameter "itemId"');
    //if (!config.title) Ext.Error.raise('missing parameter "title"');

    Ext.applyIf(config, {
        urlParams: {},
        closable: true,
        border: 0
    });

    var tab = Mjc.Admin.app.viewPort.getCenterTabPanel().getComponent(config.itemId);

    if (!tab) {
        tab = Ext.create(panelUx, config);
        Mjc.Admin.app.viewPort.getCenterTabPanel().add(tab)
    }

    Mjc.Admin.app.viewPort.getCenterTabPanel().setActiveTab(tab);

    return tab;
};