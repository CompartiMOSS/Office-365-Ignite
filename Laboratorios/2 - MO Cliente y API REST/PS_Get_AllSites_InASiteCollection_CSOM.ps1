############################################################################################################################################
# Script that allows to get all the sites defined under a SharePoint Online Site Collection using CSOM
# Required Parameters:
#  -> $sCSOMPath: Path for the CSOM assemblies.
#  -> $sUserName: User Name to connect to the SharePoint Online Site Collection.
#  -> $sPassword: Password for the user.
#  -> $sSiteCollectionUrl: Site Collection Url
############################################################################################################################################

$host.Runspace.ThreadOptions = "ReuseThread"

#Definition of the function that gets all the site collections information in a SharePoint Online tenant
function Get-SPOSitesInSC
{
    param ($sCSOMPath,$sSiteColUrl,$sUserName,$sPassword)
    try
    {    
        Write-Host "----------------------------------------------------------------------------"  -foregroundcolor Green
        Write-Host "Getting all the sites in a SharePoint Online Site Collection" -foregroundcolor Green
        Write-Host "----------------------------------------------------------------------------"  -foregroundcolor Green
     
        #Adding the Client OM Assemblies        
                #Adding the Client OM Assemblies
        $sCSOMRuntimePath=$sCSOMPath +  "\Microsoft.SharePoint.Client.Runtime.dll"          
        $sCSOMPath=$sCSOMPath +  "\Microsoft.SharePoint.Client.dll"             
        Add-Type -Path $sCSOMPath         
        Add-Type -Path $sCSOMRuntimePath       

        #SPO Client Object Model Context
        $spoCtx = New-Object Microsoft.SharePoint.Client.ClientContext($sSiteColUrl) 
        $spoCredentials = New-Object Microsoft.SharePoint.Client.SharePointOnlineCredentials($sUsername, $sPassword)  
        $spoCtx.Credentials = $spoCredentials 

        #Root Web Site
        $spoRootWebSite = $spoCtx.Web
        #Collecction of Sites under the Root Web Site
        $spoSites = $spoRootWebSite.Webs

        #Loading operations        
        $spoCtx.Load($spoRootWebSite)
        $spoCtx.Load($spoSites)
        $spoCtx.ExecuteQuery()

        #We need to iterate through the $spoSites Object in order to get individual sites information
        foreach($spoSite in $spoSites){
            $spoCtx.Load($spoSite)
            $spoCtx.ExecuteQuery()
            Write-Host $spoSite.Title " - " $spoSite.Url -Foregroundcolor White
        }
        $spoCtx.Dispose()
    }
    catch [System.Exception]
    {
        write-host -f red $_.Exception.ToString()   
    }    
}

#Required Parameters
$sSiteColUrl = "https://<O365Domain>.sharepoint.com/sites/<Site>" 
$sUserName = "<O365User>@<O365Domain>.onmicrosoft.com" 
$sPassword = Read-Host -Prompt "Enter your password: " -AsSecureString
$sCSOMPath="E:\03 Docs\19 MVP Cluster\06 Proyectos\04 Curso O365\03 HandOn Labs\02 Lab - MO Cliente"

Get-SPOSitesInSC -sCSOMPath $sCSOMPath -sSiteColUrl $sSiteColUrl -sUserName $sUserName -sPassword $sPassword



