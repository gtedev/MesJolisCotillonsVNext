Ext.ns('Mjc.Admin.Design.ux');

//#region Mjc.Admin.Design.ux.MainPanel
Ext.define('Mjc.Admin.Design.ux.MainPanel', {
    extend: 'Ext.panel.Panel',
    title: 'Design',
    //layout:'border',
    layout: {
        type: 'hbox',
        align: 'stretch'
    },
    initComponent: function () {
        var me = this;

        this.designGrid = Ext.create('Mjc.Admin.Design.ux.GridPanel', {
            flex: 0.4,
            region: 'west',
            collapseDirection: 'left',
            listeners: {
                itemdblclick: function (grid, record, item, index, e, eOpts) {
                    me.rightPanel.openDesignConfig(record.data.Design_Config_ID);
                },
                editDesignConfig: function (Design_Config_ID) {
                    me.rightPanel.openDesignConfig(Design_Config_ID);
                },
                activateDesignConfig: function (Design_Config_ID) {

                    me.mask("Chargement...  ");
                    Mjc.Admin.util.MessageBox.confirm('Confirmation', 'Voulez-vous vraiment activer cette configuration ? Notez que cela appliquera cette configuration sur le site en ligne.', function (btn) {
                        if (btn === 'yes') {
                            Mjc.direct.DesignDirect.activateDesignConfig(Design_Config_ID, function (result, action) {
                                me.unmask();
                                Mjc.Admin.util.noty.displayResultMessage(result.success, result.msg);
                                this.getStore().reload();
                            }, this);
                        }
                    }, this);
                },
                deleteDesignConfig: function (Design_Config_ID) {

                    me.mask("Chargement...  ");
                    Mjc.Admin.util.MessageBox.confirm('Confirmation', 'Voulez-vous vraiment supprimer définitivement cette configuration ?', function (btn) {
                        if (btn === 'yes') {
                            Mjc.direct.DesignDirect.deleteDesignConfig(Design_Config_ID, function (result, action) {
                                me.unmask();
                                Mjc.Admin.util.noty.displayResultMessage(result.success, result.msg);
                                this.getStore().reload();
                            }, this);
                        }
                    }, this);
                }
            }
        });

        this.rightPanel = Ext.create('Ext.tab.Panel', {
            region: 'center',
            flex: 0.6,
            openDesignConfig: function (Design_Config_ID) {

                var tab = this.getComponent('Design_Config_' + Design_Config_ID);

                if (!tab) {
                    tab = Ext.create('Mjc.Admin.Design.ux.ConfigPanel', {
                        region: 'center',
                        itemId: 'Design_Config_' + Design_Config_ID,
                        title: 'Design_Config_' + Design_Config_ID,
                        Design_Config_ID: Design_Config_ID,
                        closable: true
                    });

                    tab.on("afterloading", function () {
                        tab.unmask();
                    }, this);

                    this.add(tab);

                    this.setActiveTab(tab);
                    tab.mask("Chargement Design Config...");
                }
                else {
                    this.setActiveTab(tab);
                }
            },
            listeners: {
                //openDesign_Config: function (Design_Config_ID) {
                //    me.rightPanel.openDesignConfig(WF_Config_ID);
                //}
            }
        });


        this.items = [this.designGrid, this.rightPanel];

        this.callParent(arguments);
    }
});
//#endregion

