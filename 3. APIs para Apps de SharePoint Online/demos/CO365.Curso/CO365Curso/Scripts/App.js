'use strict';
var mSPAppWebUrl = decodeURIComponent(getUrlParametros()["SPAppWebUrl"]);
alert(mSPAppWebUrl);
var mSPHostUrl = decodeURIComponent(getUrlParametros()["SPHostUrl"]);
alert(mSPHostUrl);
var scriptbase = mSPHostUrl + "/_layouts/15/";

$(document).ready(function () {
    $.getScript(scriptbase + "SP.RequestExecutor.js", ObtenerDatosListaContactosCrossDomain_2);
});
function ObtenerDatosListaContactosCrossDomain() {
    var executor = new SP.RequestExecutor(mSPAppWebUrl);
    executor.executeAsync(
        {
            url:
               mSPAppWebUrl +
               "/_api/SP.AppContextSite(@target)/web/lists/getbytitle('Contactos Curso Office 365')/items?$select=Title,FirstName&$filter=Title eq 'Imaz'&@target='" +
               mSPHostUrl + "'",
            method: "GET",
            headers: { "Accept": "application/json; odata=verbose" },
            success: ObtenerDatosExitoso,
            error: ObtenerDatosError
        }
    );
}

function ObtenerDatosListaContactosCrossDomain_2() {
    var executor = new SP.RequestExecutor(mSPAppWebUrl);
    executor.executeAsync(
        {
            url:
               mSPAppWebUrl +
               "/_api/SP.AppContextSite(@target)/web/lists/getbytitle('Contactos Curso Office 365')/items?$select=Title,FirstName&@target='" +
               mSPHostUrl + "'",
            method: "GET",
            headers: { "Accept": "application/json; odata=verbose" },
            success: ObtenerDatosExitoso_2,
            error: ObtenerDatosError
        }
    );
}

function ObtenerDatosExitoso(pElementos) {
    var jsonObject = JSON.parse(pElementos.body);

    $.each(jsonObject.d.results, function (value, key) {
        $('#message').text('Apellido: ' + key.Title + ' Nombre: ' + key.FirstName);
    });
}

function ObtenerDatosExitoso_2(pElementos) {
    var jsonObject = JSON.parse(pElementos.body);
	$('#message').empty();
    $.each(jsonObject.d.results, function (value, key) {
        $('#message').append( '<p>Apellido: ' + key.Title + ' Nombre: ' + key.FirstName);
    });
}
function ObtenerDatosError(data, errorCode, errorMessage) {
    alert('Se produjo un error al cargar los datos: ' + errorMessage);
}
function getUrlParametros() {
    var lParametros = [], hash;
    var hashes = window.location.href.slice(window.location.href.indexOf('?') + 1).split('&');
    for (var i = 0; i < hashes.length; i++) {
        hash = hashes[i].split('=');
        lParametros.push(hash[0]);
        lParametros[hash[0]] = hash[1];
    }
    return lParametros;
}