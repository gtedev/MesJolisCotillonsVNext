Ext.ns('Mjc.Admin.ux.ProductManagement');

//#region Mjc.Admin.ux.ProductManagement.MainPanel
Ext.define('Mjc.Admin.ux.ProductManagement.MainPanel', {
    extend: 'Ext.panel.Panel',
    layout: {
        type: 'hbox',
        align: 'stretch'
    },
    title: 'Produits',
    initComponent: function () {

        var me = this;

        this.listeners = {
            activate: function (panel, eOpts) {
                if (!me.productGrid.getSelectionModel().hasSelection() && me.productGrid.getStore().getTotalCount()) {
                    me.productGrid.getSelectionModel().select(0);
                }
            }
        };


        this.productGrid = Ext.create('Mjc.Admin.ux.ProductManagement.GridPanel', {
            flex: 0.5,
            collapseDirection: 'left',
            listeners: {
                scope: this,
                load: function (store, records, successful, operation, eOpts) {
                    var hasStoreRecords = store.getTotalCount() > 0;
                    this.productPanel.setHidden(!hasStoreRecords);
                    this.emptyPanel.setHidden(hasStoreRecords);
                },
                select: function (grid, record, index, eOpts) {
                    this.productPanel.loadProduct(record.data.Product_ID);
                },
                activateProduct: function (Product_ID) {
                    me.mask("Chargement...  ");
                    Mjc.direct.ProductDirect.activateProduct(Product_ID, function (result) {
                        me.unmask();
                        Mjc.Admin.util.noty.displayResultMessage(result.success, result.msg);
                        me.productGrid.reload();
                    }, this);
                },
                deactivateProduct: function (Product_ID) {
                    me.mask("Chargement...  ");
                    Mjc.direct.ProductDirect.deactivateProduct(Product_ID, function (result) {
                        me.unmask();
                        Mjc.Admin.util.noty.displayResultMessage(result.success, result.msg);
                        me.productGrid.reload();
                    }, this);
                },
                deleteProduct: function (Product_ID) {
                    me.mask("Chargement...  ");
                    Mjc.Admin.util.MessageBox.confirm('Confirmation', 'Voulez-vous vraiment supprimer définitivement ce produit ? Notez que cela entrainera également la suppression définitive des images reliés.', function (btn) {
                        if (btn === 'yes') {
                            Mjc.direct.ProductDirect.deleteProduct(Product_ID, function (result) {
                                me.unmask();
                                Mjc.Admin.util.noty.displayResultMessage(result.success, result.msg);
                                me.productGrid.reload();
                            }, this);
                        }
                    }, this);


                }
            }
        });

        this.emptyPanel = Ext.create('Ext.panel.Panel', {
            flex: 0.5,
            hidden: false
        });

        this.productPanel = Ext.create('Mjc.Admin.ux.ProductManagement.ProductMainPanel', {
            flex: 0.5,
            title: '',
            hidden: true
        });

        this.items = [this.productGrid, this.productPanel, this.emptyPanel];
        this.callParent(arguments);
    }
});
//#endregion

