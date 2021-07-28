//#region Route
var RouteUtilities = {
    redirectToUrl: function (url) {
        window.location.href = url;
    }
};
//#endregion

//#region Cart
var CartUtilities = {
    removeFromMyCart: function (Product_ID) {
        var me = this;

        Utils.mask('#myCartContainer');

        $.post('/Cart/RemoveFromMyCart', { Product_ID: Product_ID }, function (result) {
            if (result.success) {
                me.loadMyGridCart();
                me.refreshActualCartTotal();
            }
        });
    },
    loadMyGridCart: function (callbackSuccess) {

        $.get('/Cart/MyCartGridPartialView', function (htmlResult) {
            //$("#myCartContainer").html(htmlResult);
            $("#myCartContainer").replaceWith(htmlResult);
            //Utils.unmask('myCartContainer');
            if (callbackSuccess) {
                callbackSuccess();
            }
        });
    },
    refreshItemCartNumber: function (msg) {
        $(".itemNb").html(msg);
    },
    refreshTotalCartPrice: function (msg) {
        $("#itemTotalCartPrice").html(msg);
    },
    refreshActualCartTotal: function () {
        var me = this;
        $.post('/Cart/getActualCartTotal', function (result) {
            if (result.success) {
                me.refreshItemCartNumber(result.totalCount);
                me.refreshTotalCartPrice(result.totalCartPrice);
            }
        })
    },
    runCartGridUtilities: function () {

        var me = this;

        $(".productQuantityDropdown").change(function () {

            var product_ID = $(this).attr("productId");
            var quantitySelected = $(this).children('option:selected').text();

            var form = {
                Product_ID: product_ID,
                Quantity: quantitySelected
            };

            Mjc.util.mask('#myCartContainer');

            $.ajax({
                type: 'POST',
                url: '/Product/updateProductQuantity',
                data: JSON.stringify(form),
                contentType: 'application/json; charset=utf-8',
                success: function (result) {
                    if (!result.success) {
                        Mjc.util.noty.displayResultMsg(result.success, result.msg, 3000);
                        me.loadMyGridCart();
                    } else {
                        me.refreshActualCartTotal();

                        var callbackLoadSuccess = function () {
                            Mjc.util.unmask('#myCartContainer');
                        };
                        me.loadMyGridCart(callbackLoadSuccess);

                    }
                }
            });
        });

        $('#processCommandButton').click(function (e) {
            Mjc.util.mask('#myCartContainer');
            Mjc.route.redirectToUrl('/Command/ProcessCommand');
        });
    }
};
//#endregion

//#region Util
var Utils = {
    isMobile: function () {

        return /Android|webOS|iPhone|iPad|iPod|BlackBerry|BB|PlayBook|IEMobile|Windows Phone|Kindle|Silk|Opera Mini/i.test(navigator.userAgent);
    },
    noty: {
        displayResultMsg: function (isSuccess, msg, timeout) {

            type = 'error';

            if (isSuccess) {
                type = 'success';
            }

            if (timeout == null || timeout == undefined) {
                timeout = 10000;
            }
            noty({
                text: msg,
                type: type,
                layout: 'center',
                closeWith: ['click'],
                timeout: timeout, // delay for closing event. Set false for sticky notifications
                killer: true, // for close all notifications before show
                theme: 'relax',
                maxVisible: 3,
                animation: {
                    open: 'animated flipInX',
                    close: 'animated flipOutX',
                    easing: 'swing',
                    speed: 500
                }
            });
        },
        displayMsg: function (msg, type, position, timeout) {

            if (type == null || type == undefined) {
                type = 'information';
            }
            if (position == null || type == position) {
                position = 'center';
            }

            if (timeout == null || timeout == undefined) {
                timeout = timeout;
            }
            noty({
                text: msg,
                type: type,
                layout: position,
                closeWith: ['click'],
                timeout: 10000, // delay for closing event. Set false for sticky notifications
                killer: true, // for close all notifications before show
                theme: 'relax',
                maxVisible: 3,
                animation: {
                    open: 'animated flipInX',
                    close: 'animated flipOutX',
                    easing: 'swing',
                    speed: 500
                }
            });
        },
        confirmDialog: function (config) {

            //#region Ontology
            var config = config || {};

            if (config.text == null || config.text == undefined) {
                config.text = "no message defined";
            }
            if (config.buttonOkText == null || config.buttonOkText == undefined) {
                config.buttonOkText = "no text defined";
            }
            if (config.butonCancelText == null || config.butonCancelText == undefined) {
                config.butonCancelText = "no text defined";
            }
            //#endregion

            noty({
                layout: 'center',
                modal: true,
                theme: 'relax',
                text: config.text,
                buttons: [
                    {
                        addClass: 'btn btn-primary',
                        text: config.buttonOkText,
                        onClick: function ($noty) {
                            $noty.close();
                            if (typeof config.callbackOnOk === "function") {
                                config.callbackOnOk();
                            }
                        }
                    },
                    {
                        addClass: 'btn btn-danger',
                        text: config.butonCancelText,
                        onClick: function ($noty) {
                            $noty.close();
                            if (typeof config.callbackOnCancel === "function") {
                                config.callbackOnCancel();
                            }
                        }
                    }
                ]
            });
        },
        notyMessage: function (config) {

            noty({
                text: config.text,
                type: config.type,
                layout: config.position,
                closeWith: ['click'],
                timeout: config.timeout, // delay for closing event. Set false for sticky notifications
                killer: true, // for close all notifications before show
                theme: 'relax',
                maxVisible: 3,
                animation: {
                    open: 'animated flipInX',
                    close: 'animated flipOutX',
                    easing: 'swing',
                    speed: 500
                }
            });
        }
    },
    mask: function (selector, maskSize) {

        maskLoadingCls = 'maskLoading'

        if (maskSize == 'big') {
            maskLoadingCls = 'maskLoadingBig'
        }
        //var element = $('#' + elementId);
        //if (!element.hasClass(maskLoadingCls)) {
        $(selector).addClass(maskLoadingCls);
        //}
    },
    unmask: function (selector) {
        $(selector).removeClass('maskLoading');
        $(selector).removeClass('maskLoadingBig');
    },
    opacity: function (selector, opacityValue) {
        var opacityToApply = 0.5;

        if (opacityValue != null) {
            opacityToApply = opacityValue;
        }
        if (selector != null) {
            $(selector).css('opacity', opacityToApply);
        }
    },
    isWindowWitdhUnderDocumentWidth: function () {

        var result = false;
        var widthWindow = $(window).width();
        var widthMinDoc = $(document).width();

        if (widthWindow < widthMinDoc) {
            result = true;
        }

        return result;
    },
    isEmail: function (email) {
        var regex = /^([a-zA-Z0-9_.+-])+\@(([a-zA-Z0-9-])+\.)+([a-zA-Z0-9]{2,4})+$/;
        var emailTrimed = email.trim();
        return regex.test(emailTrimed);
    },
    accordionShowBySelector: function (panelElementSelector, panelAccordionSelector) {
        var me = this;

        if (panelAccordionSelector == null) {
            panelAccordionSelector = '.accordionPanel';
        }

        var panelAccordionElement = $(panelElementSelector).next(panelAccordionSelector).get(0);
        me.accordionShowPanel(panelAccordionElement);
    },
    accordionShowByElement: function (panelAccordionElement, panelAccordionSelector) {

        var me = this;

        if (panelAccordionSelector == null) {
            panelAccordionSelector = '.accordionPanel';
        }

        var panelAccordionElement = panelAccordionElement.next(panelAccordionSelector).get(0);
        me.accordionShowPanel(panelAccordionElement);
    },
    accordionShowPanel: function (panelAccordionElement) {

        if (panelAccordionElement.style.maxHeight) {
            panelAccordionElement.style.maxHeight = null;
        } else {
            panelAccordionElement.style.maxHeight = panelAccordionElement.scrollHeight + "px";
        }
    }
};
//#endregion

