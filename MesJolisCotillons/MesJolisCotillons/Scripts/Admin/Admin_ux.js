Ext.ns('Mjc.Admin.ux');


Ext.define('Mjc.Admin.ux.Header', {
    extend: 'Ext.Toolbar',
    header: false,
    layout: {
        type: 'hbox',
        align: 'center'
    },
    height: 110,
    initComponent: function () {

        this.iconImg = Ext.create('Ext.Img', {
            src: '/Content/Images/Logo1902_SITE.png',
            margin: '0 0 0 30',
            height: 100,
            width: 100
        });
        this.textContainer = Ext.create('Ext.container.Container', {
            header: false,
            html: '<div class="bigTitle centuryFont"><span class="greenTitle">A</span>dministration<span class="greenTitle">*</span></div>',
            margin: '0 0 0 10',
            width: 200
        });
        this.items = [
            this.iconImg,
            this.textContainer,
            '->',
            {
                xtype: 'button',
                text: Mjc.Admin.util.decodeHTML(Mjc.Admin.Const.FullName),
                iconCls: Mjc.Admin.util.Icon('user-shape-yellow'),
                handler: function () {

                }
            },
            {
                xtype: 'button',
                text: 'Se déconnecter',
                margin: '5 50 5 5',
                iconCls: Mjc.Admin.util.Icon('power-button_red'),
                handler: function () {

                    Mjc.Admin.util.MessageBox.confirm('Confirmation', 'Voulez-vous vraiment vous déconnecter ?', function (btn) {
                        if (btn === 'yes') {
                            Mjc.direct.AdminDirect.SignOut(function (result, action) {
                                //if (result.success) {
                                //    window.location.href = "/Admin";
                                //}
                                window.location.href = "/Admin";

                            }, this);
                        }
                    }, this);

                }
            }];
        this.callParent(arguments);
    },
    bodyCls: 'adminHeaderCls',
    cls: 'adminHeaderCls'
});

Ext.define('Mjc.Admin.ux.CenterTabPanel', {
    extend: 'Ext.tab.Panel',
    listeners: {
        scoper: this,
        afterrender: function () {
            //this.setActiveItem(0);
            this.setActiveItem(this.commandManagement);
        }
    },
    initComponent: function () {

        this.productManagement = Ext.create('Mjc.Admin.ux.ProductManagement.MainPanel');
        this.designManagement = Ext.create('Mjc.Admin.Design.ux.MainPanel');
        this.commandManagement = Ext.create('Mjc.Admin.ux.Command.MainPanel');
        this.userManagement = Ext.create('Mjc.Admin.ux.User.MainPanel');

            

        this.items = [this.userManagement, this.productManagement, this.designManagement, this.commandManagement];
        //this.items = [this.productManagement];

        this.callParent(arguments);
    }
});

Ext.define('Mjc.Admin.ux.MainViewPort', {
    extend: 'Ext.container.Viewport',
    layout: 'border',
    initComponent: function () {

        this.header = Ext.create('Mjc.Admin.ux.Header', {
            title: 'MesJolisCotillonsHeader',
            header: false,
            region: 'north'
        });
        this.centerTabPanel = Ext.create('Mjc.Admin.ux.CenterTabPanel', {
            region: 'center'
        });
        this.items = [this.header, this.centerTabPanel];

        this.callParent(arguments);
    },
    addPanelToCenterPanel: function (panel) {
        this.centerTabPanel.add(panel);
        this.centerTabPanel.setActiveItem(panel);
    },
    getCenterTabPanel: function () {
        return this.centerTabPanel;
    }
});