//#region Mjc.Admin.Design.ux.GridPanel
Ext.define('Mjc.Admin.Design.ux.GridPanel', {
    extend: 'Ext.grid.Panel',
    title: 'Listes des configurations',
    border: false,
    //margin: '30 0 0 0',
    header: false,
    initComponent: function () {

        var me = this;

        this.columns = [{
            text: 'Design_Config_ID',
            dataIndex: 'Design_Config_ID',
            flex: 0.2
        }, {
            text: 'Nom',
            dataIndex: 'Name',
            flex: 0.7
        },
        {
            text: 'Description',
            dataIndex: 'Description',
            flex: 1
        },
        {
            text: 'Statut',
            dataIndex: 'Status',
            flex: 0.5
        },
        {
            text: 'Date création',
            dataIndex: 'CreationDatetime',
            flex: 0.5
        },
        {
            text: 'Dernier update',
            dataIndex: 'LastUpdateDatetime',
            flex: 0.5
        },
        {
            xtype: 'widgetcolumn',
            text: 'Action',
            resizable: false,
            draggable: false,
            flex: 0.4,
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
                    //hidden: !record.data.userCanActiveProduct,
                    text: 'Editer cette configuration',
                    disabled: !record.data.canEditDesignConfig,
                    iconCls: Mjc.Admin.util.Icon('pencil'),
                    handler: function () {
                        me.fireEvent('editDesignConfig', record.data.Design_Config_ID)
                    }
                },
                {
                    //hidden: !record.data.userCanActiveProduct,
                    text: 'Activer cette configuration',
                    iconCls: Mjc.Admin.util.Icon('magic-button'),
                    disabled: !record.data.canActiveDesignConfig,
                    handler: function () {
                        me.fireEvent('activateDesignConfig', record.data.Design_Config_ID)
                    }
                },
                {
                    text: 'Supprimer cette configuration...',
                    iconCls: Mjc.Admin.util.Icon('error'),
                    disabled: !record.data.canDeleteDesignConfig,
                    handler: function () {
                        me.fireEvent('deleteDesignConfig', record.data.Design_Config_ID)
                    }
                }];


                widget.setMenu(menu);
            }
        }];

        this.store = Ext.create('Mjc.Admin.Design.store.DesignConfig_Set', {
            //sorters: [{
            //    property: 'Name',
            //    direction: 'ASC'
            //}],
            listeners: {
                scope: this,
                load: function (store, records, successful, operation, eOpts) {
                    if (store.getTotalCount() > 0) {
                        me.getSelectionModel().select(0);
                    }
                    me.fireEvent('load', store, records, successful, operation, eOpts);
                }
            }
        });

        this.viewConfig = {
            getRowClass: function (record, rowIndex, rowParams, store) {

                if (record.data.Status == "Active") {
                    return 'fontBoldBlue';
                }
                else
                    return '';
            }
        };

        //this.listeners = {
        //    scope: this,
        //    itemclick: function (grid, record, item, index, event, eOpts) {
        //        this.fireEvent('itemclick', grid, record);
        //    }
        //};

        this.dockedItems = [{
            xtype: 'toolbar',
            enableOverflow: true,
            //autoScroll: true,
            dock: 'top',
            items: [{
                xtype: 'button',
                text: 'Créér un nouveau Design',
                iconCls: Mjc.Admin.util.Icon('layers'),
                margin: 10,
                handler: function () {

                    var designCreationPanel = Ext.create('Mjc.Admin.Design.ux.DesignCreationPanel', {
                        api: {
                            submit: Mjc.direct.DesignDirect.createDesignConfig
                        },
                        listeners: {
                            scope: this,
                            cancel: function () {
                                window.close();
                            },
                            submitSuccess: function (form, action, options) {
                                window.close();
                                Mjc.Admin.util.noty.bubbleRightMessage(action.result.msg, 'success');
                                me.getStore().reload();
                            }
                        },
                        buttons: [{
                            text: 'Annuler',
                            scope: this,
                            handler: function () {
                                window.close();
                            }
                        }, {
                            text: 'Créér',
                            scope: this,
                            formBind: true,
                            handler: function () {
                                designCreationPanel.submit();
                            }
                        }]
                    });

                    var window = Ext.create('Ext.window.Window', {
                        title: 'Créér un nouveau Design',
                        modal: true,
                        constrain: true,
                        resizable: false,
                        items: [designCreationPanel],
                        width: 400
                    }).show();
                }
            },
            {
                xtype: 'button',
                text: 'Gérer les Etiquettes...',
                iconCls: Mjc.Admin.util.Icon('sticker'),
                margin: 10,
                menu: [{
                    iconCls: Mjc.Admin.util.Icon('sticker'),
                    text: 'Ajouter une Etiquette...',
                    handler: function () {

                        var addEtiquettePanel = Ext.create('Mjc.Admin.Design.ux.EtiquettePanel', {
                            api: {
                                submit: Mjc.direct.DesignDirect.createEtiquette
                            },
                            listeners: {
                                scope: this,
                                submitSuccess: function (form, action, options) {
                                    window.close();
                                    Mjc.Admin.util.noty.displayResultMessage(action.result.success, action.result.msg);
                                    me.getStore().reload();
                                },
                                submitFailure: function (form, action, options) {
                                    window.close();
                                    Mjc.Admin.util.noty.displayResultMessage(action.result.success, action.result.msg);
                                    me.getStore().reload();
                                }
                            },
                            buttons: [{
                                text: 'Annuler',
                                scope: this,
                                handler: function () {
                                    window.close();
                                }
                            }, {
                                text: 'Valider',
                                scope: this,
                                formBind: true,
                                handler: function () {
                                    addEtiquettePanel.submit();
                                }
                            }]
                        });

                        var window = Ext.create('Ext.window.Window', {
                            title: 'Ajouter une Etiquette',
                            modal: true,
                            constrain: true,
                            resizable: false,
                            items: [addEtiquettePanel],
                            width: 400
                        }).show();
                    }
                }, {
                    iconCls: Mjc.Admin.util.Icon('close_red'),
                    text: 'Supprimer une Etiquette...',
                    handler: function () {

                        var deleteEtiquettePanel = Ext.create('Mjc.Admin.Design.ux.DeleteEtiquettePanel', {
                            api: {
                                submit: Mjc.direct.DesignDirect.deleteEtiquette
                            },
                            listeners: {
                                scope: this,
                                submitSuccess: function (form, action, options) {
                                    window.close();
                                    Mjc.Admin.util.noty.displayResultMessage(action.result.success, action.result.msg);
                                    me.getStore().reload();
                                },
                                submitFailure: function (form, action, options) {
                                    window.close();
                                    Mjc.Admin.util.noty.displayResultMessage(action.result.success, action.result.msg);
                                    me.getStore().reload();
                                }
                            },
                            buttons: [{
                                text: 'Annuler',
                                scope: this,
                                handler: function () {
                                    window.close();
                                }
                            }, {
                                text: 'Valider',
                                scope: this,
                                formBind: true,
                                handler: function () {

                                    //me.mask("Chargement...  ");
                                    Mjc.Admin.util.MessageBox.confirm('Confirmation', 'Voulez-vous vraiment supprimer cette étiquette ? Notez que les vues items reliés ne pourront l\'utiliser.', function (btn) {
                                        if (btn === 'yes') {
                                            deleteEtiquettePanel.submit();
                                        }
                                    }, this);


                                }
                            }]
                        });

                        var window = Ext.create('Ext.window.Window', {
                            title: 'Supprimer une étiquette',
                            modal: true,
                            constrain: true,
                            resizable: false,
                            items: [deleteEtiquettePanel],
                            width: 400
                        }).show();
                    }
                }]
            }, {
                xtype: 'button',
                text: 'Gérer les Carroussel Images...',
                iconCls: Mjc.Admin.util.Icon('album_red'),
                margin: 10,
                menu: [{
                    iconCls: Mjc.Admin.util.Icon('album_red'),
                    text: 'Ajouter un Carroussel Image...',
                    handler: function () {

                        var addCarouselImagePanel = Ext.create('Mjc.Admin.Design.ux.CarouselImagePanel', {
                            api: {
                                submit: Mjc.direct.DesignDirect.createCarouselImage
                            },
                            listeners: {
                                scope: this,
                                submitSuccess: function (form, action, options) {
                                    window.close();
                                    Mjc.Admin.util.noty.displayResultMessage(action.result.success, action.result.msg);
                                    me.getStore().reload();
                                },
                                submitFailure: function (form, action, options) {
                                    window.close();
                                    Mjc.Admin.util.noty.displayResultMessage(action.result.success, action.result.msg);
                                    me.getStore().reload();
                                }
                            },
                            buttons: [{
                                text: 'Annuler',
                                scope: this,
                                handler: function () {
                                    window.close();
                                }
                            }, {
                                text: 'Valider',
                                scope: this,
                                formBind: true,
                                handler: function () {
                                    addCarouselImagePanel.submit();
                                }
                            }]
                        });

                        var window = Ext.create('Ext.window.Window', {
                            title: 'Ajouter un Carroussel Image',
                            modal: true,
                            constrain: true,
                            resizable: false,
                            items: [addCarouselImagePanel],
                            width: 400
                        }).show();
                    }
                }, {
                    iconCls: Mjc.Admin.util.Icon('close_red'),
                    text: 'Supprimer un Carroussel Image...',
                    handler: function () {

                        var deleteCarouselImagePanel = Ext.create('Mjc.Admin.Design.ux.DeleteCarouselImagePanel', {
                            api: {
                                submit: Mjc.direct.DesignDirect.deleteCarouselImage
                            },
                            listeners: {
                                scope: this,
                                submitSuccess: function (form, action, options) {
                                    window.close();
                                    Mjc.Admin.util.noty.displayResultMessage(action.result.success, action.result.msg);
                                    me.getStore().reload();
                                },
                                submitFailure: function (form, action, options) {
                                    window.close();
                                    Mjc.Admin.util.noty.displayResultMessage(action.result.success, action.result.msg);
                                    me.getStore().reload();
                                }
                            },
                            buttons: [{
                                text: 'Annuler',
                                scope: this,
                                handler: function () {
                                    window.close();
                                }
                            }, {
                                text: 'Valider',
                                scope: this,
                                formBind: true,
                                handler: function () {

                                    //me.mask("Chargement...  ");
                                    Mjc.Admin.util.MessageBox.confirm('Confirmation', 'Voulez-vous vraiment supprimer ce Carroussel Image ? Notez que les vues items reliés ne pourront l\'utiliser.', function (btn) {
                                        if (btn === 'yes') {
                                            deleteCarouselImagePanel.submit();
                                        }
                                    }, this);


                                }
                            }]
                        });

                        var window = Ext.create('Ext.window.Window', {
                            title: 'Supprimer un Carroussel Image',
                            modal: true,
                            constrain: true,
                            resizable: false,
                            items: [deleteCarouselImagePanel],
                            width: 400
                        }).show();
                    }
                }]
            }]
        },
            {
                xtype: 'pagingtoolbar',
                store: this.store,
                dock: 'bottom',
                displayInfo: true,
            }]
        this.callParent(arguments);
    },
    reload: function () {
        this.store.reload();
    }
});
//#endregion

