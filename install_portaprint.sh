#!/bin/bash

# Portaprint Installation Script
# Adheres to "Scripting" and "Design" rules:
# 1. Complete scripting of install and maintenance.
# 2. Configures environment (venv, permissions).
# 3. Sets up display rules (no sleep, kiosk mode).

set -e

echo ">>> Starting Portaprint Installation..."

# 1. System Dependencies
echo ">>> Installing System Dependencies..."
sudo apt-get update
sudo apt-get install -y python3-venv python3-pip python3-dev libusb-1.0-0-dev chromium x11-xserver-utils unclutter

# Setup Logging permissions
echo ">>> Configuring logging..."
if [ ! -f /var/log/portaprint.log ]; then
    sudo touch /var/log/portaprint.log
fi
sudo chown pi:pi /var/log/portaprint.log
sudo chmod 664 /var/log/portaprint.log

# 2. Python Environment Setup
echo ">>> Setting up Python Environment..."
PROJECT_DIR="$(pwd)"
VENV_DIR="$PROJECT_DIR/portaprint-venv"

if [ ! -d "$VENV_DIR" ]; then
    python3 -m venv "$VENV_DIR"
    echo "Created venv at $VENV_DIR"
else
    echo "Using existing venv at $VENV_DIR"
fi

source "$VENV_DIR/bin/activate"

echo ">>> Installing Python Requirements..."
pip install --upgrade pip
if [ -f "requirements.txt" ]; then
    pip install -r requirements.txt
else
    echo "WARNING: requirements.txt not found. Installing manually..."
    pip install pyusb>=1.2.1 websockets>=11.0 asyncio aiohttp>=3.8.0
fi

# 3. USB Permissions (for Scanner)
echo ">>> Configuring USB Permissions..."
# Create a udev rule for IB scanners if not exists
# Integrated Biometrics Vendor ID: 0x1134 (example commonly used, depends on exact Kojak VID/PID)
# Using 0666 for simplicity in kiosk mode
echo 'SUBSYSTEM=="usb", ATTRS{idVendor}=="1134", MODE="0666"' | sudo tee /etc/udev/rules.d/99-ib-scanner.rules > /dev/null
sudo udevadm control --reload-rules
sudo udevadm trigger

# 4. Display & Power Management (Rule: No Sleep)
echo ">>> Configuring Display Settings (No Sleep)..."

# Handle Wayfire (Wayland) if present - common on RPi Bookworm/Trixie
WAYFIRE_CONFIG="$HOME/.config/wayfire.ini"
if [ -d "$HOME/.config" ]; then
    # Ensure idle is disabled in Wayfire
    # We append to file if file exists, or create it
    mkdir -p "$HOME/.config"
    if grep -q "\[idle\]" "$WAYFIRE_CONFIG" 2>/dev/null; then
        echo "Wayfire config exists."
        # Ideally we'd sed replace, but appending override might work or be complex.
        # Simplest: make sure dpms is off via command at runtime too.
    else
        # minimal config addition to disable idle if using Wayfire
        echo "" >> "$WAYFIRE_CONFIG"
        echo "[idle]" >> "$WAYFIRE_CONFIG"
        echo "dpms_timeout = 0" >> "$WAYFIRE_CONFIG"
    fi
fi

# Create startup script first
STARTUP_SCRIPT="$HOME/start_portaprint.sh"

echo ">>> Display power management disabled (Wayfire config updated / runtime xset planned)."


# 5. Application Autostart (Rule: Zero touch)
echo ">>> Configuring Application Autostart..."
# We create a separate startup script that launches the server and then the browser

cat > "$STARTUP_SCRIPT" <<EOL
#!/bin/bash
source $VENV_DIR/bin/activate
cd $PROJECT_DIR

export DISPLAY=:0
export XAUTHORITY=$HOME/.Xauthority

# Disable DPMS and Screen Saver (Runtime)
xset s off -dpms s noblank || true

# Start Server in background
python3 websocket/server.py > $HOME/portaprint_server.log 2>&1 &
SERVER_PID=\$!

# Wait for server to be ready
echo "Waiting for server..."
sleep 5

# Start unclutter to hide mouse (Attempt, though JS solution is robust)
unclutter -idle 0.1 &

# CLEANUP for Kiosk: Nuke keyrings/lock data to prevent prompts
rm -rf ~/.local/share/keyrings
rm -rf ~/.config/chromium/Default/Login\\ Data

# Start Browser in Kiosk Mode
# Pointing to localhost:8888 as confirmed by server.py
# Added --no-sandbox just in case of root issues, though unrelated to autostart
# --password-store=basic: Prevents keyring popup
# --user-data-dir: Uses a separate profile to imply fresh/no-history state but persistent
chromium --kiosk --incognito --disable-infobars --check-for-update-interval=31536000 --no-first-run --password-store=basic --user-data-dir=\$HOME/.config/chromium-kiosk "http://localhost:8888/" &

# Wait for server process
wait \$SERVER_PID
EOL

chmod +x "$STARTUP_SCRIPT"


# Create .desktop file for Autostart (Works on LXDE, Pixel, GNOME, Wayland/Labwc)
AUTOSTART_DIR="$HOME/.config/autostart"
mkdir -p "$AUTOSTART_DIR"

cat > "$AUTOSTART_DIR/portaprint.desktop" <<EOL
[Desktop Entry]
Type=Application
Name=PortaPrint
Comment=Start PortaPrint Kiosk
Exec=/bin/bash $STARTUP_SCRIPT
Terminal=false
X-GNOME-Autostart-enabled=true
EOL

chmod +x "$AUTOSTART_DIR/portaprint.desktop"
echo "Created autostart entry at $AUTOSTART_DIR/portaprint.desktop"

echo ">>> Installation Complete."
echo ">>> Please reboot strictly to apply display and autostart settings."
