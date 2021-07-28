Ext.ns('Mjc.Admin.ux.Command');

//#region Mjc.Admin.ux.Command.MainPanel
Ext.define('Mjc.Admin.ux.Command.MainPanel', {
    extend: 'Ext.panel.Panel',
    layout: {
        type: 'hbox',
        align: 'stretch'
    },
    title: 'Commande',
    layout: {
        type: 'hbox',
        align: 'stretch'
    },
    initComponent: function () {

        var me = this;

        this.commandLeftPanel = Ext.create('Mjc.Admin.ux.Command.CommandLeftPanel', {
            flex: 0.3,
            listeners: {
                togglePrepareButton: function () {
                    me.commandGridPanel.getStore().addFilter({ property: 'Status', value: 200 });
                },
                toggleAwaitingPaymentButton: function () {
                    me.commandGridPanel.getStore().addFilter({ property: 'Status', value: 100 });
                },
                toggleExpiredPaymentButton: function () {
                    me.commandGridPanel.getStore().addFilter({ property: 'Status', value: -200 });
                },
                toggleDeclinedPaymentButton: function () {
                    me.commandGridPanel.getStore().addFilter({ property: 'Status', value: -300 });
                },
                toggleSentButton: function () {
                    me.commandGridPanel.getStore().addFilter({ property: 'Status', value: 300 });
                },
                toggleAllButton: function () {
                    me.commandGridPanel.getStore().addFilter({ property: 'Status', value: -1000 });
                },
            }
        });

        this.commandGridPanel = Ext.create('Mjc.Admin.ux.Command.CommandGridPanel', {
            flex: 0.7,
            autoLoad: false,
            listeners: {
                itemdblclick: function (grid, record, item, index, e, eOpts) {
                    me.openTabCommand(record);
                },
                openCommand: function (record) {
                    me.openTabCommand(record);
                }
            }
        });

        //this.commandGridPanel.getStore().addFilter({ property: 'Status', value: 200 });


        this.items = [this.commandLeftPanel, this.commandGridPanel];


        this.callParent(arguments);
    },
    openTabCommand: function (record) {

        Mjc.Admin.app.addTabPanelConfig('Mjc.Admin.ux.Command.CommandPanel', {
            title: 'Command N°' + record.data.Command_ID + ' | ' + record.data.CustomerFullName + ' | ' + record.data.Destination,
            Command_ID: record.data.Command_ID,
            itemId: 'Mjc_Admin_ux_Command_CommandPanel_' + record.data.Command_ID
        });
    }
});
//#endregion

//#region Mjc.Admin.ux.Command.CommandLeftPanel
Ext.define('Mjc.Admin.ux.Command.CommandLeftPanel', {
    extend: 'Ext.panel.Panel',
    margin: '0 0 0 1',
    header: false,
    title: 'Tree',
    allowClose: null,
    autoScroll: true,
    layout: {
        type: 'vbox',
        align: 'stretch'
    },
    hidden: false,
    border: true,
    defaults: {
        //enableToggle: true,
        //toggleGroup: 'FilterCommandButton',
        //allowDepress: false
    },
    initComponent: function () {

        var me = this;

        this.treePanel = Ext.create('Ext.tree.Panel', {
            rootVisible: false,
            flex: 0.6,
            store: Ext.create('Mjc.Admin.store.Command.CommandTree_Set', {
                autoLoad: true
            })
        });

        this.prepareButton = Ext.create('Mjc.Admin.ux.Command.CommandFilterButton', {
            text: 'A préparer',
            listeners: {
                toggle: function (button, pressed, eOpts) {
                    if (pressed) {
                        me.fireEvent('togglePrepareButton');
                    }

                }
            }
        });
        //}).toggle(true);

        this.awaitingPaymentButton = Ext.create('Mjc.Admin.ux.Command.CommandFilterButton', {
            text: '<span class="' + Mjc.Admin.util.Icon('clock') + ' inlineIcon">Attente(s) paiement</span>',
            listeners: {
                toggle: function (button, pressed, eOpts) {
                    if (pressed) {
                        me.fireEvent('toggleAwaitingPaymentButton');
                    }
                }
            }
        });

        this.expiredPaymentButton = Ext.create('Mjc.Admin.ux.Command.CommandFilterButton', {
            text: '<span class="' + Mjc.Admin.util.Icon('Time-warn') + ' inlineIcon">Expirée(s)</span>',
            listeners: {
                toggle: function (button, pressed, eOpts) {
                    if (pressed) {
                        me.fireEvent('toggleExpiredPaymentButton');
                    }
                }
            }
        });

        this.declinedPaymentButton = Ext.create('Mjc.Admin.ux.Command.CommandFilterButton', {
            text: '<span class="' + Mjc.Admin.util.Icon('close_red') + ' inlineIcon">Déclinées(s)</span>',
            listeners: {
                toggle: function (button, pressed, eOpts) {
                    if (pressed) {
                        me.fireEvent('toggleDeclinedPaymentButton');
                    }
                }
            }
        });
        this.shipmentDoneButton = Ext.create('Mjc.Admin.ux.Command.CommandFilterButton', {
            text: '<span class="' + Mjc.Admin.util.Icon('arrow_large_right') + ' inlineIcon">Envoyé(s)</span>',
            listeners: {
                toggle: function (button, pressed, eOpts) {
                    if (pressed) {
                        me.fireEvent('toggleSentButton');
                    }
                }
            }
        });

        this.allButton = Ext.create('Mjc.Admin.ux.Command.CommandFilterButton', {
            text: 'Tout',
            listeners: {
                toggle: function (button, pressed, eOpts) {
                    if (pressed) {
                        me.fireEvent('toggleAllButton');
                    }
                }
            }
        });

        this.items = [this.treePanel,
        this.prepareButton,
        this.awaitingPaymentButton,
        this.expiredPaymentButton,
        this.declinedPaymentButton,
        this.shipmentDoneButton,
        this.allButton];

        this.listeners = {
            afterrender: function () {
                //console.log('afterender');
                me.prepareButton.toggle(true);
            }
        },

        this.callParent(arguments);
    }
});
//#endregion

