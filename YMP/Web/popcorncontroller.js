function loadPopcorn() {
    var medias = {
        audio: Popcorn("#myAudio"),
        video: Popcorn("#myVideo")
    },
        loadCount = 0,
        events = "play pause timeupdate seeking volumechange".split(/\s+/g);


    // iterate both media sources
    Popcorn.forEach(medias, function (media, type) {

        // when each is ready... 
        media.on("canplayall", function () {

            // trigger a custom "sync" event
            this.emit("sync");

            // Listen for the custom sync event...    
        }).on("sync", function () {

            // Once both items are loaded, sync events
            if (++loadCount == 2) {
                // Uncomment this line to silence the video
                //medias.video.mute();

                // Iterate all events and trigger them on the video 
                // whenever they occur on the audio
                events.forEach(function (event) {

                    medias.video.on(event, function () {

                        // Avoid overkill events, trigger timeupdate manually
                        if (event === "timeupdate") {

                            if (!this.media.paused) {
                                return;
                            }
                            medias.audio.emit("timeupdate");

                            return;
                        }

                        if (event === "seeking") {
                            medias.audio.currentTime(this.currentTime());
                        }

                        if (event === "volumechange") {
                            if (this.muted()) {
                                medias.audio.mute();
                            } else {
                                medias.audio.unmute();
                            }

                            medias.audio.volume(this.volume());
                        }

                        if (event === "play" || event === "pause") {
                            medias.audio[event]();
                        }
                    });
                });
            }
        });
    });
}

let video;
let audio;
let videoTrack;
let audioTrack;

function loadElement() {
    if (!video) {
        video = document.getElementById("myVideo");
        videoTrack = document.getElementById("myVideoTrack");

        video.addEventListener('play', onPlay);
        video.addEventListener('pause', onPause);
        video.addEventListener('ended', onEnded);
        video.addEventListener('timeupdate', onTimeupdate);
        video.addEventListener('durationchange', onDurationupdate);
        video.addEventListener('error', onVideoError);
    }

    if (!audio) {
        audio = document.getElementById("myAudio");
        audioTrack = document.getElementById("myAudioTrack");

        audio.addEventListener('error', onAudioError);
    }
}

const playing = 1;
const pause = 2;
const ended = 0;
const cued = -1;

let onStateChange = function () { };
let onCurrentTimeChange = function () { };
let onDurationChange = function () { };

var currVideoIndex = 0;
var currAudioIndex = 0;
var currVideoStreams = [];
var currAudioStreams = [];

async function loadVideoById(id) {
    stop();
    var [videoStreams, audioStreams] = await getVideoInfo(id);

    currVideoStreams = videoStreams;
    currAudioStreams = audioStreams;

    loadVideo(currVideoStreams[0]);
    loadAudio(currAudioStreams[0]);
}

function loadVideo(v) {
    console.log(v);
    video.src = v.url;
    video.type = v.mime;
}

function loadAudio(a) {
    console.log(a);
    audio.src = a.url;
    audio.type = a.mime;
}

function playVideo() {
    video.play();
}

function pauseVideo() {
    video.pause();
}

function stop() {
    video.src = "";
    audio.src = "";
    currVideoStreams = [];
    currAudioStreams = [];
    currAudioIndex = 0;
    currVideoIndex = 0;
    previousDuration = 0;
    previousTime = 0;
}

function seekTo(position) {
    video.currentTime = position;
    audio.currentTime = position;
}

function mute() {
   video.muted = true;
}

function unMute() {
    video.muted = false;
}