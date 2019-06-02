# Obtient les interfaces réseau INTEL
$nics = Get-WmiObject Win32_NetworkAdapter | where {$_.Name.Contains('Intel')}

# Parcourt chaque interface réseau
foreach ($nic in $nics)
{
	# Stocke la paramètre de gestion d'alimentation'
	$powerMgmt = Get-WmiObject MSPower_DeviceEnable -Namespace root\wmi | where {$_.InstanceName -match [regex]::Escape($nic.PNPDeviceID)}
 
   # Vérifie qu'on puisse bien désactiver
   if(Get-Member -inputobject $powerMgmt -name "Enable" -Membertype Properties){

     # Vérifie si c'est activé
     if ($powerMgmt.Enable -eq $True){

		# Désactive la  gestion d'alimentation
		$powerMgmt.Enable = $False
		$powerMgmt.psbase.Put()
     }
   }
}