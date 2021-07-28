Ext.ns('Mjc.Admin.ux.User');

//#region Mjc.Admin.ux.User.MainPanel
Ext.define('Mjc.Admin.ux.User.MainPanel', {
    extend: 'Ext.panel.Panel',
    layout: {
        type: 'hbox',
        align: 'stretch'
    },
    title: 'Utilisateurs',
    initComponent: function () {

        var me = this;

        this.listeners = {
            activate: function (panel, eOpts) {
                if (!me.userGrid.getSelectionModel().hasSelection() && me.userGrid.getStore().getTotalCount()) {
                    me.userGrid.getSelectionModel().select(0);
                }
            }
        };

        this.userGrid = Ext.create('Mjc.Admin.ux.User.GridPanel', {
            flex: 0.5,
            collapseDirection: 'left',
            listeners: {
                scope: this,
                load: function (store, records, successful, operation, eOpts) {
                    var hasStoreRecords = store.getTotalCount() > 0;
                    this.userPanel.setHidden(!hasStoreRecords);
                    this.emptyPanel.setHidden(hasStoreRecords);
                },
                select: function (grid, record, index, eOpts) {
                    this.userPanel.loadUser(record.data.User_ID);
                },
                //activateProduct: function (Product_ID) {
                //    me.mask("Chargement...  ");
                //    Mjc.direct.ProductDirect.activateProduct(Product_ID, function (result) {
                //        me.unmask();
                //        Mjc.Admin.util.noty.displayResultMessage(result.success, result.msg);
                //        me.productGrid.reload();
                //    }, this);
                //},
                //deactivateProduct: function (Product_ID) {
                //    me.mask("Chargement...  ");
                //    Mjc.direct.ProductDirect.deactivateProduct(Product_ID, function (result) {
                //        me.unmask();
                //        Mjc.Admin.util.noty.displayResultMessage(result.success, result.msg);
                //        me.productGrid.reload();
                //    }, this);
                //},
                //deleteProduct: function (Product_ID) {
                //    me.mask("Chargement...  ");
                //    Mjc.Admin.util.MessageBox.confirm('Confirmation', 'Voulez-vous vraiment supprimer définitivement ce produit ? Notez que cela entrainera également la suppression définitive des images reliés.', function (btn) {
                //        if (btn === 'yes') {
                //            Mjc.direct.ProductDirect.deleteProduct(Product_ID, function (result) {
                //                me.unmask();
                //                Mjc.Admin.util.noty.displayResultMessage(result.success, result.msg);
                //                me.productGrid.reload();
                //            }, this);
                //        }
                //    }, this);


                //}
            }
        });

        this.emptyPanel = Ext.create('Ext.panel.Panel', {
            flex: 0.5,
            hidden: false
        });

        this.userPanel = Ext.create('Mjc.Admin.ux.User.UserMainTabPanel', {
            flex: 0.5,
            title: '',
            hidden: true
        });

        this.items = [this.userGrid, this.emptyPanel, this.userPanel];
        this.callParent(arguments);
    }
});
//#endregion

