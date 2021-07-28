Ext.ns('Mjc.Admin.User.store');

//#region Mjc.Admin.User.store.User_Set'
Ext.define('Mjc.Admin.User.store.User_Set', {
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
                directFn: Mjc.direct.UserDirect.getUser_Set,
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