//#region Product
var ProductUtilities = {
    loadProductInfoPage: function (Product_ID) {
        window.location.href = "/Product/GetProductView/" + Product_ID;
    },
    loadProductsByMenuItemId: function (menuItemId) {
        RouteUtilities.redirectToUrl('/Product/ProductsByMenuItemIdView/' + menuItemId);
    },
    updateHtmlProductItemPage: function (Product_ID) {

        $.post('/Product/ProductItemAddButtonAndQuantityInfoHtml', { Product_ID: Product_ID }, function (result) {
            if (result.success) {
                var addButtonAndQuantityInfoHtml = result.AddButtonAndQuantityInfoHtml;
                var addButtonCls = addButtonAndQuantityInfoHtml.AddCartButtonView.AddButtonCls;

                var flagCls = addButtonAndQuantityInfoHtml.ProductQuantityInfoView.FlagCls;
                var infoText = addButtonAndQuantityInfoHtml.ProductQuantityInfoView.InfoText;

                $("#productAddButton").removeClass('active');
                $("#productAddButton").removeClass('disabled');
                $("#productAddButton").addClass(addButtonCls);

                $("#productQuantityInfo").removeClass();
                $("#productQuantityInfo").addClass('productQuantiteInfo d-inline-block ' + flagCls);
                $("#productQuantityInfo").text(infoText);
            }
        });
    },
    addProduct: function (Product_ID) {
        var me = this;

        $.post('/Product/addProduct', { Product_ID: Product_ID }, function (result) {
            if (result.success) {
                //me.loadProductInfoPage(Product_ID);
                CartUtilities.refreshActualCartTotal();
                me.updateHtmlProductItemPage(Product_ID);

                Utils.noty.notyMessage({
                    text: "Votre produit a été ajouté au panier",
                    type: "information",
                    position: 'topRight',
                    timeout: 1500
                });

            } else {
                //RouteUtilities.redirectToUrl('/');    
                Mjc.util.noty.displayResultMsg(result.success, result.msg, 3000);
                me.updateHtmlProductItemPage(Product_ID);
            }
        });
    },
    shareProductItemOnSocialNetwork: function (urlLink, urlToEncode) {
        var urlEncoded = encodeURIComponent(urlToEncode);
        var urlParameter = urlLink + urlEncoded;
        window.open(urlParameter, 'newwindow', 'width=700, height=500');
    },
    shareProductItemByEmail: function () {
        var email = '';
        var subject = 'Partagez un produit Mes Jolis Cotillons par email';
        var emailBody = 'Heyyy voici un produit fort intéressant, voici le lien: <br>' + '@Model.productItemUrl';

        //var attach = 'path';

        //document.location = "mailto:" + email + "?subject=" + subject + "&body=" + emailBody +
        //    "?attach=" + attach;

        document.location = "mailto:" + email + "?subject=" + subject + "&body=" + emailBody;
    },
    createProductsViewInstance: function (height, width, productsViewId, productsPageRequest) {

        var me = this;
        var height = height;
        var productsViewId = productsViewId;
        var productsPageRequest = productsPageRequest;

        //if (height) {
        //    $('#productsView-' + productsViewId + '').css({'min-height': height});
        //}
        //if (width) {
        //    $('#productsView-' + productsViewId + '').css({'min-width': width});
        //}

        Mjc.util.mask('#productsView-' + productsViewId + '');

        var callbackSuccess = function () {
            Mjc.util.unmask('#productsView-' + productsViewId + '');
        };

        Mjc.product.getProductsGridPartialViewAjax(productsViewId, productsPageRequest, callbackSuccess, null, "#productsGridLoading-");

        //$.ajax({
        //    type: 'POST',
        //    url: '/Product/ProductsGridPartialViewAjax',
        //    data: JSON.stringify(productsPageRequest),
        //    contentType: 'application/json; charset=utf-8',
        //    success: function (htmlResult) {
        //        //console.log("Success: " + productsViewId);
        //        $("#productsGridLoading-" + productsViewId + "").replaceWith(htmlResult);
        //        Mjc.util.unmask('#productsView-' + productsViewId + '');
        //    }
        //});
    },
    createProductsGridInstance: function (productViewId, productsPageRequest) {

        var me = this;
        me.productViewId = productViewId;
        me.productsPageRequest = productsPageRequest;

        $.fn.scrollView = function () {
            return this.each(function () {
                $('html, body').animate({
                    scrollTop: $(this).offset().top
                }, 1000);
            });
        }


        $("#productsGridContainer-" + me.productViewId + " .productItemClickable").click(function () {

            if (!$(this).hasClass("loadingMask")) {
                var productId = $(this).attr("productId");
                Mjc.product.loadProductInfoPage(productId);
            }
        });

        $(".page-" + me.productViewId + "").click(function (e) {

            if (!$(this).hasClass("loadingMask") && !$(this).hasClass("pageSelected")) {

                var pageNumber = $(this).attr("pageNumber");
                var pageNumberCurrentlySelected = $(".pageNumber.pageSelected.page-" + me.productViewId + "").attr("pageNumber");
                var productsPageRequest = me.productsPageRequest;

                var stateObj = {
                    productViewId: me.productViewId,
                    pageNumber: pageNumber,
                    productsPageRequest: productsPageRequest,
                    historyID: history.length + 1,
                    url: window.location.pathname
                };

                Mjc.history.addProductPageGridHistoryPushState(stateObj);


                productsPageRequest.pageNumber = pageNumber;


                var productViewId = $(this).attr("productViewId")
                Mjc.util.mask("#productsGridContainer-" + productViewId + "");
                Mjc.util.opacity('#productsGridContainer-' + productViewId + ' .productImage');

                $("#productsGridContainer-" + productViewId + " .productImage").addClass("productImageNoScale");
                $(".page-" + productViewId + "").addClass("loadingMask");

                e.stopPropagation();

                if (productsPageRequest.scrollUpToHeader) {

                    var heightHeaderTitle = $("#productsGridContainer-" + productViewId + "").prev(".producHeaderTitle").height();
                    var offSet2 = $("#productsGridContainer-" + productViewId + "").prev(".producHeaderTitle").offset().top - heightHeaderTitle;

                    var offset1 = $("#productsGridContainer-" + productViewId + "").prev(".producHeaderTitle").offset();
                    var y = offset1.top + $(document.body).css("border-top");
                }

                var callbackSuccess = function () {

                    if (productsPageRequest.scrollUpToHeader) {
                        $('html, body').animate({
                            scrollTop: offSet2
                        }, 'slow');
                    }
                };

                Mjc.product.getProductsGridPartialViewAjax(productViewId, productsPageRequest, callbackSuccess);

                //$.ajax({
                //    type: 'POST',
                //    url: '/Product/ProductsGridPartialViewAjax',
                //    data: JSON.stringify(productsPageRequest),
                //    contentType: 'application/json; charset=utf-8',
                //    success: function (htmlResult) {

                //        if (productsPageRequest.scrollUpToHeader) {
                //            $('html, body').animate({
                //                scrollTop: offSet2
                //            }, 'slow');
                //        }


                //        $("#productsGridContainer-" + me.productViewId + "").empty().replaceWith(htmlResult);
                //    }
                //});
            }
        });

        //var widthProductContainer = $('.productContainer').width();
        //var widthComputed = widthProductContainer * 0.15;

        //var heightProductContainer = $('.productContainer').height();
        //var heightComputed = widthComputed * 1.35;

        //$('.productItem').css('width', widthComputed);
        //$('.productItem').css('height', heightComputed);

        $('.productItem').removeClass('displayNone');
        Mjc.util.unmask('#productsGridContainer-' + me.productViewId + '');

        //var wall = new Freewall("#productsGrid-" + me.productViewId + "");
        //wall.reset({
        //    selector: '.productItem',
        //    animate: true,
        //    delay: 0,
        //    //cellW: widthComputed,
        //    //cellH: heightComputed,
        //    cellH: function (height) {

        //        var heightProductContainer = $('.productContainer').height();
        //        var heightComputed = widthComputed * 1.35;

        //        return heightComputed;
        //    },
        //    cellW: function (width) {

        //        var widthProductContainer = $('.productContainer').width();
        //        var widthComputed = widthProductContainer * 0.15;

        //        return widthComputed;
        //    },
        //    onResize: function () {

        //        // temp solution by testing mobile client
        //        if (!Utils.isMobile()) {
        //            location.reload();
        //        }
        //        //wall.fitWidth();
        //        //wall.fitZone();
        //    },
        //    //onBlockReady: function () {
        //    //    console.log("onBlockReady");
        //    //}
        //});
        //wall.fitWidth();

    },
    getProductsGridPartialViewAjax: function (productViewId, productsPageRequest, callbackSuccess, url, selectorGridToChangePrefix) {

        if (selectorGridToChangePrefix == null) {
            selectorGridToChangePrefix = "#productsGridContainer-";
        }

        if (url == null) {
            url = '/Product/ProductsGridPartialViewAjax'
        }

        //Mjc.util.mask('#productsView-' + productsViewId + '');

        $.ajax({
            type: 'POST',
            url: url,
            data: JSON.stringify(productsPageRequest),
            contentType: 'application/json; charset=utf-8',
            success: function (htmlResult) {

                $(selectorGridToChangePrefix + productViewId + "").empty().replaceWith(htmlResult);
                //Mjc.util.unmask('#productsView-' + productsViewId + '');

                if (callbackSuccess != null && typeof callbackSuccess === "function") {
                    callbackSuccess();
                }
            }
        });
    },
    runProductsGridUtilities: function (productsViewId, productsPageRequest) {

        var me = this;
        new me.createProductsGridInstance(productsViewId, productsPageRequest);
    },
    runProductsViewUtilities: function (height, width, productsViewId, productsPageRequest) {
        var me = this;
        new me.createProductsViewInstance(height, width, productsViewId, productsPageRequest);
    },
    runProductItemViewUtilities: function (Product_ID) {

        // Change config of fotorama when fullscreen, to have "contain" photo
        $('.fotorama').on('fotorama:fullscreenenter fotorama:fullscreenexit', function (e, fotorama) {

            if (e.type === 'fotorama:fullscreenenter') {
                // Options for the fullscreen
                fotorama.setOptions({
                    fit: 'contain'
                });
            } else {
                // Back to normal settings
                fotorama.setOptions({
                    fit: 'cover'
                });
            }
        }).fotorama();

        $('.categoryListed').click(function () {
            var categoryId = $(this).attr('categoryId');
            Mjc.route.redirectToUrl('/Product/ProductsByCategoryView/' + categoryId);
        });

        $('#productShippingTitle').click(function (e) {
            Utils.accordionShowBySelector('#productShippingTitle');

            if (!$(this).hasClass('.shipmentInfoOpened')) {
                $(this).html('<b>-</b> <div class="productShippingTitle">Informations livraison</div>');
                $(this).addClass('.shipmentInfoOpened');
            } else {
                $(this).html('<b>+</b> <div class="productShippingTitle">Informations livraison</div>');
                $(this).removeClass('.shipmentInfoOpened');
            }
        });

        var dataJs = { Product_ID: Product_ID };
        var data = JSON.stringify(dataJs);

        Mjc.util.mask('#otherSameProductsGridLoading');

        $.ajax({
            type: 'POST',
            url: '/Product/OtherSameProductsGridPartialViewAjax',
            data: data,
            contentType: 'application/json; charset=utf-8',
            success: function (htmlResult) {

                $('#otherSameProductsGridLoading').replaceWith(htmlResult);
                Mjc.util.unmask('#otherSameProductsGridLoading');
            }
        });
    },
    runProductsViewUtilitiesWithHistory: function (height, width, currentHistoryState, productsViewIdToSearch) {

        var productsPageRequest = currentHistoryState.productsPageRequest;
        productsPageRequest.pageNumber = currentHistoryState.pageNumber;
        var productsViewId = productsViewIdToSearch;

        if (currentHistoryState.url == "/") {
            var searchResultArray = currentHistoryState.otherGrid.filter(function (obj) {
                return obj.ProductViewId == productsViewId;
            });

            if (searchResultArray != null && searchResultArray.length > 0) {
                var resultItem = searchResultArray[0];
                Mjc.product.runProductsViewUtilities(height, width, resultItem.ProductViewId, resultItem.pageRequest);
            }
        } else {
            Mjc.product.runProductsViewUtilities(height, width, productsViewId, productsPageRequest);
        }
    }
};
//#endregion

