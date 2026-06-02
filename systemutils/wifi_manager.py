import subprocess
import platform
import logging
import socket

logger = logging.getLogger("WifiManager")

class WifiManager:
    def __init__(self):
        self.os_type = platform.system()
    
    def get_ip_address(self):
        """Get the primary local IP address."""
        try:
            # Connect to a public DNS to let the OS figure out the best interface
            s = socket.socket(socket.AF_INET, socket.SOCK_DGRAM)
            s.settimeout(0.1)
            # Doesn't actually connect
            s.connect(("8.8.8.8", 80))
            ip = s.getsockname()[0]
            s.close()
            return ip
        except Exception:
            return "127.0.0.1"

    def scan_networks(self):
        """Scan for available wireless networks."""
        # Check for MacOS
        if self.os_type == "Darwin":
            try:
                # Use airport utility
                airport_path = "/System/Library/PrivateFrameworks/Apple80211.framework/Versions/Current/Resources/airport"
                result = subprocess.check_output([airport_path, "-s"]).decode("utf-8")
                
                networks = []
                # Skip header line
                lines = result.splitlines()
                if len(lines) > 0:
                    lines = lines[1:]
                
                for line in lines:
                    line = line.strip()
                    if not line: continue
                    parts = line.split()
                    rssi = -100
                    ssid = "Unknown"
                    
                    # Find part that looks like a MAC address
                    mac_index = -1
                    for i, part in enumerate(parts):
                        if part.count(':') == 5:
                            mac_index = i
                            break
                    
                    if mac_index > 0:
                        ssid = " ".join(parts[:mac_index])
                        try:
                            rssi = int(parts[mac_index+1])
                        except:
                            rssi = -90
                    
                    # Convert RSSI to percent: -100 to -30 maps to 0 to 100
                    quality = 2 * (rssi + 100)
                    if quality < 0: quality = 0
                    if quality > 100: quality = 100
                    
                    networks.append({"ssid": ssid, "signal": quality})
                
                # Sort
                networks.sort(key=lambda x: x["signal"], reverse=True)
                return networks
            except Exception as e:
                logger.error(f"MacOS Scan failed with airport: {e}")
                
                # Fallbck to system_profiler
                try:
                    logger.info("Attempting system_profiler scan...")
                    res = subprocess.check_output(["system_profiler", "SPAirPortDataType"]).decode("utf-8")
                    networks = []
                    in_other = False
                    current_ssid = None
                    
                    for line in res.splitlines():
                        if "Other Local Wi-Fi Networks:" in line:
                            in_other = True
                            continue
                        if not in_other: continue
                        
                        sline = line.strip()
                        if not sline: continue
                        
                        if line.endswith(":"):                            
                            indent = len(line) - len(line.lstrip())
                            if indent == 12: # Approximate
                                current_ssid = sline[:-1]
                                networks.append({"ssid": current_ssid, "signal": 50})
                        
                        if "Signal / Noise:" in line and current_ssid:
                            try:
                                parts = sline.split(":")[1].split("/")
                                rssi = int(parts[0].replace("dBm","").strip())
                                quality = 2 * (rssi + 100)
                                if quality < 0: quality = 0
                                if quality > 100: quality = 100
                                networks[-1]["signal"] = quality
                            except:
                                pass
                                
                    if networks:
                        networks.sort(key=lambda x: x["signal"], reverse=True)
                        return networks
                        
                except Exception as ex:
                    logger.error(f"system_profiler scan failed: {ex}")
                
        if self.os_type != "Linux":
            return [
                {"ssid": "PortaPrint_Test_5G", "signal": 95},
                {"ssid": "Guest_Network", "signal": 70},
                {"ssid": "Weak_Signal_Net", "signal": 30}
            ]
        
        try:
            # Rescan first to ensure list is fresh
            try:
                 subprocess.run(["nmcli", "dev", "wifi", "rescan"], stderr=subprocess.DEVNULL, timeout=5)
            except Exception:
                 pass

            # nmcli -t -f SSID,SIGNAL dev wifi list
            result = subprocess.check_output(
                ["nmcli", "-t", "-f", "SSID,SIGNAL", "dev", "wifi", "list"],
                stderr=subprocess.STDOUT
            ).decode("utf-8")
            
            networks = []
            seen_ssids = set()
            
            for line in result.splitlines():
                if not line.strip(): continue
                parts = line.split(":")
                if len(parts) >= 2:
                    ssid = parts[0].strip()
                    try:
                        signal = int(parts[1].strip())
                    except ValueError:
                        signal = 0
                    
                    if ssid and ssid not in seen_ssids:
                        seen_ssids.add(ssid)
                        networks.append({"ssid": ssid, "signal": signal})
            
            # Sort by signal strength
            networks.sort(key=lambda x: x["signal"], reverse=True)
            return networks
            
        except Exception as e:
            logger.error(f"Wifi Scan failed: {e}")
            return []

    def connect_network(self, ssid, password):
        """Connect to a wifi network."""
        logger.info(f"Attempting to connect to {ssid}")
        
        if self.os_type != "Linux":
            import time
            time.sleep(2)
            return True

        try:
            # nmcli dev wifi connect <ssid> password <password>
            cmd = ["nmcli", "dev", "wifi", "connect", ssid, "password", password]
            subprocess.check_call(cmd)
            return True
        except subprocess.CalledProcessError as e:
            logger.error(f"Failed to connect to {ssid}: {e}")
            return False
