var v_MenuId = '';
$(document).ready(function () {
     
});
 
var v_Htmlraw_Success   = '<div class="text-center"><img src="../Imagen/success2.jpg" style="width:100px;height:100px;" alt="Alternate Text" /></div><div class="text-center" style="margin-top:-10px;"><h2 style="color: #492877;;" class="text-center"  {pTitulo} >' + '</h2></div><div class="text-center"><p style="color: #575757;" class="text-center"><h3><strong> {pMensaje} ' + '</strong></h3></p></div>';
var v_Htmlraw_warning   = '<div class="text-center"><img src="../Imagen/warning.jpg" style="width:100px;height:100px;" alt="Alternate Text" /></div><div class="text-center" style="margin-top:-10px;"><h2 style="color: #492877;;" class="text-center">  {pTitulo}' + '</h2></div><div class="text-center"><p style="color: #575757;" class="text-center"><h3><strong>  {pMensaje} ' +'</strong></h3></p></div>';
var v_Htmlraw_Info      = '<div class="text-center" ><img src="../Imagen/info.jpg" style="width:100px;height:100px;" alt="Alternate Text" /></div><div class="text-center" style="margin-top:-10px;"><h2 style="color: #492877;;" class="text-center">  {pTitulo}' + '</h2></div><div class="text-center"><p style="color: #575757;" class="text-center"><h3><strong>  {pMensaje} ' + '</strong></h3></p> </div>';
var v_Htmlraw_Danger    = '<div class="text-center"><img src="../Imagen/danger2.jpg" style="width:100px;height:100px;" alt="Alternate Text" /></div><div class="text-center" style="margin-top:-10px;"><h2 style="color: #492877;;" class="text-center">  {pTitulo}' + '</h2></div><div class="text-center"><p style="color: #575757;" class="text-center"><h3><strong> {pMensaje} ' + '</strong></h3></p></div>';


function fc_MensajeBootBox(pTipo, pTitulo, pMensaje) {
    var v_Htmlraw = '';
    if (pTipo   == "success") {
        v_Htmlraw = v_Htmlraw_Success.replace('{pTitulo}', pTitulo).replace('{pMensaje}', '<pre><div style="font-size:18pt;">' + pMensaje + '</div></pre>');
    }
    if (pTipo   == "warning") {
        v_Htmlraw = v_Htmlraw_warning.replace('{pTitulo}', pTitulo).replace('{pMensaje}', pMensaje);
    }
    if (pTipo   == "info") {
        v_Htmlraw = v_Htmlraw_Info.replace('{pTitulo}', pTitulo).replace('{pMensaje}', pMensaje);
    }
    if (pTipo   == "danger") {
        v_Htmlraw = v_Htmlraw_Danger.replace('{pTitulo}', pTitulo).replace('{pMensaje}', pMensaje);
    }
    bootbox.alert({
        message: v_Htmlraw 
    });
}

function DocumentoNoAutorizadoXML(pTipoDocumentoId) {
    var v_Titulo = '';
    if (pTipoDocumentoId == '01') {
        v_Titulo = 'Factura';
    }
    else if (pTipoDocumentoId == '03') {
        v_Titulo = 'Boleta';
    }
    else if (pTipoDocumentoId == '07') {
        v_Titulo = 'Nota de Crédito';
    }
    else if (pTipoDocumentoId == '08') {
        v_Titulo = 'Nota de Débito';
    }
    else if (pTipoDocumentoId == 'RC') {
        v_Titulo = 'Resumén';
    }
    else if (pTipoDocumentoId == '20') {
        v_Titulo = 'Retención';
    }
    else if (pTipoDocumentoId == '40') {
        v_Titulo = 'Percepción';
    }
    var v_Htmlraw = '<div class="text-center"><img src="../Imagen/warning.jpg" style="width:100px;height:100px;" alt="Alternate Text" /></div><div class="text-center" style="margin-top:-10px;"><h2 style="color: #492877;;" class="text-center">' + v_Titulo + '</h2></div><div class="text-center"><p style="color: #575757;" class="text-center"><h3><strong>El documento debe ser autorizado para poder descargase.</strong></h3></p></div>';
    bootbox.alert({
        message: v_Htmlraw
    });
}