//#region User
var UserUtilities = {
    setLabelAccount: function () {
        $.post('/Customer/getCurrentCustomerUser', function (result) {
            var label = "se connecter";
            if (result.success && result.isAuthenticated) {
                label = result.CurrentUser.FullName;
                $("#myCustomerAccountLi").addClass("dropdown");
            }
            $("#myCustomerAccount").html(label);

        });
    },
    signOut: function () {
        $.post('/Login/SignOut', function (result) {
            if (result.success) {
                var currenUrl = $(location).attr('href');
                RouteUtilities.redirectToUrl(currenUrl);
            }
        });
    },
    runLoginUtilities: function () {

        var me = this;

        $(document).bind('keypress', function (e) {
            if (e.keyCode == 13) {
                $('.loginSubmitButton').trigger('click');
            }
        });

        $('.loginSubmitButton').click(function (e) {

            var formElement = $(this).parent();
            var email = $(formElement).find('.eMailInput').val();
            var password = $(formElement).find('.passwordInput').val();

            if (me.isEmailAndPasswordValid(email, password)) {

                Utils.mask('.loginForm');

                // Change opacity to display mask
                Utils.opacity('.userTexfield');
                Utils.opacity('.loginSubmitButton');

                //var me = this;
                $.post('/Login/CheckLoginPassword', { email: email, password: password }, function (result) {

                    if (result.success) {
                        $(formElement).submit();
                    } else {
                        Mjc.util.noty.displayResultMsg(result.success, result.msg, 2000);
                        $(formElement).find('.passwordInput').val("");
                        Mjc.util.unmask('.loginForm');
                        Utils.opacity('.userTexfield', 1);
                        Utils.opacity('.loginSubmitButton', 1);
                    }
                });
            }
        });
    },
    isEmailAndPasswordValid: function (email, password) {

        $('input').removeClass('mandatoryField');

        var isValid = false;
        if (email && !Mjc.util.isEmail(email)) {
            Mjc.util.noty.displayResultMsg(false, "Veuillez saisir un email valide", 2000);
            $('#eMailInput').addClass('mandatoryField');
        }
        else if (email && !password) {
            Mjc.util.noty.displayResultMsg(false, "Veuillez saisir les champs requis", 2000);
            $('#passwordInput').addClass('mandatoryField');
        }
        else if (email && password) {
            isValid = true;
        }

        return isValid;
    }
};
//#endregion