//#region Mjc.Admin.ux.Command.CommandGridPanel
Ext.define('Mjc.Admin.ux.Command.CommandGridPanel', {
    extend: 'Ext.grid.Panel',
    margin: '0 0 0 1',
    title: 'Commandes',
    header: false,
    allowClose: null,
    autoScroll: true,
    hidden: false,
    border: true,
    flex: 0.6,
    initComponent: function () {

        var me = this;

        this.columns = [{
            text: 'NuméroCommande',
            dataIndex: 'Command_ID',
            flex: 0.1
        },
        {
            text: 'Date',
            dataIndex: 'CreationDateTime',
            flex: 0.2
        },
       {
           text: 'Client',
           dataIndex: 'CustomerFullName',
           flex: 0.2
       },
       {
           text: 'Destination',
           dataIndex: 'Destination',
           flex: 0.3
       },
       {
           text: 'Montant',
           dataIndex: 'TotalPrice',
           flex: 0.1
       },
       {
           text: 'Statut',
           dataIndex: 'Status',
           flex: 0.2,
           renderer: function (val, meta, record) {

               switch (record.data.StatusInt) {
                   case 100:
                       meta.tdCls = "awaitingPaymentCls";
                       break;
                   case 200:
                       meta.tdCls = "paymentSuccessCls";
                       break;
                   case 300:
                       meta.tdCls = "deliveredCls";
                       break;
                   case -100:
                       meta.tdCls = "disableCls";
                       break;
                   case -200:
                       meta.tdCls = "expiredCls";
                       break;
                   case -300:
                       meta.tdCls = "declinedCls";
                       break;
               }
               meta.tdAttr = 'data-qtip="' + val + '"';
               return val;
           }
       },
       {
           xtype: 'widgetcolumn',
           text: 'Action',
           resizable: false,
           draggable: false,
           flex: 0.3,
           scope: this,
           widget: {
               grid: this,
               xtype: 'button',
               iconCls: Mjc.Admin.util.Icon('cog-wheel-grey'),
               scope: this
           },
           onWidgetAttach: function (column, widget, record) {

               var menu = [
               {
                   text: 'Ouvrir la Commande...',
                   iconCls: Mjc.Admin.util.Icon('tabs'),
                   handler: function () {
                       me.fireEvent('openCommand', record)
                   }
               }];


               widget.setMenu(menu);
           }
       }];

        this.store = Ext.create('Mjc.Admin.store.Command_Set', {
            autoLoad: false
        });

        this.dockedItems = [
        {
            xtype: 'pagingtoolbar',
            store: this.store,
            dock: 'bottom',
            displayInfo: true,
        }];

        this.callParent(arguments);
    }
});
//#endregion

//#region Mjc.Admin.ux.Command.CommandFilterButton
Ext.define("Mjc.Admin.ux.Command.CommandFilterButton", {
    extend: 'Ext.Button',
    constructor: function (cfg) {

        var me = this;
        cfg = cfg || {};

        me.callParent([Ext.apply({
            enableToggle: true,
            toggleGroup: 'FilterCommandButton',
            allowDepress: false,
            width: '100%',
            padding: 15,
            border: false,
            cls: 'greyBackground',
            overCls: 'filterButtonCommandClsOver'
        }, cfg)]);
    }

});
//#endregion

//#region Mjc.Admin.ux.Command.CommandActionButton
Ext.define("Mjc.Admin.ux.Command.CommandActionButton", {
    extend: 'Ext.Button',
    constructor: function (cfg) {

        var me = this;
        cfg = cfg || {};

        me.callParent([Ext.apply({
            enableToggle: true,
            toggleGroup: 'ActionCommandButton',
            allowDepress: false,
            width: '100%',
            padding: 15,
            border: false,
            cls: 'greyBackground',
            overCls: 'filterButtonCommandClsOver'
        }, cfg)]);
    }
});
//#endregion

//#region Mjc.Admin.ux.Command.CommandPanel
Ext.define('Mjc.Admin.ux.Command.CommandPanel', {
    extend: 'Ext.tab.Panel',
    Command_ID: null,
    initComponent: function () {

        var me = this;

        this.mainPanel = Ext.create('Mjc.Admin.ux.Command.CommandWorkspacePanel', {
            title: 'La Commande',
            autoLoad: true,
            modelId: this.Command_ID,
            Command_ID: this.Command_ID,
            flex: 0.3
        });

        this.historyPanel = Ext.create('Mjc.Admin.ux.Command.CommandHistoryGridPanel', {
            Command_ID: this.Command_ID,
            title: 'Historique',
            flex: 0.3
        });

        //this.documentPanel = Ext.create('Mjc.Admin.ux.Command.CommandDocumentPanel', {
        //    Command_ID: this.Command_ID,
        //    title: 'Documents',
        //    flex: 0.3
        //});

        this.documentPanel = Ext.create('Mjc.Admin.ux.Command.CommandDocumentPanel', {
            Command_ID: this.Command_ID,
            title: 'Documents',
            flex: 0.3
        });

        //this.documentPanel = Ext.create('Ext.panel.Panel');

        this.detailsPanel = Ext.create('Mjc.Admin.ux.Command.CommandDetailsPanel', {
            title: 'Détails',
            flex: 0.3
        });
        this.items = [this.mainPanel, this.documentPanel, this.historyPanel, this.detailsPanel];

        this.callParent(arguments);
    }
});
//#endregion

//#region Mjc.Admin.ux.Command.CommandWorkspacePanel
Ext.define('Mjc.Admin.ux.Command.CommandWorkspacePanel', {
    //extend: 'Ext.panel.Panel',
    extend: 'Mjc.Admin.widget.ux.directForm.Panel',
    autoLoad: false,
    layout: {
        type: 'hbox',
        align: 'stretch'
    },
    api: {
        load: Mjc.direct.CommandDirect.getInformationCommand,
    },
    Command_ID: null,
    title: 'Commande N°',
    layout: {
        type: 'hbox',
        align: 'stretch'
    },
    initComponent: function () {

        var me = this;

        this.informationPanel = Ext.create('Mjc.Admin.ux.Command.CommandInformationPanel', {
            modelId: this.Command_ID,
            autoLoad: true,
            flex: 0.2
        });

        this.productsPanel = Ext.create('Mjc.Admin.ux.Command.CommandProductsPanel', {
            header: false,
            Command_ID: this.Command_ID,
            flex: 0.4
        });

        this.actionPanel = Ext.create('Mjc.Admin.ux.Command.CommandActionPanel', {
            hidden: false,
            title: 'Action',
            flex: 0.3,
            listeners: {
                submitCommandActionSuccess: function () {
                    me.reload();
                }
            }
        });

        this.commandActionInfoPanel = Ext.create('Mjc.Admin.ux.Command.CommandInformationActionPanel', {
            hidden: true,
            title: 'Action Informations',
            flex: 0.3,
        });

        this.items = [this.informationPanel, this.productsPanel, this.actionPanel, this.commandActionInfoPanel];

        this.on('loadSuccess', function (form, action, options) {
            var record = action.result;
            this.informationPanel.initPanel(record);
            this.productsPanel.initPanel(record);


            this.commandActionInfoPanel.initPanel(record);

            this.actionPanel.setRecord(record);
            this.actionPanel.initPanel();

            //if (record.data.StatusInt != 300) {
            //    this.actionPanel = Ext.create('Mjc.Admin.ux.Command.CommandActionPanel', {
            //        title: 'Action',
            //        flex: 0.3,
            //        listeners: {
            //            submitCommandActionSuccess: function () {
            //                me.reload();
            //            }
            //        }
            //    });
            //    this.add(this.actionPanel);
            //    this.actionPanel.initPanel(record);
            //}

        }, this);

        this.callParent(arguments);
    }
});
//#endregion