//#region Mjc.Admin.Design.ux.ConfigPanel
Ext.define('Mjc.Admin.Design.ux.ConfigPanel', {
    extend: 'Ext.panel.Panel',
    margin: '0 0 0 1',
    allowClose: null,
    autoScroll: true,
    Design_Config_ID: null,
    hidden: false,
    border: true,
    layout: {
        type: 'hbox',
        align: 'stretch'
    },
    flex: 0.6,
    initComponent: function () {

        var me = this;

        this.wfConfigStore = Ext.create('Ext.data.TreeStore', {
            autoLoad: false,
            root: {
                children: []
            },
        });

        this.configDataStore = Ext.create('Mjc.Admin.store.Design.Design_ConfigStore', {
            filters: [{ property: 'Design_Config_ID', value: this.Design_Config_ID }],
            autoLoad: true,
        });

        this.configDataStore.on('load', function (me, records, successful, eOpts) {

            var config = this.configDataStore.getAt(0).data;
            this.loadDesign_Config(config);

            if (eOpts.refreshConfig) {

                this.applyRefreshConfig(eOpts.refreshConfig);
            }

            this.wfConfigStore.fireEvent('afterloading');
            this.unmask();

        }, this);

        this.treePanel = Ext.create('Ext.tree.Panel', {
            resizable: true,
            autoScroll: true,
            layout: 'fit',
            fields: ['text', 'description'],
            columns: [{
                xtype: 'treecolumn',
                text: 'Name',
                dataIndex: 'text',
                width: 270,
                sortable: false
            }, {
                text: 'Description',
                dataIndex: 'description',
                flex: 1,
                sortable: false
            }],
            viewConfig: {
                plugins: {
                    ptype: 'treeviewdragdrop',
                    ddGroup: 'DesignConfigDD'
                },
                listeners: {
                    scope: this,
                    nodedragover: function (targetNode, position, dragData, e, eOpts) {

                        var record = dragData.records[0];

                        if (Ext.isEmpty(record.data.nodeTargetForDrop) || Ext.isEmpty(targetNode.data.type)) {
                            return false;
                        }


                        if (this.treePanel.isNodeDroppable(record.data.nodeTargetForDrop, targetNode.data.type)) {

                            return (targetNode.data.type == "HeaderRoot") || (targetNode.data.type == "CarouselImagesRoot") || (targetNode.data.type == "MenuItem" && targetNode.data.allowSubMenuItem == true);

                        } else
                            return false;
                    },
                    beforedrop: function (node, data, overModel, dropPosition, dropHandlers) {

                        // Defer the handling
                        dropHandlers.wait = true;
                        var record = data.records[0];

                        switch (record.data.Type) {
                            case "MenuItem":
                                Mjc.direct.DesignDirect.addItemToDesignConfig(overModel.data.Design_Config_ID, overModel.data.type, overModel.data.entity_Id, record.data.Name, null, function (result, action) {

                                    Mjc.Admin.util.noty.displayResultMessage(result.success, result.msg);

                                    if (result.success) {
                                        this.refreshConfig(overModel.data.id, record.data.Type);
                                    }

                                }, this);
                                break;
                            case "CarouselImage":
                                Mjc.direct.DesignDirect.addItemToDesignConfig(overModel.data.Design_Config_ID, overModel.data.type, overModel.data.entity_Id, record.data.Type, record.data.CarouselImage_ID, function (result, action) {

                                    Mjc.Admin.util.noty.displayResultMessage(result.success, result.msg);

                                    if (result.success) {
                                        this.refreshConfig(overModel.data.id, record.data.Type);
                                    }

                                }, this);
                                break;
                        }


                    },
                    drop: function (node, data, overModel, dropPosition, eOpts) {
                        //var child = {text:'new node'};
                        //node.add(child);
                    },
                    itemdblclick: function (tree, record, item, index, e, eOpts) {

                        if (!(
                            record.data.type === 'MenuItem'
                            )) { return; }

                        var title;
                        var itemName;

                        if (record.data.type === 'MenuItem') {
                            title = record.data.text;
                        }

                        var propertyPanel = new Ext.create('Mjc.Admin.Design.ux.DesignConfigPropertyPanel', {
                            itemName: title,
                            propertyPanelTabId: "propertiesPanel_WF_ExitCheckType" + record.data.entity_Id,
                            record: record,
                            closabe: false
                        });

                        var window = Ext.create('Ext.window.Window', {
                            title: 'Properties ' + title,
                            resizable: true,
                            layout: 'fit',
                            width: 500,
                            height: 600,
                            modal: true,
                            closable: true,
                            items: [propertyPanel],
                        }).show();
                        window.on('close', function (windowPanel, eOpts) {
                            this.refreshConfig();
                        }, this);
                    }

                }
            },
            store: this.wfConfigStore,
            rootVisible: false,
            width: 500,
            isNodeDroppable: function (targetNodeArray, typeNode) {

                var countNodeTarget = targetNodeArray.filter(function (targetNodeName)
                { return (targetNodeName === (typeNode)); })

                if (countNodeTarget.length > 0) {
                    return true;
                } else {
                    return false;
                }

            }
        });

        //var MoveItemUp = Ext.create('Ext.menu.Item', {
        //    scope: this,
        //    iconCls: TP.util.Icon('Up'),
        //    text: i18n.t("TP_Admin_Contract:ContextMenu.MoveItemUp"),
        //    handler: function () {

        //        var msgBox = i18n.t("TP_Admin_Contract:TemplateConfirmationMessages.MoveUp");

        //        Ext.MessageBox.confirm('', msgBox, function (btn) {

        //            if (btn === 'yes') {

        //                TP.direct.Workflow.moveNodeItem(menuItem.record.data.type, menuItem.record.data.entity_Id, "UP", function (result) {

        //                    if (result.success) {

        //                        this.refreshConfig();
        //                        TP.util.info('', i18n.t("TP_Admin_Contract:ContractTemplateMessages.ItemMoved"));

        //                    } else {

        //                        TP.util.error(i18n.t("TP_Admin_Contract:ContractTemplateMessages.UpErrorMsg"));
        //                    }


        //                }, this);
        //            }
        //        }, this);

        //    }
        //});

        //var MoveItemDown = Ext.create('Ext.menu.Item', {
        //    scope: this,
        //    iconCls: TP.util.Icon('Down'),
        //    text: i18n.t("TP_Admin_Contract:ContextMenu.MoveItemDown"),
        //    handler: function () {

        //        var msgBox = i18n.t("TP_Admin_Contract:TemplateConfirmationMessages.MoveDown");

        //        Ext.MessageBox.confirm('', msgBox, function (btn) {

        //            if (btn === 'yes') {

        //                TP.direct.Workflow.moveNodeItem(menuItem.record.data.type, menuItem.record.data.entity_Id, "DOWN", function (result) {

        //                    if (result.success) {

        //                        this.refreshConfig();
        //                        TP.util.info('', i18n.t("TP_Admin_Contract:ContractTemplateMessages.ItemMoved"));

        //                    } else {

        //                        TP.util.error(i18n.t("TP_Admin_Contract:ContractTemplateMessages.DownErrorMsg"));
        //                    }


        //                }, this);
        //            }
        //        }, this);

        //    }
        //});

        var removeButton = Ext.create('Ext.menu.Item', {
            scope: this,
            iconCls: Mjc.Admin.util.Icon('close_red'),
            text: 'Supprimer cet item...',
            handler: function () {

                me.removeItem(menuItem.record);
            }
        });

        var changePositionButton = Ext.create('Ext.menu.Item', {
            scope: this,
            iconCls: Mjc.Admin.util.Icon('shuffle_blue'),
            text: 'Changer la position de l\'item',
            handler: function () {

                me.changePosition(menuItem.record);
            }
        });

        var menuItem = new Ext.menu.Menu({
            disabled: true,
            record: null,
            items: [
                    removeButton,
                    changePositionButton
            ]
        });

        this.treePanel.on('itemcontextmenu', function (view, record, item, index, event) {

            changePositionButton.setDisabled(false);

            switch (record.data.type) {
                case "MenuItem": case "HeaderRoot":
                    menuItem.setDisabled(false);
                    menuItem.record = record;
                    break;
                case "CarouselImage":
                    menuItem.setDisabled(false);
                    //changePositionButton.setDisabled(true);
                    menuItem.record = record;
                    break;
                default:
                    menuItem.setDisabled(true);
            }

            menuItem.showAt(event.getXY());
            event.stopEvent();
        }, this);

        this.menuItemGrid = Ext.create('Mjc.Admin.Design.ux.MenuItemGrid', {
            viewConfig: {
                plugins: {
                    ptype: 'gridviewdragdrop',
                    ddGroup: 'DesignConfigDD',
                    enableDrop: false
                }
            },
            border: true,
            autoLoad: true,
            title: 'MenuItem'
        });

        this.carouselImagesGrid = Ext.create('Mjc.Admin.Design.ux.CarouselImageGrid', {
            viewConfig: {
                plugins: {
                    ptype: 'gridviewdragdrop',
                    ddGroup: 'DesignConfigDD',
                    enableDrop: false
                }
            },
            border: true,
            autoLoad: true,
            title: 'Carroussel Images'
        });

        this.wfConfigTabPanel = Ext.create('Ext.tab.Panel', {
            flex: 1,
            hidden: false,
            items: [this.menuItemGrid, this.carouselImagesGrid]
        });

        this.items = [this.treePanel, this.wfConfigTabPanel];

        this.callParent(arguments);
    },
    loadDesign_Config: function (config) {

        this.wfConfigStore.getRoot().appendChild(config);
        this.setAllIconCls();
        this.treePanel.getView().refresh();

    },
    setAllIconCls: function () {

        this.wfConfigStore.getRoot().cascadeBy(function (node) {


            var nodeConfig = node.data;

            switch (nodeConfig.type) {
                case "MenuItem":
                    nodeConfig.iconCls = Mjc.Admin.util.Icon('menuItem');
                    break;
                case "HeaderRoot": case "HomePage":
                    nodeConfig.iconCls = Mjc.Admin.util.Icon('folder');
                    break;
                case "CarouselImagesRoot":
                    nodeConfig.iconCls = Mjc.Admin.util.Icon('album_red');
                    break;
                case "CarouselImage":
                    nodeConfig.iconCls = Mjc.Admin.util.Icon('image_2');
                    break;
                default:
                    //nodeConfig.iconCls = TP.util.Icon("layers");
            }
        });
    },
    applyRefreshConfig: function (refreshConfig) {

        var targetNodeId = refreshConfig.targetNodeId;
        var expandConfig = refreshConfig.expandConfig;
        var nodeTypeAdded = refreshConfig.nodeTypeAdded;

        expandConfig.forEach(function (item) {

            var node = this.wfConfigStore.getNodeById(item.id);

            if (node && item.expanded == true) {
                node.expand();
            }

        }, this);

        var targetNode = this.wfConfigStore.getNodeById(targetNodeId);

        if (targetNode) {
            if (targetNode.data.type === "WF_Config_State" && nodeTypeAdded === "WF_FunctionType") {
                targetNode.expand();
                var idfunctionNode = targetNode.data.children[0].id;
                var functionNode = this.wfConfigStore.getNodeById(idfunctionNode);
                functionNode.expand();
            }

            else if (targetNode.data.type === "WF_Config_State" && nodeTypeAdded === "WF_ExitCheckType") {
                targetNode.expand();
                var idExitNode = targetNode.data.children[1].id;
                var exitNode = this.wfConfigStore.getNodeById(idExitNode);
                exitNode.expand();
            }
            else {
                targetNode.expand();
            }


            this.treePanel.getSelectionModel().select(targetNode);
        }
        this.treePanel.getView().refresh();

        this.treePanel.unmask();
    },
    refreshConfig: function (targetNodeId, nodeTypeAdded) {

        this.treePanel.mask("Refreshing...");
        var expandArray = [];

        this.wfConfigStore.getRoot().cascadeBy(function (node) {

            expandArray.push({ id: node.data.id, expanded: node.data.expanded, hierarchyDegree: node.data.hierarchyDegree })

        }, this);

        expandArray = this.orderDescByHierarchyDegree(expandArray);

        this.wfConfigStore.getRoot().removeAll();

        this.configDataStore.load({
            refreshConfig: {
                expandConfig: expandArray,
                targetNodeId: targetNodeId,
                nodeTypeAdded: nodeTypeAdded
            }
        });



        this.treePanel.getView().refresh();

    },
    orderDescByHierarchyDegree: function (expandArray) {

        function compare(a, b) {

            if (a.hierarchyDegree == null || a.hierarchyDegree < b.hierarchyDegree)
                return -1;
            if (b.hierarchyDegree == null || a.hierarchyDegree > b.hierarchyDegree)
                return 1;

            return 0;
        }

        expandArray.sort(compare);
        expandArray.reverse();

        return expandArray;

    },
    removeItem: function (record) {

        var msgBox = 'Etes-vous sur de vouloir supprimer cet item ?';
        if (!Ext.isEmpty(record.data.childrenCount) && record.data.childrenCount > 0) {
            msgBox = 'Supprimer cet item entrainera la supression de l\'item et de ces sous-enfants, êtes-vous sur ?';
        }
        Ext.MessageBox.confirm('Supprimer item ?', msgBox, function (btn) {

            if (btn === 'yes') {

                Mjc.direct.DesignDirect.removeItemFromDesignConfig(this.Design_Config_ID, record.data.type, record.data.entity_Id, record.data.PositionItem, function (result, action) {

                    Mjc.Admin.util.noty.displayResultMessage(result.success, result.msg);
                    this.refreshConfig();

                }, this);
            }
        }, this);
    },
    changePosition: function (record) {
        var me = this;

        var changePositionPanel = Ext.create('Mjc.Admin.ux.ProductManagement.DesignChangePositionPanel', {
            api: {
                submit: Mjc.direct.DesignDirect.changePositionItem
            },
            Design_Config_ID: record.data.Design_Config_ID,
            ItemId: record.data.MenuItemId,
            Type: record.data.type,
            PositionItem: record.data.PositionItem,
            listeners: {
                scope: this,
                cancel: function () {
                    window.close();
                },
                submitSuccess: function (form, action, options) {
                    window.close();
                    Mjc.Admin.util.noty.displayResultMessage(action.result.success, action.result.msg);
                    me.refreshConfig();
                },
                submitFailure: function (form, action, options) {
                    window.close();
                    Mjc.Admin.util.noty.displayResultMessage(action.result.success, action.result.msg);
                    me.refreshConfig();
                }
            },
            buttons: [{
                text: 'Annuler',
                scope: this,
                handler: function () {
                    window.close();
                }
            }, {
                text: 'Valider',
                scope: this,
                formBind: true,
                handler: function () {
                    changePositionPanel.submit();
                }
            }]
        });

        var window = Ext.create('Ext.window.Window', {
            title: 'Changer position de l\'item',
            modal: true,
            constrain: true,
            resizable: false,
            items: [changePositionPanel],
            width: 400
        }).show();
    }
});
//#endregion

