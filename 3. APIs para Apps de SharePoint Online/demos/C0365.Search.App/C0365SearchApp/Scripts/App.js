var results;
 
var context = SP.ClientContext.get_current();
var user = context.get_web().get_currentUser();
 
// This code runs when the DOM is ready and creates a context object which is needed to use the SharePoint object model
$(document).ready(function () { 
    $("#searchButton").click(function () {
        var keywordQuery = new Microsoft.SharePoint.Client.Search.Query.KeywordQuery(context);
        keywordQuery.set_queryText($("#searchTextBox").val()); 
        var searchExecutor = new Microsoft.SharePoint.Client.Search.Query.SearchExecutor(context);
        results = searchExecutor.executeQuery(keywordQuery); 
        context.executeQueryAsync(onQuerySuccess, onQueryFail)
    });
});
 
 //Éxito en la ejecución
function onQuerySuccess() {
	$("#SearchResults").empty();
	$("#SearchResults").append('<table>');
	$("#SearchResults").append('<tr>');
   	$("#SearchResults").append('<td><b>Título</b></td>');
   	$("#SearchResults").append('<td><b>Autor</b></td>');
	$("#SearchResults").append('<td><b>Fecha</b></td>');
   	$("#SearchResults").append('<td><b>Ruta</b></td>');
   	$("#SearchResults").append('</tr>');
   	$.each(results.m_value.ResultTables[0].ResultRows, function () {
       	$("#SearchResults").append('<tr>');
       	$("#SearchResults").append('<td>' + this.Title + '</td>');
       	$("#SearchResults").append('<td>' + this.Author + '</td>');
       	$("#SearchResults").append('<td>' + this.Write + '</td>');
       	$("#SearchResults").append('<td>' + this.Path + '</td>');
       	$("#SearchResults").append('</tr>');
   		});
   	$("#SearchResults").append('</table>');	 
}
 
 //Error en la ejecución
function onQueryFail(sender, args) {
    alert('Query failed. Error:' + args.get_message());
}