//#region Mjc.Admin.ux.Command.CommandInformationPanel
Ext.define('Mjc.Admin.ux.Command.CommandInformationPanel', {
    //extend: 'Mjc.Admin.widget.ux.directForm.Panel',
    extend: 'Ext.panel.Panel',
    layout: {
        type: 'vbox',
        align: 'stretch'
    },
    autoLoad: false,
    autoScroll: true,
    //api: {
    //    load: Mjc.direct.CommandDirect.getInformationCommand,
    //},
    header: false,
    //title: 'Commande N°',
    initComponent: function () {

        this.summaryPanel = Ext.create('Ext.panel.Panel', {
            //title: 'Infos',
            border: false,
            //padding: 20,
            flex: 0.4,
            //bodyCls: 'infoPanelCls',
            initPanel: function (record) {
                var html = '<div class="infoBox">' +
                                '<div class="infoCommand"><div class="infoCommandTitle">Command N°:</div>' + record.data.Command_ID + '</div>' +
                                '<div class="infoCommand"><div class="infoCommandTitle">Date création:</div>' + record.data.CreationDateTime + '</div>' +
                                '<div class="infoCommand"><div class="infoCommandTitle">Client ID:</div>' + record.data.Customer_User_FK + '</div>' +
                                '<div class="infoCommand"><div class="infoCommandTitle">Client:</div>' + record.data.CustomerFullName + '</div>' +
                                '<div class="infoCommand"><div class="infoCommandTitle">Email:</div> <span class="infoEmail">' + record.data.eMail + '</span></div>' +
                            '</div>'

                this.setHtml(html);
            }
        });

        this.addressShipmentPanel = Ext.create('Ext.panel.Panel', {
            //title: 'Livraison à',
            border: false,
            //padding: 20,
            flex: 0.4,
            initPanel: function (record) {
                this.setHidden(true);
                if (record.data.ShipmentType != 2) {
                    var html = '<div class="infoBox">' +
                        '<div class="infoCommand"><div class="infoCommandTitle">Livraison à:</div></div>' +
                        '<div class="infoCommand">' + record.data.Address.CustomerFullName + '</div>' +
                        '<div class="infoCommand">' + record.data.Address.Address1 + '</div>' +
                        '<div class="infoCommand">' + record.data.Address.ZipCode + ' ' + record.data.Address.City + '</div>' +
                        '<div class="infoCommand">' + record.data.Address.Country + '</div>';
                    '</div>'
                    this.setHidden(false);
                    this.setHtml(html);
                }

            }
        });

        this.optionShipmentPanel = Ext.create('Ext.panel.Panel', {
            border: false,
            hidden: true,
            flex: 0.2,
            initPanel: function (record) {
                //if (record.data.isOptionShipmentChosen) {
                //    var html = '<div class="infoBox">' +
                //                '<div class="infoCommand"><div class="infoCommandTitle">Option Sécurité:</div>' + record.data.OptionShipmentSecureType + ' (' + record.data.OptionShipmentCharge + ' €)</div>' +
                //    '</div>'

                //    this.setHtml(html);
                //    this.setHidden(false);
                //}
                var shipmentText = record.data.ShipmentTypeString;
                if (record.data.isOptionShipmentChosen) {
                    shipmentText += ' (' + record.data.OptionShipmentCharge + ' €)';
                }
                var html = '<div class="infoBox">'
                                + '<div class="infoCommand"><div class="infoCommandTitle">Type Livraison: </div>'
                                    + '<span>' + shipmentText + '</span>'
                                + '</div>' +
                           '</div>'

                this.setHtml(html);
                this.setHidden(false);
            }
        });

        this.statusPanel = Ext.create('Ext.panel.Panel', {
            border: false,
            //title: 'Statut',
            flex: 0.3,
            //bodyCls:'awaitingPaymentCls',
            initPanel: function (record) {
                var html = '<div class="infoBox">' +
                                '<div class="infoCommandStatus"> <h2> => ' + record.data.Status + '</h2></div>' +
                            '</div>'
                this.initBodyCls(record);
                this.setHtml(html);
            },
            initBodyCls: function (record) {
                var cls = "";
                switch (record.data.StatusInt) {
                    case 100:
                        cls = "awaitingPaymentCls";
                        break;
                    case 200:
                        cls = "paymentSuccessCls";
                        break;
                    case 300:
                        cls = "deliveredCls";
                        break;
                    case -100:
                        cls = "disableCls";
                        break;
                    case -200:
                        cls = "expiredCls";
                        break;
                }
                this.addBodyCls(cls);
            }
        });

        this.items = [this.summaryPanel, this.addressShipmentPanel, this.optionShipmentPanel, this.statusPanel];

        //this.on('loadSuccess', function (form, action, options) {
        //    var record = action.result;
        //    this.summaryPanel.initPanel(record);
        //    this.addressShipmentPanel.initPanel(record);
        //    this.statusPanel.initPanel(record);
        //}, this);

        this.callParent(arguments);
    },
    initPanel: function (record) {
        this.summaryPanel.initPanel(record);
        this.addressShipmentPanel.initPanel(record);
        this.optionShipmentPanel.initPanel(record);
        this.statusPanel.initPanel(record);
    }
});
//#endregion

//#region Mjc.Admin.ux.Command.CommandProductsGridPanel
Ext.define('Mjc.Admin.ux.Command.CommandProductsGridPanel', {
    extend: 'Ext.grid.Panel',
    Command_ID: null,
    margin: '0 0 0 1',
    title: 'Commande produits',
    header: false,
    allowClose: null,
    autoScroll: true,
    hidden: false,
    border: true,
    flex: 0.6,
    //features: [{
    //    ftype: 'summary',
    //    position: 'bottom'
    //}],
    initComponent: function () {

        if (this.Command_ID == null) {
            Ext.Error.raise("Mjc.Admin.ux.Command.CommandProductsGridPanel : missing parameter Command_ID");
        }

        this.columns = [{
            text: 'Numéro produit',
            dataIndex: 'Product_ID',
            flex: 0.1
        },
        {
            text: 'Nom affiché',
            dataIndex: 'DisplayName',
            flex: 0.4
        },
        {
            text: 'Nom produit',
            dataIndex: 'Name',
            flex: 0.3,
            hidden: true
        },
        {
            text: 'Prix unitaire',
            dataIndex: 'Price',
            flex: 0.1
        },
       {
           text: 'Qté',
           dataIndex: 'Quantity',
           flex: 0.1
       },
       {
           text: 'Total ligne',
           dataIndex: 'TotalCommandProduct',
           //summaryType: 'sum',
           flex: 0.1
       }];

        this.store = Ext.create('Mjc.Admin.store.CommandProduct_Set', {
            filters: [{ property: 'Command_ID', value: this.Command_ID }],
            autoLoad: false
        });

        this.dockedItems = [
        {
            xtype: 'pagingtoolbar',
            hidden: true,
            store: this.store,
            dock: 'bottom',
            displayInfo: true,
        }];

        this.callParent(arguments);
    }
});
//#endregion

