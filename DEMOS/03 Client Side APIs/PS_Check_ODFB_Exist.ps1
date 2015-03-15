#http://blog.blksthl.com/2014/08/08/office-365-guide-series-verify-provisioned-onedrives-using-powershell/
#
# By Thomas Balkeståhl - http://blog.blksthl.com
#
$o365cred = Get-Credential -Username "<O365Admin>@<O365Domain>.onmicrosoft.com" -Message "Supply a Office365 Admin"
#$Userlist = read-host "submit your list of users that have been provisioned"
$Userlist="<User1>@<O365Domain>.onmicrosoft.com","<User2>@<O365Domain>.onmicrosoft.com","<User3>@<O365Domain>.onmicrosoft.com","angegon@nuberosnet.onmicrosoft.com","formacionsp@nuberos.es","jvilloria@nuberosnet.onmicrosoft.com","mdacosta@nuberosnet.onmicrosoft.com"
$Userlist = $Userlist -replace " ", ""
$Emails = $userlist -split ","
#Splitting list into Array
Foreach($Email in $Emails)
{
    # Constructing URL from the UPN/Email address
    $struser = $Email
    $pos= $strUser.IndexOf("@")
    $len = $struser.Length -1
    $strUser = $strUser.SubString(0, $pos)
    $strUser = $strUser -replace "\.", "_"
    $orgpos = $pos + 1
    $orglen = $len - $pos
    $strOrg = $Email.SubString($orgpos, $orglen)
    $strOrgNamePos = $strOrg.IndexOf(".")
    $strOrgName = $strOrg.SubString(0, $strOrgNamePos)
    $strOrgSuffixPos = $strOrgNamePos +1
    $strOrgNameLen = $strOrg.Length - $strOrgSuffixPos
    $strOrgSuffix = $strOrg.SubString($strOrgSuffixPos, $strOrgNameLen)
    $strOrg = $strOrg -replace "\.", "_"
    $PersonalOrgURL = "https://" + $strOrgName + "-my.sharepoint.com/personal/"
    $SiteUrl= $PersonalOrgURL + $strUser
    $SiteUrl= $SiteUrl+ "_" + $strOrg
    write-host "Verifying user:" $Email
$HTTP_Request = [System.Net.WebRequest]::Create($SiteUrl)
$HTTP_Request.UseDefaultCredentials = $true
$HTTP_Request.Credentials = $o365cred
try {
    $HTTP_Response = $HTTP_Request.GetResponse()
}
catch [System.Net.WebException] {
    $HTTP_Response = $_.Exception.Response
}
$HTTP_Status = $HTTP_Response.StatusCode
If ($HTTP_Status -eq 200 -or $HTTP_Status -eq 403 )   { 
    Write-Host -ForegroundColor Green "Site for user $Email exists!" 
}
Else {
    Write-Host -ForegroundColor Yellow "The OneDrive site for user $Email does not respond, try again later or provision it again"
}
$HTTP_Request = $null
$HTTP_Response = $null
$HTTP_Status = $Null
}
