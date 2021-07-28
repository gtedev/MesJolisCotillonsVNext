Ext.ns('Mjc.Admin.widget.ux');

//#region 'Mjc.Admin.widget.ux.directForm.Panel
Ext.define('Mjc.Admin.widget.ux.directForm.Panel', {
    extend: 'Ext.form.Panel',
    modelId: null,
    paramOrder: ['id'],
    autoLoad: false,
    initComponent: function () {

        this.callParent(arguments);

        this.on({
            loadSuccess: function () { },
            loadFailure: function () { },
            submitSuccess: function () { },
            submitFailure: function () { },
            afterrender: function (me) {
                if (this.autoLoad) me.load();
            },
            afterlayout: function (me) {
            }
        });
    },
    reload: function (options) {
        this.load(this.modelId, options);
    },
    load: function (modelId, options) {
        var options = options || {};
        Ext.applyIf(options, {
            beforeLoad: true,
            loadSuccess: true,
            loadFailure: true,
        });

        this.modelId = modelId || this.modelId;

        this.mask('Chargement...');
        if (options.beforeLoad)
            this.onDirectBeforeLoad(options);

        this.form.load({
            scope: this,
            success: function (form, action) {
                this.unmask();
                if (options.loadSuccess)
                    this.onDirectLoadSuccess(form, action, options);

                if (action.result.msg != null) {

                    var responseMsg = action.result.msg;
                    var msgType = action.result.msgType || 'success';
                    //TP.util.noty(responseMsg, msgType);


                }
            },
            failure: function (form, action) {
                this.unmask();
                if (options.loadFailure)
                    this.onDirectLoadFailure(form, action, options);

                if (action.result && action.result.msg != null) {
                    var responseMsg = action.result.msg;
                    //TP.util.noty(responseMsg, 'error');
                } else {
                    //TP.util.noty('Error while loading', 'error');
                }
            },
            params: {
                id: this.modelId
            }
        });
    },

    submit: function (options) {
        var options = options || {};

        Ext.applyIf(options, {
            beforeSubmit: true,
            submitSuccess: true,
            submitFailure: true,
        });

        this.mask();
        if (options.beforeSubmit)
            this.onDirectBeforeSubmit(options);

        this.form.submit({
            scope: this,
            success: function (form, action) {
                this.unmask();
                if (options.submitSuccess)
                    this.onDirectSubmitSuccess(form, action, options);

                if (action.result.msg != null) {

                    var responseMsg = action.result.msg;
                    var msgType = action.result.msgType || 'success';
                    //TP.util.noty(responseMsg, msgType);

                }
            },
            failure: function (form, action) {
                this.unmask();
                if (options.submitFailure)
                    this.onDirectSubmitFailure(form, action, options);

                if (action.result.msg != null) {
                    var responseMsg = action.result.msg;
                    //TP.util.noty(responseMsg, 'error');
                } else {
                    //TP.util.noty('Error while Saving', 'error');
                }

            }
        });
    },
    onDirectBeforeLoad: function (options) {
        this.fireEvent('beforeLoad', this, options);
    },
    onDirectLoadSuccess: function (form, action, options) {
        //this.isLoaded = true;
        this.unmask();
        this.fireEvent('loadSuccess', this, action, options);
    },
    onDirectLoadFailure: function (form, action, options) {
        this.unmask();
        this.fireEvent('loadFailure', this, action, options);
    },
    onDirectBeforeSubmit: function (options) {
        this.fireEvent('beforeSubmit', this, options);
    },
    onDirectSubmitSuccess: function (form, action, options) {
        this.unmask();
        this.fireEvent('submitSuccess', this, action, options);
    },
    onDirectSubmitFailure: function (form, action, options) {
        this.unmask();
        this.fireEvent('submitFailure', this, action, options);
    }


});
//#endregion