//#region Mjc.Admin.ux.Command.CommandProductsPanel
Ext.define('Mjc.Admin.ux.Command.CommandProductsPanel', {
    extend: 'Ext.panel.Panel',
    Command_ID: null,
    title: 'Produits commandés',
    header: true,
    layout: {
        type: 'vbox',
        align: 'stretch'
    },
    initComponent: function () {

        this.productsGridPanel = Ext.create('Mjc.Admin.ux.Command.CommandProductsGridPanel', {
            autoLoad: true,
            border: false,
            Command_ID: this.Command_ID,
            //flex: 0.4
            flex: 0.8
        });
        this.bottomSummaryPanel = Ext.create('Ext.panel.Panel', {
            border: true,
            flex: 0.2,
            initPanel: function (record) {
                var html = '<div class="infoBox">' +
                                '<div class="infoCommand"><div class="infoCommandTotalTitle">Frais de livraison: </div>' + record.data.DeliveryCharge + ' €</div>' +
                               '<div class="infoCommand"><div class="infoCommandTotalTitle">Total commande: </div>' + record.data.TotalPrice + ' €</div>' +
                            '</div>'

                this.setHtml(html);
            }
        });
        this.items = [this.productsGridPanel, this.bottomSummaryPanel];

        this.callParent(arguments);
    },
    initPanel: function (record) {
        this.bottomSummaryPanel.initPanel(record);
    }
});
//#endregion

