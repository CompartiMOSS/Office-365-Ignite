'use strict';

//Variables necesarias
     var hostUrl 
     var context
     var hostcontext
     var web
     var user
     var message = ""
     var lists

 //Llamamos a la función sharePointReady cuando el DOM está listo
 $(document).ready(function () {
     SP.SOD.executeFunc('sp.js', 'SP.ClientContext', sharePointReady);
     $("#getListCount").click(function (event) {
        getWebProperties();
        event.preventDefault();
    });

    $("#createlistbutton").click(function (event) {
        createlist();
        event.preventDefault();
    });

    $("#deletelistbutton").click(function (event) {
        deletelist();
        event.preventDefault();
    });

 });

// Esta función crea el contexto de SharePoint necesario para trabajar con el CSOM
 function sharePointReady() {
     var hostUrl = decodeURIComponent(getQueryStringParameter("SPHostUrl"));
     context = new SP.ClientContext.get_current();
     hostcontext = new SP.AppContextSite(context, hostUrl);
     web = hostcontext.get_web();
     loadWebTitle();
     loadUserName();
     displayLists();
 }

//Función que carga la información del Sitio 
function loadWebTitle()
{
    context.load(web, "Title");
    context.executeQueryAsync(onGetWebSuccess, onGetWebFail);
}

//Éxito en loadWebTitle
function onGetWebSuccess() {
    message = message + "El título del sitio host es: " + web.get_title() + ". ";
    updateMessage();
}

//Error en loadWebTitle
function onGetWebFail(sender, args) {
    alert('Error al obtener el sitio. Error:' + args.get_message());
}

 // Función que carga la información del usuario
function loadUserName() {
     user = web.get_currentUser();
     context.load(user);
     context.executeQueryAsync(onGetUserNameSuccess, onGetUserNameFail);
}

//Éxito en la carga de la información del usuario
function onGetUserNameSuccess() {
    message = message + "Hola " + user.get_title() + ". ";
    updateMessage();
}

 // Error en la carga del usuario
function onGetUserNameFail(sender, args) {
    alert('Error al obtener el nombre del usuario. Error:' + args.get_message());
}

//Actualización del div message
function updateMessage()
{
    $('#message').text(message);
}

//Función para obtener parámetros de la Url
function getQueryStringParameter(param) {
     var params = document.URL.split("?")[1].split("&");
     //var strParams = "";     
     for (var i = 0; i < params.length; i = i + 1) {
         var singleParam = params[i].split("=");
         if (singleParam[0] == param) {
             return singleParam[1];
         }
     }
}
 
 //Función que obtiene las listas del sitio actual (cuenta)   
function getWebProperties() {
    context.load(lists);
    context.executeQueryAsync(onWebPropsSuccess, onWebPropsFail);
}

//Éxito en el acceso a las listas del sitio
function onWebPropsSuccess(sender, args) {
    alert('Número de listas en el sitio host: ' + lists.get_count());
}

//Error en el acceso a las listas del sitio
function onWebPropsFail(sender, args) {
    alert('Error al obtener las listas. Error: ' + args.get_message());
}

//Muestra las listas del sitio host en el control de tipo ListBox
function displayLists() { 
    lists = web.get_lists();
    context.load(lists);
    context.executeQueryAsync(onGetListsSuccess, onGetListsFail);
}

//Éxito en la operación de obtener las listas
function onGetListsSuccess(sender, args) {
    var listEnumerator = lists.getEnumerator();
    var selectListBox = document.getElementById("selectlistbox");
	//Primero se limpia el combo
    if (selectListBox.hasChildNodes()) {
        while (selectListBox.childNodes.length >= 1) {
            selectListBox.removeChild(selectListBox.firstChild);
        }
    }
    //Carga de las listas en el combo
    while (listEnumerator.moveNext()) {
        var selectOption = document.createElement("option");
        selectOption.value = listEnumerator.get_current().get_title();
        selectOption.innerHTML = listEnumerator.get_current().get_title();
        selectListBox.appendChild(selectOption);
    }
}

//Error en la operación de obtener las listas
function onGetListsFail(sender, args) {
    alert('Error en la operación con listas. Error: ' + args.get_message());
}

//Función para crear una lista
function createlist() {
    var listCreationInfo = new SP.ListCreationInformation();
    var listTitle = document.getElementById("createlistbox").value;
    listCreationInfo.set_title(listTitle);
    listCreationInfo.set_templateType(SP.ListTemplateType.genericList);
    lists = web.get_lists();
    var newList = lists.add(listCreationInfo);
    context.load(newList);
    context.executeQueryAsync(onListCreationSuccess, onListCreationFail);
}

//Éxito en la creación de la lista
function onListCreationSuccess() {
	alert('Lista creada con éxtio');
	$('#createlistbox').val("");
    displayLists();
}

//Error en la creación de la lista
function onListCreationFail(sender, args) {
    alert('Error al obtener las listas. ' + args.get_message());
}

//Función para borrar una lista
function deletelist() {    
    var selectListBox = document.getElementById("selectlistbox");
    var selectedListTitle = selectListBox.value;
    var selectedList = web.get_lists().getByTitle(selectedListTitle);
    selectedList.deleteObject();
    context.executeQueryAsync(onDeleteListSuccess, onDeleteListFail);
}

//Éxito en el borrado de la lista
function onDeleteListSuccess() {
	alert('Lista borrada con éxito');
    displayLists();
}

//Error en el borrado de la lista
function onDeleteListFail(sender, args) {
    alert('Error al borrar la lista. ' + args.get_message());
}