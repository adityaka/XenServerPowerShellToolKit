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
        [String]$VmName,
        [Parameter(Mandatory=$True)]
        [String]$SnapShotName
        
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

## Simplifying the logic instead of too many filtering options 
## the only option to filter is the snapshot name 
## this will only work for a linear snapshot tree
## A recursive call can be made for snapshot tree 
## the tree enumeration should be research in a different commandlet 
function Get-VMSnapShots{
    param(
        [Parameter(Mandatory=$True)]
        [String]$VMName,
        [Parameter(Mandatory=$False)]
        [String]$SnapshotName = $null 
    )

    $VMSnapShots = $null 
    try{
       $VM = Get-XenVM -Name $VMName -ErrorAction Stop  
        Write-Debug -Message ("Snapshots for this VM " + $VM.Snapshots.Length) 
    
        $VMSnapShots = $VM.Snapshots | %{ Get-XenVM -Ref $_.opaque_ref}
        if ($SnapshotName){
            $VMSnapShots = $VMSnapShots | Where-Object -FilterScript { $_.Name_Label -eq $SnapShotName}
        }
        return $VMSnapShots

     #bad procedural aproach but we will never reach here if we have find the bug and fix it 
      

    }
    catch {
        Write-Error -Message "Can't Find VM Snapshots Last Exception message : "  
        write-error -Message $_.Exception 
    }


}


function Restore-Snapshot {
    param(
        [Parameter(Mandatory=$True)]
        [string]$VMName,
        [Parameter(Mandatory=$True)]
        [string]$SnapShotName 
    )

    $SnapShot = Get-VMSnapShots -VMName $VMName -SnapshotName $SnapShotName
    $VM = Get-XenVM -Name $VMName   
    #Revert Snapshot 
    Invoke-XenVm -VM $VM -XenAction Revert -SnapShot $SnapShot

    #check if the VM is off then turn it on 

     

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
    Restore-Snapshot -VMName $VmName -SnapShotName $SnapShotName 
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