//#region Mjc.Admin.ux.Command.CommandActionPanel
Ext.define('Mjc.Admin.ux.Command.CommandActionPanel', {
    extend: 'Ext.form.Panel',
    autoScroll: true,
    record: null,
    api: {
        submit: Mjc.direct.CommandDirect.actionOnCommand,
    },
    initComponent: function () {

        var me = this;

        this.commandId = Ext.create('Ext.form.field.Hidden', {
            name: 'Command_ID'
        });

        this.commandChoiceActionField = Ext.create('Ext.form.field.Hidden', {
            name: 'CommandChoiceAction'
        });


        this.forcePaidCommandButton = Ext.create('Mjc.Admin.ux.Command.CommandActionButton', {
            choiceValue: 600,
            text: '<span style="text-align:left !important;" class="' + Mjc.Admin.util.Icon('arrow_large_right') + ' inlineIcon">Passer la commande en statut "Payée"</span>',
            hidden: true,
            //margin: '30 0 0 0',            
            cls: 'yellowButtonBackground',
            overCls: 'yellowButtonBackgroundOver',
            listeners: {
                toggle: function (button, pressed, eOpts) {
                    if (pressed) {
                        me.radioGroupPaymentInfos.setHiddenAndDisabled(false);
                        me.textAreaField.setHiddenAndDisabled(false);

                        me.saveButton.setDisabled(false);
                        me.commandChoiceActionField.setValue(this.choiceValue);
                    }
                }
            }
        });
        //}).toggle(true);

        this.shipmentDoneButton = Ext.create('Mjc.Admin.ux.Command.CommandActionButton', {
            choiceValue: 400,
            hidden: true,
            text: '<span style="text-align:left !important;" class="' + Mjc.Admin.util.Icon('arrow_large_right') + ' inlineIcon">J\'ai envoyé la commande par la Poste</span>',
            cls: 'blueButtonBackground',
            overCls: 'blueButtonBackgroundOver',
            listeners: {
                toggle: function (button, pressed, eOpts) {
                    if (pressed) {
                        me.feesPaidInPostOfficeField.setHiddenAndDisabled(false);
                        me.colissimoNumberField.setHiddenAndDisabled(false);
                        me.shipmentDate.setHiddenAndDisabled(false);
                        me.textAreaField.setHiddenAndDisabled(false);

                        me.saveButton.setDisabled(false);
                        me.commandChoiceActionField.setValue(this.choiceValue);
                    }
                }
            }
        });

        this.deliverPersonallyButton = Ext.create('Mjc.Admin.ux.Command.CommandActionButton', {
            choiceValue: 800,
            hidden: true,
            text: '<span style="text-align:left !important;" class="' + Mjc.Admin.util.Icon('arrow_large_right') + ' inlineIcon">J\'ai donné le produit en main propres</span>',
            cls: 'redButtonBackground',
            overCls: 'redButtonBackgroundOver',
            listeners: {
                toggle: function (button, pressed, eOpts) {
                    if (pressed) {
                        me.feesPaidInPostOfficeField.setHiddenAndDisabled(true);
                        me.colissimoNumberField.setHiddenAndDisabled(true);
                        me.shipmentDate.setHiddenAndDisabled(false);
                        me.radioGroupPaymentInfos.setHiddenAndDisabled(true);
                        me.textAreaField.setHiddenAndDisabled(false);

                        me.saveButton.setDisabled(false);
                        me.commandChoiceActionField.setValue(this.choiceValue);
                    }
                }
            }
        });

        this.disableCommandButton = Ext.create('Mjc.Admin.ux.Command.CommandActionButton', {
            choiceValue: -200,
            hidden: true,
            text: '<span style="text-align:left !important;" class="' + Mjc.Admin.util.Icon('arrow_large_right') + ' inlineIcon">Je désactive la commande</span>',
            cls: 'greyButtonBackground',
            overCls: 'greyButtonBackgroundOver',
            listeners: {
                toggle: function (button, pressed, eOpts) {
                    if (pressed) {
                        me.feesPaidInPostOfficeField.setHiddenAndDisabled(true);
                        me.colissimoNumberField.setHiddenAndDisabled(true);
                        me.shipmentDate.setHiddenAndDisabled(true);
                        me.radioGroupPaymentInfos.setHiddenAndDisabled(true);
                        me.textAreaField.setHiddenAndDisabled(false);

                        me.saveButton.setDisabled(false);
                        me.commandChoiceActionField.setValue(this.choiceValue);
                    }
                }
            }
        });

        this.feesPaidInPostOfficeField = Ext.create('Ext.form.field.Number', {
            minValue: 0,
            name: 'FeesPaidInPostOfficeField',
            allowBlank: false,
            margin: 30,
            decimalPrecision: 2,
            allowDecimals: true,
            hidden: true,
            fieldCls: 'editableField',
            width: '80%',
            labelAlign: 'top',
            fieldLabel: 'Frais de livraison payés à la Poste',
            setHiddenAndDisabled: function (status) {
                this.setDisabled(status);
                this.setHidden(status);
            }
        });

        this.colissimoNumberField = Ext.create('Ext.form.field.Text', {
            name: 'ColissimoNumber',
            allowBlank: true,
            margin: 30,
            decimalPrecision: 2,
            allowDecimals: true,
            hidden: true,
            fieldCls: 'editableField',
            width: '80%',
            labelAlign: 'top',
            fieldLabel: 'Numéro de suivi colissimo',
            setHiddenAndDisabled: function (status) {
                this.setDisabled(status);
                this.setHidden(status);
            }
        });

        this.shipmentDate = Ext.create('Ext.form.field.Date', {
            name: 'ShipmenDate',
            format: "d/m/Y",
            submitFormat: "d/m/Y",
            allowBlank: false,
            margin: 30,
            decimalPrecision: 2,
            allowDecimals: true,
            hidden: true,
            fieldCls: 'editableField',
            width: '80%',
            labelAlign: 'top',
            fieldLabel: 'Date d\'expédition',
            setHiddenAndDisabled: function (status) {
                this.setDisabled(status);
                this.setHidden(status);
            },
            maxValue: new Date()
        });

        this.radioGroupPaymentInfos = Ext.create('Ext.form.RadioGroup', {
            name: 'RadioButtonForcePayment',
            allowBlank: false,
            labelAlign: 'top',
            fieldLabel: 'Raisons',
            minHeight: 200,
            columns: 1,
            //padding: 30,
            margin: 30,
            border: false,
            vertical: true,
            hidden: true,
            items: [
                { xtype: 'radio', boxLabel: 'Le client a envoyé un chèque', name: 'radioButtonForcePayment', inputValue: 10 },
                { xtype: 'radio', boxLabel: 'Le client a fait un virement', name: 'radioButtonForcePayment', inputValue: 20 },
                { xtype: 'radio', boxLabel: 'Le client a payé en liquide', name: 'radioButtonForcePayment', inputValue: 30 },
                { xtype: 'radio', boxLabel: 'Le client a payé par Paypal hors site', name: 'radioButtonForcePayment', inputValue: 40 },
                { xtype: 'radio', boxLabel: 'Autres (mentionnez dans le commentaire)', name: 'radioButtonForcePayment', inputValue: 50 }
            ],
            setHiddenAndDisabled: function (status) {
                this.setDisabled(status);
                this.setHidden(status);
            }
        });



        this.textAreaField = Ext.create('Ext.form.field.TextArea', {
            name: 'CommentFromAdminUser',
            labelAlign: 'top',
            fieldLabel: 'Laissez un commentaire sur votre action',
            allowBlank: false,
            flex: 0.7,
            margin: 30,
            hidden: true,
            fieldCls: 'editableField',
            autoScroll: true,
            width: '80%',
            height: '50%',
            setHiddenAndDisabled: function (status) {
                this.setDisabled(status);
                this.setHidden(status);
            }
        });


        this.items = [this.commandId,
                      this.commandChoiceActionField,
                      this.forcePaidCommandButton,
                      this.shipmentDoneButton,
                      this.deliverPersonallyButton,
                      this.disableCommandButton,
                      this.feesPaidInPostOfficeField,
                      this.colissimoNumberField,
                      this.shipmentDate,
                      this.radioGroupPaymentInfos,
                      this.textAreaField];

        this.cancelButton = Ext.create('Ext.button.Button', {
            text: 'Annuler choix',
            iconCls: Mjc.Admin.util.Icon('refresh'),
            handler: function () {
                Mjc.Admin.util.noty.bubbleRightMessage('Vous avez réinitialisé le formulaire', 'information');
                me.initPanel();
            }
        });

        this.saveButton = Ext.create('Ext.button.Button', {
            disabled: true,
            //formBind: true,
            text: 'Sauvegarder',
            handler: function () {
                me.submitActionCommand();
            }
        });
        this.buttons = [this.cancelButton, this.saveButton];

        this.callParent(arguments);
    },
    initPanel: function () {
        //this.setHidden(false);
        this.resetAllItems();

        if (this.record == null) {
            Ext.Error.raise("Mjc.Admin.ux.Command.CommandActionPanel: initPanel method: record is null");
            return;
        }

        this.commandId.setValue(this.record.data.Command_ID);

        switch (this.record.data.StatusInt) {

            case 100:
                //this.radioGroupPaymentInfos.setHidden(false);
                this.forcePaidCommandButton.setHidden(false);
                this.disableCommandButton.setHidden(false);
                break;
            case 200:
                this.shipmentDoneButton.setHidden(false);
                this.deliverPersonallyButton.setHidden(false);
                this.disableCommandButton.setHidden(false);
                break;
            case 300: case -100: case -200:
                this.cancelButton.setDisabled(true);
                //this.setDisabled(true);
                this.setHidden(true);
                break;
        }
    },
    resetAllItems: function () {

        this.radioGroupPaymentInfos.reset();
        this.reset(); // form resetting

        this.forcePaidCommandButton.setHidden(true);
        this.shipmentDoneButton.setHidden(true);
        this.deliverPersonallyButton.setHidden(true);
        this.disableCommandButton.setHidden(true);

        this.forcePaidCommandButton.toggle(false);
        this.shipmentDoneButton.toggle(false);
        this.deliverPersonallyButton.toggle(false);
        this.disableCommandButton.toggle(false);

        this.shipmentDate.setHiddenAndDisabled(true);
        this.feesPaidInPostOfficeField.setHiddenAndDisabled(true);
        this.colissimoNumberField.setHiddenAndDisabled(true);
        this.radioGroupPaymentInfos.setHiddenAndDisabled(true);
        this.textAreaField.setHiddenAndDisabled(true);

        this.saveButton.setDisabled(true);
    },
    submitActionCommand: function () {
        var form = this.getForm();
        var me = this;

        if (!form.isValid()) {
            Mjc.Admin.util.noty.errorCenterMessage('Veuillez renseigner les champs obligatoires (scroller vers le bas pour voir les champs manquants)');
            return;
        }

        Ext.Msg.confirm('Sauvegarder', 'Confirmer vous l\'action sur cette commande ?', function (btn, text) {
            if (btn === 'yes') {

                me.mask('Chargement...');

                form.submit({
                    scope: this,
                    success: function (form, action) {
                        me.unmask();
                        me.fireEvent('submitCommandActionSuccess');
                    },
                    failure: function () { }
                });
            }
        }, this);





    },
    setRecord: function (record) {
        this.record = record;
    }
});
//#endregion

