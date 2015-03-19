############################################################################################################################################
#Script that allows to get the site collections in a SPO Tenant using CSOM
# Required Parameters:
#  -> $sCSOMPath: Path for the CSOM assemblies.
#  -> $sUserName: User Name to connect to the SharePoint Online Site Collection.
#  -> $sPassword: Password for the user.
#  -> $sSiteUrl: SharePoint Online Administration Url.
############################################################################################################################################

$host.Runspace.ThreadOptions = "ReuseThread"

#Definition of the function that gets the list of site collections in the tenant using CSOM
function Get-SPOTenantSiteCollections
{
    param ($sCSOMPath,$sSiteUrl,$sUserName,$sPassword)
    try
    {    
        Write-Host "----------------------------------------------------------------------------"  -foregroundcolor Green
        Write-Host "Getting the Tenant Site Collections" -foregroundcolor Green
        Write-Host "----------------------------------------------------------------------------"  -foregroundcolor Green
     
        #Adding the Client OM Assemblies        
        $sCSOMRuntimePath=$sCSOMPath +  "\Microsoft.SharePoint.Client.Runtime.dll"  
        $sCSOMTenantPath=$sCSOMPath +  "\Microsoft.Online.SharePoint.Client.Tenant.dll"
        $sCSOMPath=$sCSOMPath +  "\Microsoft.SharePoint.Client.dll"             
        Add-Type -Path $sCSOMPath         
        Add-Type -Path $sCSOMRuntimePath
        Add-Type -Path $sCSOMTenantPath

        #SPO Client Object Model Context
        $spoCtx = New-Object Microsoft.SharePoint.Client.ClientContext($sSiteUrl) 
        $spoCredentials = New-Object Microsoft.SharePoint.Client.SharePointOnlineCredentials($sUsername, $sPassword)  
        $spoCtx.Credentials = $spoCredentials
        $spoTenant= New-Object Microsoft.Online.SharePoint.TenantAdministration.Tenant($spoCtx)
        $spoTenantSiteCollections=$spoTenant.GetSiteProperties(0,$true)
        $spoCtx.Load($spoTenantSiteCollections)
        $spoCtx.ExecuteQuery()
        
        #We need to iterate through the $spoTenantSiteCollections object to get the information of each individual Site Collection
        foreach($spoSiteCollection in $spoTenantSiteCollections){
            
            Write-Host "Url: " $spoSiteCollection.Url " - Template: " $spoSiteCollection.Template " - Owner: "  $spoSiteCollection.Owner -ForegroundColor White
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

Get-SPOTenantSiteCollections -sCSOMPath $sCSOMPath -sSiteUrl $sSiteUrl -sUserName $sUserName -sPassword $sPassword