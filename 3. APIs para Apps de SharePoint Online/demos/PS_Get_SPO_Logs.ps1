############################################################################################################################################
#Script that allows to get the logs for SharePoint Online
# Required Parameters:
#  -> $sUserName: User Name to connect to the SharePoint Online Site Collection.
#  -> $sPassword: Password for the user.
#  -> $sSiteUrl: SharePoint Online Administration Url.
############################################################################################################################################

$host.Runspace.ThreadOptions = "ReuseThread"

#http://www.vrdmn.com/2014/03/view-tenant-uls-logs-in-sharepoint.html
#http://stackoverflow.com/questions/10487011/creating-a-datetime-object-with-a-specific-utc-datetime-in-powershell

#Definition of the function that gets the logs for SharePoint Online
function Get-SPOLogs
{
    param ($sCSOMPath,$sSiteUrl,$sUserName,$sPassword)
    try
    {    
        Write-Host "----------------------------------------------------------------------------"  -foregroundcolor Green
        Write-Host "Getting the logs for SharePoint Online" -foregroundcolor Green
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
        $spoCredentials = New-Object Microsoft.SharePoint.Client.SharePointOnlineCredentials($sUserName, $sPassword)  
        $spoCtx.Credentials = $spoCredentials
        $uTime=Get-Date
        $utcTime=$uTime.ToUniversalTime()
        $spoTenant= New-Object Microsoft.Online.SharePoint.TenantAdministration.Tenant($spoCtx)
        $spoTenantLog=New-Object Microsoft.Online.SharePoint.TenantAdministration.TenantLog($spoCtx)
        $spoLogEntries=$spoTenantLog.GetEntries($utcTime.AddDays(-500),$utcTime,50)
        #$spoLogEntries=$spoTenantLog.GetEntries()
        $spoCtx.Load($spoLogEntries)
        $spoCtx.ExecuteQuery()
        
        #We need to iterate through the $spoGroups Object in order to get individual Group information
        foreach($spoLogEntry in $spoLogEntries){
            Write-Host $spoLogEntry.TimestampUtc " - " $spoLogEntry.Message  " - " $spoLogEntry.CorrelationId " - " $spoLogEntry.Source " - " $spoLogEntry.User " - " $spoLogEntry.CategoryId   
        }
        $spoCtx.Dispose()
    }
    catch [System.Exception]
    {
        write-host -f red $_.Exception.ToString()   
    }    
}

#Required Parameters
$sSiteUrl = "https://nuberosnet-admin.sharepoint.com/" 
$sUserName = "<O365Admin>@<O365Domain>.onmicrosoft.com" 
$sPassword = Read-Host -Prompt "Enter your password: " -AsSecureString
$sCSOMPath="E:\03 Docs\19 MVP Cluster\06 Proyectos\04 Curso O365\04 Demos\01 PS\DLLs"

Get-SPOLogs -sCSOMPath $sCSOMPath -sSiteUrl $sSiteUrl -sUserName $sUserName -sPassword $sPassword