//#region Mjc.Admin.ux.Command.CommandInformationActionPanel
Ext.define('Mjc.Admin.ux.Command.CommandInformationActionPanel', {
    extend: 'Ext.panel.Panel',
    autoScroll: true,
    record: null,
    initComponent: function () {

        var me = this;

        this.commandChoiceActionField = Ext.create('Ext.form.field.Display', {
            minValue: 0,
            name: 'CommandChoiceAction',
            margin: 30,
            //width: '80%',
            labelAlign: 'top',
            fieldLabel: 'Action',
        });

        this.feesPaidInPostOfficeField = Ext.create('Ext.form.field.Display', {
            minValue: 0,
            name: 'FeesPaidInPostOfficeField',
            margin: 30,
            //width: '80%',
            labelAlign: 'top',
            fieldLabel: 'Frais de livraison payés à la Poste',
        });

        this.colissimoNumberField = Ext.create('Ext.form.field.Display', {
            name: 'ColissimoNumber',
            allowBlank: true,
            margin: 30,
            //width: '80%',
            labelAlign: 'top',
            fieldLabel: 'Numéro de suivi colissimo',
        });

        this.PaypalTxnId = Ext.create('Ext.form.field.Display', {
            name: 'Paypal Txn Id',
            allowBlank: true,
            margin: 30,
            //width: '80%',
            labelAlign: 'top',
            fieldLabel: 'Paypal Txn Id',
        });

        this.shipmentDate = Ext.create('Ext.form.field.Display', {
            name: 'ShipmenDate',
            allowBlank: true,
            margin: 30,
            //width: '80%',
            labelAlign: 'top',
            fieldLabel: 'Date de livraison'
        });

        this.textAreaField = Ext.create('Ext.form.field.Display', {
            name: 'CommentFromAdminUser',
            labelAlign: 'top',
            fieldLabel: 'Commentaire sur l\'action',
            flex: 0.7,
            margin: 30,
            readOnly: true,
            autoScroll: true,
            //width: '80%',
            height: '50%'
        });


        this.items = [
                      //this.commandId,
                      //this.commandChoiceActionField,
                      //this.forcePaidCommandButton,
                      //this.shipmentDoneButton,
                      //this.deliverPersonallyButton,
                      //this.disableCommandButton,
                      this.commandChoiceActionField,
                      this.shipmentDate,
                      this.colissimoNumberField,
                      this.feesPaidInPostOfficeField,
                      this.PaypalTxnId,
                      //this.radioGroupPaymentInfos,
                      this.textAreaField];

        this.callParent(arguments);
    },
    initPanel: function (record) {

        this.record = record;

        switch (this.record.data.StatusInt) {
            case 300: case -100:

                this.setHidden(false);

                var actionInfo = this.record.data.CommandInformationDelivered;

                this.feesPaidInPostOfficeField.setValue('<span class="comment">' + actionInfo.FeesPaidInPostOffice + '</span>');
                this.colissimoNumberField.setValue('<span class="comment">' + actionInfo.ColissimoNumber + '</span>');
                this.shipmentDate.setValue('<span class="comment">' + actionInfo.ShipmentDate + '</span>');
                this.PaypalTxnId.setValue('<span class="comment">' + actionInfo.Paypal_Txn_id + '</span>');
                this.textAreaField.setValue('<span class="comment">' + actionInfo.CommentFromAdminUser + '</span>');
                this.commandChoiceActionField.setValue('<span class="comment">' + actionInfo.CommandChoiceAction + '</span>');

                if (actionInfo.CommandChoiceActionInt == 800) {
                    this.feesPaidInPostOfficeField.setHidden(true);
                    this.colissimoNumberField.setHidden(true);
                }
                break;
        }
    },
    setRecord: function (record) {
        this.record = record;
    }
});
//#endregion

//#region Mjc.Admin.ux.Command.CommandDetailsPanel
Ext.define('Mjc.Admin.ux.Command.CommandDetailsPanel', {
    extend: 'Ext.panel.Panel',
    initComponent: function () {

        var me = this;


        this.callParent(arguments);
    }
});
//#endregion

//#region Mjc.Admin.ux.Command.CommandHistoryGridPanel
Ext.define('Mjc.Admin.ux.Command.CommandHistoryGridPanel', {
    extend: 'Ext.grid.Panel',
    Command_ID: null,
    margin: '0 0 0 1',
    title: 'Historique',
    header: false,
    allowClose: null,
    autoScroll: true,
    hidden: false,
    border: true,
    initComponent: function () {

        if (this.Command_ID == null) {
            Ext.Error.raise("Mjc.Admin.ux.Command.CommandProductsGridPanel : missing parameter Command_ID");
        }

        this.columns = [{
            text: 'Date',
            dataIndex: 'Date',
            flex: 0.2
        },
        {
            text: 'Description de l\'action',
            dataIndex: 'Description',
            flex: 0.4,
            renderer: function (val, meta, record) {
                if (!Ext.isEmpty(val)) {
                    meta.tdAttr = 'data-qtip="' + val + '"';
                }
                return val;
            }
        },
        {
            text: 'Commentaire',
            dataIndex: 'CommentFromAdminUser',
            flex: 0.7,
            renderer: function (val, meta, record) {
                if (!Ext.isEmpty(val)) {
                    meta.tdAttr = 'data-qtip="' + val + '"';
                }
                //return '<span class="comment">' + val + '</span>';
                return val;
            }
        }];

        this.store = Ext.create('Mjc.Admin.store.CommandHistory_Set', {
            filters: [{ property: 'Command_ID', value: this.Command_ID }],
        });

        this.dockedItems = [
        {
            xtype: 'pagingtoolbar',
            //hidden: true,
            store: this.store,
            dock: 'bottom',
            displayInfo: true,
        }];

        this.callParent(arguments);
    }
});
//#endregion