//#region Command
var CommandUtilities = {
    resetCommandSteps: function () {

        $(".accordionB").removeClass("active");

        //$(".panel").attr('style', 'maxHeight = null');
        $(".panel").collapse('hide');

        // Remove displayTable class that handle overflow when displaying commentText in radioButton
        $(".panel").removeClass("displayTable");
    },
    handlePanelShow: function (panelElement) {

        $(panelElement).collapse('show');

    },
    openCommandStep: function (stepNameId) {
        var me = this;

        $(".accordionB").removeClass("active");
        $("#" + stepNameId).parent('.disabled-button').removeClass('disabled-button');
        $("#" + stepNameId).addClass("active");
        $("#" + stepNameId).addClass("editableStep");
        $("#" + stepNameId).attr('data-toggle', "collapse");

        // Add displayTable class on selected panel to handle overflow when displaying commentText in radioButton
        $("#" + stepNameId).next('.panel').addClass("displayTable");
        var panelElement = $("#" + stepNameId).next('.panel').get(0);

        me.handlePanelShow(panelElement);
    },
    runStepsCheckoutHandling: function () {
        var me = this;

        $(".commandGreenButton").click(function (e) {

            var buttonId = $(this).attr('id');

            switch (buttonId) {
                case 'loginStepButton':

                    $('#eMailInputNoLogin').removeClass('mandatoryField');
                    var email = $('#eMailInputNoLogin').val();

                    if (!Mjc.util.isEmail(email)) {
                        Mjc.util.noty.displayResultMsg(false, "Veuillez saisir un email valide", 2000);
                        $('#eMailInputNoLogin').addClass('mandatoryField');
                    }
                    else {

                        Mjc.util.mask("#commandWithoutLoginBlock");
                        Mjc.util.opacity("#commandWithoutLoginBlock .userTexfield", 0.5);

                        $.post('/Customer/isEmailAlreadyUsed', { email: email }, function (result) {
                            Mjc.util.unmask("#commandWithoutLoginBlock");
                            Mjc.util.opacity("#commandWithoutLoginBlock .userTexfield", 1);
                            if (result.success) {
                                me.openCommandStep('invoiceStep');
                            } else {
                                Mjc.util.noty.displayResultMsg(result.success, result.msg, 2000);
                            }
                        });
                    }

                    break;
                case 'invoiceStepButton':
                    if (me.isAddressFormCorrectlyFilled('addressInvoiceForm')) {
                        me.openCommandStep('shipmentStep');
                    }
                    //if ($('#radioUseAddressInvoice').is(':checked')) {
                    //    me.openCommandStep('checkoutStep');
                    //} else {
                    //    me.openCommandStep('shipmentStep');
                    //}
                    break;
                case 'shipmentStepButton':

                    if ($('#radioShipmentNone').is(':checked') || me.isAddressFormCorrectlyFilled('addressShipmentForm')) {
                        me.openCommandStep('checkoutStep');
                    }
                    break;
            }
        });

        $(".accordionB").on('click', function (e) {

            if ($(this).hasClass('editableStep')) {

                //   me.resetCommandSteps();

                $(".accordionB").removeClass("active");

                this.classList.toggle("active");
                var panel = this.nextElementSibling;

                $(panel).addClass('displayTable');
                //  me.handlePanelShow(panel);
            }
        });
    },
    runStickyFixingCommandSummaryScript: function () {

        var me = this;
        //if (Utils.isWindowWitdhUnderDocumentWidth()) {
        //    return;
        //}
        //$("#commandSumarryGrid").sticky({ topSpacing: 70 });
    },
    runStepsCheckoutHandlingOrReload: function (isUserConnected) {
        var me = this;
        me.runStickyFixingCommandSummaryScript();
        if (!isUserConnected) {
            me.openCommandStep('loginStep');
        } else {
            me.openCommandStep('invoiceStep');
        }


        $.post('/Cart/getActualCartTotal', function (result) {
            if (result.success) {
                if (result.totalCount == 0) {
                    location.reload();
                } else {
                    me.runStepsCheckoutHandling();
                }
            }
        })
    },
    getCheckOutFormDatas: function () {
        var me = this;
        var emailNoLogin = null;
        var firstnameInvoice, lastnameInvoice, addressInvoice, cityInvoice, zipCodeInvoice, countryInvoice = "";
        var firstnameShipment, lastnameShipment, addressShipment, cityShipment, zipCodeShipment, countryShipment = "";
        var isAddressInvoiceSameFromShipment = false;
        var isOptionShipmentChecked = false;
        var isNoShipmentChecked = false;
        var shipmentType = -1;

        firstnameInvoice = $('#addressInvoiceForm input[name="firstname"]').val();
        lastnameInvoice = $('#addressInvoiceForm input[name="lastname"]').val();
        addressInvoice = $('#addressInvoiceForm input[name="address"]').val();
        cityInvoice = $('#addressInvoiceForm input[name="city"]').val();
        zipCodeInvoice = $('#addressInvoiceForm input[name="zipCode"]').val();
        countryInvoice = $('#addressInvoiceForm select[name="Country"]').val();

        firstnameShipment = $('#addressShipmentForm input[name="firstname"]').val();
        lastnameShipment = $('#addressShipmentForm input[name="lastname"]').val();
        addressShipment = $('#addressShipmentForm input[name="address"]').val();
        cityShipment = $('#addressShipmentForm input[name="city"]').val();
        zipCodeShipment = $('#addressShipmentForm input[name="zipCode"]').val();
        countryShipment = $('#addressShipmentForm select[name="Country"]').val();

        //if ($('#radioUseAddressInvoice').is(':checked')) {
        //    isAddressInvoiceSameFromShipment = true;
        //}

        if ($('#radioNormalShipment').is(':checked')) {
            isOptionShipmentChecked = true;
            shipmentType = 0;
        }

        if ($('#radioOptionShipment').is(':checked')) {
            isOptionShipmentChecked = true;
            shipmentType = 1;
        }

        if ($('#radioShipmentNone').is(':checked')) {
            isNoShipmentChecked = true;
            shipmentType = 2;
        }

        if ($('#radioNoLoginMode').is(':checked')) {
            emailNoLogin = $('#eMailInputNoLogin').val();
        }

        return {
            FirstNameInvoice: firstnameInvoice,
            LastNameInvoice: lastnameInvoice,
            AddressInvoice: addressInvoice,
            CityInvoice: cityInvoice,
            ZipCodeInvoice: zipCodeInvoice,
            CountryInvoice: countryInvoice,
            FirstNameShipment: firstnameShipment,
            LastNameShipment: lastnameShipment,
            AddressShipment: addressShipment,
            CityShipment: cityShipment,
            ZipCodeShipment: zipCodeShipment,
            CountryShipment: countryShipment,
            isAddressInvoiceSameFromShipment: isAddressInvoiceSameFromShipment,
            isOptionShipmentChecked: isOptionShipmentChecked,
            isNoShipmentChecked: isNoShipmentChecked,
            shipmentType: shipmentType,
            emailNoLogin: emailNoLogin
        };
    },
    runCheckoutProcessing: function () {

        var me = this;

        $('#checkoutStepButton').click(function (e) {

            var callbackOnOk = function () {

                var KEYCODE_ENTER = 13;
                var KEYCODE_ESC = 27;

                //$(document).keyup(function (e) {
                //    if (e.keyCode == KEYCODE_ESC) {
                //        Mjc.util.noty.displayMsg("Vous avez décidé d'annuler la procédure de paiement...", "error");
                //        setTimeout(function () {
                //            location.reload();
                //        }, 2000);

                //    };
                //});

                //var bodyEl = $("body");
                //$("body").addClass("maskPayment");
                Utils.mask('body');
                Mjc.util.noty.displayMsg("Vous allez être redirigé vers Paypal...");

                $("#checkoutStepButton").addClass('commandGreenButtonDisabled');
                $("#checkoutStepButton").removeClass('commandGreenButton');
                $("#checkoutStepButton").unbind("click");

                var dataForm = me.getCheckOutFormDatas();

                $.ajax({
                    type: 'POST',
                    url: '/Command/ProcessCheckout',
                    data: JSON.stringify(dataForm),
                    contentType: 'application/json; charset=utf-8',
                    success: function (result) {
                        if (result.success) {

                            $(document).keyup(function (e) {
                                if (e.keyCode == KEYCODE_ESC) {
                                    Mjc.util.noty.displayMsg("Vous avez décidé d'annuler la procédure de paiement...", "error");
                                    setTimeout(function () {
                                        location.reload();
                                    }, 2000);

                                }
                                ;
                            });

                            var buttonHtml = result.buttonHtml;
                            $("#checkoutHiddenForm").replaceWith(buttonHtml);
                            $("#checkoutPaypalForm").submit();
                        }
                    }
                });
            }

            var config = {
                buttonOkText: 'Continuer',
                butonCancelText: 'Annuler',
                text: 'En continuant, vous allez être redirigé vers Paypal pour finaliser votre commande et votre panier va être vidé, êtes-vous sur ?',
                callbackOnOk: callbackOnOk
            };

            Mjc.util.noty.confirmDialog(config);
        });
    },
    isAddressFormCorrectlyFilled: function (addressFormId) {

        //reset form by removing red warning border
        //$('#' + addressFormId + ' input').removeClass('mandatoryField');
        //$('#' + addressFormId + ' select').removeClass('mandatoryField');

        firstname = $('#' + addressFormId + ' input[name="firstname"]').val();
        lastname = $('#' + addressFormId + ' input[name="lastname"]').val();
        address = $('#' + addressFormId + ' input[name="address"]').val();
        city = $('#' + addressFormId + ' input[name="city"]').val();
        zipCode = $('#' + addressFormId + ' input[name="zipCode"]').val();
        country = $('#' + addressFormId + ' select[name="Country"]').val();


        //#region Ontology
        if (!firstname) {
            $('#' + addressFormId + ' input[name="firstname"]').addClass('mandatoryField');
        }

        if (!lastname) {
            $('#' + addressFormId + ' input[name="lastname"]').addClass('mandatoryField');
        }

        if (!address) {
            $('#' + addressFormId + ' input[name="address"]').addClass('mandatoryField');
        }

        if (!city) {
            $('#' + addressFormId + ' input[name="city"]').addClass('mandatoryField');
        }

        if (!zipCode) {
            $('#' + addressFormId + ' input[name="zipCode"]').addClass('mandatoryField');
        }

        if (!country) {
            $('#' + addressFormId + ' input[name="Country"]').addClass('mandatoryField');
        }
        //#endregion

        var isCorrectlyFilled = firstname && lastname && address && city && zipCode && country;

        if (!isCorrectlyFilled) {
            Utils.noty.notyMessage({
                text: "Veuillez remplir les champs requis",
                type: "error",
                position: 'center',
                timeout: 2500
            });
        }

        return isCorrectlyFilled;
    },
    runCommandAddressShipmentUtilities: function (optionShipmentPrice) {

        var me = this;

        var optionShipmentPrice = optionShipmentPrice;
        var priceShipment = parseFloat($('#commandSummaryDeliveryPrice').attr('value'));
        var priceComputed = priceShipment + optionShipmentPrice;

        $('#radioOptionShipment').click(function (e) {

            $(".shipmentComment").attr('style', 'maxHeight = null');
            var commentElement = $('#optionShipmentComment').get(0);

            if (commentElement.style.maxHeight) {
                commentElement.style.maxHeight = null;
            } else {
                commentElement.style.maxHeight = commentElement.scrollHeight + "px";
            }

            me.handlePanelShow(commentElement);

            $('#commandSummaryRowTotal').addClass('displayNone');
            $('#commandSummaryRowTotalWithNoCharges').addClass('displayNone');
            $('#commandDeliveryPriceLineThrough').addClass('displayNone');


            $('#commandDeliveryPriceNormal').removeClass('displayNone');
            $('#commandSummaryRowOptionShipment').removeClass('displayNone');
            $('#commandSummaryRowTotalWithOptionShipment').removeClass('displayNone');

            $("#addressShipmentForm input").prop('disabled', false);
            $("#addressShipmentForm select").prop('disabled', false);
            $("#addressShipmentForm").css('opacity', 1);

        });

        $('#radioNormalShipment').click(function (e) {

            $(".shipmentComment").attr('style', 'maxHeight = null');

            $('#commandSummaryRowOptionShipment').addClass('displayNone');
            $('#commandSummaryRowTotalWithOptionShipment').addClass('displayNone');
            $('#commandSummaryRowTotalWithNoCharges').addClass('displayNone');
            $('#commandDeliveryPriceLineThrough').addClass('displayNone');

            $('#commandDeliveryPriceNormal').removeClass('displayNone');
            $('#commandSummaryRowTotal').removeClass('displayNone');

            $("#addressShipmentForm input").prop('disabled', false);
            $("#addressShipmentForm select").prop('disabled', false);
            $("#addressShipmentForm").css('opacity', 1);

        });

        $('#radioShipmentNone').click(function (e) {

            $(".shipmentComment").attr('style', 'maxHeight = null');
            //var commentElement = $('#noneShipmentComment').get(0);
            //me.handlePanelShow(commentElement);

            $('#commandSummaryRowOptionShipment').addClass('displayNone');
            $('#commandSummaryRowTotalWithOptionShipment').addClass('displayNone');
            $('#commandSummaryRowTotal').addClass('displayNone');
            $('#commandDeliveryPriceNormal').addClass('displayNone');

            $('#commandSummaryRowTotalWithNoCharges').removeClass('displayNone');
            $('#commandDeliveryPriceLineThrough').removeClass('displayNone');

            $("#addressShipmentForm input").prop('disabled', true);
            $("#addressShipmentForm select").prop('disabled', true);
            $("#addressShipmentForm").css('opacity', 0.2);
        });

        $("#checkboxSameAddressThanInvoice").click(function (e) {
            if ($(this).is(':checked')) {

                var firstname = $('#addressInvoiceForm input[name="firstname"]').val();
                var lastname = $('#addressInvoiceForm input[name="lastname"]').val();
                var address = $('#addressInvoiceForm input[name="address"]').val();
                var city = $('#addressInvoiceForm  input[name="city"]').val();
                var zipCode = $('#addressInvoiceForm  input[name="zipCode"]').val();
                var country = $('#addressInvoiceForm  select[name="Country"]').val();

                $('#addressShipmentForm input[name="firstname"]').val(firstname);
                $('#addressShipmentForm input[name="lastname"]').val(lastname);
                $('#addressShipmentForm input[name="address"]').val(address);
                $('#addressShipmentForm  input[name="city"]').val(city);
                $('#addressShipmentForm  input[name="zipCode"]').val(zipCode);
                $('#addressShipmentForm  select[name="Country"]').val(country);
            } else {
                //$('#addressShipmentForm input[name="firstname"]').val("");
                //$('#addressShipmentForm input[name="lastname"]').val("");
                //$('#addressShipmentForm input[name="address"]').val("");
                //$('#addressShipmentForm  input[name="city"]').val("");
                //$('#addressShipmentForm  input[name="zipCode"]').val("");
                //$('#addressShipmentForm  select[name="Country"]').val("");

                $('#addressShipmentForm')[0].reset();
            }
        });
    },
    runCommandLoginUtilities: function () {

        var me = this;

        $("#commandWithoutLogin").css('opacity', 0.2);
        $("#eMailInputNoLogin").prop('disabled', true);
        $("#loginStepButton").addClass('loginButtonDisabled');

        $(document).bind('keypress', function (e) {
            if (e.keyCode == 13) {
                $('#loginSubmitButton').trigger('click');
            }
        });

        $('#loginSubmitButton').click(function (e) {

            var email = $('#eMailInput').val();
            var password = $('#passwordInput').val();

            if (Mjc.user.isEmailAndPasswordValid(email, password)) {

                Mjc.util.mask('#loginForm');

                // Change opacity to display mask
                Mjc.util.opacity('.userTexfield');
                Mjc.util.opacity('#loginSubmitButton');

                $.post('/Login/LoginAjax', { email: email, password: password }, function (result) {

                    if (result.success) {
                        Mjc.util.noty.displayResultMsg(result.success, result.msg, 2000);
                        setTimeout(function () {
                            location.reload();
                        }, 1500);

                    } else {
                        Mjc.util.noty.displayResultMsg(result.success, result.msg, 2000);
                        $('#passwordInput').val("");
                    }

                    Mjc.util.unmask('#loginForm');
                    Mjc.util.opacity('.userTexfield', 1);
                    Mjc.util.opacity('#loginSubmitButton', 1);
                });
            }
        });

        $('#radioNoLoginMode').click(function (e) {
            $("#eMailInput").prop('disabled', true);
            $("#passwordInput").prop('disabled', true);
            $("#commandWithLogin").css('opacity', 0.2);
            $("#loginSubmitButton").prop('disabled', true);
            $("#loginSubmitButton").addClass('loginButtonDisabled');

            $("#eMailInputNoLogin").prop('disabled', false);
            $("#commandWithoutLogin").css('opacity', 1);
            $("#loginStepButton").prop('disabled', false);
            $("#loginStepButton").removeClass('loginButtonDisabled');

        });

        $('#radioCustomerLoginMode').click(function (e) {

            $("#eMailInput").prop('disabled', false);
            $("#passwordInput").prop('disabled', false);
            $("#commandWithLogin").css('opacity', 1);
            $("#loginSubmitButton").prop('disabled', false);
            $("#loginSubmitButton").removeClass('loginButtonDisabled');

            $("#commandWithoutLogin").css('opacity', 0.2);
            $("#eMailInputNoLogin").prop('disabled', true);
            $("#loginStepButton").prop('disabled', true);
            $("#loginStepButton").addClass('loginButtonDisabled');
        });
    }
};
//#endregion