//#region Mjc.Admin.Design.ux.DesignConfigPropertyGrid
Ext.define('Mjc.Admin.Design.ux.DesignConfigPropertyGrid', {
    extend: 'Ext.grid.Panel',
    autoLoad: false,
    typeToConfig: null,
    propertyValue: null,
    selectedValueProperty: null,
    entity_Id: null,
    entity_KeyName: null,
    Design_Config_ID: null,
    selType: 'cellmodel',
    plugins: {
        ptype: 'cellediting',
        clicksToEdit: 1,
        listeners: {
            beforeedit: function (e, editor) {
                if (editor.record.data.isMultiSelect == true)
                    return false;
            }
        }
    },
    initComponent: function () {

        this.listeners = {
            select: function (gridview, record, index, eOpts) {

                var columnValue = this.getColumns("Value");

                //var extType = record.data.isMultiSelect ? 'Ext.form.field.Tag' : 'Ext.form.field.ComboBox';
                var selectedValue = null;
                var propertyName = record.data.Name;

                switch (record.data.FieldType) {
                    case 'MultiSelect':
                        columnValue.setEditor();
                        this.fireEvent('loadGridValues', record);
                        break;
                    case 'SingleSelect':
                        this.fireEvent('hideGridValues', record);
                        columnValue.setEditor(Ext.create('Ext.form.field.ComboBox', {
                            xtype: 'combo',
                            allowBlank: false,
                            forceSelection: true,
                            store: Ext.create('Ext.data.Store', {
                                fields: ['valueField', 'displayField'],
                                data: record.data.AvailableValues
                            }),
                            triggerAction: 'all',
                            displayField: "displayField",
                            valueField: "valueField",
                            editable: false,
                            listeners: {
                                scope: this,
                                select: function (combo, record, eOpts) {

                                    var selectedValue = record;
                                    this.selectedValueProperty = selectedValue;

                                    this.fireEvent('updateProperty', propertyName, selectedValue.data.valueField);
                                }
                            }
                        }));
                        break;
                    case 'Text':
                        this.fireEvent('hideGridValues', record);
                        columnValue.setEditor(Ext.create('Ext.form.field.Text', {
                            allowBlank: false,
                            value: record.data.Value,
                            listeners: {
                                scope: this,
                                change: function (field, newValue, oldValue, eOpts) {
                                    this.fireEvent('updateProperty', propertyName, newValue);
                                },
                                buffer: 800
                            }
                        }));
                        break;
                    case 'Number':
                        this.fireEvent('hideGridValues', record);
                        columnValue.setEditor(Ext.create('Ext.form.field.Number', {
                            minValue: 0,
                            allowBlank: false,
                            value: record.data.Value,
                            listeners: {
                                scope: this,
                                change: function (field, newValue, oldValue, eOpts) {
                                    this.fireEvent('updateProperty', propertyName, newValue);
                                },
                                buffer: 800
                            }
                        }));
                        break;
                    default:
                        break;
                }
            }
        };

        this.columns = [
            {
                text: 'Name',
                width: 150,
                dataIndex: 'Name',
                renderer: function (value, metaData, record, rowIndex, colIndex, store) {
                    metaData.tdAttr = 'data-qtip="' + record.data.Description + '"';
                    return record.data.Name;
                }
            },
            {
                text: 'Value',
                flex: 1,
                dataIndex: 'Value',
                renderer: function (value, metaData, record, rowIndex, colIndex, store) {
                    metaData.tdAttr = 'data-qtip="' + value + '"';
                    return value;
                }
            }
        ];

        this.store = Ext.create('Mjc.Admin.Design.store.DesignMenuItemProperty_Set', {
            filters: [{ id: 'entity_KeyName', property: this.entity_KeyName, value: this.entity_Id }, { id: 'Design_Config_ID', property: 'Design_Config_ID', value: this.Design_Config_ID }],
            autloLoad: true,
            listeners: {
                scope: this,
                load: function (store, records, successful, eOpts) {

                    if (!Ext.isEmpty(eOpts.refreshConfig)) {

                        if (Ext.isFunction(eOpts.refreshConfig.callback)) {

                            eOpts.refreshConfig.callback();
                        }
                    }
                }
            }
        });

        this.callParent(arguments);
    },
    getColumns: function (columnName) {

        var column = null;

        if (columnName == null) {
            return null;
        } else {
            for (var i = 0; i < this.columns.length; i++) {
                var col = this.columns[i];
                if (col.text == columnName)
                    column = col;
            }

            return column;
        }
    }
});
//#endregion