//#region Mjc.Admin.ux.Command.CommandDocumentPanel
Ext.define('Mjc.Admin.ux.Command.CommandDocumentPanel', {
    extend: 'Ext.panel.Panel',
    title: 'Documents',
    closable: false,
    layout: 'fit',
    store: null,
    Command_ID: null,
    constructor: function (cfg) {
        var me = this;
        cfg = cfg || {};

        this.leftPanel = Ext.create('Mjc.Admin.ux.Command.DocumentGridPanel', {
            Command_ID: cfg.Command_ID,
            scope: this,
            flex: 0.5,
            store: this.store,
            scope: this,
            listeners: {
                refresh: function () {
                    this.fireEvent('refresh');
                    if (this.rightPanel.Document_ID != null)
                        this.rightPanel.reload();
                },
                select: function (grid, record, index, eOpts) {
                    this.rightPanel.reload(record);
                },
                scope: this
            }
        });

        this.rightPanel = Ext.create('Mjc.Admin.ux.Command.DocumentPanel', {
            flex: 0.5
        });

        var docStore = this.leftPanel.getStore();
        docStore.on('load', function () {
            if (docStore.getCount() == 0) {
                this.rightPanel.resetPanel();
            }
        }, this);

        me.callParent([Ext.apply({
            layout: {
                type: 'hbox',
                align: 'stretch'
            },
            items: [this.leftPanel, this.rightPanel]
        }, cfg)]);
    },
    initComponent: function () {

        var me = this;

        this.on('afterRender', function (panel, action) {
            this.reload();
        }, this);

        this.callParent(arguments);

    },
    reload: function () {

        //TP.direct.Document.getContextDocumentPanel(this.ContextId, this.Context, function (result) {
        //    this.leftPanel.configureActions(result.data.ActionTracker);
        //    this.leftPanel.getStore().reload({
        //        scope: this.leftPanel,
        //        callback: function (records, operation, success) {

        //            if (this.getStore().getCount() > 0) {

        //                var selModel = this.getSelectionModel();
        //                selModel.select(0);
        //            }

        //        }
        //    });
        //}, this);
    }
});
//#endregion

//#region  Mjc.Admin.ux.Command.DocumentGridPanel
Ext.define('Mjc.Admin.ux.Command.DocumentGridPanel', {
    Command_ID: null,
    extend: 'Ext.grid.Panel',
    viewConfig: {
        deferEmptyText: false
    },
    store: null,
    initComponent: function () {

        var me = this;

        var rendererFileType = function (value, metaData, record, rowIndex, colIndex, store) {
            if (record.data.FileType === "PDF") { metaData.css = Mjc.Admin.util.Icon('doc_pdf'); }
            else if (record.data.FileType === "WORD") { metaData.css = Mjc.Admin.util.Icon('page_word'); }
            else if (record.data.FileType === "EXCEL") { metaData.css = Mjc.Admin.util.Icon('page_excel'); }
            else if (record.data.FileType === "PPT") { metaData.css = Mjc.Admin.util.Icon('document-powerpoint'); }
            else if (record.data.FileType === "IMAGE") { metaData.css = Mjc.Admin.util.Icon('image_2'); }
            //else if (record.data.FileType === "FILE_TYPE_UNKNOWN") { metaData.css = ...; }

            return '';
        }

        this.columns = [
            { xtype: 'rownumberer', width: 30 },
            //{ text: 'Date d\'ajout', width: 85, dataIndex: 'CreationDateTime', xtype: 'datecolumn', format: 'd/m/Y' },
            { text: 'Date d\'ajout', flex: 0.5, dataIndex: 'CreationDateTime' },
            { width: 30, renderer: rendererFileType, align: 'center', tooltip: 'Format du fichier' },
            {
                text: 'Nom du document',
                flex: 1,
                dataIndex: 'Name',
                renderer: function (value, metadata, record) {
                    if (record.data.Description != null) {
                        metadata.tdAttr = 'data-qtip="Description: ' + record.data.Description + '"';
                    }
                    return value;
                }
            },
            { text: 'Description', flex: 1, dataIndex: 'Description' },
            { text: 'Type', flex: 0.3, dataIndex: 'FileType' },
            {
                xtype: 'actioncolumn',
                width: 30,
                tooltip: "Télécharger",
                scope: this,
                items: [{
                    scale: 'large',
                    tooltip: "Télécharger",
                    getClass: function (value, meta, record) {

                        return Mjc.Admin.util.Icon('download');
                    },
                    handler: function (grid, rowIndex, colIndex) {
                        var record = grid.getStore().getAt(rowIndex);
                        Mjc.Admin.util.downloadFile('DocumentDirect/downloadDocument/', {
                            id: record.data.Document_ID
                        });
                    }
                }]
            },
            {
                xtype: 'actioncolumn',
                width: 30,
                tooltip: "Supprimer le document",
                scope: this,
                items: [{
                    tooltip: "Supprimer le document",
                    scale: 'large',
                    getClass: function (value, meta, record) {
                        return Mjc.Admin.util.Icon('error');
                    },
                    handler: function (grid, rowIndex, colIndex) {
                        var record = grid.getStore().getAt(rowIndex);
                        Ext.Msg.confirm('Suppression de document', 'Etes vous sur de vouloir supprimer le document' + '<span class=\"blueImportantMessage\">' + record.data.DocumentFileName + '</span>', function (btn, text) {
                            if (btn === 'yes') {
                                Mjc.direct.CommandDirect.deleteCommandDocument(record.data.Document_ID, function () {
                                    grid.getStore().reload();
                                }, this);
                            }
                        }, this);
                    }
                }]
            }
        ];

        this.store = Ext.create('Mjc.Admin.store.Command.CommandDocument_Set', {
            filters: [{ property: 'Command_ID', value: this.Command_ID }],
            autoLoad: true,
            listeners: {
                load: function (grid, records, successful, operation, eOpts) {
                    if (this.getTotalCount() > 0) {
                        var selModel = me.getSelectionModel();
                        selModel.select(0);
                    }
                }
            }
        })

        this.btnAddDocumentMenu = Ext.create('Ext.button.Button', {
            text: 'Ajouter document',
            iconCls: Mjc.Admin.util.Icon('add_file'),
            scope: this,
            hidden: false,
            handler: function () {


                var form = Ext.create('Mjc.Admin.ux.Command.AddDocumentPanel', {
                    Command_ID: this.Command_ID
                    //documentComboData: [{
                    //    DocumentType: 10,
                    //    DocumentTypeString: 'PDF'
                    //}, {
                    //    DocumentType: 20,
                    //    DocumentTypeString: 'IMAGE'
                    //},
                    //{
                    //    DocumentType: 30,
                    //    DocumentTypeString: 'PPT'
                    //}]
                });

                form.getForm().on('actioncomplete', function (form, action) {
                    if (!action.result.hasError) {
                        window.close();

                        var documentId = action.result.Document_ID;
                        var selModel = this.getSelectionModel();
                        var store = this.getStore();

                        store.reload({
                            scope: this,
                            callback: function (records, operation, success) {

                                if (store.getCount() > 0) {
                                    var index = store.find('Document_ID', documentId);
                                    selModel.select(store.getAt(index));
                                }

                            }
                        });
                    }
                }, this);

                var window = Ext.create('Ext.window.Window', {
                    width: 600,
                    title: 'Ajouter un document...',
                    resizable: false,
                    width: 400,
                    modal: true,
                    items: [form],
                }).show();

            }
        });

        this.dockedItems = [
            {
                xtype: 'toolbar',
                items: [this.btnAddDocumentMenu],
                dock: 'top',
            },
            {
                xtype: 'pagingtoolbar',
                store: this.store,
                displayInfo: true,
                style: { background: 'none' },
                dock: 'bottom'
            }
        ];

        this.callParent(arguments);

    }
});
//#endregion

