Ext.define('simplegridfilter', {
    extend: "Ext.AbstractPlugin",
    alias: "plugin.simplegridfilter",

    fieldName: 'name',
    delay: 600,
    dockPosition: 'bottom',
    widthField: null,
    textField: null,
    emptyTextField: null,
    tbFill: true,
    //toolbarPositionTarget: null,
    /**
     * This method add filter input at the end of top toolbar elements
     * or if top toolbar doesnt exist, create new one with filter input
     *
     * @param component The ext panel cmp
     */
    setWidth: function () {
        if (this.widthField === null) { return 200; } else { return this.widthField; }
    },
    setText: function () { if (this.textField == null) { return ""; } else { return this.textField; } },
    setEmptyText: function () { if (this.emptyTextField == null) { return ""; } else { return this.emptyTextField; } },
    init: function (component) {

        var me = this;

        this.toolbar = component.down('toolbar[dock=' + this.dockPosition + ']');

        var items = this.tbFill ? [{ xtype: 'tbfill' }] : [];
        Ext.Array.push(items, {
            xtype: 'label',
            text: this.setText()
        });
        this.field = Ext.create('Ext.form.field.Text', {
            width: this.setWidth(),
            emptyText: this.setEmptyText(),
            isDirty: function () { return false; },
            triggers: {
                reset: {
                    scope: this,
                    cls: 'x-form-clear-trigger',
                    handler: function () {
                        this.reset();
                        this.field.focus();
                    }
                }
            },
            listeners: {
                change: function (field, newValue, oldValue, e) {
                    me.filterFn(newValue, true);
                    if (this.preventNextLoad)
                        this.preventNextLoad = false;
                    else
                        me.getStore().load();
                },
                scope: this,
                buffer: me.delay
            }
        });
        Ext.Array.push(items, this.field);

        if (this.toolbar == null) {

            this.toolbar = Ext.create('Ext.toolbar.Toolbar', {
                xtype: 'toolbar',
                dock: this.dockPosition,
                items: items
            });

            component.addDocked(this.toolbar);
        }
        else
            this.toolbar.add(items);
    },
    filterFn: function (value, suppressEvent) {
        var me = this, value;
        var store = this.getStore();
        if (value) {
            store.addFilter({ id: me.fieldName, property: me.fieldName, value: value }, suppressEvent);
        } else {
            store.removeFilter(me.fieldName, suppressEvent)
        }
    },
    getStore: function () {
        return this.field.up('grid').getStore();
    },
    preventNextLoad: false,
    reset: function (preventReload) {
        this.preventNextLoad = preventReload ? true : false;
        this.filterFn('', true);
        this.field.reset();
    },
    getToolbar: function () {
        return this.toolbar;
    }
});