//#region Mjc.Admin.Design.ux.DesignConfigPropertyPanel
Ext.define('Mjc.Admin.Design.ux.DesignConfigPropertyPanel', {
    extend: 'Ext.panel.Panel',
    //closable: true,
    hidden: false,
    record: null,
    itemName: null,
    layout: {
        type: 'vbox',
        align: 'stretch'
    },
    initComponent: function () {
        var me = this;

        this.propertyGrid = Ext.create('Mjc.Admin.Design.ux.DesignConfigPropertyGrid', {
            title: this.record.data.type + ' for ' + this.itemName,
            iconCls: Mjc.Admin.util.Icon('menuItem'),
            typeToConfig: this.record.data.type,
            entity_Id: this.record.data.MenuItemId,
            entity_KeyName: this.record.data.type,
            Design_Config_ID: this.record.data.Design_Config_ID,
            flex: 0.5,
            propertyValue: this.record.data.text,
            selectRowOrDefault: function (propertyName) {
                var propertyRecord = this.getStore().findRecord('Name', propertyName);
                if (propertyRecord) {
                    this.getSelectionModel().select(propertyRecord);
                } else {
                    this.getSelectionModel().select(0);
                }
            }
        });

        this.propertyGrid.on('updateProperty', function (propertyName, value) {

            Mjc.direct.DesignDirect.updateActionEngineConfig(this.record.data.Design_Config_ID, this.propertyGrid.entity_KeyName, propertyName, this.propertyGrid.entity_Id, value, null, function (success, b) {

                if (success) {
                    this.propertyGrid.getStore().reload();
                    this.propertyGrid.getView().refresh();
                }
            }, this);

        }, this);



        this.propertyGridValues = Ext.create('Ext.grid.Panel', {
            store: Ext.create('Ext.data.Store', {
                model: Ext.create('Ext.data.Model')
            }),
            entity_Id: this.record.data.MenuItemId,
            propertyName: null,
            closable: false,
            hidden: true,
            flex: 1,
            columns: [
                {
                    text: 'Value',
                    flex: 0.8,
                    dataIndex: 'Value',
                },
                {
                    xtype: 'checkcolumn',
                    flex: 0.2,
                    dataIndex: 'Checked',
                    listeners: {
                        scope: this,
                        checkchange: function (checkbox, rowIndex, checked, eOpts) {

                            var valueString = this.propertyGridValues.getStore().getAt(rowIndex).data.Value;

                            Mjc.direct.DesignDirect.updateActionEngineConfig(this.record.data.Design_Config_ID, this.propertyGrid.entity_KeyName, this.propertyGridValues.propertyName, this.propertyGridValues.entity_Id, valueString, checked, function (success) {
                                if (success) {
                                    this.propertyGridValues.fireEvent('checkboxChanged');
                                }
                            }, this);
                        }
                    }
                }],
            listeners: {
                beforeclose: function (me) {
                    me.setHidden(true);
                    return false;
                }
            },
            loadCheckedValues: function (record) {
                this.record = record;
                this.getStore().loadData(record.data.AvailableValuesChecked);
                this.setTitle("Values for property " + record.data.Name);
                this.propertyName = record.data.Name;
                this.getView().refresh();
                this.setHidden(false);
            }
        });


        this.propertyGrid.on('loadGridValues', function (record) {
            this.propertyGridValues.show();
            this.propertyGridValues.mask('Loading...');
            this.propertyGridValues.loadCheckedValues(record);
            this.propertyGridValues.unmask();

        }, this);

        this.propertyGrid.on('hideGridValues', function (record) {
            this.propertyGridValues.hide();
        }, this);

        this.propertyGridValues.on('checkboxChanged', function (record) {
            console.log("checkboxChanged:toto");
            var refreshConfig = {
                callback: function () {
                    me.propertyGrid.selectRowOrDefault(me.propertyGridValues.record.data.Name);
                }
            };
            this.propertyGrid.getStore().reload(refreshConfig);
        }, this);

        this.items = [this.propertyGrid, this.propertyGridValues];

        this.callParent(arguments);
    }
});
//#endregion

