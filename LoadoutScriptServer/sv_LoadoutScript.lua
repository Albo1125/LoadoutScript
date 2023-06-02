function lofunc(source, args, rawCommand)
	TriggerClientEvent("Loadouts:ShowMenu", source)
end

RegisterCommand('lo', lofunc, false)

function fmsuniformsfunc(source, args, rawCommand)
	TriggerClientEvent("Loadouts:FMSUniforms", source)
end

RegisterCommand('fmsuniforms', fmsuniformsfunc, false)