//#region Header
var HeaderUtilities = {
    runHeaderUtilities: function () {

        var currentState = history.state;

        $(window).on('popstate', function (e) {
            var currentState = history.state;
            Mjc.history.handleHistoryItem(currentState);
        });

        $('.itemMenuClickable').click(function (e) {
            var menuItemid = $(this).attr("menuItemId");
            Mjc.product.loadProductsByMenuItemId(menuItemid);
            //var object = {
            //    type: 'itemMenuClickable',
            //    action: function () {
            //        Mjc.product.loadProductsByMenuItemId(menuItemid);
            //    }
            //};
            //history.pushState(object, null, null);
            e.stopPropagation();
        });

        $('#SignOutButton').click(function () {
            Mjc.user.signOut();
        });

        $('#user-responsive-btn').click(function () {

            $('.main-website').removeClass('collapseMenu');

            $('.main-container').append('<div class="modal-mask"></div>');

            setTimeout(function () {
                $('.modal-mask').addClass('active');
                $('.main-website').addClass('expand');
                $('.collapsible-menu').removeClass('position-absolute');
                $('.collapsible-menu').addClass('position-fixed');
            }, 10);
        });

        $('#collapsible-btn').click(function () {
            $('.main-website').removeClass('expand');
            $('.main-website').addClass('collapseMenu');
            $('.collapsible-menu').removeClass('position-fixed');
            $('.collapsible-menu').addClass('position-absolute');
            $('.modal-mask').remove();
        });


        $('.menu-accordion-item').click(function (e) {
            var parentElement = $(this).parent();
            Utils.accordionShowByElement(parentElement);
            if ($(this).hasClass('opened')) {
                $(this).removeClass('opened');
            } else {
                $(this).addClass('opened');
            }

        });

        //Mjc.cart.refreshActualCartTotal();
        //Mjc.user.setLabelAccount();

        ////var distanceToTop = $('#menu').offset().top;

        ////$(window).scroll(function () {

        ////    var isWindowWidthOk = !Utils.isWindowWitdhUnderDocumentWidth();

        ////    var isWindowWidthOk = true;

        ////    var isApplyed = false;
        ////    var currentScroll = $(this).scrollTop();
        ////    if (currentScroll >= distanceToTop && !isApplyed && isWindowWidthOk) {
        ////        $('#menu').addClass("menuFixed");
        ////        isApplyed = true;
        ////    } else {
        ////        $('#menu').removeClass("menuFixed");
        ////        isApplyed = false;
        ////    }
        ////});
    },
    runCarouselUtilities: function () {

        var distanceToTop = $('#menu').offset().top;
        var distanceBandeauToTop = $('#paintBandeau').offset().top
        //var $fotorama = $('.fotorama');
        var $fotorama = $('.fotorama').fotorama();
        var interval = $fotorama.data('autoplay');
        var fotorama = $fotorama.data('fotorama');
        fotorama.startAutoplay(interval);

        var iscssApplyed = false;
        var lastScrollPosition = 0;

        $(window).scroll(function () {

            var currentScroll = $(this).scrollTop();
            if (currentScroll >= distanceToTop && lastScrollPosition < currentScroll && !iscssApplyed) {
                //fotorama.stopAutoplay();
                iscssApplyed = true;
            } else if (currentScroll < distanceToTop && lastScrollPosition > currentScroll && iscssApplyed) {
                //fotorama.startAutoplay(interval);
            }
            lastScrollPosition = currentScroll;
        });
    }
};
//#endregion

