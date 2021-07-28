Ext.ns('Mjc.Admin.store.ProductManagement');

//#region Mjc.Admin.store.ProductManagement.Product_Set'
Ext.define('Mjc.Admin.store.ProductManagement.Product_Set', {
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
                directFn: Mjc.direct.ProductDirect.getProduct_Set,
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

//#region Mjc.Admin.store.ProductManagement.ProductImages_Set'
Ext.define('Mjc.Admin.store.ProductManagement.ProductImages_Set', {
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
                directFn: Mjc.direct.ProductDirect.getProductImages_Set,
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

//#region Mjc.Admin.store.ProductManagement.Category_Set'
Ext.define('Mjc.Admin.store.ProductManagement.Category_Set', {
    extend: 'Ext.data.Store',
    constructor: function (cfg) {
        var me = this;
        cfg = cfg || {};
        me.callParent([Ext.apply({
            autoLoad: true,
            buffered: false,
            //remoteFilter: true,
            proxy: {
                type: 'direct',
                directFn: Mjc.direct.ProductDirect.getCategory_Set,
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

//#region Mjc.Admin.store.ProductManagement.ProductFlags_Set'
Ext.define('Mjc.Admin.store.ProductManagement.ProductFlags_Set', {
    extend: 'Ext.data.Store',
    constructor: function (cfg) {
        var me = this;
        cfg = cfg || {};
        me.callParent([Ext.apply({
            autoLoad: true,
            buffered: false,
            //remoteFilter: true,
            proxy: {
                type: 'direct',
                directFn: Mjc.direct.ProductDirect.getProductFlags_Set,
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

//#region Mjc.Admin.store.ProductManagement.ProductKeyWords_Set'
Ext.define('Mjc.Admin.store.ProductManagement.ProductKeyWords_Set', {
    extend: 'Ext.data.Store',
    constructor: function (cfg) {
        var me = this;
        cfg = cfg || {};
        me.callParent([Ext.apply({
            autoLoad: true,
            buffered: false,
            //remoteFilter: true,
            proxy: {
                type: 'direct',
                directFn: Mjc.direct.ProductDirect.getProductKeyWords_Set,
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

//#region Mjc.Admin.store.ProductManagement.KeyWord_Set'
Ext.define('Mjc.Admin.store.ProductManagement.KeyWord_Set', {
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
                directFn: Mjc.direct.ProductDirect.getKeyWord_Set,
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




//#region Mjc.Admin.store.ProductManagement.ProductType_Set'
Ext.define('Mjc.Admin.store.ProductManagement.ProductType_Set', {
    extend: 'Ext.data.Store',
    constructor: function (cfg) {
        var me = this;
        cfg = cfg || {};
        me.callParent([Ext.apply({
            autoLoad: true,
            buffered: false,
            //remoteFilter: true,
            proxy: {
                type: 'direct',
                directFn: Mjc.direct.ProductDirect.getProductType_Set,
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

//#region Mjc.Admin.store.ProductManagement.ProductFragility_Set'
Ext.define('Mjc.Admin.store.ProductManagement.ProductFragility_Set', {
    extend: 'Ext.data.Store',
    constructor: function (cfg) {
        var me = this;
        cfg = cfg || {};
        me.callParent([Ext.apply({
            autoLoad: true,
            buffered: false,
            //remoteFilter: true,
            proxy: {
                type: 'direct',
                directFn: Mjc.direct.ProductDirect.getProductFragility_Set,
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

//#region Mjc.Admin.store.ProductManagement.ProductDeliveryBoxType_Set'
Ext.define('Mjc.Admin.store.ProductManagement.ProductDeliveryBoxType_Set', {
    extend: 'Ext.data.Store',
    constructor: function (cfg) {
        var me = this;
        cfg = cfg || {};
        me.callParent([Ext.apply({
            autoLoad: true,
            buffered: false,
            //remoteFilter: true,
            proxy: {
                type: 'direct',
                directFn: Mjc.direct.ProductDirect.getProductDeliveryBoxType_Set,
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