//#region Mjc.Admin.ux.ProductManagement.GridPanel
Ext.define('Mjc.Admin.ux.ProductManagement.GridPanel', {
    extend: 'Ext.grid.Panel',
    title: 'Produits',
    border: false,
    //margin: '30 0 0 0',
    header: false,
    initComponent: function () {

        var me = this;
        this.plugins = [{ ptype: 'simplegridfilter', fieldName: 'SearchProductFilter', emptyTextField: 'Rechercher...', widthField: 200 }];

        this.columns = [{
            text: 'Nom',
            dataIndex: 'Name',
            flex: 1
        },
        {
            text: 'Catégorie',
            dataIndex: 'Category',
            flex: 0.7
        },
        {
            text: 'Nom affiché',
            dataIndex: 'DisplayName',
            flex: 1
        },
        {
            text: 'Description',
            dataIndex: 'Description',
            flex: 1,
            hidden: true
        },
        {
            text: 'Prix Unitaire',
            dataIndex: 'Price',
            flex: 0.5
        },
        {
            text: 'Statut',
            dataIndex: 'Status',
            flex: 0.5
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
                //    {
                //    text: 'Editer produit...',
                //    iconCls: Mjc.Admin.util.Icon('bullet_edit')
                //},
                {
                    hidden: !record.data.userCanDeactivateProduct,
                    text: 'Désactiver produit...',
                    iconCls: Mjc.Admin.util.Icon('switch_off'),
                    handler: function () {
                        me.fireEvent('deactivateProduct', record.data.Product_ID)
                    }
                },
                {
                    hidden: !record.data.userCanActiveProduct,
                    text: 'Activer produit...',
                    iconCls: Mjc.Admin.util.Icon('switch_on'),
                    handler: function () {
                        me.fireEvent('activateProduct', record.data.Product_ID)
                    }
                },
                {
                    text: 'Supprimer produit...',
                    iconCls: Mjc.Admin.util.Icon('error'),
                    handler: function () {
                        me.fireEvent('deleteProduct', record.data.Product_ID)
                    }
                }];


                widget.setMenu(menu);
            }
        }];

        this.store = Ext.create('Mjc.Admin.store.ProductManagement.Product_Set', {
            sorters: [{
                property: 'Name',
                direction: 'ASC'
            }],
            listeners: {
                scope: this,
                load: function (store, records, successful, operation, eOpts) {
                    if (store.getTotalCount() > 0) {
                        //me.getSelectionModel().select(0);
                    }
                    me.fireEvent('load', store, records, successful, operation, eOpts);
                }
            }
        });

        //this.listeners = {
        //    scope: this,
        //    itemclick: function (grid, record, item, index, event, eOpts) {
        //        this.fireEvent('itemclick', grid, record);
        //    }
        //};

        this.dockedItems = [{
            xtype: 'toolbar',
            dock: 'top',
            items: [{
                xtype: 'button',
                text: 'Ajouter un produit',
                iconCls: Mjc.Admin.util.Icon('add'),
                margin: 10,
                handler: function () {

                    var productPanel = Ext.create('Mjc.Admin.ux.ProductManagement.ProductInformationPanel', {
                        api: {
                            submit: Mjc.direct.ProductDirect.createProduct
                        },
                        autoScroll: true,
                        listeners: {
                            scope: this,
                            cancel: function () {
                                window.close();
                            },
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
                            iconCls: Mjc.Admin.util.Icon('error'),
                            scope: this,
                            handler: function () {
                                window.close();
                            }
                        }, {
                            text: 'Créér',
                            iconCls: Mjc.Admin.util.Icon('next'),
                            scope: this,
                            formBind: true,
                            handler: function () {
                                productPanel.submit();
                            }
                        }]
                    });
                    productPanel.getProduct_IdDisplay().setHidden(true);

                    var window = Ext.create('Ext.window.Window', {
                        layout: 'fit',
                        title: 'Ajouter un produit',
                        modal: true,
                        autoScroll: true,
                        constrain: true,
                        resizable: false,
                        items: [productPanel],
                        height: '70%',
                        width: '50%'
                    }).show();
                }
            },
            {
                xtype: 'button',
                text: 'Gérer catégorie...',
                iconCls: Mjc.Admin.util.Icon('add_purple'),
                margin: 10,
                menu: [{
                    iconCls: Mjc.Admin.util.Icon('edit'),
                    text: 'Modifier une catégorie...',
                    handler: function () {

                        var modifyCategoryPanel = Ext.create('Mjc.Admin.ux.ProductManagement.CategoryEditPanel', {
                            autoScroll: true,
                            height: 500
                        });

                        var window = Ext.create('Ext.window.Window', {
                            title: 'Modifier une catégorie',
                            modal: true,
                            constrain: true,
                            resizable: false,
                            autoScroll: true,
                            items: [modifyCategoryPanel],
                            //height: '70%',
                            width: '70%'
                        }).show();
                    }
                },
                {
                    iconCls: Mjc.Admin.util.Icon('add_purple'),
                    text: 'Ajouter une catégorie...',
                    handler: function () {

                        var addCategoryPanel = Ext.create('Mjc.Admin.ux.ProductManagement.CategoryPanel', {
                            api: {
                                submit: Mjc.direct.ProductDirect.createCategory
                            },
                            listeners: {
                                scope: this,
                                cancel: function () {
                                    window.close();
                                },
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
                                text: 'Créér',
                                scope: this,
                                formBind: true,
                                handler: function () {
                                    addCategoryPanel.submit();
                                }
                            }]
                        });

                        var window = Ext.create('Ext.window.Window', {
                            title: 'Ajouter une catégorie',
                            modal: true,
                            constrain: true,
                            resizable: false,
                            items: [addCategoryPanel],
                            width: 400
                        }).show();
                    }
                }, {
                    iconCls: Mjc.Admin.util.Icon('close_red'),
                    text: 'Supprimer une catégorie...',
                    handler: function () {

                        var deleteCategoryPanel = Ext.create('Mjc.Admin.ux.ProductManagement.DeleteCategoryPanel', {
                            api: {
                                submit: Mjc.direct.ProductDirect.deleteCategory
                            },
                            listeners: {
                                scope: this,
                                cancel: function () {
                                    window.close();
                                },
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
                                    deleteCategoryPanel.submit();
                                }
                            }]
                        });

                        var window = Ext.create('Ext.window.Window', {
                            title: 'Supprimer une catégorie',
                            modal: true,
                            constrain: true,
                            resizable: false,
                            items: [deleteCategoryPanel],
                            width: 400
                        }).show();
                    }
                }]
            },
            {
                xtype: 'button',
                text: 'Gérer les mots clés...',
                iconCls: Mjc.Admin.util.Icon('hashtag'),
                margin: 10,
                menu: [{
                    iconCls: Mjc.Admin.util.Icon('hashtag'),
                    text: 'Ajouter un mot clé...',
                    handler: function () {

                        var addKeyWordPanel = Ext.create('Mjc.Admin.ux.ProductManagement.KeyWordPanel', {
                            api: {
                                submit: Mjc.direct.ProductDirect.createKeyWord
                            },
                            listeners: {
                                scope: this,
                                cancel: function () {
                                    window.close();
                                },
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
                                text: 'Créér',
                                scope: this,
                                formBind: true,
                                handler: function () {
                                    addKeyWordPanel.submit();
                                }
                            }]
                        });

                        var window = Ext.create('Ext.window.Window', {
                            title: 'Ajouter un mot clé',
                            modal: true,
                            constrain: true,
                            resizable: false,
                            items: [addKeyWordPanel],
                            width: 400
                        }).show();
                    }
                }, {
                    iconCls: Mjc.Admin.util.Icon('close_red'),
                    text: 'Supprimer un mot clé...',
                    handler: function () {

                        var deleteKeyWordPanel = Ext.create('Mjc.Admin.ux.ProductManagement.DeleteKeyWordPanel', {
                            api: {
                                submit: Mjc.direct.ProductDirect.deleteKeyWord
                            },
                            listeners: {
                                scope: this,
                                cancel: function () {
                                    window.close();
                                },
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
                                    deleteKeyWordPanel.submit();
                                }
                            }]
                        });

                        var window = Ext.create('Ext.window.Window', {
                            title: 'Supprimer un mot clé...',
                            modal: true,
                            constrain: true,
                            resizable: false,
                            items: [deleteKeyWordPanel],
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
                enableOverflow: true
            }]
        this.callParent(arguments);
    },
    reload: function () {
        this.store.reload();
    }
});
//#endregion