//#region Inscription
InscriptionUtilities = {
    runInscriptionProcessing: function () {

        var me = this;

        $('.inscriptionSubmitButton').click(function (e) {
            // Special stuff to do when this link is clicked...
            var form = $('#inscriptionCustomerForm').serialize();

            if (me.isInscriptionFormCorrectlyFilled()) {

                Mjc.util.mask('#inscriptionCustomerForm');
                Mjc.util.opacity('#inscriptionCustomerForm input', 0.5);
                Mjc.util.opacity('#inscriptionCustomerForm select', 0.5);

                $.ajax({
                    type: 'POST',
                    url: '/Login/InscriptionCustomer',
                    dataType: 'json',
                    data: form,
                    success: function (result) {
                        var msgToDisplay = "";
                        var type = 'error';

                        if (!result.success) {

                            Mjc.util.unmask('#inscriptionCustomerForm');
                            Mjc.util.opacity('#inscriptionCustomerForm input', 1);
                            Mjc.util.opacity('#inscriptionCustomerForm select', 1);

                            switch (result.msg) {
                                case -50:
                                    msgToDisplay = "Les mots de passe saisies doivent avoir au moins 6 caractères"
                                    break;
                                case -100:
                                    msgToDisplay = "Les champs requis ne sont pas correctement remplis"
                                    break;
                                case -200:
                                    msgToDisplay = "Les mots de passe saisies ne correspondent pas"
                                    break;
                                case -300:
                                    msgToDisplay = "Cet email est déjà utilisé"
                                    break;
                            }

                            Mjc.util.noty.displayResultMsg(result.success, msgToDisplay);

                        } else {

                            if (result.msg == 100) {
                                msgToDisplay = "Un email vous a été envoyé"
                                type = 'success';
                            }

                            $.ajax({
                                type: 'POST',
                                url: '/Customer/CustomerInscriptionFirstStepValidationPartialView',
                                success: function (htmlResult) {

                                    $(".inscriptionContainer").replaceWith(htmlResult);
                                }
                            });
                        }
                    }
                });
            }

            // Cancel the default action
            e.preventDefault();
        });
    },
    isInscriptionFormCorrectlyFilled: function () {

        var errorMsg = "";
        var customerFormId = 'inscriptionCustomerForm';
        //reset form by removing red warning border
        $('#' + customerFormId + ' input').removeClass('mandatoryField');
        //$('#' + customerFormId + ' select').removeClass('mandatoryField');

        email = $('#' + customerFormId + ' input[name="eMail"]').val();
        password = $('#' + customerFormId + ' input[name="Password"]').val();
        confirmedPassword = $('#' + customerFormId + ' input[name="ConfirmedPassword"]').val();
        firstname = $('#' + customerFormId + ' input[name="FirstName"]').val();
        lastname = $('#' + customerFormId + ' input[name="LastName"]').val();

        //#region Ontology
        if (!email) {
            $('#' + customerFormId + ' input[name="eMail"]').addClass('mandatoryField');
        }

        if (!password) {
            $('#' + customerFormId + ' input[name="Password"]').addClass('mandatoryField');
        }

        if (!confirmedPassword) {
            $('#' + customerFormId + ' input[name="ConfirmedPassword"]').addClass('mandatoryField');
        }

        if (!firstname) {
            $('#' + customerFormId + ' input[name="FirstName"]').addClass('mandatoryField');
        }

        if (!lastname) {
            $('#' + customerFormId + ' input[name="LastName"]').addClass('mandatoryField');
        }

        //#endregion

        var isCorrectlyFilled = email && password && confirmedPassword && firstname && lastname;

        if (!isCorrectlyFilled) {
            errorMsg = "Veuillez remplir les champs requis";
        }
        else if (!Utils.isEmail(email)) {
            errorMsg = "Veuillez saisir un email valide";
            isCorrectlyFilled = false;

            $('#' + customerFormId + ' input[name="eMail"]').addClass('mandatoryField');
        }
        else if (password != confirmedPassword) {
            errorMsg = "Les mots de passe ne correspondent pas";
            isCorrectlyFilled = false;

            $('#' + customerFormId + ' input[name="Password"]').addClass('mandatoryField');
            $('#' + customerFormId + ' input[name="ConfirmedPassword"]').addClass('mandatoryField');

        } else if (password.length < 6 || confirmedPassword.length < 6) {
            errorMsg = "Les mots de passe doivent avoir un minimum de 6 caractères";
            isCorrectlyFilled = false;

            $('#' + customerFormId + ' input[name="Password"]').addClass('mandatoryField');
            $('#' + customerFormId + ' input[name="ConfirmedPassword"]').addClass('mandatoryField');
        }

        if (!isCorrectlyFilled) {
            Utils.noty.notyMessage({
                text: errorMsg,
                type: "error",
                position: 'center',
                timeout: 2500
            });
        }
        return isCorrectlyFilled;
    },
    runPasswordForgottenUtilities: function () {

        $('.emailSubmitButton').click(function (e) {

            var formElement = $(this).parent();
            var email = $(formElement).find('.eMailInput input[name="eMail"]').val();

            if (!Mjc.util.isEmail(email)) {
                Mjc.util.noty.displayResultMsg(false, "Veuillez saisir un email valide", 2000);
                return;
            }

            Utils.mask('.formArea');
            // Change opacity to display mask
            Utils.opacity('.userTexfield');
            Utils.opacity('.emailSubmitButton');

            $.post('/Login/EmailPasswordForRedefinition', { eMail: email }, function (result) {
                Mjc.util.noty.displayResultMsg(result.success, result.msg, 2000);
                $(formElement).find('.eMailInput').val('');
                Mjc.util.unmask('.formArea');
                Utils.opacity('.userTexfield', 1);
                Utils.opacity('.emailSubmitButton', 1);
            });
        });
    }
};
//#endregion

