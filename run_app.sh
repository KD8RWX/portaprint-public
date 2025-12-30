#!/bin/bash

# Portaprint Access Point Launcher

# Navigate to script directory
DIR="$( cd "$( dirname "${BASH_SOURCE[0]}" )" && pwd )"
cd "$DIR"

echo "Starting Portaprint Service..."

# Start the Python Server (background)
cd websocket && python3 server.py &
SERVER_PID=$!
cd ..

# Give it a moment to bind to port
sleep 2

# Check if we are on a Raspberry Pi (Linux) or Dev (Mac)
if [[ "$OSTYPE" == "linux-gnu"* ]]; then
    # Linux / Raspberry Pi
    echo "Launching Kiosk Browser..."
    
    # Environment variables for display (if needed)
    export DISPLAY=:0.0
    
    # Chromium args for perfect kiosk
    ARGS="--kiosk --no-first-run --noerrdialogs --disable-infobars --check-for-update-interval=31536000 --overscroll-history-navigation=0"
    
    # Try different browser binaries
    if command -v chromium-browser &> /dev/null; then
        chromium-browser $ARGS http://localhost:8888/ &
    elif command -v chromium &> /dev/null; then
        chromium $ARGS http://localhost:8888/ &
    else
        echo "No chromium browser found. Sending server to foreground."
    fi
    
    BROWSER_PID=$!
    
    # Wait for the server to finish (it exits when receiving EXIT_APP)
    wait $SERVER_PID
    
    # Server exited, kill browser
    echo "Server exited, closing browser..."
    kill $BROWSER_PID
    
elif [[ "$OSTYPE" == "darwin"* ]]; then
    # macOS Dev
    echo "Launching default browser for testing..."
    open http://localhost:8888/
    
    echo "Server running. Press Ctrl+C to stop or use CTRL+ALT+Q in web UI."
    wait $SERVER_PID
fi