//#region Mjc.Admin.ux.ProductManagement.ProductInformationPanel
Ext.define('Mjc.Admin.ux.ProductManagement.ProductInformationPanel', {
    extend: 'Mjc.Admin.widget.ux.directForm.Panel',
    Product_ID: null,
    defaults: {
        margin: 20,
        width: '60%'
    },
    initComponent: function () {

        var me = this;

        this.product_IdDisplay = Ext.create('Ext.form.field.Display', {
            fieldLabel: 'Product_ID',
            //name: 'Product_ID'
        });

        this.product_IdHidden = Ext.create('Ext.form.field.Hidden', {
            fieldLabel: 'Product_ID',
            name: 'Product_ID'
        });

        //this.categoryProductCombo = Ext.create('Mjc.Admin.ux.ProductManagement.CategoryComboBox');
        this.categoryProductCombo = Ext.create('Mjc.Admin.ux.ProductManagement.ProductCategoriesField', {
            cls: 'formFieldEditableCls',
            allowBlank: false
        });

        this.nameTextField = Ext.create('Ext.form.field.Text', {
            allowBlank: false,
            fieldLabel: 'Nom du produit',
            name: 'Name',
            cls: 'formFieldEditableCls'
        });

        //this.productFlagsField = Ext.create('Mjc.Admin.ux.ProductManagement.ProductFlagsField', {
        //    //allowBlank: false,
        //    cls: 'formFieldEditableCls'
        //});  

        this.productKeyWordsField = Ext.create('Mjc.Admin.ux.ProductManagement.ProductKeyWordsField', {
            //allowBlank: false,
            cls: 'formFieldEditableCls'
        });

        this.displayNameTextField = Ext.create('Ext.form.field.Text', {
            fieldLabel: 'Nom à afficher',
            name: 'DisplayName',
            cls: 'formFieldEditableCls'
        });

        this.descriptionTextArea = Ext.create('Ext.form.field.TextArea', {
            allowBlank: false,
            fieldLabel: 'Description',
            name: 'Description',
            height: 250,
            cls: 'formFieldEditableCls'
        });

        this.stockQuantity = Ext.create('Ext.form.field.Number', {
            //margin: '50 20 20 20',
            allowBlank: false,
            minValue: 0,
            fieldLabel: 'Quantité',
            name: 'StockQuantity',
            cls: 'formFieldEditableCls'
        });

        this.productWeightField = Ext.create('Ext.form.field.Number', {
            allowBlank: false,
            minValue: 0,
            maxValue: 30000,
            fieldLabel: 'Poids (g)',
            name: 'ProductWeight',
            cls: 'formFieldEditableCls'
        });

        this.productHeightField = Ext.create('Ext.form.field.Number', {
            margin: '50 20 20 20',
            //allowBlank: false,
            minValue: 0,
            fieldLabel: 'Hauteur (mm)',
            name: 'ProductHeight',
            cls: 'formFieldEditableCls'
        });

        this.productWidthField = Ext.create('Ext.form.field.Number', {
            //allowBlank: false,
            minValue: 0,
            fieldLabel: 'Largeur (mm)',
            name: 'ProductWidth',
            cls: 'formFieldEditableCls'
        });

        this.productDepthField = Ext.create('Ext.form.field.Number', {
            //allowBlank: false,
            minValue: 0,
            fieldLabel: 'Profondeur (mm)',
            name: 'ProductDepth',
            cls: 'formFieldEditableCls'
        });

        this.productDiameterField = Ext.create('Ext.form.field.Number', {
            //allowBlank: false,
            minValue: 0,
            fieldLabel: 'Diamètre (mm)',
            name: 'ProductDiameter',
            cls: 'formFieldEditableCls'
        });

        this.priceTextField = Ext.create('Ext.form.field.Number', {
            margin: '50 20 20 20',
            allowBlank: false,
            minValue: 0,
            fieldLabel: 'Prix',
            name: 'Price',
            cls: 'formFieldEditableCls'
        });

        this.productTypeCombo = Ext.create('Mjc.Admin.ux.ProductManagement.ProductTypeComboBox', {
            margin: '50 20 20 20',
            allowBlank: false,
            value: 1,
            cls: 'formFieldEditableCls',
            listeners: {
                change: function (combo, newValue, oldValue, eOpts) {
                    if (newValue == 1) {  //UNIQUE
                        //me.stockQuantity.setValue(1);
                        me.stockQuantity.addCls('x-item-disabled');
                        me.stockQuantity.setReadOnly(true);
                    } else {
                        me.stockQuantity.setReadOnly(false);
                        me.stockQuantity.removeCls('x-item-disabled');
                    }
                }
            }
        });

        this.productFragilityCombo = Ext.create('Mjc.Admin.ux.ProductManagement.ProductFragilityComboBox', {
            margin: '50 20 20 20',
            allowBlank: false,
            cls: 'formFieldEditableCls'
        });

        this.productDeliveryBoxTypeComboBo = Ext.create('Mjc.Admin.ux.ProductManagement.ProductDeliveryBoxTypeComboBox', {
            allowBlank: false,
            cls: 'formFieldEditableCls'
        });

        this.items = [this.product_IdHidden, this.product_IdDisplay, this.categoryProductCombo, this.productKeyWordsField, this.nameTextField, this.displayNameTextField, this.descriptionTextArea, /*this.productFlagsField,*/, this.productTypeCombo, this.stockQuantity,
         this.productFragilityCombo, this.productDeliveryBoxTypeComboBo, this.productHeightField, this.productWidthField, this.productDepthField, this.productDiameterField, this.productWeightField, this.priceTextField];

        this.on('loadSuccess', function (form, action, options) {

            var record = action.result;
            this.product_IdDisplay.setValue(this.product_IdHidden.getValue());
            //if (!Ext.isEmpty(action.result.data.Category_FK)) {
            //    var record = this.categoryProductCombo.getStore().findRecord('Category_ID', action.result.data.Category_FK);
            //    if (record != null)
            //        this.categoryProductCombo.select(record);
            //}


            if (record.data.Type == 1) {
                me.stockQuantity.addCls('x-item-disabled');
                me.stockQuantity.setReadOnly(true);
            } else {
                me.stockQuantity.removeCls('x-item-disabled');
                me.stockQuantity.setReadOnly(false);
            }

        }, this)

        this.on('activate', function (form, action, options) {
            if (!Ext.isEmpty(this.Product_ID)) {
                this.categoryProductCombo.getStore().reload();
                this.productKeyWordsField.getStore().reload();
                this.load(this.Product_ID);
            }
        }, this)

        this.callParent(arguments);
    },
    getProduct_IdDisplay: function () {
        return this.product_IdDisplay;
    },
    reloadPanel: function () {
        this.categoryProductCombo.getStore().reload();
        this.productKeyWordsField.getStore().reload();
        this.load(this.Product_ID);
    }
});
//#endregion