function fc_MensajeBootBoxConfirm(pTipo, pTitulo, pMensaje, pfuncionConfirma, pfuncionCancel) {
    var v_Htmlraw = '';
    if (pTipo   == "success") {
        v_Htmlraw   = v_Htmlraw_Success.replace('{pTitulo}', pTitulo).replace('{pMensaje}', pMensaje);
    }
    if (pTipo   == "warning") {
        v_Htmlraw   = v_Htmlraw_warning.replace('{pTitulo}', pTitulo).replace('{pMensaje}', pMensaje);;
    }
    if (pTipo   == "info") {
        v_Htmlraw   = v_Htmlraw_Info.replace('{pTitulo}', pTitulo).replace('{pMensaje}', pMensaje);;
    }
    if (pTipo   == "danger") {
        v_Htmlraw   = v_Htmlraw_Danger.replace('{pTitulo}', pTitulo).replace('{pMensaje}', pMensaje);;
    }
    bootbox.dialog({
        message : v_Htmlraw,
        buttons : {
            Cancel  : {
                label       : "Cancelar",
                className   : "btn-danger",
                callback    : function () {
                    pfuncionCancel();
                    return "Cancel";
                }
            },
            success : {
                label       : "Confirmar",
                className   : "btn-primary",
                callback    : function () {
                    pfuncionConfirma();
                    return "OK";
                }
            }
        }
    });
}
$(window).bind("resize", function () {
    
}).triggerHandler("resize");

function validateEmail(pEmail) {
    var regex = /^([\w-\.]+@([\w-]+\.)+[\w-]{2,4})?$/;
    var result = pEmail.replace(/\s/g, "").split(/,|;/);
    for (var i = 0; i < result.length; i++) {
        if (!regex.test(result[i])) {
            return false;
        }
    }
    return true;
}
$(window).on('resize.jqGrid', function () {

    $("table").jqGrid("setGridWidth", $("table").closest(".container-fluid").width());
});

function fc_Mensaje_Tiempo_Agotado(pData) {
    if (pData != null) {
        if (pData.indexOf('Tiempo Agotado') != -1) {
            var fc_Redireccionar = function () {
                location.href = '/';
            }
            fc_MensajeBootBoxConfirm('danger', "Tiempo Agotado", 'Tiempo de Espera Agotado, por favor ingrese nuevamente al Sistema', fc_Redireccionar, fc_Redireccionar);

        }
    }
}

function fc_jqgrid_responsive_table() {

    var $jqGridActuales = $("table");
    $jqGridActuales.each(function (i) {

        var v_AnchoJqgrid = $(this).closest(".ui-jqgrid").width();
        var v_Columnas = $(this).jqGrid('getGridParam', 'colModel')
        var v_TotalColumnas = 0;

        if (v_Columnas != undefined) {

            for (var a = 0; a < v_Columnas.length ; a++) {
                if (v_Columnas[a].hidden == false) {
                    v_TotalColumnas += v_Columnas[a].widthOrg;
                }
            }
            var v_NuevoTamanio = $(this).closest(".ui-jqgrid").parent().width();
            $(this).jqGrid("setGridWidth", v_NuevoTamanio, true);

            if (v_AnchoJqgrid < v_TotalColumnas) {
                fc_reDefineColWidth(v_Columnas, $(this)[0].id);
            }
        }

    });

}
function fc_reDefineColWidth(v_Columnas, pIdTabla) {

    for (var j = 0; j < v_Columnas.length ; j++) {

        $('.ui-jqgrid-labels > th:eq(' + j + ')').css('width', v_Columnas[j].widthOrg); // will set the column header widths
        $('#' + pIdTabla + ' tr').find('td:eq(' + j + ')').each(function () { $(this).css('width', v_Columnas[j].widthOrg); }) // will set the column widths
    }
}