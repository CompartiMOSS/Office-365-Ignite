'use strict';

//Variables necesarias
var context = SP.ClientContext.get_current();
var request = new SP.WebRequestInfo();

$(document).ready(function () {
    request.set_url(
        "http://services.odata.org/Northwind/Northwind.svc/Categories"
        );
    request.set_method("GET");
    // Respuesta en formato JSON
    request.set_headers({ "Accept": "application/json;odata=verbose" });
    var response = SP.WebProxy.invoke(context, request);
    document.getElementById("categories").innerHTML =
                "<P>Cargando categor&iacute;as...</P>";
    context.executeQueryAsync(successHandler, errorHandler);
    function successHandler() {
        // Comprobamos que el status code es 200
        if (response.get_statusCode() == 200) {
            var categories;
            var output;
            // Carga de la fuente OData a partir de la respuesta
            categories = JSON.parse(response.get_body());
            // Procesado de los resultados
            output = "<ul>";
            for (var i = 0; i < categories.d.results.length; i++) {
                var categoryName;
                var description;
                categoryName = categories.d.results[i].CategoryName;
                description = categories.d.results[i].Description;
                output += "<li>" + categoryName + ":&nbsp;" +
                    description + "</li>";
            }
            output += "</ul>";
            document.getElementById("categories").innerHTML = output;
        }
        else {
            var errordesc;
            errordesc = "<P>Código de estado: " +
                response.get_statusCode() + "<br/>";
            errordesc += response.get_body();
            document.getElementById("categories").innerHTML = errordesc;
        }
    }
    function errorHandler() {
        document.getElementById("categories").innerHTML =
            response.get_body();
    }	
});