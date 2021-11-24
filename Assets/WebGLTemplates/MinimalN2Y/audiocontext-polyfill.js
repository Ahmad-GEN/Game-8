window.AudioContext = window.AudioContext || window.webkitAudioContext ||
    function () {

        this.createGain = function () {
            return {
                connect: function () {

                },
                gain: {},
            };
        };

        this.createPanner = function () {
            return {
                disconnect: function () { },
                setPosition: function () { }
            };
        };

        this.createBufferSource = function () {
            return {
                disconnect: function () { },
                connect: function () { },
                buffer: null,
                start: function (delay, offset) {
                    var self = this;
                    if (self.buffer) {
                        var audio = new Audio("data:audio/mpeg;base64," + self.buffer.base64);
                        audio.play();
                        audio.played = function () {
                            if (self.onended) {
                                self.onended();
                            }
                        }
                    } else {
                        if (self.onended) {
                            self.onended();
                        }
                    }
                },
                onended: null,
                playbackRate: {}
            };
        };

        this.decodeAudioData = function (audioData, successCallback, errorCallback) {
            try {
                var soundData = {
                    base64: btoa(String.fromCharCode.apply(null, new Uint8Array(audioData)))
                };
                successCallback(soundData);
            } catch (err) {
                errorCallback();
            }
        };

        this.listener = {};

        this.listener.setPosition = function () {

        }

        this.listener.setOrientation = function () {

        }
    };
console.log("audiocontext-polyfill_version:1.6");
var isRunOnce=false;
var isMac = navigator.platform.toUpperCase().indexOf('MAC')>=0;
let isIOS = /iPad|iPhone|iPod/.test(navigator.platform) ||
    (navigator.platform === 'MacIntel' && navigator.maxTouchPoints > 1)
if (isIOS || isMac) {
    var OriginalAudioContext = AudioContext;
    window.AudioContext = function AudioContext() {
        var iOSCtx = new OriginalAudioContext();

        var body = document.body;
        var tmpBuf = iOSCtx.createBufferSource();
        var tmpProc = iOSCtx.createScriptProcessor(256, 1, 1);

        body.addEventListener('touchstart', instantProcess, false);
        //window.addEventListener("blur",instantProcess);
        body.addEventListener('click', instantProcess, false);
        function instantProcess()
        {
            console.log("instantProcess() called");
            try
            {
                console.log("instantProcess() running");
                tmpBuf.start(0);
                tmpBuf.connect(tmpProc);
                tmpProc.connect(iOSCtx.destination);
                isRunOnce = true;
            }
            catch(err) {
                console.log("instantProcess() message");
                console.log(err.message);
            }
        }

        // This function will be called once and for all.
        tmpProc.onaudioprocess = function () {
            tmpBuf.disconnect();
            tmpProc.disconnect();
           // body.removeEventListener('touchstart', instantProcess, false);
            tmpProc.onaudioprocess = null;
        };

        return iOSCtx;
    };
};