//#region Mjc.Admin.Design.ux.MenuItemGrid
Ext.define('Mjc.Admin.Design.ux.MenuItemGrid', {
    extend: 'Ext.grid.Panel',
    autoLoad: null,
    border: false,
    //WF_Config_ID: null,
    initComponent: function () {

        this.columns = [
                    { xtype: 'rownumberer' },
                    { text: "Name", flex: 1, dataIndex: 'Name' },
        ];

        this.store = Ext.create('Mjc.Admin.Design.store.MenuItem_Set', {
            autoLoad: false,
            //filters: [{ property: 'WF_Config_ID', value: this.WF_Config_ID }]
        });

        this.callParent(arguments);
    },
});
//#endregion

//#region Mjc.Admin.Design.ux.DesignCreationPanel
Ext.define('Mjc.Admin.Design.ux.DesignCreationPanel', {
    extend: 'Mjc.Admin.widget.ux.directForm.Panel',
    defaults: {
        margin: 20,
        width: '80%'
    },
    initComponent: function () {

        this.nameTextField = Ext.create('Ext.form.field.Text', {
            allowBlank: false,
            fieldLabel: 'Nom de la config',
            name: 'Name',
            cls: 'formFieldEditableCls'
        });

        this.descriptionTextArea = Ext.create('Ext.form.field.TextArea', {
            allowblank: true,
            fieldLabel: 'Description',
            name: 'Description',
            height: 250,
            cls: 'formFieldEditableCls'
        });

        this.items = [this.nameTextField, this.descriptionTextArea];

        this.callParent(arguments);
    }
});
//#endregion

