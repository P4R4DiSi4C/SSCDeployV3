# Obtient les hub usb
$hubs = Get-WmiObject Win32_USBHub

# Obtient les controlleurs usb
$controllers = Get-WmiObject Win32_USBControllerDevice

# Coche gestion d'alimentation
$powerMgmt = Get-WmiObject MSPower_DeviceEnable -Namespace root\wmi


foreach ($p in $powerMgmt)
{
	$IN = $p.InstanceName.ToUpper()
	
    foreach ($h in $hubs)
	{
		$PNPDI = $h.PNPDeviceID
        if ($IN -like "*$PNPDI*")
        {
            $p.enable = $False
            $p.psbase.put()
        }
	}

    foreach ($h in $controllers)
	{
		$PNPDI = $h.PNPDeviceID
        if ($IN -like "*$PNPDI*")
        {
            $p.enable = $False
            $p.psbase.put()
        }
	}
}