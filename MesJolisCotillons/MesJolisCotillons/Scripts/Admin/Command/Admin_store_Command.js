Ext.ns('Mjc.Admin.store.Command');

//#region Mjc.Admin.store.Command.CommandTree_Set'
Ext.define('Mjc.Admin.store.Command.CommandTree_Set', {
    extend: 'Ext.data.TreeStore',
    constructor: function (cfg) {
        var me = this;
        cfg = cfg || {};

        me.callParent([Ext.apply({
            autoLoad: true,
            buffered: false,
            remoteFilter: true,
            proxy: {
                type: 'direct',
                directFn: Mjc.direct.CommandDirect.getCommandTree_Set,
                reader: {
                    type: 'json',
                    //rootProperty: 'children',
                    //totalProperty: 'total'
                }
            }
        }, cfg)]);
    }
});
//#endregion

//#region Mjc.Admin.store.Command_Set'
Ext.define('Mjc.Admin.store.Command_Set', {
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
                directFn: Mjc.direct.CommandDirect.getCommand_Set,
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

//#region Mjc.Admin.store.CommandProduct_Set'
Ext.define('Mjc.Admin.store.CommandProduct_Set', {
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
                directFn: Mjc.direct.CommandDirect.getCommandProduct_Set,
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

//#region Mjc.Admin.store.CommandHistory_Set'
Ext.define('Mjc.Admin.store.CommandHistory_Set', {
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
                directFn: Mjc.direct.CommandDirect.getCommand_History_Set,
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


//#region Mjc.Admin.store.Command.CommandDocument_Set
Ext.define('Mjc.Admin.store.Command.CommandDocument_Set', {
    extend: 'Ext.data.Store',
    constructor: function (cfg) {
        var me = this;
        cfg = cfg || {};
        me.callParent([Ext.apply({
            model: 'Mjc.Admin.Document.model.Document',
            autoLoad: false,
            remoteFilter: true,
            proxy: {
                type: 'direct',
                directFn: Mjc.direct.CommandDirect.getCommandDocument_Set,
                reader: {
                    type: 'json',
                    rootProperty: 'data'
                }
            }
        }, cfg)]);
    }
});
//#endregion
