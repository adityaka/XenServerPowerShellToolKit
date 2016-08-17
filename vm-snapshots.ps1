<#
 
 .AUTHOR : Aditya Nath Kapur 
 .SUMMARY 
 This script enumerates the snapshots for a vm and reverts it 

 Usefull commands for future 
 Get All VM Names  which have snapshots 
 get-xenvm | where-object -FilterScript { $_.snapshots.length -gt 0} | select-object -Property Name_Label 
.DATE : 17-08-2016
#>

## GLOBALS ##



## - Script parameters ## 
[CmdletBinding()]
    param(
        [Parameter(Mandatory=$True)]
        [string]$XenserverAddress,
        [Parameter(Mandatory=$True)]
        [String]$XenserverUserName,
        [Parameter(Mandatory=$False)]
        [String]$XenserverPassword=$null ,
        [Parameter(Mandatory=$True)]
        [String]$VmName
        
    )

## END GLOBALS ##

## initialization 
$XenserverModules = get-module Xenserver* 

if ($XenserverModules.Length -le 0){
    #Xenserver module is not loaded let's get the sucker 
    try{
        import-module -Name XenServerPSModule -ErrorAction Stop 
    }catch{
        Write-Error -Message "Can't find XenServerPSModule please ensure the module is in the path and we are running a valid powershell version"
    }
}

## End initialization 

## functions 


function Get-VMSnapShot{
    param(
        [String]$VMName ,
        [String]$Filter=$null, #if someone is using By Ref they should have the string format as OpaqueRef:<uuid> (convert ToString)
        [switch]$BySnapShotName=$False
    )

    $VMSnapShots = $null 
    try{
    $VM = Get-XenVM -Name $VMName -ErrorAction Stop  
    Write-Debug -Message ("Snapshots for this VM " + $VM.Snapshots.Length) 
    
     if( -not $Filter  ){
         $VMSnapShots = $VM.Snapshots | %{(Get-XenVM -Ref $_.opaque_ref).Name_Label}
         return $VMSnapShots

     }

     if($BySnapShotName){
        $VMSnapshots = $VM.snapshots | %{ Get-XenVm -Ref $_.opaque_ref | where-object -FilterScript { $_.name_label -eq $Filter} }
        if ($VMSnapShots.Length -le 0){
            Write-Debug -Message "Can't find snapshots for the VM"
        }
        return $VMSnapShots
     }
     else{
         $VMSnapshots = $VM.snapshots | where-object -FilterScript {$_.opaque_ref.ToString() -eq $Filter }  
        if ($VMSnapShots.Length -le 0){
                Write-Debug -Message "Can't find snapshots for the VM using opaque reference"
        }
         return $VMSnapShots
     }

     #bad procedural aproach but we will never reach here if we have find the bug and fix it 
      

    }
    catch {
        Write-Error -Message "Can't Find VM Snapshots"  
        write-error -Message $_.Exception 
    }


}

#TODO: Write more stuff to manipulate snapshots and find VM snapshot references 
#TODO: find a way to revert the snapshots to the VM 

function ScriptEntry{

    if($XenserverPassword -eq $null){
        ## TODO : Security Note SecureString didn't work somehow research more and make use of it 
        $XenserverPassword = Read-host -Prompt "Enter Xenserver Password" 
    }

    #Set the global connection and disconnect once the script is done 
    try{
    
    connect-xenserver -Server $XenserverAddress -UserName $XenserverUserName -Password $XenserverPassword -SetDefaultSession  
    $VMSnapshots = Get-VMSnapShot -VMName $VmName 
    $VMSnapshots | %{ Write-Host $_}
   }
   catch{
       write-host "can't workout stuff "
       write-debug -Message $_.Exception

   }
    
    #Error checks  command line arguments 


}


##End functions

## Curtain Raiser 
ScriptEntry 
## Cleanup Section ## 
Get-XenSession -Server $XenserverAddress | Disconnect-XenServer  