//#region Mjc.Admin.ux.User.GridPanel
Ext.define('Mjc.Admin.ux.User.GridPanel', {
    extend: 'Ext.grid.Panel',
    title: 'Utlisateurs',
    border: false,
    //margin: '30 0 0 0',
    header: false,
    initComponent: function () {

        var me = this;
        this.plugins = [{ ptype: 'simplegridfilter', fieldName: 'SearchProductFilter', emptyTextField: 'Rechercher...', widthField: 200 }];

        this.columns = [{
            text: 'User_ID',
            dataIndex: 'User_ID',
            flex: 0.3
        },
        {
            text: 'Nom',
            dataIndex: 'LastName',
            flex: 1
        },
        {
            text: 'Prénom',
            dataIndex: 'FirstName',
            flex: 1,
        },
        {
            text: 'eMail',
            dataIndex: 'eMail',
            flex: 1,
        },
        {
            text: 'Customer',
            dataIndex: 'isCustomerUser',
            flex: 0.5,
            hidden: true
        },
        {
            text: 'Admin',
            dataIndex: 'isAdminUser',
            flex: 0.5,
            hidden: true
        },
        {
            text: 'Role',
            dataIndex: 'isAdminUserText',
            flex: 0.5
        },
        {
            xtype: 'widgetcolumn',
            text: 'Action',
            resizable: false,
            draggable: false,
            flex: 0.5,
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
                    hidden: true,
                    text: 'Désactiver Utilisateur...',
                    iconCls: Mjc.Admin.util.Icon('switch_off'),
                    handler: function () {
                        me.fireEvent('deactivateUser', record.data.Product_ID)
                    }
                },
                {
                    hidden: true,
                    text: 'Activer Utilisateur...',
                    iconCls: Mjc.Admin.util.Icon('switch_on'),
                    handler: function () {
                        me.fireEvent('activateUser', record.data.Product_ID)
                    }
                },
                {
                    text: 'Supprimer Utilisateur...',
                    iconCls: Mjc.Admin.util.Icon('error'),
                    disabled: true,
                    handler: function () {
                        me.fireEvent('deleteUser', record.data.Product_ID)
                    }
                }];


                widget.setMenu(menu);
            }
        }];

        this.store = Ext.create('Mjc.Admin.User.store.User_Set', {
            sorters: [{
                property: 'Name',
                direction: 'ASC'
            }],
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

        this.dockedItems = [{
            xtype: 'toolbar',
            dock: 'top',
            items: [{
                xtype: 'button',
                text: 'Ajouter un Utilisateur',
                iconCls: Mjc.Admin.util.Icon('user-shape-yellow'),
                margin: 10,
                handler: function () {

                    var userPanel = Ext.create('Mjc.Admin.ux.User.UserInformationPanel', {
                        api: {
                            submit: Mjc.direct.UserDirect.createUser
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
                                userPanel.submit();
                            }
                        }]
                    });
                    userPanel.getUser_IdDisplay().setHidden(true);

                    var window = Ext.create('Ext.window.Window', {
                        layout: 'fit',
                        title: 'Ajouter un Utilisateur',
                        modal: true,
                        autoScroll: true,
                        constrain: true,
                        resizable: false,
                        items: [userPanel],
                        height: '70%',
                        width: '50%'
                    }).show();
                }
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

//#region Mjc.Admin.ux.User.UserInformationPanel
Ext.define('Mjc.Admin.ux.User.UserInformationPanel', {
    extend: 'Mjc.Admin.widget.ux.directForm.Panel',
    User_ID: null,
    defaults: {
        margin: 20,
        width: '60%'
    },
    isEmailFieldReadOnly: false,
    allowBlankPasswordFields: false,
    api: {
        load: Mjc.direct.UserDirect.getUser,
        submit: Mjc.direct.UserDirect.editInformationProduct
    },
    listeners: {
        //afterrender: function () {
        //    this.isAdminCheckbox.setValue(true);
        //    this.isAdminCheckbox.setValue(false);
        //}
    },
    initComponent: function () {

        var me = this;

        this.user_IdDisplay = Ext.create('Ext.form.field.Display', {
            fieldLabel: 'User_ID',
        });

        this.userIdHidden = Ext.create('Ext.form.field.Hidden', {
            fieldLabel: 'User_ID',
            name: 'User_ID'
        });

        //this.isAdminCheckboxHidden = Ext.create('Ext.form.field.Hidden', {
        //    fieldLabel: 'isAdminCheckbox',
        //    name: 'isAdminCheckbox'
        //});

        this.lastNameTextField = Ext.create('Ext.form.field.Text', {
            allowBlank: false,
            fieldLabel: 'Nom',
            name: 'LastName',
            cls: 'formFieldEditableCls'
        });

        this.firstNameTextField = Ext.create('Ext.form.field.Text', {
            allowBlank: false,
            fieldLabel: 'Prénom',
            name: 'FirstName',
            cls: 'formFieldEditableCls'
        });

        if (me.isEmailFieldReadOnly) {
            this.emailextField = Ext.create('Ext.form.field.Display', {
                allowBlank: false,
                fieldLabel: 'Email',
                name: 'eMail',
                vtype: 'email',
                emptyText: 'Assurez vous que le mail existe',
                cls: 'formFieldEditableCls',
                //readOnly: me.isEmailFieldReadOnly
            });
        } else {
            this.emailextField = Ext.create('Ext.form.field.Text', {
                allowBlank: false,
                fieldLabel: 'Email',
                name: 'eMail',
                vtype: 'email',
                emptyText: 'Assurez vous que le mail existe',
                cls: 'formFieldEditableCls',
                //readOnly: me.isEmailFieldReadOnly
            });
        }

        this.password = Ext.create('Ext.form.field.Text', {
            allowBlank: this.allowBlankPasswordFields,
            inputType: 'password',
            fieldLabel: 'Mot de passe',
            name: 'Password',
            emptyText: '6 lettres minimum',
            minLength: 6,
            cls: 'formFieldEditableCls'
        });

        this.confirmedPassword = Ext.create('Ext.form.field.Text', {
            allowBlank: this.allowBlankPasswordFields,
            inputType: 'password',
            fieldLabel: 'Confirmation Mot de passe',
            name: 'ConfirmedPassword',
            emptyText: '6 lettres minimum',
            minLength: 6,
            cls: 'formFieldEditableCls'
        });

        //this.isAdminCheckbox = Ext.create('Ext.form.field.Checkbox', {
        //    //labelWidth: 100,
        //    fieldLabel: 'Attribuer droit Admin ?',
        //    name: 'isAdminCheckbox',
        //    listeners: {
        //        change: function (checkbox, newValue, oldValue, eOpts) {
        //            if (newValue) {
        //                this.isAdminCheckboxHidden.setValue(1);
        //            } else {
        //                this.isAdminCheckboxHidden.setValue(0);
        //            }

        //        }
        //    }
        //});

        this.isAdminCheckbox = Ext.create('Ext.form.ComboBox', {
            fieldLabel: 'Attribuer droit Admin ?',
            name: 'isAdminCheckbox',
            store: Ext.create('Ext.data.Store', {
                fields: ['property', 'value'],
                data: [
                    { "property": true, "value": "Oui" },
                    { "property": false, "value": "Non" },
                ]
            }),
            queryMode: 'local',
            displayField: 'value',
            valueField: 'property',
            value: false
        });



        this.items = [this.userIdHidden, this.user_IdDisplay, this.emailextField, this.lastNameTextField, this.firstNameTextField, this.password, this.confirmedPassword, this.isAdminCheckbox];

        this.on('loadSuccess', function (form, action, options) {
            var record = action.result;
            this.user_IdDisplay.setValue(this.userIdHidden.getValue());

            me.isAdminCheckbox.setReadOnly(false);
            me.isAdminCheckbox.removeCls('x-item-disabled');

            if (record.data.isCurrentUser) {
                me.isAdminCheckbox.addCls('x-item-disabled');
                me.isAdminCheckbox.setReadOnly(true);
            }

        }, this)

        this.on('activate', function (form, action, options) {
            if (!Ext.isEmpty(this.User_ID)) {
                this.load(this.User_ID);
            }
        }, this)

        this.callParent(arguments);
    },
    getUser_IdDisplay: function () {
        return this.user_IdDisplay;
    },
    reloadPanel: function () {
        this.load(this.User_ID);
    }
});
//#endregion

//#region Mjc.Admin.ux.User.UserMainTabPanel
Ext.define('Mjc.Admin.ux.User.UserMainTabPanel', {
    extend: 'Ext.tab.Panel',
    User_ID: null,
    initComponent: function () {
        var me = this;

        this.userInformationPanel = Ext.create('Mjc.Admin.ux.User.UserInformationPanel', {
            modelId: this.User_ID,
            autoLoad: false,
            autoScroll: true,
            isEmailFieldReadOnly: true,
            allowBlankPasswordFields: true,
            api: {
                load: Mjc.direct.UserDirect.getUser,
                submit: Mjc.direct.UserDirect.editInformationUser
            },
            User_ID: this.User_ID,
            title: 'Fiche Utilisateur',
            listeners: {
                scope: this,
                beforeload: function () {

                },
                loadSuccess: function (form, action, options) {
                    var userName = action.result.data.FullName + ' - ' + action.result.data.eMail;
                    me.setTitle(userName);
                },

                submitSuccess: function (form, action, options) {
                    Mjc.Admin.util.noty.displayResultMessage(action.result.success, action.result.msg);
                    me.userInformationPanel.reloadPanel();
                },
                submitFailure: function (form, action, options) {
                    Mjc.Admin.util.noty.displayResultMessage(action.result.success, action.result.msg);
                    me.userInformationPanel.reloadPanel();
                }
            },
            buttons: [{
                text: 'Annuler modification',
                iconCls: Mjc.Admin.util.Icon('refresh'),
                scope: this,
                handler: function () {
                    this.userInformationPanel.load(this.Product_ID);
                }
            }, {
                text: 'Sauvegarder',
                formBind: true,
                iconCls: Mjc.Admin.util.Icon('save'),
                scope: this,
                handler: function () {

                    Mjc.Admin.util.MessageBox.confirm('Confirmation', 'Voulez-vous vraiment éditer cet utlisateur ?', function (btn) {
                        if (btn === 'yes') {
                            this.userInformationPanel.submit();
                        }
                    }, this);

                }
            }]
        });

        this.userCommand = Ext.create('Ext.panel.Panel', {
            title: 'Commandes',
            User_ID: this.User_ID,
        });

        this.items = [this.userInformationPanel, this.userCommand];
        this.callParent(arguments);
    },
    loadUser: function (User_ID) {
        this.User_ID = User_ID;
        this.userInformationPanel.User_ID = User_ID;
        this.setActiveItem(this.userInformationPanel);
        this.userInformationPanel.load(this.User_ID);
    }
});
//#endregion





