import subprocess
import re

def scan_system_profiler():
    try:
        print("Running system_profiler...")
        # Reduce output volume
        cmd = ["system_profiler", "SPAirPortDataType"]
        result = subprocess.check_output(cmd).decode("utf-8")
        print("Got output.")
        
        networks = []
        in_other_networks = False
        current_ssid = None
        
        lines = result.splitlines()
        for line in lines:
            stripped = line.strip()
            # print(line)
            
            if "Other Local Wi-Fi Networks:" in line:
                in_other_networks = True
                continue
            
            if not in_other_networks:
                continue
                
            # Indentation matters in system_profiler output
            indent = len(line) - len(line.lstrip())
            
            # SSID lines are usually indented by 8 spaces or so, properties by more.
            # Example:
            #         MyNetwork:
            #           PHY Mode: ...
            
            if line.endswith(":"):
                # Potential SSID
                # Use simple heuristic: Not a property line
                if indent == 12: # Adjust based on actual output
                     current_ssid = stripped[:-1]
                     networks.append({"ssid": current_ssid, "signal": 50}) # Default
            
            if "Signal / Noise:" in line and current_ssid:
                 # Signal / Noise: -65 dBm / -92 dBm
                 # Parse RSSI
                 try:
                     parts = line.split(":")
                     val_part = parts[1].strip()
                     rssi_str = val_part.split()[0]
                     rssi = int(rssi_str)
                     
                     # Map RSSI
                     quality = 2 * (rssi + 100)
                     if quality < 0: quality = 0
                     if quality > 100: quality = 100
                     
                     # Update last network
                     networks[-1]["signal"] = quality
                 except:
                     pass

        return networks

    except Exception as e:
        print(f"Error: {e}")
        return []

if __name__ == "__main__":
    nets = scan_system_profiler()
    print(nets)
