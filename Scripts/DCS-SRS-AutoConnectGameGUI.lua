-- Version 1.5.5.0
-- ONLY COPY THIS FILE IS YOU ARE GOING TO HOST A SERVER!
-- The file must be in Saved Games\DCS\Scripts\Hooks or Saved Games\DCS.openalpha\Scripts\Hooks
-- Make sure you enter the correct address into SERVER_SRS_HOST below.
-- You can add an optional Port. e.g. "127.0.0.1:5002"

-- User options --
local SRSAuto = {}
SRSAuto.SERVER_SRS_HOST = "127.0.0.1"
SRSAuto.SERVER_SEND_AUTO_CONNECT = true -- set to false to disable auto connect or just remove this file

-- DO NOT EDIT BELOW HERE --
SRSAuto.unicast = true

-- Utils --
local HOST_PLAYER_ID = 1

SRSAuto.MESSAGE_PREFIX = "SRS Running @ " -- DO NOT MODIFY!!!

package.path  = package.path..";.\\LuaSocket\\?.lua;"
package.cpath = package.cpath..";.\\LuaSocket\\?.dll;"

local socket = require("socket")

SRSAuto.UDPSendSocket = socket.udp()
SRSAuto.UDPSendSocket:settimeout(0)

SRSAuto.logFile = io.open(lfs.writedir()..[[Logs\DCS-SRS-AutoConnect.log]], "w")

function SRSAuto.log(str)
    if SRSAuto.logFile then
        SRSAuto.logFile:write(str.."\n")
        SRSAuto.logFile:flush()
    end
end

-- Register callbacks --

SRSAuto.onPlayerConnect = function(id)
	if not DCS.isServer() then
        return
    end
	if SRSAuto.SERVER_SEND_AUTO_CONNECT and id ~= HOST_PLAYER_ID then
        SRSAuto.log(string.format("Sending auto connect message to player %d on connect ", id))
		net.send_chat_to(string.format(SRSAuto.MESSAGE_PREFIX .. "%s", SRSAuto.SERVER_SRS_HOST), id)
	end
end

SRSAuto.onPlayerChangeSlot = function(id)
    if not DCS.isServer() then
        return
    end
    if SRSAuto.SERVER_SEND_AUTO_CONNECT and id ~= HOST_PLAYER_ID then
        SRSAuto.log(string.format("Sending auto connect message to player %d on switch ", id))
        net.send_chat_to(string.format(SRSAuto.MESSAGE_PREFIX .. "%s", SRSAuto.SERVER_SRS_HOST), id)
   end
end

DCS.setUserCallbacks(SRSAuto)
net.log("Loaded - DCS-SRS-AutoConnect1.5.5.0")
