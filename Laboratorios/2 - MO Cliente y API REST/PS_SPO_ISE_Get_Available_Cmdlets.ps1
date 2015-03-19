$sUserName = "<O365User>@<O365Domain>.onmicrosoft.com" 
$sMessage="Introduce your SPO Credentials"
$sSPOAdminCenterUrl="https://<O365Domain>-admin.sharepoint.com/" 
$msolcred = get-credential -UserName $sUserName -Message $sMessage
Connect-SPOService -Url $sSPOAdminCenterUrl -Credential $msolcred 
$spoCmdlets=Get-Command | where {$_.ModuleName -eq “Microsoft.Online.SharePoint.PowerShell"}
Write-Host "There are " $spoCmdlets.Count " Cmdlets in SharePoint Online"
$spoCmdlets  
