'use strict'; 
    var hostweburl;
    var appweburl;
    // Cargamos las librerías JavaScript necesarias
    $(document).ready(function () {
        //Obtenemos las URIs necesarias
        hostweburl = decodeURIComponent(getQueryStringParameter("SPHostUrl"));
		alert(hostweburl);
        appweburl = decodeURIComponent(getQueryStringParameter("SPAppWebUrl"));
		alert(appweburl);

        //Asignación de eventos
        $("#btnCreateList").click(function (event) {
            createList();
            event.preventDefault();
        });
		$("#btnDeleteList").click(function (event) {
			DeleteList();
            event.preventDefault();
        });
		$("#btnGetLists").click(function (event) {
			getLists();
            event.preventDefault();
        });
        // Los recursos se encuentran bajo el directorio Layouts
        // web_url/_layouts/15/resource
        var scriptbase = hostweburl + "/_layouts/15/";
		//Carga de la librería SP.RequestExecutor.js para temas de peticiones cross-domain
        $.getScript(scriptbase + "SP.RequestExecutor.js");
    }); 

	//
    // Funciones de Utilidad
	//
    // Devolver un valor del Query String
    function getQueryStringParameter(paramToRetrieve) {
        var params = document.URL.split("?")[1].split("&");
        for (var i = 0; i < params.length; i = i + 1) {
            var singleParam = params[i].split("=");
            if (singleParam[0] == paramToRetrieve) return singleParam[1];
        }
    } 

    // Función para crear la Lista mediante REST
    function createList() {
        var listName = document.getElementById("iListName").value;
        var executor;
        // Inicializamos el RequestExecutor con la URL de la App
        executor = new SP.RequestExecutor(appweburl);
		//Petición REST -> Crear una lista
        executor.executeAsync({
            url: appweburl + "/_api/SP.AppContextSite(@target)/web/Lists?@target='" + hostweburl + "'",
            method: "POST",
            body: "{ '__metadata': { 'type': 'SP.List' }, 'BaseTemplate': 100,'Description': '" + listName + "', 'Title':'" + listName + "'}",
            headers: {
                "content-type": "application/json; odata=verbose"
            },
            success: createListSuccessHandler,
            error: createListErrorHandler
        });
    }

    //Función manejadora si todo OK
    function createListSuccessHandler(data) {
        alert("Lista creada con éxito")
    }

    // Función manejadora si no OK
    function createListErrorHandler(data, errorCode, errorMessage) {
        alert("No ha sido posible crear la Lista: " + errorMessage);
    }
	
	// Función para borrar la Lista mediante REST
    function DeleteList() {
        var listName = document.getElementById("iListName").value;
        var executor;
        // Inicializamos el RequestExecutor con la URL de la App
        executor = new SP.RequestExecutor(appweburl);
		//Petición REST -> Crear una lista
        executor.executeAsync({
            url: appweburl + "/_api/SP.AppContextSite(@target)/web/lists/getbytitle('" + listName + "')?@target='" + hostweburl + "'",
            method: "POST",            
            headers: {
                "IF-MATCH": "*",
    			"X-HTTP-Method": "DELETE"
            },
            success: deleteListSuccessHandler,
            error: deleteListErrorHandler
        });
    }
/*getbytitle('CompartiMOSS')
executor.executeAsync({
  url: "<app web url>/_api/SP.AppContextSite(@target)/web
    /lists(guid'51925dd7-2108-481a-b1ef-4bfa4e69d48b')
    ?@target='<host web url>'",
  method: "POST",
  headers: { 
    "IF-MATCH”: "*",
    "X-HTTP-Method": "DELETE"
  },
  success: successHandler,
  error: errorHandler
});
*/
    //Función manejadora si todo OK
    function deleteListSuccessHandler(data) {
        alert("Lista borrada con éxito")
    }

    // Función manejadora si no OK
    function deleteListErrorHandler(data, errorCode, errorMessage) {
        alert("No ha sido posible borrar la Lista: " + errorMessage);
    }
	
	//Función que devuelve todas las lisas del Sitio Host
	function getLists() {
		var executor;
		// Initializamos el RequestExecutor
		executor = new SP.RequestExecutor(appweburl);
		executor.executeAsync({
			url: appweburl + "/_api/SP.AppContextSite(@target)/web/Lists?@target='" + hostweburl + "'",
			method: "GET",
			headers: {
				"Accept": "application/json; odata=verbose"
					 },
			success: getListsSuccessHandler,
			error: getListsErrorHandler
		});
	}

	//Si todo ha ido OK
	function getListsSuccessHandler(data) {
		var jsonObject = JSON.parse(data.body);
		var selectLists = document.getElementById("sSiteLists");
		//Vaciamos el ListBox
		if (selectLists.hasChildNodes()) {
			while (selectLists.childNodes.length >= 1) {
			selectLists.removeChild(selectLists.firstChild);
			}
		}
		//Llenamos el ListBox
		var results = jsonObject.d.results;
		for (var i = 0; i < results.length; i++) {
			var selectOption = document.createElement("option");
			selectOption.value = results[i].Title;
			selectOption.innerText = results[i].Title;
			selectLists.appendChild(selectOption);
		}
	}
	
	// Función manejadora si no OK
    function getListsErrorHandler(data, errorCode, errorMessage) {
        alert("No se han podido obtener las listas del Sitio " + errorMessage);
    }