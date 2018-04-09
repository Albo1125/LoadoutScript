AddEventHandler('chatMessage', function(source, name, message)
    cm = stringsplit(message, " ")
	if(cm[1] == "/lo") then
		CancelEvent()
		TriggerClientEvent("Loadouts:ShowMenu", source)
    end
end)

print("Loadoutscript by Albo1125 (FiveM)")

function stringsplit(inputstr, sep)
    if sep == nil then
        sep = "%s"
    end
    local t={} ; i=1
    for str in string.gmatch(inputstr, "([^"..sep.."]+)") do
        t[i] = str
        i = i + 1
    end
    return t
end