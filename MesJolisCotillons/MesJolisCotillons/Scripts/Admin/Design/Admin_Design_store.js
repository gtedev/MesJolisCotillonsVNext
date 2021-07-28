Ext.ns('Mjc.Admin.Design.store');
Ext.ns('Mjc.Admin.Design.model');

//#region Mjc.Admin.Design.store.DesignConfig_Set'
Ext.define('Mjc.Admin.Design.store.DesignConfig_Set', {
    extend: 'Ext.data.Store',
    constructor: function (cfg) {
        var me = this;
        cfg = cfg || {};
        me.callParent([Ext.apply({
            autoLoad: true,
            buffered: false,
            remoteFilter: true,
            proxy: {
                type: 'direct',
                directFn: Mjc.direct.DesignDirect.getDesign_Config_Set,
                reader: {
                    type: 'json',
                    rootProperty: 'data',
                    totalProperty: 'total'
                }
            }
        }, cfg)]);
    }
});
//#endregion

//#region Mjc.Admin.store.Design.Design_ConfigStore  model/store

Ext.define('Mjc.Admin.Design.model.DesignConfig_Node', {
    extend: 'Ext.data.Model',
    fields: [
        { name: 'text', type: 'string' },
        { name: 'name', type: 'string' },
        { name: 'iconCls', type: 'string' },
        { name: 'expanded', type: 'boolean' }
    ]
});

Ext.define('Mjc.Admin.store.Design.Design_ConfigStore', {
    extend: 'Ext.data.Store',
    constructor: function (cfg) {
        var me = this;
        cfg = cfg || {};
        me.callParent([Ext.apply({
            model: 'Mjc.Admin.Design.model.DesignConfig_Node',
            autoLoad: true,
            buffered: false,
            remoteFilter: true,
            proxy: {
                type: 'direct',
                directFn: Mjc.direct.DesignDirect.getDesign_ConfigTree,
                reader: {
                    type: 'json',
                    rootProperty: 'data'
                }
            }
        }, cfg)]);
    }
});
//#endregion 

//#region Mjc.Admin.Design.store.DesignMenuItemProperty_Set - model/store
Ext.define('Mjc.Admin.Design.model.DesignMenuItemProperty', {
    extend: 'Ext.data.Model',
});

Ext.define('Mjc.Admin.Design.store.DesignMenuItemProperty_Set', {
    extend: 'Ext.data.Store',
    constructor: function (cfg) {
        var me = this;
        cfg = cfg || {};
        me.callParent([Ext.apply({
            model: 'Mjc.Admin.Design.model.DesignMenuItemProperty',
            autoLoad: true,
            buffered: false,
            remoteFilter: true,
            proxy: {
                type: 'direct',
                directFn: Mjc.direct.DesignDirect.getDesignMenuItemProperty_Set,
                reader: {
                    type: 'json',
                    rootProperty: 'data'
                }
            }
        }, cfg)]);
    }
});
//#endregion 

//#region Mjc.Admin.Design.store.MenuItem_Set'
Ext.define('Mjc.Admin.Design.store.MenuItem_Set', {
    extend: 'Ext.data.Store',
    constructor: function (cfg) {
        var me = this;
        cfg = cfg || {};
        me.callParent([Ext.apply({
            autoLoad: true,
            buffered: false,
            remoteFilter: true,
            proxy: {
                type: 'direct',
                directFn: Mjc.direct.DesignDirect.getMenuItem_Set,
                reader: {
                    type: 'json',
                    rootProperty: 'data',
                    totalProperty: 'total'
                }
            }
        }, cfg)]);
    }
});
//#endregion

//#region Mjc.Admin.Design.store.Etiquette_Set'
Ext.define('Mjc.Admin.Design.store.Etiquette_Set', {
    extend: 'Ext.data.Store',
    constructor: function (cfg) {
        var me = this;
        cfg = cfg || {};
        me.callParent([Ext.apply({
            autoLoad: true,
            buffered: false,
            remoteFilter: true,
            proxy: {
                type: 'direct',
                directFn: Mjc.direct.DesignDirect.getEtiquette_Set,
                reader: {
                    type: 'json',
                    rootProperty: 'data',
                    totalProperty: 'total'
                }
            }
        }, cfg)]);
    }
});
//#endregion

//#region Mjc.Admin.Design.store.CarouselImage_Set'
Ext.define('Mjc.Admin.Design.store.CarouselImage_Set', {
    extend: 'Ext.data.Store',
    constructor: function (cfg) {
        var me = this;
        cfg = cfg || {};
        me.callParent([Ext.apply({
            autoLoad: true,
            buffered: false,
            remoteFilter: true,
            proxy: {
                type: 'direct',
                directFn: Mjc.direct.DesignDirect.getCarouselImage_Set,
                reader: {
                    type: 'json',
                    rootProperty: 'data',
                    totalProperty: 'total'
                }
            }
        }, cfg)]);
    }
});
//#endregion