//#region 'Mjc.Admin.widget.ux.PdfPanel
Ext.define('Mjc.Admin.widget.ux.PdfPanel', {
    extend: 'Ext.Panel',
    modelId: null,
    url: null,
    constructor: function (cfg) {
        var me = this;
        cfg = cfg || {};

        if (cfg.modelId === null) Ext.Error.raise('Missing modelId.');
        if (cfg.url === null) Ext.Error.raise('Missing url.');

        if (!Ext.String.endsWith(cfg.url, '/'))
            cfg.url = cfg.url + '/';

        me.callParent([Ext.apply({
            autoDestroy: true,
            layout: 'fit',
            padding: 0,
            bodyCssClass: 'hidden-overflow',
            autoScroll: false,
            frame: false
        }, cfg)]);
    },
    initComponent: function () {
        var config = {
            html: '<object data="' + this.url + this.modelId + '#view=FitH&pagemode=none&scrollbar=1&toolbar=0&statusbar=0&messages=0&navpanes=0" type="application/pdf" width="100%" height="100%">'
                + '<p>Il semble que votre navigateur n\'est pas configuré pour afficher les PDFs '
                + ' Pas de soucis, <a href="' + this.url + '">cCliquez ici pour télécharger le fichier</a></p>'
                + '</object>'
        };
        Ext.apply(this, config);
        this.callParent(arguments);
    }
});
//#endregion

//#region 'Mjc.Admin.widget.ux.ImagePanel
Ext.define('Mjc.Admin.widget.ux.ImagePanel', {
    extend: 'Ext.Panel',
    modelId: null,
    url: null,
    constructor: function (cfg) {
        var me = this;
        cfg = cfg || {};

        if (cfg.modelId === null) Ext.Error.raise('Missing modelId.');
        if (cfg.url === null) Ext.Error.raise('Missing url.');

        if (!Ext.String.endsWith(cfg.url, '/'))
            cfg.url = cfg.url + '/';

        me.callParent([Ext.apply({
            autoDestroy: true,
            layout: 'fit',
            layout: 'anchor',
            padding: 0,
            //bodyCssClass: 'hidden-overflow',
            bodyStyle: 'background-color:lightgray',
            autoScroll: true,
            frame: false
        }, cfg)]);
    },
    initComponent: function () {
        var button = Ext.create('Ext.button.Button', {
            //hidden: true, //should be disabled until the store has changed
            text: 'Print',
            iconCls: Mjc.Admin.util.Icon('printer'),

            scope: this,
            handler: function () {
                var pwin = window.open('', '', 'height=1,width=1');
                pwin.document.write('<html><head><title>Impression</title>');
                css = 'body {margin: 0;padding: 0;}' +
                        '* {box-sizing: border-box;-moz-box-sizing: border-box;}' +
                        '.page {width: 21cm;min-height: 29.7cm;padding: 1cm;margin: 1cm auto;}' +
                        '.subpage {padding: 1cm;height: 265mm;outline: 2cm #FFEAEA solid;}' +
                        'img{height:265mm;width:auto;}' +
                        'img{height:auto;width:100%;}' +
                        '@page {size: A4;margin: 0;}' +
                        '@media print {.page {margin: 0;border: initial;border-radius: initial;width: initial;min-height: initial;box-shadow: initial;background: initial;page-break-after: avoid;}}';
                pwin.document.write('<style type="text/css">' + css + '</style>');
                pwin.document.write('<script type="text/javascript">window.onload = function () {window.print();window.close();};</script>');

                pwin.document.write('</head><body ><div class="page"><div class="subpage">');
                img = '<img src="' + this.url + this.modelId + '">';
                pwin.document.write(img);  // data = all image
                pwin.document.write('</div></div></body></html>');
                pwin.document.close();
            },
            tooltip: { text: 'Cliquez ici pour télécharger le document', title: 'Print...' }
        });
        var bar = Ext.create('Ext.toolbar.Toolbar', {
            defaults: {
                //width: '100%',
                //iconAlign: 'right'
            },
            items: ['->', button]
        });
        var config = {
            html: '<img id="pictureElement' + this.pID +
                    '"  src="' + this.url + this.modelId +
                    '" style="width:100%;max-width:850px;display:block;margin:0 auto;">',
            tbar: bar
        };
        Ext.apply(this, config);
        this.callParent(arguments);
    }
});
//#endregion