//#region Customer
CustomerUtilities = {
    runCustomerAccountEmailPasswordUtilities: function () {
        var me = this;

        var phones = [{ "mask": "(+33) # ## ## ## ##" }];
        $('#mobilePhoneInput').inputmask({
            mask: phones,
            greedy: true,
            definitions: { '#': { validator: "[0-9]", cardinality: 1 } }
        });

        $('.customerAccountEditButton').click(function (e) {

            if (me.isCustomerAccountEmailPasswordCorrectlyFilled()) {
                // Special stuff to do when this link is clicked...
                var form = $('#customerAccountFormEdition').serialize();

                Utils.mask('#customerAccountFormEdition');
                Utils.opacity('#customerAccountFormEdition input');

                $.ajax({
                    type: 'POST',
                    url: '/Customer/editCustomerAccountParameters',
                    dataType: 'json',
                    data: form,
                    success: function (result) {
                        Mjc.util.noty.displayResultMsg(result.success, result.msg, 2000);

                        $.ajax({
                            type: 'GET',
                            url: '/Customer/CustomerAccountEmailPasswordPartialView',
                            success: function (htmlResult) {
                                $(".customerAccountContent").replaceWith(htmlResult);
                            }
                        });
                    }
                });
            }
            // Cancel the default action
            e.preventDefault();
        });

        $('.signOutButton').click(function () {
            Mjc.user.signOut();
        });
    },
    runCustomerAccountAddressShipmentFormUtilities: function () {
        var me = this;

        $('.customerAccountEditButton').click(function (e) {

            if (me.isCustomerAccountPersonalDataFormCorrectlyFilled()) {
                // Special stuff to do when this link is clicked...
                var form = $('#customerAccountFormEdition').serialize();

                Utils.mask('#customerAccountFormEdition');
                Utils.opacity('#customerAccountFormEdition input');
                Utils.opacity('#customerAccountFormEdition select');

                $.ajax({
                    type: 'POST',
                    url: '/Customer/editCustomerAccountAddressShipment',
                    dataType: 'json',
                    data: form,
                    success: function (result) {
                        Mjc.util.noty.displayResultMsg(result.success, result.msg, 2000);

                        $.ajax({
                            type: 'GET',
                            url: '/Customer/CustomerAccountAddressShipmentFormPartialView',
                            success: function (htmlResult) {
                                $(".customerAccountContent").replaceWith(htmlResult);
                            }
                        });
                    }
                });
            }

            // Cancel the default action
            e.preventDefault();
        });

        $('.signOutButton').click(function () {
            Mjc.user.signOut();
        });
    },
    runCustomerAccountAddressInvoiceFormUtilities: function () {
        var me = this;

        $('.customerAccountEditButton').click(function (e) {

            if (me.isCustomerAccountPersonalDataFormCorrectlyFilled()) {
                // Special stuff to do when this link is clicked...
                var form = $('#customerAccountFormEdition').serialize();

                Utils.mask('#customerAccountFormEdition');
                Utils.opacity('#customerAccountFormEdition input');
                Utils.opacity('#customerAccountFormEdition select');

                $.ajax({
                    type: 'POST',
                    url: '/Customer/editCustomerAccountAddressInvoice',
                    dataType: 'json',
                    data: form,
                    success: function (result) {
                        Mjc.util.noty.displayResultMsg(result.success, result.msg, 2000);

                        $.ajax({
                            type: 'GET',
                            url: '/Customer/CustomerAccountAddressInvoiceFormPartialView',
                            success: function (htmlResult) {
                                $(".customerAccountContent").replaceWith(htmlResult);
                            }
                        });
                    }
                });
            }

            // Cancel the default action
            e.preventDefault();
        });

        $('.signOutButton').click(function () {
            Mjc.user.signOut();
        });
    },
    isCustomerAccountEmailPasswordCorrectlyFilled: function () {

        var errorMsg = "";
        var customerFormId = 'customerAccountFormEdition';
        //reset form by removing red warning border
        $('#' + customerFormId + ' input').removeClass('mandatoryField');
        //$('#' + customerFormId + ' select').removeClass('mandatoryField');

        email = $('#' + customerFormId + ' input[name="Email"]').val();
        password = $('#' + customerFormId + ' input[name="Password"]').val();
        confirmedPassword = $('#' + customerFormId + ' input[name="ConfirmedPassword"]').val();
        firstname = $('#' + customerFormId + ' input[name="FirstName"]').val();
        lastname = $('#' + customerFormId + ' input[name="LastName"]').val();

        //#region Ontology
        if (!email) {
            $('#' + customerFormId + ' input[name="Email"]').addClass('mandatoryField');
        }

        if (!firstname) {
            $('#' + customerFormId + ' input[name="FirstName"]').addClass('mandatoryField');
        }

        if (!lastname) {
            $('#' + customerFormId + ' input[name="LastName"]').addClass('mandatoryField');
        }

        //#endregion

        var isCorrectlyFilled = email && firstname && lastname;

        if (!isCorrectlyFilled) {
            errorMsg = "Veuillez remplir les champs requis";
        }
        else if (!Utils.isEmail(email)) {
            errorMsg = "Veuillez saisir un email valide";
            isCorrectlyFilled = false;

            $('#' + customerFormId + ' input[name="Email"]').addClass('mandatoryField');
        }
        else if ((password || confirmedPassword) && (password != confirmedPassword)) {

            errorMsg = "Les mots de passe ne correspondent pas";
            isCorrectlyFilled = false;

            $('#' + customerFormId + ' input[name="Password"]').addClass('mandatoryField');
            $('#' + customerFormId + ' input[name="ConfirmedPassword"]').addClass('mandatoryField');
        }

        if (!isCorrectlyFilled) {
            Utils.noty.notyMessage({
                text: errorMsg,
                type: "error",
                position: 'center',
                timeout: 2500
            });
        }
        return isCorrectlyFilled;
    },
    isCustomerAccountPersonalDataFormCorrectlyFilled: function () {

        var errorMsg = "";
        var customerFormId = 'customerAccountFormEdition';

        //reset form by removing red warning border
        $('#' + customerFormId + ' input').removeClass('mandatoryField');
        $('#' + customerFormId + ' select').removeClass('mandatoryField');

        address = $('#' + customerFormId + ' input[name="address"]').val();
        zipCode = $('#' + customerFormId + ' input[name="zipCode"]').val();
        city = $('#' + customerFormId + ' input[name="city"]').val();
        country = $('#' + customerFormId + ' select[name="Country"]').val();


        //#region Ontology

        if (!address) {
            $('#' + customerFormId + ' input[name="address"]').addClass('mandatoryField');
        }

        if (!zipCode) {
            $('#' + customerFormId + ' input[name="zipCode"]').addClass('mandatoryField');
        }

        if (!city) {
            $('#' + customerFormId + ' input[name="city"]').addClass('mandatoryField');
        }

        if (!country) {
            $('#' + customerFormId + ' input[name="Country"]').addClass('mandatoryField');
        }

        //#endregion

        var isCorrectlyFilled = address && zipCode && city && country;

        if (!isCorrectlyFilled) {
            errorMsg = "Veuillez remplir les champs requis";
        }

        if (!isCorrectlyFilled) {
            Utils.noty.notyMessage({
                text: errorMsg,
                type: "error",
                position: 'center',
                timeout: 2500
            });
        }
        return isCorrectlyFilled;
    },
    runCustomerRedefinitionPasswordFormUtilities: function (tokenLinkGenerated) {

        $('.redefinitionPasswordSubmitButton').click(function (e) {

            var formElement = $(this).parent();
            var password = $(formElement).find('.passwordInput').val();
            var confirmedPassword = $(formElement).find('.confirmedPasswordInput').val();

            //if (!Mjc.util.isEmail(email)) {
            //    Mjc.util.noty.displayResultMsg(false, "Veuillez saisir un email valide", 2000);
            //    return;
            //}


            Utils.mask('.formArea');
            // Change opacity to display mask
            Utils.opacity('.userTexfield');
            Utils.opacity('.redefinitionPasswordSubmitButton');

            $.post('/Customer/redefinePassword', {
                tokenLink: tokenLinkGenerated,
                password: password,
                confirmedPassword: confirmedPassword
            }, function (result) {

                Mjc.util.noty.displayResultMsg(result.success, result.msg, 2000);

                if (result.success) {
                    $.ajax({
                        type: 'POST',
                        url: '/Customer/CustomerRedefinePasswordValidationPartialView',
                        success: function (htmlResult) {

                            $(".loginContainer").replaceWith(htmlResult);
                        }
                    });
                } else {

                    Utils.unmask('.formArea');
                    Utils.opacity('.userTexfield', 1);
                    Utils.opacity('.redefinitionPasswordSubmitButton', 1);
                    $('.passwordInput').val('');
                    $('.confirmedPasswordInput').val('');
                }
            });
        });
    },
    handleClickCustomerAccountView: function (itemClicked, isAddHistory) {

        if (isAddHistory == null) {
            isAddHistory = true;
        }

        if ($(itemClicked).hasClass("customerAccountMenuItem")) {
            var url = $(itemClicked).attr("url");
        }
        else {
            var url = $('#CustomerAccountSelect option:selected').attr('url');
        }

        if (!$(itemClicked).hasClass("loadingMask")) {

            $(".customerAccountMenuItem").removeClass('customerMenuItemSelected');
            $(itemClicked).addClass("customerMenuItemSelected");

            if (url != "") {
                if (isAddHistory) {

                    var stateObj = {
                        customerViewId: $(itemClicked).attr("id"),
                        customerViewUrl: url,
                        url: window.location.pathname
                    };

                    Mjc.history.addCustomerAccountHistoryPushState(stateObj);
                }

                $.ajax({
                    type: 'GET',
                    url: url + "PartialView",
                    success: function (htmlResult) {
                        $(".customerAccountContent").replaceWith(htmlResult);
                    },
                    statusCode: {
                        403: function (xhr) {
                            location.reload();  // If session expired, it will return a 403 HTTP CODE which means that ressources is not more accessible
                        }
                    }
                });
            }
        }
    }
};
//#endregion