//#region Mjc.Admin.ux.ProductManagement.CategoryPanel
Ext.define('Mjc.Admin.ux.ProductManagement.CategoryPanel', {
    extend: 'Mjc.Admin.widget.ux.directForm.Panel',
    defaults: {
        margin: 20,
        width: '80%'
    },
    initComponent: function () {

        this.categoryIdHidden = Ext.create('Ext.form.field.Hidden', {
            fieldLabel: 'Category_ID',
            name: 'Category_ID'
        });

        this.nameTextField = Ext.create('Ext.form.field.Text', {
            allowBlank: false,
            fieldLabel: 'Catégorie',
            name: 'Name',
            cls: 'formFieldEditableCls'
        });

        this.displayNameTextField = Ext.create('Ext.form.field.Text', {
            allowBlank: false,
            fieldLabel: 'Catégorie à afficher',
            name: 'DisplayName',
            cls: 'formFieldEditableCls'
        });

        this.comboBoxEtiquette = Ext.create('Mjc.Admin.Design.ux.EtiquetteComboBox', {
            allowBlank: true,
            fieldLabel: 'Etiquette',
            name: 'Etiquette_FK',
            emptyMessage: 'Selectionnez une étiquette...',
            cls: 'formFieldEditableCls'
        });

        this.on('loadSuccess', function (form, action, options) {

        }, this)

        this.items = [this.categoryIdHidden, this.nameTextField, this.displayNameTextField, this.comboBoxEtiquette];

        this.callParent(arguments);
    }
});
//#endregion

//#region Mjc.Admin.ux.ProductManagement.DeleteCategoryPanel
Ext.define('Mjc.Admin.ux.ProductManagement.DeleteCategoryPanel', {
    extend: 'Mjc.Admin.widget.ux.directForm.Panel',
    defaults: {
        margin: 20,
        width: '80%'
    },
    initComponent: function () {

        this.comboBoxCategory = Ext.create('Mjc.Admin.ux.ProductManagement.CategoryComboBox', {
            allowBlank: false,
            fieldLabel: 'Catégorie',
            name: 'Category_ID',
            emptyMessage: 'Selectionnez une catégorie à supprimer...',
            cls: 'formFieldEditableCls'
        });

        this.items = [this.comboBoxCategory];

        this.callParent(arguments);
    }
});
//#endregion