//#region Mjc.Admin.Design.ux.DesignChangePositionPanel
Ext.define('Mjc.Admin.ux.ProductManagement.DesignChangePositionPanel', {
    extend: 'Mjc.Admin.widget.ux.directForm.Panel',
    Design_Config_ID: null,
    ItemId: null,
    Type: null,
    PositionItem: null,
    defaults: {
        margin: 20,
        width: '80%'
    },
    initComponent: function () {

        //this.description = Ext.create('Ext.form.field.Display', {
        //    name: 'Description',
        //    margin: 20,
        //    value: "Pour information, la position du premier élément débute à 0"
        //});

        this.designIdHidden = Ext.create('Ext.form.field.Hidden', {
            name: 'Design_Config_ID',
            value: this.Design_Config_ID
        });

        this.itemIdHidden = Ext.create('Ext.form.field.Hidden', {
            name: 'itemId',
            value: this.ItemId
        });

        this.typeHidden = Ext.create('Ext.form.field.Hidden', {
            name: 'targetItemType',
            value: this.Type
        });

        this.PositionItemInTreeHidden = Ext.create('Ext.form.field.Hidden', {
            name: 'PositionItem',
            value: this.PositionItem
        });

        this.numberPositionField = Ext.create('Ext.form.field.Number', {
            allowBlank: false,
            minValue: 1,
            fieldLabel: 'Entrez la position de l\'item',
            name: 'newIndexPosition',
            cls: 'formFieldEditableCls'
        });


        this.items = [/*this.description,*/ this.numberPositionField, this.designIdHidden, this.itemIdHidden, this.typeHidden, this.PositionItemInTreeHidden];

        this.callParent(arguments);
    }
});
//#endregion