//#region Information
InformationUtilities = {
    runQuestionsAnwsersUtilites: function () {


        $('.informationSimpleText h5').click(function (e) {

            Utils.accordionShowByElement($(this));

            if (!$(this).hasClass('opened')) {
                $(this).addClass('opened');
            } else {
                $(this).removeClass('opened');
            }
        });
    },
    runContactFormUtilities: function () {
        var me = this;

        var phones = [{ "mask": "(+33) # ## ## ## ##" }];
        $('#phoneInput').inputmask({
            mask: phones,
            greedy: true,
            definitions: { '#': { validator: "[0-9]", cardinality: 1 } }
        });

        $('.contactFormSubmitButton').click(function (e) {
            // Special stuff to do when this link is clicked...
            var form = $('#contactForm').serialize();

            if (me.isContactFormCorrectlyFilled()) {

                Mjc.util.mask('#contactForm');
                Mjc.util.opacity('#contactForm input', 0.5);
                Mjc.util.opacity('#contactForm textarea', 0.5);

                $.ajax({
                    type: 'POST',
                    url: '/Information/SendContactFormMessage',
                    dataType: 'json',
                    data: form,
                    success: function (result) {
                        var msgToDisplay = "";
                        var type = 'error';

                        Mjc.util.unmask('#contactForm');
                        Mjc.util.opacity('#contactForm input', 1);
                        Mjc.util.opacity('#contactForm textarea', 1);

                        Mjc.util.noty.displayResultMsg(result.success, result.msg);

                        $('#contactForm input[name="Email"]').val("");
                        $('#contactForm input[name="FirstName"]').val("");
                        $('#contactForm input[name="LastName"]').val("");
                        $('#contactForm input[name="Object"]').val("");
                        $('#contactForm input[name="Phone"]').val("");
                        $('#contactForm textarea[name="Message"]').val("");
                    }
                });
            }

            // Cancel the default action
            e.preventDefault();
        });
    },
    isContactFormCorrectlyFilled: function () {

        var errorMsg = "";
        var contactFormId = 'contactForm';
        //reset form by removing red warning border
        $('#' + contactFormId + ' input').removeClass('mandatoryField');
        $('#' + contactFormId + ' textarea').removeClass('mandatoryField');

        var email = $('#' + contactFormId + ' input[name="Email"]').val();
        var firstname = $('#' + contactFormId + ' input[name="FirstName"]').val();
        var lastname = $('#' + contactFormId + ' input[name="LastName"]').val();
        var object = $('#' + contactFormId + ' input[name="Object"]').val();

        var phone = $('#' + contactFormId + ' input[name="Phone"]').val();
        var message = $('#' + contactFormId + ' textarea[name="Message"]').val();

        //#region Ontology
        if (!email) {
            $('#' + contactFormId + ' input[name="Email"]').addClass('mandatoryField');
        }

        if (!firstname) {
            $('#' + contactFormId + ' input[name="FirstName"]').addClass('mandatoryField');
        }

        if (!lastname) {
            $('#' + contactFormId + ' input[name="LastName"]').addClass('mandatoryField');
        }

        if (!object) {
            $('#' + contactFormId + ' input[name="Object"]').addClass('mandatoryField');
        }

        if (!message) {
            $('#' + contactFormId + ' textarea[name="Message"]').addClass('mandatoryField');
        }

        //#endregion

        var isCorrectlyFilled = email && firstname && lastname && object && message;

        if (!isCorrectlyFilled) {
            errorMsg = "Veuillez remplir les champs requis";
        }
        else if (!Utils.isEmail(email)) {
            errorMsg = "Veuillez saisir un email valide";
            isCorrectlyFilled = false;

            $('#' + contactFormId + ' input[name="Email"]').addClass('mandatoryField');
        }


        if (!isCorrectlyFilled) {
            Utils.noty.notyMessage({
                text: errorMsg,
                type: "error",
                position: 'center',
                timeout: 2500
            });
        }
        return isCorrectlyFilled;
    },
}
//#endregion

//#region History
var arrayGridRequest = [];

var HistoryUtilities = {
    handleHistoryItem: function (state) {

        if (state == null) {
            location.reload();
        }
        else if (state.historyType == "ProductGrid") {

            var productsPageRequest = state.productsPageRequest;
            productsPageRequest.pageNumber = state.pageNumber;

            Mjc.util.mask('#productsGridContainer-' + state.productViewId + '');

            Mjc.product.getProductsGridPartialViewAjax(state.productViewId, productsPageRequest, null, null);

            //$.ajax({
            //    type: 'POST',
            //    url: '/Product/ProductsGridPartialViewAjax',
            //    data: JSON.stringify(productsPageRequest),
            //    contentType: 'application/json; charset=utf-8',
            //    success: function (htmlResult) {

            //        //if (productsPageRequest.scrollUpToHeader) {
            //        //    $('html, body').animate({
            //        //        scrollTop: offSet2
            //        //    }, 'slow');
            //        //}

            //        $("#productsGridContainer-" + state.productViewId + "").empty().replaceWith(htmlResult);
            //    }
            //});

        } else if (state.historyType == "CustomerAccount") {

            var customerViewId = state.customerViewId;
            var itemClicked = $('#' + customerViewId);
            Mjc.customer.handleClickCustomerAccountView(itemClicked, false);

            //$('#' + customerViewId).click();

            //$.ajax({
            //    type: 'GET',
            //    url: state.customerViewUrl,
            //    success: function (htmlResult) {
            //        $(".customerAccountContent").replaceWith(htmlResult);
            //    }
            //});
        }
    },
    addPageRequestIntoIndexArray: function (pageRequest) {
        arrayGridRequest.push({ ProductViewId: pageRequest.ProductsViewId, pageRequest: pageRequest });
    },
    addProductPageGridHistoryPushState: function (stateObj) {

        stateObj.historyType = "ProductGrid";

        if (stateObj.url == "/") {
            var copiedArrayObject = JSON.parse(JSON.stringify(arrayGridRequest));

            var searchItemToRemove = copiedArrayObject.filter(function (obj) {
                return obj.ProductViewId == stateObj.productViewId;
            });

            if (searchItemToRemove != null && searchItemToRemove.length > 0) {
                var itemToRemove = searchItemToRemove[0];
                var index = copiedArrayObject.map(function (item) {
                    return item.ProductViewId;
                }).indexOf(itemToRemove.ProductViewId);
                copiedArrayObject.splice(index, 1);
                copiedArrayObject.push({
                    ProductViewId: stateObj.productViewId,
                    pageRequest: stateObj.productsPageRequest
                });

                //update global arrayGridRequest
                arrayGridRequest = copiedArrayObject;
                stateObj.otherGrid = copiedArrayObject;
            }

        }
        ;
        history.pushState(stateObj, null, null);
    },
    addCustomerAccountHistoryPushState: function (stateObj) {
        stateObj.historyType = "CustomerAccount";
        history.pushState(stateObj, null, stateObj.customerViewUrl + "View");
    }
}
//#endregion


var Mjc = {
    util: Utils,
    cart: CartUtilities,
    product: ProductUtilities,
    user: UserUtilities,
    route: RouteUtilities,
    command: CommandUtilities,
    header: HeaderUtilities,
    customer: CustomerUtilities,
    inscription: InscriptionUtilities,
    information: InformationUtilities,
    history: HistoryUtilities
};