//#region Mjc.Admin.ux.Command.DocumentPanel
Ext.define('Mjc.Admin.ux.Command.DocumentPanel', {
    extend: 'Ext.tab.Panel',
    activeTab: 0,
    modelId: null,
    constructor: function (cfg) {
        var me = this;
        cfg = cfg || {};

        me.callParent([Ext.apply({
            dockedItems: [this.toolbar],
            items: []
        }, cfg)]);
    },
    reload: function (record) {

        this.removeAll();
        this.reloadLayout(record);
    },
    reloadLayout: function (record) {

        var newTab = null;

        if (record.data.FileType == 'PDF') {
            newTab = Ext.create('Mjc.Admin.widget.ux.PdfPanel', {
                url: 'DocumentDirect/getDocument',
                autoScroll: false,
                modelId: record.data.Document_ID,
                title: record.data.Name
            })
        } else if (record.data.FileType == 'IMAGE') {
            newTab = Ext.create('Mjc.Admin.widget.ux.ImagePanel', {
                url: 'DocumentDirect/getDocument',
                autoScroll: true,
                modelId: record.data.Document_ID,
                title: record.data.Name
            })
        }

        if (newTab != null) {
            this.add(newTab);
            this.setActiveItem(newTab);
        }
    },
    getDetailsPanel: function () {
        return this.detailsPanel;
    },
    resetPanel: function () {
        this.removeAll();
    }
});
//#endregion

//#region Mjc.Admin.ux.Command.DetailsPanel
Ext.define('Mjc.Admin.ux.Command.DetailsPanel', {
    extend: 'Mjc.Admin.widget.ux.directForm.Panel',
    title: 'Details',
    modelId: null,
    autoLoad: false,
    api: {
        load: Mjc.direct.DocumentDirect.getDocumentDetails
    },
    modelId: null,
    overflowY: 'auto',
    constructor: function (cfg) {
        var me = this;
        cfg = cfg || {
            layout: 'fit'
        };

        me.callParent([Ext.apply({
            bodyPadding: 30
        }, cfg)]);
    },
    initComponent: function () {

        this.fieldsetDetails = {
            xtype: 'fieldset',
            title: '<div style ="color:#074e7c;font-weight:bold">' + 'Détails' + '</div>', //TODO removeStyle
            collapsed: false,
            layout: 'anchor',
            items: [{
                layout: 'form',
                border: false,
                labelWidth: 80,
                width: 500,
                defaults: {
                    xtype: 'displayfield',
                    anchor: '0'
                },
                items: [
                    {
                        name: 'Document_ID',
                        fieldLabel: 'Document_ID',
                    }, {
                        name: 'DocumentFileName',
                        fieldLabel: 'Nom du document'
                    }, {
                        name: 'FileType',
                        fieldLabel: 'Type'
                    }, {
                        name: 'DateUploaded',
                        fieldLabel: 'Date d\'ajout'
                        //renderer: Ext.util.Format.dateRenderer('d/m/Y')
                    }
                ]
            }]
        };

        this.items = [this.fieldsetDetails];

        this.callParent(arguments);
    }
});
//#endregion

//#region Mjc.Admin.ux.Command.AddDocumentPanel
Ext.define('Mjc.Admin.ux.Command.AddDocumentPanel', {
    extend: 'Ext.form.Panel',
    documentComboData: null,
    Command_ID: null,
    api: {
        submit: Mjc.direct.CommandDirect.UploadCommandDocument
    },
    constructor: function (cfg) {
        var me = this;
        cfg = cfg || {};

        me.callParent([Ext.apply({
            frame: false,
            bodyPadding: 10,
            border: false,
            bodyStyle: { background: 'transparent' },
            defaults: {
                anchor: '100%',
                labelWidth: 100
            },
        }, cfg)]);
    },
    initComponent: function () {

        this.commandId = Ext.create('Ext.form.field.Hidden', {
            name: 'Command_ID',
            value: this.Command_ID
        });

        this.fileField = Ext.create('Ext.form.field.File', {
            xtype: 'filefield',
            name: 'File',
            fieldLabel: 'Fichier',
            msgTarget: 'side',
            allowBlank: false,
            anchor: '100%',
            width: '100%',
            buttonText: 'Parcourir...',
        });

        this.msg = Ext.create('Ext.form.field.Display', {
            name: 'msg',
            inputValue: 1,
            anchor: '100%',
            width: '100%',
            value: ""
        })

        //this.comboDocumentType = Ext.create('Ext.form.ComboBox', {
        //    typeAhead: true,
        //    triggerAction: 'all',
        //    lazyRender: true,
        //    allowBlank: false,
        //    forceSelection: true,
        //    fieldLabel: 'Type du document',
        //    emptyText: 'Choisissez votre type de document...',
        //    queryMode: 'local',
        //    store: Ext.create('Ext.data.Store', {
        //        fields: ['DocumentType', 'DocumentTypeString'],
        //        data: []
        //    }),
        //    name: 'DocumentType',
        //    listeners: {
        //        collapse: function (box) {
        //            box.fireEvent('blur');
        //        }
        //    },
        //    valueField: 'DocumentType',
        //    displayField: 'DocumentTypeString',
        //    listClass: 'x-combo-list-small'
        //});
        //this.comboDocumentType.getStore().loadData(this.documentComboData);

        var config = {
            items: [{
                layout: 'form',
                border: false,
                items: [
                    this.commandId,
                    this.fileField,
                    Ext.create('Ext.form.field.TextArea', {
                        name: 'Description',
                        inputValue: 1,
                        fieldLabel: 'Description',
                        emptyText: 'Décrivez s\'il vous plait',
                        anchor: '100%',
                        width: '100%',
                        allowBlank: false
                    }),
                    this.msg]
            }
            ],
            buttons: [
            {
                formBind: true,
                text: 'Valider',
                handler: this.submit,
                scope: this
            }],
            buttonAlign: 'right'
        }
        Ext.apply(this, config);
        this.callParent(arguments);
    },
    submit: function () {

        var form = this.getForm();
        var me = this;

        this.mask('Chargement');

        if (form.isValid()) {

            form.submit({
                scope: this,
                method: 'POST',
                success: function (form, action) {
                    Mjc.Admin.util.noty.displayResultMessage(true, action.result.msg);
                    this.unmask();
                },
                failure: function (form, action) {
                    Mjc.Admin.util.noty.errorCenterMessage(action.result.msg);
                }
            });
        }
    }
});
//#endregion