//#region Mjc.Admin.Design.ux.EtiquettePanel
Ext.define('Mjc.Admin.Design.ux.EtiquettePanel', {
    extend: 'Mjc.Admin.widget.ux.directForm.Panel',
    constructor: function (cfg) {

        var me = this;
        cfg = cfg || {
        };
        me.callParent([Ext.apply({
            frame: false,
            bodyPadding: 10,
            border: false,
            bodyStyle: {
                background: 'transparent'
            },
            defaults: {
                anchor: '100%',
                labelWidth: 100
            }
        }, cfg)]);
    },
    initComponent: function () {


        var config = {
            items: [{
                xtype: 'textfield',
                allowBlank: false,
                name: 'Etiquette_Name',
                fieldLabel: 'Nom de l\'Etiquette',
            },
            {
                xtype: 'filefield',
                name: 'file',
                fieldLabel: 'Attacher une image',
                msgTarget: 'side',
                allowBlank: false,
                anchor: '100%',
                width: '100%',
                buttonText: 'Parcourir...',
            }]
        }
        Ext.apply(this, config);
        this.callParent(arguments);
    }
});
//#endregion

//#region Mjc.Admin.Design.ux.DeleteEtiquettePanel
Ext.define('Mjc.Admin.Design.ux.DeleteEtiquettePanel', {
    extend: 'Mjc.Admin.widget.ux.directForm.Panel',
    defaults: {
        margin: 20,
        width: '80%'
    },
    initComponent: function () {

        this.comboBoxEtiquette = Ext.create('Mjc.Admin.Design.ux.EtiquetteComboBox', {
            allowBlank: false,
            fieldLabel: 'Etiquette',
            name: 'Etiquette_ID',
            emptyMessage: 'Selectionnez une étiquette à supprimer...',
            cls: 'formFieldEditableCls'
        });

        this.items = [this.comboBoxEtiquette];

        this.callParent(arguments);
    }
});
//#endregion

//#region  Mjc.Admin.Design.ux.EtiquetteComboBox
Ext.define('Mjc.Admin.Design.ux.EtiquetteComboBox', {
    extend: 'Ext.form.ComboBox',
    constructor: function (cfg) {
        var me = this;
        cfg = cfg || {};
        me.callParent([Ext.apply({
            //allowBlank: false,
            name: 'Etiquette_ID',
            fieldLabel: 'Etiquette',
            store: Ext.create('Mjc.Admin.Design.store.Etiquette_Set', {
                autoLoad: true
            }),
            queryMode: 'local',
            remoteFilter: true,
            minChars: 2,
            forceSelection: true,
            typeAhead: true,
            emptyText: 'Choisissez une étiquette..',
            triggerAction: 'all',
            displayField: 'Etiquette_Name',
            valueField: 'Etiquette_ID'
        }, cfg)]);
    }
});
//#endregion

//#region Mjc.Admin.Design.ux.CarouselImagePanel
Ext.define('Mjc.Admin.Design.ux.CarouselImagePanel', {
    extend: 'Mjc.Admin.widget.ux.directForm.Panel',
    constructor: function (cfg) {

        var me = this;
        cfg = cfg || {
        };
        me.callParent([Ext.apply({
            frame: false,
            bodyPadding: 10,
            border: false,
            bodyStyle: {
                background: 'transparent'
            },
            defaults: {
                anchor: '100%',
                labelWidth: 100
            }
        }, cfg)]);
    },
    initComponent: function () {

        var config = {
            items: [{
                xtype: 'textfield',
                allowBlank: false,
                name: 'CarouselImage_Name',
                fieldLabel: 'Nom du Carroussel Image',
            },
            {
                xtype: 'filefield',
                name: 'file',
                fieldLabel: 'Attacher une image',
                msgTarget: 'side',
                allowBlank: false,
                anchor: '100%',
                width: '100%',
                buttonText: 'Parcourir...',
            }]
        }
        Ext.apply(this, config);
        this.callParent(arguments);
    }
});
//#endregion

//#region Mjc.Admin.Design.ux.DeleteCarouselImagePanel
Ext.define('Mjc.Admin.Design.ux.DeleteCarouselImagePanel', {
    extend: 'Mjc.Admin.widget.ux.directForm.Panel',
    defaults: {
        margin: 20,
        width: '80%'
    },
    initComponent: function () {

        this.comboBoxEtiquette = Ext.create('Mjc.Admin.Design.ux.CarouselImageComboBox', {
            allowBlank: false,
            fieldLabel: 'Carroussel Image',
            name: 'CarouselImage_ID',
            emptyMessage: 'Selectionnez un Carroussel Image à supprimer...',
            cls: 'formFieldEditableCls'
        });

        this.items = [this.comboBoxEtiquette];

        this.callParent(arguments);
    }
});
//#endregion

//#region  Mjc.Admin.Design.ux.CarouselImageComboBox
Ext.define('Mjc.Admin.Design.ux.CarouselImageComboBox', {
    extend: 'Ext.form.ComboBox',
    constructor: function (cfg) {
        var me = this;
        cfg = cfg || {};
        me.callParent([Ext.apply({
            //allowBlank: false,
            name: 'CarouselImage_ID',
            fieldLabel: 'Carroussel Image',
            store: Ext.create('Mjc.Admin.Design.store.CarouselImage_Set', {
                autoLoad: true
            }),
            queryMode: 'local',
            remoteFilter: true,
            minChars: 2,
            forceSelection: true,
            typeAhead: true,
            emptyText: 'Choisissez un carroussel..',
            triggerAction: 'all',
            displayField: 'CarouselImage_Name',
            valueField: 'CarouselImage_ID'
        }, cfg)]);
    }
});
//#endregion

//#region Mjc.Admin.Design.ux.CarouselImageGrid
Ext.define('Mjc.Admin.Design.ux.CarouselImageGrid', {
    extend: 'Ext.grid.Panel',
    autoLoad: null,
    border: false,
    initComponent: function () {

        this.columns = [
                    { xtype: 'rownumberer' },
                    { text: "Name", flex: 1, dataIndex: 'CarouselImage_Name' },
        ];

        this.store = Ext.create('Mjc.Admin.Design.store.CarouselImage_Set', {
            autoLoad: false
        });


        this.dockedItems = [{
            xtype: 'pagingtoolbar',
            store: this.store,
            dock: 'bottom',
            displayInfo: true,
            enableOverflow: true
        }];

        this.callParent(arguments);
    },
});
//#endregion