//#region Mjc.Admin.ux.ProductManagement.ImagesGridPanel
Ext.define('Mjc.Admin.ux.ProductManagement.ImagesGridPanel', {
    extend: 'Ext.grid.Panel',
    title: 'Images',
    border: false,
    Product_ID: null,
    //margin: '30 0 0 0',
    header: false,
    initComponent: function () {

        var me = this;

        var rendererImg = function (value, metaData, record, rowIndex, colIndex, store) {

            return '<img  height="50" width="50" src="' + '/ProductDirect/GetImageBlob/' + record.data.Blob_ID + '" />';
        };

        this.columns = [
            {
                text: 'Aperçu',
                dataIndex: 'Blob_ID',
                flex: 0.2,
                renderer: rendererImg
            },
            {
                text: 'Blob_ID',
                dataIndex: 'Blob_ID',
                flex: 0.2
            }, {
                text: 'Nom du fichier',
                dataIndex: 'FileName',
                flex: 0.6
            },
            {
                text: 'Taille en Kb',
                dataIndex: 'FileSizeKbytes',
                flex: 0.2
            },
            {
                text: 'Date de création',
                dataIndex: 'CreationDateTime',
                flex: 0.3
            },
            {
                xtype: 'widgetcolumn',
                text: 'Action',
                resizable: false,
                draggable: false,
                flex: 0.2,
                scope: this,
                widget: {
                    grid: this,
                    xtype: 'button',
                    iconCls: Mjc.Admin.util.Icon('edit_image'),
                    scope: this
                },
                onWidgetAttach: function (column, widget, record) {
                    var me = this;
                    var menu = [
                    {
                        iconCls: 'delete',
                        text: 'Supprimer image...',
                        iconCls: Mjc.Admin.util.Icon('delete_image'),
                        handler: function () {

                            Mjc.Admin.util.MessageBox.confirm('Confirmation', 'Voulez-vous vraiment supprimer cet image ?', function (btn) {
                                if (btn === 'yes') {
                                    Mjc.direct.ProductDirect.deleteProductImage(record.data.Product_ID, record.data.Blob_ID, function (result, action) {
                                        if (result.success) {
                                            Mjc.Admin.util.noty.bubbleRightMessage(action.result.msg, 'success');
                                            me.getStore().reload();
                                            this.fireEvent('deleteSuccess');
                                        }
                                    }, this);
                                }
                            }, this);
                        }
                    }];

                    widget.setMenu(menu);
                }
            }];

        this.store = Ext.create('Mjc.Admin.store.ProductManagement.ProductImages_Set', {
            autoLoad: false,
            filter: [],
            listeners: {
                scope: this,
                load: function (store, records, successful, operation, eOpts) {
                    me.getSelectionModel().select(0);
                    this.fireEvent('load', store, records, successful, operation, eOpts);
                }
            }
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

//#region Mjc.Admin.ux.ProductManagement.ProductImagesPanel
Ext.define('Mjc.Admin.ux.ProductManagement.ProductImagesPanel', {
    extend: 'Ext.panel.Panel',
    layout: {
        type: 'vbox',
        align: 'stretch'
    },
    Product_ID: null,
    autoScroll: true,
    initComponent: function () {

        this.imagePreview = Ext.create('Ext.Img', {
            margin: '0 0 0 50',
            width: 200,
            height: 200,
        });

        this.imageGridPanel = Ext.create('Mjc.Admin.ux.ProductManagement.ImagesGridPanel', {
            //minHeight: 400,
            flex: 0.5,
            listeners: {
                scope: this,
                select: function (grid, record, index, eOpts) {
                    this.imagePreview.setSrc('/ProductDirect/GetImageBlob/' + record.data.Blob_ID);
                    this.imagePreview.setTitle('Aperçu de ' + record.data.FileName);
                },
                load: function (store, records, successful, operation, eOpts) {
                    if (store.getTotalCount() == 0)
                        this.imagePreview.setSrc("");
                }
            }
        });

        this.items = [
            {
                xtype: 'container',
                height: 300,
                flex: 0.3,
                autoScroll: true,
                margin: 30,
                layout: {
                    type: 'vbox',
                    align: 'center',
                    pack: 'center'
                },
                items: [this.imagePreview]
            },
        this.imageGridPanel];

        this.on('activate', function (form, action, options) {
            if (!Ext.isEmpty(this.Product_ID)) {
                var filter = [{ property: 'Product_ID', value: this.Product_ID }];
                this.imageGridPanel.getStore().addFilter(filter);
            }
        }, this)

        this.dockedItems = [{
            xtype: 'toolbar',
            dock: 'top',
            items: [{
                xtype: 'button',
                scope: this,
                text: 'Ajouter une image',
                iconCls: Mjc.Admin.util.Icon('add_image'),
                margin: 10,
                handler: function () {

                    var uploadImagePanel = Ext.create('Mjc.Admin.ux.ProductManagement.UploadImagePanel', {
                        Product_ID: this.Product_ID,
                        listeners: {
                            scope: this,
                            cancel: function () {
                                window.close();
                            },
                            uploadSuccess: function (form, action) {
                                window.close();
                                Mjc.Admin.util.noty.bubbleRightMessage(action.result.msg, 'success');
                                this.imageGridPanel.getStore().reload();

                            }
                        }
                    });
                    var window = Ext.create('Ext.window.Window', {
                        title: 'Ajouter une image',
                        modal: true,
                        constrain: true,
                        resizable: false,
                        items: [uploadImagePanel],
                        width: 500
                    }).show();
                }
            }]
        }]

        this.callParent(arguments);
    }
});
//#endregion

//#region Mjc.Admin.ux.ProductManagement.MainPanel
Ext.define('Mjc.Admin.ux.ProductManagement.ProductMainPanel', {
    extend: 'Ext.tab.Panel',
    Product_ID: null,
    initComponent: function () {
        var me = this;

        this.productInformationPanel = Ext.create('Mjc.Admin.ux.ProductManagement.ProductInformationPanel', {
            modelId: this.Product_ID,
            autoLoad: false,
            autoScroll: true,
            api: {
                load: Mjc.direct.ProductDirect.getProduct,
                submit: Mjc.direct.ProductDirect.editInformationProduct
            },
            Product_ID: this.Product_ID,
            title: 'Fiche produit',
            listeners: {
                scope: this,
                beforeload: function () {

                },
                loadSuccess: function (form, action, options) {
                    var name = action.result.data.Name;
                    me.setTitle(name);
                },

                submitSuccess: function (form, action, options) {
                    Mjc.Admin.util.noty.displayResultMessage(action.result.success, action.result.msg);
                    me.productInformationPanel.reloadPanel();
                },
                submitFailure: function (form, action, options) {
                    Mjc.Admin.util.noty.displayResultMessage(action.result.success, action.result.msg);
                    me.productInformationPanel.reloadPanel();
                }
            },
            buttons: [{
                text: 'Annuler modification',
                iconCls: Mjc.Admin.util.Icon('refresh'),
                scope: this,
                handler: function () {
                    this.productInformationPanel.load(this.Product_ID);
                }
            }, {
                text: 'Sauvegarder',
                formBind: true,
                iconCls: Mjc.Admin.util.Icon('save'),
                scope: this,
                handler: function () {

                    //this.productInformationPanel.submit({
                    //    success: function (form, action) {
                    //        Mjc.Admin.util.info('Enregistré');
                    //    },
                    //    failure: function (form, action) {
                    //        var responseMsg = action.result.msg;
                    //    }
                    //});

                    this.productInformationPanel.submit();
                }
            }]
        });

        this.productImagesPanel = Ext.create('Mjc.Admin.ux.ProductManagement.ProductImagesPanel', {
            title: 'Images',
            Product_ID: this.Product_ID,
        });

        this.items = [this.productInformationPanel, this.productImagesPanel];
        //this.items = [this.productInformationPanel];
        this.callParent(arguments);
    },
    loadProduct: function (Product_ID) {
        this.Product_ID = Product_ID;
        this.productInformationPanel.Product_ID = Product_ID;
        this.productImagesPanel.Product_ID = Product_ID;
        this.setActiveItem(this.productInformationPanel);
        this.productInformationPanel.load(this.Product_ID);
    }
});
//#endregion

//#region Mjc.Admin.ux.ProductManagement.UploadImagePanel
Ext.define('Mjc.Admin.ux.ProductManagement.UploadImagePanel', {
    extend: 'Ext.form.Panel',
    Product_ID: null,
    constructor: function (cfg) {
        if (!cfg.Product_ID) {
            Ext.Error.raise('missing parameter "Product_ID"'); return;
        };

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

        this.cancelButton = Ext.create('Ext.button.Button', {
            scope: this,
            text: 'Annuler',
            handler: function () {
                this.fireEvent('cancel');
            }
        });

        var config = {
            items: [{
                xtype: 'hiddenfield', name: 'Product_ID', value: this.Product_ID
            }, {
                xtype: 'filefield',
                name: 'file',
                fieldLabel: 'Ajouter une image',
                msgTarget: 'side',
                allowBlank: false,
                anchor: '100%',
                width: '100%',
                buttonText: 'Parcourir...',
            }],
            api: {
                submit: Mjc.direct.ProductDirect.uploadProductImage
            },
            paramOrder: 'ID',
            buttons: [this.cancelButton, {
                formBind: true,
                text: 'Upload',
                handler: this.uploadImage,
                scope: this
            }],
            buttonAlign: 'right'
        }
        Ext.apply(this, config);
        this.callParent(arguments);
    },
    uploadImage: function () {
        var form = this.getForm();
        var me = this;
        if (form.isValid()) {

            me.mask("Chargement...");

            form.submit({
                scope: this,
                success: function (form, action) {
                    this.fireEvent('uploadSuccess', form, action);
                },
                failure: function () {

                }
            });
        }
    },
    getCancelButton: function () {
        return this.cancelButton;
    }

});
//#endregion

//#region  Mjc.Admin.ux.ProductManagement.CategoryComboBox
Ext.define('Mjc.Admin.ux.ProductManagement.CategoryComboBox', {
    extend: 'Ext.form.ComboBox',
    constructor: function (cfg) {
        var me = this;
        cfg = cfg || {};
        me.callParent([Ext.apply({
            //allowBlank: false,
            name: 'Category_FK',
            fieldLabel: 'Categorie',
            store: Ext.create('Mjc.Admin.store.ProductManagement.Category_Set', {
                autoLoad: true
            }),
            queryMode: 'local',
            remoteFilter: true,
            minChars: 2,
            forceSelection: true,
            typeAhead: true,
            emptyText: 'Choisissez une catégorie..',
            triggerAction: 'all',
            displayField: 'DisplayName',
            valueField: 'Category_FK'
        }, cfg)]);
    }
});
//#endregion

//#region  Mjc.Admin.ux.ProductManagement.ProductFlagsField
Ext.define('Mjc.Admin.ux.ProductManagement.ProductFlagsField', {
    extend: 'Ext.form.field.Tag',
    constructor: function (cfg) {
        var me = this;
        cfg = cfg || {};

        me.callParent([Ext.apply({
            name: 'Product_Flags_Set',
            fieldLabel: 'Mot clés',
            store: Ext.create('Mjc.Admin.store.ProductManagement.ProductFlags_Set', {
                autoLoad: true
            }),
            fieldLabel: 'Mot clés',
            emptyText: 'Selectionnez des mot-clés à associer',
            displayField: 'DisplayFlag',
            valueField: 'Flag',
            queryMode: 'local',
            filterPickList: true
        }, cfg)]);
    }
});
//#endregion

//#region  Mjc.Admin.ux.ProductManagement.ProductKeyWordsField
Ext.define('Mjc.Admin.ux.ProductManagement.ProductKeyWordsField', {
    extend: 'Ext.form.field.Tag',
    constructor: function (cfg) {
        var me = this;
        cfg = cfg || {};

        me.callParent([Ext.apply({
            name: 'ProductKeyWords_Set',
            fieldLabel: 'Mot clés',
            store: Ext.create('Mjc.Admin.store.ProductManagement.ProductKeyWords_Set', {
                autoLoad: true
            }),
            fieldLabel: 'Mot clés',
            emptyText: 'Selectionnez des mot-clés à associer',
            displayField: 'KeyWord',
            valueField: 'KeyWord_ID',
            queryMode: 'local',
            filterPickList: true
        }, cfg)]);
    }
});
//#endregion

//#region  Mjc.Admin.ux.ProductManagement.ProductCategoriesField
Ext.define('Mjc.Admin.ux.ProductManagement.ProductCategoriesField', {
    extend: 'Ext.form.field.Tag',
    constructor: function (cfg) {
        var me = this;
        cfg = cfg || {};

        me.callParent([Ext.apply({
            name: 'ProductCategory_Set',
            fieldLabel: 'Mot clés',
            store: Ext.create('Mjc.Admin.store.ProductManagement.Category_Set', {
                autoLoad: true
            }),
            fieldLabel: 'Catégories',
            emptyText: 'Selectionnez des catégories à associer',
            displayField: 'DisplayName',
            valueField: 'Category_FK',
            queryMode: 'local',
            filterPickList: true
        }, cfg)]);
    }
});
//#endregion

//#region Mjc.Admin.ux.ProductManagement.KeyWordPanel
Ext.define('Mjc.Admin.ux.ProductManagement.KeyWordPanel', {
    extend: 'Mjc.Admin.widget.ux.directForm.Panel',
    defaults: {
        margin: 20,
        width: '80%'
    },
    initComponent: function () {

        this.nameTextField = Ext.create('Ext.form.field.Text', {
            allowBlank: false,
            fieldLabel: 'Entrez votre nouveau mot clé',
            name: 'Value',
            cls: 'formFieldEditableCls'
        });

        this.items = [this.nameTextField];

        this.callParent(arguments);
    }
});
//#endregion

//#region Mjc.Admin.ux.ProductManagement.DeleteKeyWordPanel
Ext.define('Mjc.Admin.ux.ProductManagement.DeleteKeyWordPanel', {
    extend: 'Mjc.Admin.widget.ux.directForm.Panel',
    defaults: {
        margin: 20,
        width: '80%'
    },
    initComponent: function () {

        this.comboBoxKeyWord = Ext.create('Mjc.Admin.ux.ProductManagement.KeyWordComboBox', {
            allowBlank: false,
            fieldLabel: 'Mot clés',
            name: 'KeyWord_ID',
            emptyMessage: 'Selectionnez un mot clé à supprimer...',
            cls: 'formFieldEditableCls'
        });

        this.items = [this.comboBoxKeyWord];

        this.callParent(arguments);
    }
});
//#endregion

//#region  Mjc.Admin.ux.ProductManagement.KeyWordComboBox
Ext.define('Mjc.Admin.ux.ProductManagement.KeyWordComboBox', {
    extend: 'Ext.form.ComboBox',
    constructor: function (cfg) {
        var me = this;
        cfg = cfg || {};
        me.callParent([Ext.apply({
            //allowBlank: false,
            name: 'KeyWord_ID',
            fieldLabel: 'Mot clé',
            store: Ext.create('Mjc.Admin.store.ProductManagement.KeyWord_Set', {
                autoLoad: true
            }),
            queryMode: 'local',
            remoteFilter: true,
            minChars: 2,
            forceSelection: true,
            typeAhead: true,
            emptyText: 'Choisissez un mot clé..',
            triggerAction: 'all',
            displayField: 'Value',
            valueField: 'KeyWord_ID'
        }, cfg)]);
    }
});
//#endregion

//#region Mjc.Admin.ux.ProductManagement.CategoryEditPanel
Ext.define('Mjc.Admin.ux.ProductManagement.CategoryEditPanel', {
    extend: 'Ext.panel.Panel',
    layout: {
        type: 'hbox',
        align: 'stretch'
    },
    initComponent: function () {

        var me = this;

        this.leftPanel = Ext.create('Mjc.Admin.ux.ProductManagement.CategoryGridPanel', {
            flex: 0.5,
            listeners: {
                select: function (grid, record, index, eOpts) {
                    me.categoryPanel.loadCategory(record.data.Category_ID);
                }
            }
        });

        this.categoryPanel = Ext.create('Mjc.Admin.ux.ProductManagement.CategoryPanel', {
            api: {
                load: Mjc.direct.ProductDirect.getCategory,
                submit: Mjc.direct.ProductDirect.editCategory
            },
            flex: 0.5,
            buttons: [{
                iconCls: Mjc.Admin.util.Icon('refresh'),
                text: 'Rafraichir',
                scope: this,
                handler: function () {
                    me.categoryPanel.reload();
                }
            }, {
                text: 'Sauvegarder',
                formBind: true,
                iconCls: Mjc.Admin.util.Icon('save'),
                scope: this,
                formBind: true,
                handler: function () {
                    me.categoryPanel.submit();
                }
            }],
            listeners: {
                submitSuccess: function (form, action, options) {
                    Mjc.Admin.util.noty.displayResultMessage(action.result.success, action.result.msg);
                },
                submitFailure: function (form, action, options) {
                    Mjc.Admin.util.noty.displayResultMessage(action.result.success, action.result.msg);
                }
            },
            loadCategory: function (Category_ID) {
                this.Category_ID = Category_ID;
                this.modelId = Category_ID;
                this.load(this.Category_ID);
            }
        });

        this.items = [this.leftPanel, this.categoryPanel];

        this.callParent(arguments);
    }
});
//#endregion

//#region  Mjc.Admin.ux.ProductManagement.CategoryGridPanel
Ext.define('Mjc.Admin.ux.ProductManagement.CategoryGridPanel', {
    extend: 'Ext.grid.Panel',
    initComponent: function () {

        var me = this;

        this.columns = [{
            text: 'Category_ID',
            dataIndex: 'Category_ID',
            flex: 0.2
        },
        {
            text: 'Catégorie',
            dataIndex: 'Name',
            flex: 0.4
        },
        {
            text: 'Nom affiché',
            dataIndex: 'DisplayName',
            flex: 0.4
        },
        {
            text: 'Etiquette associée',
            dataIndex: 'EtiquetteName',
            flex: 0.4
        }];

        this.store = Ext.create('Mjc.Admin.store.ProductManagement.Category_Set', {
            autoLoad: true,
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

        this.dockedItems = [
            {
                xtype: 'pagingtoolbar',
                store: this.store,
                dock: 'bottom',
                displayInfo: true,
                enableOverflow: true
            }];

        this.callParent(arguments);
    }
});
//#endregion

//#region  Mjc.Admin.ux.ProductManagement.ProductTypeComboBox
Ext.define('Mjc.Admin.ux.ProductManagement.ProductTypeComboBox', {
    extend: 'Ext.form.ComboBox',
    constructor: function (cfg) {
        var me = this;
        cfg = cfg || {};
        me.callParent([Ext.apply({
            //allowBlank: false,
            name: 'Type',
            fieldLabel: 'Type de produit',
            store: Ext.create('Mjc.Admin.store.ProductManagement.ProductType_Set', {
                autoLoad: true
            }),
            queryMode: 'local',
            remoteFilter: true,
            minChars: 2,
            forceSelection: true,
            typeAhead: true,
            emptyText: 'Choisissez un type de produit..',
            triggerAction: 'all',
            displayField: 'DisplayProductType',
            valueField: 'ProductType'
        }, cfg)]);
    }
});
//#endregion

//#region  Mjc.Admin.ux.ProductManagement.ProductFragilityComboBox
Ext.define('Mjc.Admin.ux.ProductManagement.ProductFragilityComboBox', {
    extend: 'Ext.form.ComboBox',
    constructor: function (cfg) {
        var me = this;
        cfg = cfg || {};
        me.callParent([Ext.apply({
            //allowBlank: false,
            name: 'ProductFragility',
            fieldLabel: 'Fragilité produit',
            store: Ext.create('Mjc.Admin.store.ProductManagement.ProductFragility_Set', {
                autoLoad: true
            }),
            queryMode: 'local',
            remoteFilter: true,
            minChars: 2,
            forceSelection: true,
            typeAhead: true,
            emptyText: 'Choisissez une fragilité de produit..',
            triggerAction: 'all',
            displayField: 'DisplayProductFragility',
            valueField: 'ProductFragility'
        }, cfg)]);
    }
});
//#endregion

//#region  Mjc.Admin.ux.ProductManagement.ProductDeliveryBoxTypeComboBox
Ext.define('Mjc.Admin.ux.ProductManagement.ProductDeliveryBoxTypeComboBox', {
    extend: 'Ext.form.ComboBox',
    constructor: function (cfg) {
        var me = this;
        cfg = cfg || {};
        me.callParent([Ext.apply({
            //allowBlank: false,
            name: 'ProductDeliveryBoxType',
            fieldLabel: 'Type de carton',
            store: Ext.create('Mjc.Admin.store.ProductManagement.ProductDeliveryBoxType_Set', {
                autoLoad: true
            }),
            queryMode: 'local',
            remoteFilter: true,
            minChars: 2,
            forceSelection: true,
            typeAhead: true,
            emptyText: 'Choisissez une type de carton..',
            triggerAction: 'all',
            displayField: 'DisplayProductDeliveryBoxType',
            valueField: 'ProductDeliveryBoxType'
        }, cfg)]);
    }
});
//#endregion








