const App = {
    socket: null,
    reconnectInterval: 1000,
    currentView: 'view-loading',
    ipAddress: '---.---.---.---',
    isConnectedToRemote: false,

    init() {
        this.setStatus("Connecting to Server...");
        this.connect();
        this.setupKeyHandlers();
        this.setupWifiHandlers();
        this.initActivityMonitor();
        // this.initBurnInDrift(); // REMOVED as per request
    },

    initActivityMonitor() {
        const body = document.body;
        const panels = document.querySelectorAll('.glass-panel');
        let activityTimeout;

        // Define timeouts per state
        const TIMEOUT_READY = 30000; // 30s
        const TIMEOUT_DONE = 120000; // 2 mins

        // Function to dim the screen
        const goDim = () => {
            // NEVER dim if we are scanning
            if (this.currentView === 'view-scanning') return;

            body.classList.remove('user-active');
            body.classList.remove('show-cursor');
            panels.forEach(p => p.classList.add('dimmed'));
        };

        // Function to wake up
        const goActive = () => {
            body.classList.add('user-active');
            body.classList.add('show-cursor');
            panels.forEach(p => p.classList.remove('dimmed'));

            clearTimeout(activityTimeout);

            // Determine timeout based on state
            let delay = TIMEOUT_READY;

            if (this.currentView === 'view-scanning') {
                return; // No timeout while scanning
            }

            // If we just finished? Logic handled by switchView mostly, 
            // but here we just check if we are NOT scanning.
            // If the user said "2 minutes after completion", we might need a flag.
            // For now, let's use standard logic: 
            // If interactive (wifi/keyboard), maybe longer? 
            // Sticking to basic rule: Ready = Timeout.

            activityTimeout = setTimeout(goDim, delay);
        };

        // Expose goActive to be called mostly
        this.triggerActivity = goActive;

        // Trigger on load
        goActive();

        // Listen for movement/clicks
        ['mousemove', 'click', 'touchstart', 'keydown'].forEach(evt => {
            document.addEventListener(evt, goActive);
        });
    },

    // Removed initBurnInDrift

    setStatus(msg) {
        const el = document.querySelector('#view-loading .status-text');
        if (el) el.innerText = msg;
    },

    connect() {
        console.log("Connecting to WebSocket...");
        // Assuming the server is running on the hostname (localhost) port 8888
        const host = window.location.hostname || 'localhost';
        // FIXED: Added /ws to the URL to match server route
        this.socket = new WebSocket(`ws://${host}:8888/ws`);

        this.socket.onopen = () => {
            console.log("WebSocket Connected");
            this.setStatus("Syncing...");
            // Request System Info immediately
            this.send({ action: "GET_SYSTEM_INFO" });
        };

        this.socket.onmessage = (event) => {
            try {
                const data = JSON.parse(event.data);
                this.handleMessage(data);
            } catch (e) {
                console.error("Error parsing message", e);
            }
        };

        this.socket.onclose = () => {
            console.warn("WebSocket Disconnected. Reconnecting...");
            // Show loading if disconnected
            this.switchView('view-loading');
            setTimeout(() => this.connect(), this.reconnectInterval);
        };

        this.socket.onerror = (err) => {
            console.error("WebSocket Error", err);
        };
    },

    send(data) {
        if (this.socket && this.socket.readyState === WebSocket.OPEN) {
            this.socket.send(JSON.stringify(data));
        }
    },

    handleMessage(data) {
        console.log("Received:", data);
        switch (data.type) {
            case 'log':
                console.log("Server Log:", data.message);
                break;

            case 'system_info':
                this.updateSystemInfo(data);
                break;

            case 'wifi_list':
                this.renderWifiList(data.networks);
                break;

            case 'wifi_status':
                this.handleWifiStatus(data);
                break;

            case 'client_connection_status':
                this.handleClientConnection(data.connected);
                break;

            case 'status':
                // Creating status/scanning feedback
                this.handleStatusUpdate(data);
                break;

            case 'result':
                // Scan finished
                this.switchView('view-ready');
                break;
        }
    },

    updateSystemInfo(data) {
        const ipDisplay = document.getElementById('device-ip');
        this.ipAddress = data.ip || '---.---.---.---';
        ipDisplay.innerText = this.ipAddress;

        // Generate QR Code if we have a valid IP
        const qrContainer = document.getElementById('qrcode');
        if (qrContainer && this.ipAddress !== '---.---.---.---') {
            qrContainer.innerHTML = ''; // Clear previous

            try {
                if (window.QRCode) {
                    new QRCode(qrContainer, {
                        text: this.ipAddress,
                        width: 128,
                        height: 128,
                        colorDark: "#000000",
                        colorLight: "#ffffff",
                        correctLevel: QRCode.CorrectLevel.M
                    });
                }
            } catch (e) {
                console.error("QR Generation failed", e);
            }
        }

        // Logic flow:
        // 1. If IP is invalid/loopback (no network), go to WiFi Wizard (unless explicitly skipped)
        // 2. If IP is valid, go to Ready State

        if (!data.connected && !this.ipAddress.startsWith("192") && !this.ipAddress.startsWith("10") && !this.ipAddress.startsWith("172")) {
            // Likely not connected to a proper LAN
            // But checking 'connected' flag from backend is safer
            if (data.wifi_needed || data.ip === '127.0.0.1' || !data.ip) {
                this.switchView('view-wifi');
                this.send({ action: "WIFI_SCAN" });
                return;
            }
        }

        // If we have an IP, we are ready
        if (this.currentView === 'view-loading' || this.currentView === 'view-wifi') {
            this.switchView('view-ready');
        }
    },

    handleClientConnection(isConnected) {
        this.isConnectedToRemote = isConnected;
        const statusText = document.getElementById('connection-status');
        const dot = document.querySelector('.dot');

        if (isConnected) {
            statusText.innerText = "Target Connected";
            statusText.style.color = "var(--success-color)";
            dot.classList.add('connected');
            // We stay in Ready view until a scan starts
        } else {
            statusText.innerText = "Waiting for Client";
            statusText.style.color = "var(--text-secondary)";
            dot.classList.remove('connected');
        }
    },

    handleStatusUpdate(data) {
        // Data can be a string (legacy/simple) or object
        const msg = (typeof data === 'string') ? data : data.message;
        const param = (typeof data === 'object') ? data.scan_param : null;

        const el = document.querySelector('#view-loading .status-text');
        if (el) el.innerText = msg;

        // Also update scanning text if visible
        const scanInstr = document.getElementById('scan-instruction');
        if (scanInstr) scanInstr.innerText = msg;

        // If 'Starting Capture', switch view
        if (msg.includes("Starting Capture")) {
            this.switchView('view-scanning');
            // Ensure we update visualizer even if param is present (0 or string)
            if (param !== undefined && param !== null) {
                console.log("Updating visualizer with param:", param);
                this.updateScanVisualizer(param);
            } else {
                // If param is missing, we might want to default to something or just show full hand?
                // Visualizer defaults to full hand if logic fails, so calling it with empty might be safer?
                // But let's check.
                console.log("No param for visualizer, showing default.");
                this.updateScanVisualizer(null); // Force default reset
            }
        } else if (msg.includes("Device Initialized")) {
            // If initialized, go to ready
            this.switchView('view-ready');
        } else if (msg.includes("Capture Cancelled") || msg.includes("Capture Timed Out")) {
            this.switchView('view-ready');
        }
    },

    updateScanVisualizer(param) {
        console.log("updateScanVisualizer called with:", param);
        const img = document.getElementById('hand-image');
        if (!img) return;

        let p = parseInt(param);

        // Handle Map legacy string params or empty inputs
        if (isNaN(p)) {
            if (param === 'R_SLAP') p = 13;
            else if (param === 'L_SLAP') p = 14;
            else if (param === 'THUMBS') p = 15;
            else {
                // PARAM MISSING OR INVALID
                // User Requirement: NEVER show generic hand. MUST select a finger.
                // We default to Right Index (2) as the most common single finger scan.
                console.warn("Invalid/Missing scan param. Defaulting to Right Index (2).");
                p = 2;
            }
        }

        let side = 'left';
        let finger = '';
        let text = "Finger";

        // Logic Mapping
        // Right Hand: 1-5, 13
        // Thumbs (15) -> We'll use Right Thumb for now as per previous logic
        if ((p >= 1 && p <= 5) || p === 13 || p === 15) {
            side = 'right';
        }

        switch (p) {
            // Right
            case 1: finger = 'thumb'; text = "Right Thumb"; break;
            case 2: finger = 'index'; text = "Right Index"; break;
            case 3: finger = 'middle'; text = "Right Middle"; break;
            case 4: finger = 'ring'; text = "Right Ring"; break;
            case 5: finger = 'little'; text = "Right Little"; break;

            // Left
            case 6: finger = 'thumb'; text = "Left Thumb"; break;
            case 7: finger = 'index'; text = "Left Index"; break;
            case 8: finger = 'middle'; text = "Left Middle"; break;
            case 9: finger = 'ring'; text = "Left Ring"; break;
            case 10: finger = 'little'; text = "Left Little"; break;

            // Slaps
            case 13: finger = 'four'; text = "Right 4 Fingers"; break;
            case 14: finger = 'four'; text = "Left 4 Fingers"; break;

            // Thumbs (15)
            case 15:
                side = 'right';
                finger = 'thumb';
                text = "Both Thumbs";
                break;

            default:
                // Fallback catch-all: Right Index
                console.warn("Unknown scan code:", p, "Defaulting to Right Index.");
                side = 'right';
                finger = 'index';
                text = "Right Index";
        }

        // Construct filename: hand-{side}-{finger}.png
        const filename = `hand-${side}-${finger}.png`;
        const fullSrc = `/assets/hand/${filename}?v=${new Date().getTime()}_${Math.floor(Math.random() * 10000)}`;

        img.src = fullSrc;
        img.alt = filename; // Show filename if broken

        // Update Title
        document.querySelector('#view-scanning h1').innerText = "Scanning " + text;
    },
    switchView(viewId) {
        document.querySelectorAll('.view').forEach(el => el.classList.remove('active'));
        document.getElementById(viewId).classList.add('active');
        this.currentView = viewId;

        // Reset power/dimming state when view changes
        if (this.triggerActivity) {
            this.triggerActivity();
        }
    },

    setupKeyHandlers() {
        document.addEventListener('keydown', (event) => {
            // CTRL + ALT + Q
            if (event.ctrlKey && event.altKey && (event.key.toLowerCase() === 'q')) {
                console.log("Exit Sequence Triggered");
                this.send({ action: "EXIT_APP" });

                // Also try to close the window
                try {
                    window.close();
                } catch (e) { console.log("Cannot close window via JS", e); }
            }
        });
    },

    setupWifiHandlers() {
        const listContainer = document.getElementById('wifi-list-container');
        const listContent = document.getElementById('wifi-list');
        const formContainer = document.getElementById('wifi-form-container');
        const passwordInput = document.getElementById('wifi-password');

        const btnBack = document.getElementById('btn-wifi-back');
        const btnConnect = document.getElementById('btn-wifi-connect');

        // Keyboard elements
        const keyboardView = document.getElementById('view-keyboard');
        const keyboardInput = document.getElementById('keyboard-input');
        const keyboardCloseBtn = document.getElementById('btn-keyboard-close');
        const keyboardCancelBtn = document.getElementById('btn-keyboard-cancel');

        let selectedSsid = null;
        let isConnectingMode = false;
        let keyboard = null;

        const updateNav = () => {
            if (isConnectingMode) {
                listContainer.classList.add('hidden');
                formContainer.classList.remove('hidden');
                btnBack.querySelector('span').innerText = 'Cancel';
                btnConnect.classList.remove('hidden');
            } else {
                listContainer.classList.remove('hidden');
                formContainer.classList.add('hidden');
                btnBack.querySelector('span').innerText = 'Back';
                btnConnect.classList.add('hidden');
            }
        };

        // Keyboard Logic
        const openKeyboard = (initialValue) => {
            keyboardView.classList.add('active');
            keyboardInput.value = initialValue || "";

            if (!keyboard && window.SimpleKeyboard) {
                const SimpleKeyboard = window.SimpleKeyboard.default;
                keyboard = new SimpleKeyboard({
                    onChange: input => {
                        keyboardInput.value = input;
                    },
                    onKeyPress: button => {
                        if (button === "{enter}") closeKeyboard(true);
                        if (button === "{escape}") closeKeyboard(false);
                    },
                    theme: "hg-theme-default hg-layout-default myTheme",
                    layout: {
                        default: [
                            "` 1 2 3 4 5 6 7 8 9 0 - = {bksp}",
                            "q w e r t y u i o p [ ] \\",
                            "a s d f g h j k l ; ' {enter}",
                            "{shift} z x c v b n m , . / {shift}",
                            "{space}"
                        ],
                        shift: [
                            "~ ! @ # $ % ^ & * ( ) _ + {bksp}",
                            "Q W E R T Y U I O P { } |",
                            "A S D F G H J K L : \" {enter}",
                            "{shift} Z X C V B N M < > ? {shift}",
                            "{space}"
                        ]
                    }
                });
            } else if (keyboard) {
                keyboard.setInput(initialValue || "");
            }
        };

        const closeKeyboard = (save) => {
            keyboardView.classList.remove('active');
            if (save) {
                passwordInput.value = keyboardInput.value;
            }
        };

        // Bind Keyboard Events
        passwordInput.addEventListener('click', () => {
            openKeyboard(passwordInput.value);
        });

        keyboardCloseBtn.addEventListener('click', () => closeKeyboard(true));
        keyboardCancelBtn.addEventListener('click', () => closeKeyboard(false));

        // Back / Cancel Button
        btnBack.addEventListener('click', () => {
            if (isConnectingMode) {
                isConnectingMode = false;
                updateNav();
            } else {
                this.switchView('view-ready');
            }
        });

        // Connect Button
        btnConnect.addEventListener('click', () => {
            const password = passwordInput.value;
            if (selectedSsid) {
                this.switchView('view-loading');
                this.send({
                    action: "WIFI_CONNECT",
                    ssid: selectedSsid,
                    password: password
                });
            }
        });

        // Click on Network Item
        listContent.addEventListener('click', (e) => {
            const item = e.target.closest('.wifi-item');
            if (item) {
                selectedSsid = item.dataset.ssid;
                document.getElementById('selected-ssid').innerText = selectedSsid;
                passwordInput.value = '';

                isConnectingMode = true;
                updateNav();
                // Instead of focus, maybe just wait for user to tap input?
                // Or auto-open keyboard:
                // openKeyboard('');
            }
        });

        // Open Wifi Button (Entry Point)
        const openWifiBtn = document.getElementById('btn-open-wifi');
        if (openWifiBtn) {
            openWifiBtn.addEventListener('click', () => {
                this.switchView('view-wifi');
                isConnectingMode = false;
                updateNav();
                this.send({ action: "WIFI_SCAN" });
            });
        }
    },

    renderWifiList(networks) {
        const listContainer = document.getElementById('wifi-list');
        listContainer.innerHTML = ''; // Clear current

        if (!networks || networks.length === 0) {
            listContainer.innerHTML = '<div style="padding:10px; opacity:0.7">No networks found. Retrying...</div>';
            setTimeout(() => this.send({ action: "WIFI_SCAN" }), 5000);
            return;
        }

        networks.forEach(net => {
            const div = document.createElement('div');
            div.className = 'wifi-item';
            div.dataset.ssid = net.ssid;
            // Signal strength icon logic could go here
            div.innerHTML = `
                <span>${net.ssid}</span>
                <span style="opacity:0.5">${net.signal || ''}%</span>
            `;
            listContainer.appendChild(div);
        });
    },

    handleWifiStatus(data) {
        if (data.connected) {
            // Connected successfully!
            // Wait for IP update
            setTimeout(() => this.send({ action: "GET_SYSTEM_INFO" }), 2000);
        } else {
            alert("Failed to connect: " + (data.message || "Unknown error"));
            this.switchView('view-wifi');
            this.send({ action: "WIFI_SCAN" }); // Refresh list
        }
    }
};

window.addEventListener('load', () => App.init());
